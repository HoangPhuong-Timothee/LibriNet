import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/core/models/order.model';
import { OrderParams } from 'src/app/core/models/params.model';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

  orders?: Order[]
  totalOrders: number = 0
  orderParams: OrderParams
  sortOptions = [
    { name: 'Mới nhất', value: 'newest' },
    { name: 'Cũ nhất', value: 'oldest' }
  ]
  statusOptions = [
    { name: 'Tất cả', value: '' },
    { name: 'Đã giao hàng', value: 'PaymentReceived' },
    { name: 'Đang chờ xử lý', value: 'Pending' },
    { name: 'Đơn hàng đã hủy', value: 'PaymentFailed' }
  ]

  constructor(
    private orderService: OrderService
  ) { this.orderParams = orderService.getOrderParams() }

  ngOnInit(): void {
    this.getUserOrders()
  }

  getUserOrders() {
    this.orderService.getUserOrders().subscribe({
      next: response => {
        this.orders = response.data
        this.totalOrders = response.count
      }
    })
  }

  onPageChanged(event: any) {
    const params = this.orderService.getOrderParams()
    if (params.pageIndex !== event) {
      params.pageIndex = event
      this.orderService.setOrderParams(params)
      this.orderParams = params
      this.getUserOrders()
    }
  }

  onSortSelected(event: any) {
    const params = this.orderService.getOrderParams()
    params.sort = event.target.value
    this.orderService.setOrderParams(params)
    this.orderParams = params
    this.getUserOrders()
  }

  onStatusSelected(event: any) {
    const params = this.orderService.getOrderParams()
    params.orderStatus = event.target.value
    this.orderService.setOrderParams(params)
    this.orderParams = params
    this.getUserOrders()
  }
}
