import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { DeliveryMethod } from 'src/app/core/models/delivery-method.model';
import { BasketService } from 'src/app/core/services/basket.service';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.css']
})
export class CheckoutDeliveryComponent implements OnInit {

  @Input() checkoutForm?: FormGroup
  deliveryMethods: DeliveryMethod[] = []

  constructor(
    private orderService: OrderService,
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    this.orderService.getAllDeliveryMethods().subscribe({
      next: dms => this.deliveryMethods = dms
    })
  }

  setDeliveryPrice(deliveryMethod: DeliveryMethod) {
    this.basketService.setDeliveryPrice(deliveryMethod)
  }

}
