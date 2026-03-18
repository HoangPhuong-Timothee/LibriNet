export interface Author {
    id: number
    name: string
    createInfo?: string
    updateInfo?: string
}

export interface AddAuthorRequest {
  name: string
}

export interface UpdateAuthorRequest {
  id: number
  name: string
}
