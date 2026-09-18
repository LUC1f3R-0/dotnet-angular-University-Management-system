import { inject } from '@angular/core';
import { CanActivateFn, Router, } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from './auth.service';

export const authGuard:
  CanActivateFn = (_route, _state) => {
    const auth = inject(AuthService);
    const router = inject(Router);
    
    return auth.ensureInitialized().pipe(
      map(authenticated => {
        if (authenticated) {
          return true;
        }

        return router.createUrlTree([
          '/login',
        ]);
      }),
    );
  };
