import { Component } from '@angular/core';
import { BasketItem } from 'src/app/core/models/basket.model';
import { BasketService } from 'src/app/core/services/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent {

  constructor(public basketService: BasketService) { }

  increaseQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item)
  }

  removeItem(event: { id: number, quantity: number }) {
    this.basketService.removeItemFromBasket(event.id, event.quantity)
  }
}
