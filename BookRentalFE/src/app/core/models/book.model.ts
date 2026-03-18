import { BookImage } from "./book-image.model"

export interface Book {
    id: number
    title: string
    author: string
    genre?: string
    publisher?: string
    imageUrl: string
    description?: string
    publishYear?: number
    price: number
    quantityInStock: number
    createInfo?: string
    updateInfo?: string
    isAvailable: boolean
    isbn?: string
    bookImages?: BookImage[]
}
