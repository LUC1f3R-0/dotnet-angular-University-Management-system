import { inject } from '@angular/core';
import { CanActivateFn, Router, } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from './auth.service';

export const roleGuard: CanActivateFn = (route, _state) => {
  
  const auth = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data['roles'] as string[] | undefined;

  return auth.ensureInitialized().pipe(
    map(authenticated => {
      if (!authenticated) {
        return router.createUrlTree(['/login']);
      }

      if (!allowedRoles || allowedRoles.length === 0) {
        return true;
      }

      const user = auth.currentUser();

      if (user && allowedRoles.includes(user.role)) {
        return true;
      }

      return router.createUrlTree(['/dashboard']);
    }),
  );
};
