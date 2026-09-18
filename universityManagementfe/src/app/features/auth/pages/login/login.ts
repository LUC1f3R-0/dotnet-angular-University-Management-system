import {
  Component,
  inject,
  signal,
} from '@angular/core';

import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  HttpErrorResponse,
} from '@angular/common/http';

import {
  Router,
} from '@angular/router';

import {
  finalize,
} from 'rxjs';

import {
  AuthService,
} from '../../../../core/auth/auth.service';

@Component({
  imports: [
    ReactiveFormsModule,
  ],

  selector: 'app-login',

  styleUrl: './login.css',

  templateUrl: './login.html',
})
export class Login {
  private readonly auth =
    inject(AuthService);

  private readonly router =
    inject(Router);

  readonly isLogging =
    signal(false);

  readonly errorMessage =
    signal('');

  readonly loginForm =
    new FormGroup({
      email:
        new FormControl(
          '',
          {
            nonNullable: true,

            validators: [
              Validators.required,
              Validators.email,
              Validators.maxLength(
                100,
              ),
            ],
          },
        ),

      password:
        new FormControl(
          '',
          {
            nonNullable: true,

            validators: [
              Validators.required,
            ],
          },
        ),
    });


  loginSubmit(): void {
    if (
      this.loginForm.invalid
    ) {
      this.loginForm
        .markAllAsTouched();

      return;
    }

    this.errorMessage.set('');

    this.isLogging.set(true);

    this.auth
      .login(
        this.loginForm
          .getRawValue(),
      )
      .pipe(
        finalize(() => {
          this.isLogging
            .set(false);
        }),
      )
      .subscribe({
        next: () => {
          void this.router
            .navigateByUrl(
              '/dashboard',
            );
        },

        error: (
          error: unknown,
        ) => {
          if (
            error instanceof
            HttpErrorResponse
          ) {
            this.errorMessage.set(
              error.error?.message ??
                'Login failed.',
            );

            return;
          }

          if (
            error instanceof Error
          ) {
            this.errorMessage.set(
              error.message,
            );

            return;
          }

          this.errorMessage.set(
            'Login failed.',
          );
        },
      });
  }
}
