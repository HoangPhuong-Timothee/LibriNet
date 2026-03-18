import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { CheckoutDeliveryComponent } from './checkout-delivery/checkout-delivery.component';
import { CheckoutPaymentComponent } from './checkout-payment/checkout-payment.component';
import { CheckoutReviewComponent } from './checkout-review/checkout-review.component';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { CheckoutUserInfoComponent } from './checkout-user-info/checkout-user-info.component';
import { CheckoutComponent } from './checkout.component';


@NgModule({
  declarations: [
    CheckoutUserInfoComponent,
    CheckoutDeliveryComponent,
    CheckoutPaymentComponent,
    CheckoutComponent,
    CheckoutReviewComponent
  ],
  imports: [
    CommonModule,
    CheckoutRoutingModule,
    SharedModule
  ]
})
export class CheckoutModule { }
