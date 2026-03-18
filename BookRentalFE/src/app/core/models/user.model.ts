export interface User {
  id: number
  email: string
  userName: string
  imageUrl: string
  dateOfBirth: string
  gender: string
  phoneNumber: string
  roles: string[] | string
  token: string
}

export interface ModifyProfileRequest {
  userName: string
  dateOfBirth: Date | string
  gender: string
  phoneNumber: string
  fullName: string
  street: string
  ward: string
  district: string
  city: string
  postalCode: string
}

export interface Member {
  id: number
  email: string
  imageUrl: string
  dateOfBirth: string
  gender: string
  phoneNumber: string
  address: string
  roles: string
}
