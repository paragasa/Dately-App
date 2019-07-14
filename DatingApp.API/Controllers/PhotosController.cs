using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers {
    [Authorize]
    [Route ("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
         /*
        Cloudinary setting stored in IConfig 
         */
        public PhotosController (IDatingRepository repo, IMapper mapper,
         IOptions<CloudinarySettings> cloudinaryConfig) {
            this._cloudinaryConfig = cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName, 
                _cloudinaryConfig.Value.ApiKey, 
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        /* Need FROM FORM for Request to be read by Controller */
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
            [FromForm]PhotoForCreationDto photoForCreationDto){

            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            
            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();
            
            if (file.Length >0){
                using( var stream = file.OpenReadStream()){
                    var uploadParams = new ImageUploadParams {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Crop("fill")
                        .Width(500).Height(500).Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            
            if(!userFromRepo.Photos.Any(u => u.IsMain)){
                photo.IsMain = true;
            }
            
            userFromRepo.Photos.Add(photo); 

            if(await _repo.SaveAll()){
                
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("could not add photo");
        }

        [HttpPost("{id}/setMain")]
         public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {   
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            
            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(u => u.Id == id))
                return Unauthorized();
            
          
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return(BadRequest("Photo is already set to main"));

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;
            if(await _repo.SaveAll())
                return NoContent();
            
            return BadRequest("Could not set photo as main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {   
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            
            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(u => u.Id == id))
                return Unauthorized();
            
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return(BadRequest("Cannot delete main photo"));

            if(photoFromRepo.PublicId!= null){
                //Access cloudinary deletion variables
            var delParams = new DeletionParams(photoFromRepo.PublicId);
            var delResult = _cloudinary.Destroy(delParams);
            
            //returns back a string
            if(delResult.Result =="ok")
                _repo.Delete(photoFromRepo);

            }

            if(photoFromRepo.PublicId == null){
                 _repo.Delete(photoFromRepo);
            }
            
            if(await _repo.SaveAll())
                return Ok();
            
            return BadRequest("Failed to dlete photo");
            
        }
    }
}