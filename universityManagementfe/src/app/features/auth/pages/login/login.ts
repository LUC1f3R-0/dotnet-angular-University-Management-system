import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../data-access/auth-api.service';
import { LoginRequest } from '../../models/login.model';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {
  
  isLogging = signal(false);

  private authService = inject(AuthService);
  
  loginForm = new FormGroup({
    loginEmail: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(3)
      ]
    }),
    loginPassword: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(3)
      ]
    })
  });
  loginSubmit() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched()
      return
    }
    this.isLogging.set(true)
    
    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: response => {
        
      }
    })
  }
  // onType(event: Event) {
  //   const value = (event.target as HTMLInputElement).value;
  //   console.log(value);
  // }
}
