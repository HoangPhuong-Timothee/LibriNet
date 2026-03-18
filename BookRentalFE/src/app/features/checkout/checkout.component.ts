import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BasketService } from 'src/app/core/services/basket.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private basketService: BasketService
  ) { }

  checkoutForm = this.fb.group({
    userInfoForm: this.fb.group({
      fullName: ['', Validators.required],
      street: ['', Validators.required],
      ward: ['', Validators.required],
      district: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required]
    }),
    deliveryForm: this.fb.group({
      deliveryMethod: ['', Validators.required]
    }),
    paymentForm: this.fb.group({
      paymentMethod: ['COD', Validators.required]
    })
  })

  ngOnInit(): void {
    this.getUserInfoFormValue()
    this.getDeliveryMethodFormValue()
  }

  getUserInfoFormValue() {
    this.userService.getUserAddress().subscribe({
      next: userInfo => {
        userInfo && this.checkoutForm.get('userInfoForm')?.patchValue(userInfo)
      }
    })
  }

  getDeliveryMethodFormValue() {
    const basket = this.basketService.getCurrentBasketValue()
    if (basket && basket.deliveryMethodId) {
      this.checkoutForm.get('deliveryForm')?.get('deliveryMethod')?.patchValue(basket.deliveryMethodId.toString())
    }
  }
}
