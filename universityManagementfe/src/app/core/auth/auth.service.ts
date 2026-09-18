import { computed, inject, Injectable, signal, } from '@angular/core';
import { Observable, catchError, finalize, map, of, shareReplay, tap, } from 'rxjs';
import { AuthApiService } from '../../features/auth/data-access/auth-api.service';
import { AuthUser, LoginRequest, } from '../../features/auth/models/login.model';

export type AuthStatus = | 'unknown' | 'authenticated' | 'unauthenticated';

@Injectable({
  providedIn: 'root',
})
  
export class AuthService {
  private readonly api = inject(AuthApiService);
  private readonly statusState = signal<AuthStatus>('unknown');
  private readonly userState = signal<AuthUser | null>(null);
  private initializationRequest: Observable<boolean> | null = null;
  private refreshRequest: Observable<void> | null = null;

  readonly status = this.statusState.asReadonly();
  readonly currentUser = this.userState.asReadonly();

  readonly isAuthenticated = computed(() =>
    this.statusState() === 'authenticated',
  );


  login(request: LoginRequest,): Observable<AuthUser> {
    return this.api.login(request).pipe(
      map(response => {
        if (!response.success || response.data === null) {
          throw new Error(
            response.message ?? 'Login failed.',
          );
        }

        const user: AuthUser = {
          userUuid: response.data.userUuid,
          name: response.data.name,
          email: response.data.email,
          role: response.data.role,
        };

        return user;
      }),

      tap(user => {
        this.userState.set(user);
        this.statusState.set('authenticated',);
      }),
    );
  }


  ensureInitialized(): Observable<boolean> {
    const status = this.statusState();
    if (status === 'authenticated') {
      return of(true);
    }

    if (status === 'unauthenticated') {
      return of(false);
    }

    if (this.initializationRequest) {
      return this.initializationRequest;
    }

    const request = this.api.me().pipe(
      map(response => {
        if (!response.success || response.data === null) {
          this.clearSession();

          return false;
        }

        const user: AuthUser = {
          userUuid: response.data.userUuid,
          sessionUuid: response.data.sessionUuid,
          role: response.data.role,
        };
        this.userState.set(user);
        this.statusState.set('authenticated',);

        return true;
      }),

          catchError(() => {
            this.clearSession();

            return of(false);
          }),

          finalize(() => {
            this.initializationRequest = null;
          }),

      shareReplay({
        bufferSize: 1, refCount: false,
      }),
    );

    this.initializationRequest = request;
    return request;
  }


  refreshSession(): Observable<void> {
    if (this.refreshRequest) {
      return this.refreshRequest;
    }

    const request = this.api.refresh().pipe(
      map(() => undefined),
      finalize(() => {
        this.refreshRequest = null;
      }),

      shareReplay({
        bufferSize: 1,
        refCount: false,
      }),
    );

    this.refreshRequest = request;

    return request;
  }


  logout(): Observable<void> {
    return this.api.logout().pipe(
      map(() => undefined),
      finalize(() => {
        this.clearSession();
      }),
    );
  }

  clearSession(): void {
    this.userState.set(null);
    this.statusState.set('unauthenticated',);
  }
}
