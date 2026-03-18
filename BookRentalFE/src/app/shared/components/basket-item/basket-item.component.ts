import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BasketItem } from 'src/app/core/models/basket.model';
import { BasketService } from 'src/app/core/services/basket.service';

@Component({
  selector: 'app-basket-item',
  templateUrl: './basket-item.component.html',
  styleUrls: ['./basket-item.component.css']
})
export class BasketItemComponent {

  @Output() addItem = new EventEmitter<BasketItem>()
  @Output() removeItem = new EventEmitter<({ id: number, quantity: number })>()
  @Input() isBasket = true

  constructor(public basketService: BasketService) { }

  addBasketItem(item: BasketItem) {
    this.addItem.emit(item)
  }

  removeBasketItem(id: number, quantity: number) {
    this.removeItem.emit({ id, quantity })
  }
}
