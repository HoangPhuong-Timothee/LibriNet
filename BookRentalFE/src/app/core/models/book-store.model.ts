export interface BookStore {
  id: number
  storeName: string
  storeAddress: string
  createInfo: string
  updateInfo: string
  totalQuantity: number
}

export interface AddBookStoreRequest {
  storeName: string
  storeAddress: string
}

export interface UpdateBookStoreRequest {
  id: number
  storeName: string
  storeAddress: string
}
