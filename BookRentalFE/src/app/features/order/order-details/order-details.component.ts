import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderWithDetails } from 'src/app/core/models/order.model';
import { OrderService } from 'src/app/core/services/order.service';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {

  id?: number
  orderDetails?: OrderWithDetails

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bcService: BreadcrumbService,
    private orderService: OrderService
  ) { this.bcService.set('@orderDetails', ' ') }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      let id = params.get('id')
      if (id) {
        this.getOrderDetails(+id)
      }
    })
  }

  getOrderDetails(id: number) {
    this.orderService.getOrderDetails(id).subscribe({
      next: (response) => {
        this.orderDetails = response
        this.bcService.set('@orderDetails', `#${this.orderDetails.orderId.toString()}-${this.orderDetails.userEmail}`)
      },
      error: (error) => {
        console.error(error)
      }
    })
  }

  get userInfo() {
    return this.orderDetails?.fullName + ", "
      + this.orderDetails?.street + ", "
      + this.orderDetails?.ward + ", "
      + this.orderDetails?.district + ", "
      + this.orderDetails?.city + ", "
      + this.orderDetails?.postalCode
  }

  backToOrders() {
    return this.router.navigateByUrl('/order')
  }

}
