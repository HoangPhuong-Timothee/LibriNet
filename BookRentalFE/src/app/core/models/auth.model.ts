export interface LoginResponse {
  id: number
  token: string
  role: string[] | string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
  confirmPassword: string
  phoneNumber: string
  gender: string
  dateOfBirth: Date | string
}

export interface ChangePasswordRequest {
  oldPassword: string
  newPassword: string
  confirmPassword: string
}

export interface RefreshTokenRequest {
  refreshToken: string
  accessToken: string
}
