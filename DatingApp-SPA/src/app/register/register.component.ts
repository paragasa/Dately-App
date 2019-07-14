import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
// @Input() valuesFromHome: any;
@Output() cancelRegister = new EventEmitter();
user: User;
registrationForm: FormGroup;
bsConfig: Partial<BsDatepickerConfig>;
constructor(private router: Router , private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder) { }

/**
 * bsConfig for bootstrap calender
 */
ngOnInit() {
  this.bsConfig = {
    containerClass: 'theme-green'
  };
  this.createRegisterForm();
}

createRegisterForm() {
  this.registrationForm = this.fb.group({
    username: ['', Validators.required],
    knownAs: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    gender: ['male'],
    city: ['', Validators.required],
    country: ['', Validators.required],
    password : ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPassword : ['', Validators.required]
    
  }, {validator: this.passwordMatchValidator});
}

passwordMatchValidator(g: FormGroup) {
  return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch: true};
}
register() {
  if (this.registrationForm.valid) {
    this.user = Object.assign({}, this.registrationForm.value); // clones values to empty struct
    this.authService.register(this.user).subscribe(() => {
      this.alertify.success('registration successfull');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.authService.login(this.user).subscribe(() => {
        this.router.navigate(['/members']);
      });
    }
    );
  }
}

cancel() {
  this.cancelRegister.emit(false);
}

}
