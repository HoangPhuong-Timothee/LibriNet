export interface Publisher {
    id: number
    name: string
    address?: string
    createInfo?: string
    updateInfo?: string
}

export interface AddPublisherRequest {
  name: string
  address: string
}
export interface UpdatePublisherRequest {
  id: number
  name: string
  address: string
}
