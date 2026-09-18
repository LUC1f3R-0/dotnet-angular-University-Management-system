import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

import {ApiResponse,CurrentUserResponse,LoginRequest,LoginResponse,} from '../models/login.model';

@Injectable({
  providedIn: 'root',
})
  
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.baseUrl;

  login(request: LoginRequest,): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.baseUrl}/auth/login`,request,);
  }

  me(): Observable<ApiResponse<CurrentUserResponse>> {
    return this.http.get<ApiResponse<CurrentUserResponse>>(`${this.baseUrl}/account/me`,);
  }

  refresh(): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(`${this.baseUrl}/auth/refresh`,{},);
  }

  logout(): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(`${this.baseUrl}/auth/logout`,{},);
  }
}
