import { HttpErrorResponse, HttpInterceptorFn, } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError, } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

export const authInterceptor:
  HttpInterceptorFn = (request, next) => {
    const auth = inject(AuthService);
    const router =inject(Router);
    const isApiRequest = request.url.startsWith(
      environment.baseUrl,
    );

    if (!isApiRequest) {
      return next(request);
    }

    const requestWithCookies = request.clone({
      withCredentials: true,
    });

    const isLoginRequest = request.url.includes(
      '/auth/login',
    );

    const isRefreshRequest = request.url.includes(
      '/auth/refresh',
    );

    const isLogoutRequest = request.url.includes(
      '/auth/logout',
    );

    return next(requestWithCookies,).pipe(catchError(error => {
      if (!(error instanceof HttpErrorResponse) || error.status !== 401) {
        return throwError(
          () => error,
        );
      }

        // Never attempt refresh because
        // login/refresh/logout itself
        // returned 401.
      if (isLoginRequest || isRefreshRequest || isLogoutRequest) {
        return throwError(
          () => error,
        );
      }

      return auth.refreshSession().pipe(
        switchMap(() =>
          next(
            requestWithCookies,
          ),
        ),

        catchError(refreshError => {
          auth.clearSession();
          void router.navigate(
            ['/login'],
          );

          return throwError(
            () => refreshError,
          );
        },
        ),
      );
    }),
    );
  };
