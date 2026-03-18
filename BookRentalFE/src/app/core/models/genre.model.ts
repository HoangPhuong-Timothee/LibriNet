export interface Genre {
    id: number
    name: string
    createInfo?: string
    updateInfo?: string
}
export interface AddGenreRequest {
  name: string
}

export interface UpdateGenreRequest {
  id: number
  name: string
}
