import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {
  loginForm = new FormGroup({
    loginEmail: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(3)
    ]),
    loginPassword: new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(3)
    ]),
  });
  loginSubmit() {
    if (this.loginForm.invalid) {
      return
    }
    console.log(this.loginForm.getRawValue())
  }
}

