import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { LoginRequest } from "../models/login.model";
import { environment } from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  private http = inject(HttpClient);
  
  private environment = environment;
  
  login(request: LoginRequest) {
    console.log(request);
    return this.http.post(`${this.environment.baseUrl}/auth/login`, request)
  }
}