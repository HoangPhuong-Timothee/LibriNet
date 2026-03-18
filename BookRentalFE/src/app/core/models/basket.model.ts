import * as cuid from 'cuid'

export interface Basket {
    id: string
    basketItems: BasketItem[]
    deliveryPrice: number
    clientSecret?: string
    paymentIntentId?: string
    deliveryMethodId?: number
}

export interface BasketItem {
    id: number
    bookTitle: string
    price: number
    quantity: number
    imageUrl: string
    author: string
}

export interface BasketTotals {
    delivery: number
    subtotal: number
    total: number
}

export class Basket implements Basket {
    id = cuid()
    basketItems: BasketItem[] = []
    deliveryPrice: number = 0
}
