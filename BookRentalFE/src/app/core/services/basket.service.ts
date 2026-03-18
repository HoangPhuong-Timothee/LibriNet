import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, map } from 'rxjs';
import { isBook } from 'src/app/shared/helpers/is-book';
import { mapToBasketItem } from 'src/app/shared/helpers/map-to-basketItem';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../models/basket.model';
import { Book } from '../models/book.model';
import { DeliveryMethod } from '../models/delivery-method.model';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  private basketSource = new BehaviorSubject<Basket | null>(null)
  basket$ = this.basketSource.asObservable()
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null)
  basketTotal$ = this.basketTotalSource.asObservable()

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  createOrUpdatePaymentIntent() {
    const basket = this.getCurrentBasketValue()
    if (!basket) {
      throw new Error('Có lỗi giỏ sách')
    }
    return this.http.post<Basket>(`${environment.baseAPIUrl}/api/Payments/stripe/${basket.id}`, {}).pipe(
      map((basket) => {
        this.setBasket(basket)
      })
    )
  }

  setDeliveryPrice(deliveryMethod: DeliveryMethod) {
    const basket = this.getCurrentBasketValue()
    if (basket) {
      basket.deliveryPrice = deliveryMethod.price
      basket.deliveryMethodId = deliveryMethod.id
      this.setBasket(basket)
    }
  }

  get basketItemCount$() {
    return this.basket$.pipe(
      map(basket => basket ? basket.basketItems.reduce((sum, item) => sum + item.quantity, 0) : 0)
    )
  }

  //Get the basket from Redis
  getBasket(id: string) {
    return this.http.get<Basket>(`${environment.baseAPIUrl}/api/Baskets?id=${id}`).subscribe({
      next: ((basket) => {
        if (basket)
        {
          this.basketSource.next(basket)
          this.calculateTotal()
        }
      })
    })
  }

  //Update a bakset in Redis
  setBasket(basket: Basket) {
    return this.http.post<Basket>(`${environment.baseAPIUrl}/api/Baskets`, basket).subscribe({
      next: ((basket) => {
        if (basket)
        {
          this.basketSource.next(basket)
          this.calculateTotal()
        }
      })
    })
  }

  //Get basket info when changing
  getCurrentBasketValue() {
    return this.basketSource.value
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Baskets?id=${basket.id}`).subscribe({
      next: () => {
        this.deleteLocalBasket()
      }
    })
  }

  //Delete basket
  deleteLocalBasket() {
    this.basketSource.next(null)
    this.basketTotalSource.next(null)
    localStorage.removeItem('basket_key')
  }

  //Add an item/book to the basket
  addItemToBasket(item: Book | BasketItem, quantity = 1) {
    if (isBook(item)) {
      item = mapToBasketItem(item);
    }
    //Check if is there any current basket in storage, if not create new basket
    const basket = this.getCurrentBasketValue() ?? this.createBasket()
    //Add or update basket item
    basket.basketItems = this.addOrUpdateBasketItem(basket.basketItems, item, quantity)
    //Set basket item change in storage
    this.setBasket(basket)
  }

  createBasket(): Basket {
    const basket = new Basket()
    localStorage.setItem('basket_key', basket.id)
    return basket
  }

  //Remove an item/book from the basket
  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue()
    if (!basket) return
    const item = basket.basketItems.find(x => x.id === id)
    if (item) {
      item.quantity -= quantity
      if (item.quantity === 0) {
        basket.basketItems = basket.basketItems.filter(x => x.id !== id)
      }
      if (basket.basketItems.length > 0) {
        this.setBasket(basket)
      } else {
        this.deleteBasket(basket)
      }
    }
  }

  private addOrUpdateBasketItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const index = items.findIndex(x => x.id === itemToAdd.id)
    if (index === -1) {
      itemToAdd.quantity = quantity
      items.push(itemToAdd)
    } else {
      items[index].quantity += quantity
    }
    return items
  }

  private calculateTotal() {
    const basket = this.getCurrentBasketValue()
    if (!basket) {
      return
    }
    const subtotal = basket.basketItems.reduce((a, b) => a + (b.price * b.quantity), 0)
    const total = subtotal + basket.deliveryPrice
    this.basketTotalSource.next({ delivery: basket.deliveryPrice, total, subtotal })
  }
}
