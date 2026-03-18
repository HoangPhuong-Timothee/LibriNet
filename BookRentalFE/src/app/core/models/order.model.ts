import { Address } from "./address.model";

export interface CreateOrderRequest {
  deliveryMethodId: string
  shippingAddress: Address
}

export interface OrderWithDetails {
  orderId: number
  userEmail: string
  orderDate: Date
  fullName: string
  street: string
  ward: string
  district: string
  city: string
  postalCode: string
  subtotal: number
  deliveryShortName: string
  deliveryTime: string
  deliveryPrice: number
  orderItems: OrderItem[]
}

export interface OrderItem {
  orderItemId: number
  bookId: number
  bookTitle: string
  bookImageUrl: string
  price: number
  quantity: number
}

export interface Order {
  orderId: number
  userEmail: string
  orderDate: string
  orderTotal: number
  status: string
  paymentIntentId: string
  deliveryShortName: string
  paymentMethod: string
}
