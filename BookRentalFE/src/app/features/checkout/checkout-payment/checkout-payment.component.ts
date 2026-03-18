import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Address } from 'src/app/core/models/address.model';
import { Basket } from 'src/app/core/models/basket.model';
import { CreateOrderRequest } from 'src/app/core/models/order.model';
import { BasketService } from 'src/app/core/services/basket.service';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.css']
})
export class CheckoutPaymentComponent {

  @Input() checkoutForm?: FormGroup
  loading = false

  constructor(
    private basketService: BasketService,
    private orderService: OrderService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  get codPaymentFormComplete() {
    return this.checkoutForm?.get('paymentForm')?.value.paymentMethod === 'COD'
  }

  async submitOrder() {
    this.loading = true
    const basket = this.basketService.getCurrentBasketValue()
    console.log(basket)
    console.log(basket?.id)
    if (!basket) {
    throw new Error('Không thể lấy thông tin giò hàng')
    }
    try {
      await this.createOrder(basket)
    } catch (error: any) {
      console.log("Có lỗi: ", error)
      this.toastr.error(error.message)
    } finally {
      this.loading = false
    }
  }

  async createOrder(basket: Basket | null) {
    if (!basket) {
      throw new Error('Giỏ hàng rỗng')
    }
    const orderToCreate = this.getOrderToCreate()
    this.orderService.createOrder(orderToCreate, basket.id).subscribe({
      next: (response) => {
        this.toastr.success(response.message)
        this.basketService.deleteBasket(basket)
        this.router.navigateByUrl('/order')
      },
      error: (error) => {
        this.toastr.error(error.message)
        console.log("Có lỗi: ", error)
      }
    })
  }

  getOrderToCreate():  CreateOrderRequest {
    const deliveryMethodId = this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value
    const shippingAddress = this.checkoutForm?.get('userInfoForm')?.value as Address
    if (!deliveryMethodId || !shippingAddress) {
      throw new Error('Có lỗi xảy ra khi thực hiện thanh toán giỏ hàng')
    }
    return {
      deliveryMethodId: deliveryMethodId,
      shippingAddress: shippingAddress
    }
  }
}
