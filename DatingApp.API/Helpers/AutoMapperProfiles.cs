using AutoMapper;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Linq;
using System.Xml.Linq;
using System;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForList>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(dob => dob.DateOfBirth.CalculateAge()); //use resolve to calculate age from dob of user
                }); 
            CreateMap<User, UserForDetailDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(dob => dob.DateOfBirth.CalculateAge()); //use resolve to calculate age from dob of user
                });
                
            CreateMap<Photo, PhotosForDetailDto>();
            
            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<PhotoForCreationDto, Photo>();
            
             CreateMap<UserForRegisterDto, User>();

    

            
        }

        
    }
}