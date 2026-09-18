export interface ApiResponse<T> {
  success: boolean;
  message: string | null;
  data: T | null;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userUuid: string;
  name: string;
  email: string;
  role: string;
}

export interface CurrentUserResponse {
  userUuid: string;
  sessionUuid: string;
  role: string;
}

export interface AuthUser {
  userUuid: string;
  role: string;

  sessionUuid?: string;

  name?: string;
  email?: string;
}
