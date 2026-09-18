import {
  Component,
  inject,
} from '@angular/core';

import {
  Router,
  RouterLink,
} from '@angular/router';

import {
  AuthService,
} from '../../../../core/auth/auth.service';

@Component({
  imports: [
    RouterLink,
  ],

  selector: 'app-dashboard',

  styleUrl: './dashboard.css',

  templateUrl: './dashboard.html',
})
export class Dashboard {
  readonly auth =
    inject(AuthService);

  private readonly router =
    inject(Router);


  logout(): void {
    this.auth
      .logout()
      .subscribe({
        next: () => {
          void this.router
            .navigateByUrl(
              '/login',
            );
        },

        error: () => {
          // AuthService clears local
          // state even if logout API fails.
          void this.router
            .navigateByUrl(
              '/login',
            );
        },
      });
  }
}
