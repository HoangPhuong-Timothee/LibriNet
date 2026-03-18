import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/core/services/basket.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.css']
})
export class CheckoutReviewComponent {

  @Input() appStepper?: CdkStepper

  constructor(
    private basketService: BasketService,
    private toastr: ToastrService
  ) { }

  createPaymentIntent() {
    this.basketService.createOrUpdatePaymentIntent().subscribe({
      next: () => this.appStepper?.next(),
      error: error => this.toastr.error(error.message)
    })
  }
}
