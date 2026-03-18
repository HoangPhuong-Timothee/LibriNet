import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { Order, OrderWithDetails } from 'src/app/core/models/order.model';
import { OrderParams } from 'src/app/core/models/params.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { OrderService } from 'src/app/core/services/order.service';
import { OrderDetailsDialogComponent } from './order-details-dialog/order-details-dialog.component';
import { OrderFilterDialogComponent } from './order-filter-dialog/order-filter-dialog.component';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {

  searchTerm: string = ''
  ordersList: Order[] = []
  orderWithDetails?: OrderWithDetails
  adminOrderParams: OrderParams
  totalOrders = 0
  columns = [
    { field: 'orderId', header: 'Mã đơn hàng' },
    { field: 'userEmail', header: 'Email đặt hàng' },
    { field: 'orderDate', header: 'Ngày đặt hàng' },
    { field: 'orderTotal', header: 'Tổng tiền', pipe: 'currency', pipeArgs: 'VND' },
    { field: 'paymentIntentId', header: 'Mã giỏ hàng' },
    {
      field: 'status',
      header: 'Trạng thái đơn hàng',
      class: (row: Order) => {
        if (row.status === 'Đang chờ xử lý')
          return 'rounded text-white p-2 bg-warning fw-bold border border-warning'
        if (row.status === 'Đã giao hàng')
          return 'rounded text-white p-2 bg-success fw-bold border border-success'
        if (row.status === 'Đơn hàng bị từ chối')
          return 'rounded text-white p-2 bg-danger fw-bold border border-danger'
        return ''
      }
    },
    { field: 'paymentMethod', header: 'Thanh toán' },
  ]
  actions = [
    {
      label: 'Xem chi tiết đơn hàng',
      icon: 'visibility',
      tooltip: 'Xem thông tin chi tiết đơn hàng',
      action: (row: Order) => {
        this.openOrderDetailsDialog(row)
      }
    },
    {
      label: 'Đã giao hàng',
      icon: 'check_circle',
      tooltip: 'Xác nhận đã giao hàng',
      action: (row: Order) => {
        this.acceptOrder(row)
      }
    },
    {
      label: 'Từ chối đơn hàng',
      icon: 'cancel',
      tooltip: 'Xác nhận từ chối đơn hàng',
      action: (row: Order) => {
        this.declineOrder(row)
      }
    }
  ]

  constructor(
    private orderService: OrderService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private dialogService: DialogService
  ) { this.adminOrderParams = orderService.getAdminOrderParams() }

  ngOnInit(): void {
    this.getUserOrdersForAdmin()
  }

  getUserOrdersForAdmin() {
    this.orderService.getUserOrdersForAdmin().subscribe({
      next: response => {
        this.ordersList = response.data
        this.totalOrders = response.count
      },
      error: error => {
        console.log("Có lỗi: ", error)
      }
    })
  }
  openFilterDialog() {
    let dialog = this.dialog.open(OrderFilterDialogComponent, {
      minWidth: '500px',
      data: {
        title: 'Lọc dữ liệu',
        selectedOrderStatus: this.adminOrderParams.orderStatus,
        selectedStartDate: this.adminOrderParams.startDate,
        selectedEndDate: this.adminOrderParams.endDate,
        selectedOrderEmail: this.adminOrderParams.orderEmail
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          const params = this.orderService.getAdminOrderParams()
          params.orderStatus = result.selectedOrderStatus
          params.startDate = result.selectedStartDate
          params.endDate = result.selectedEndDate
          params.orderEmail = result.selectedOrderEmail
          params.pageIndex = 1
          this.orderService.setAdminOrderParams(params)
          this.adminOrderParams = params
          this.getUserOrdersForAdmin()
        }
      }
    })
  }

  openOrderDetailsDialog(order: Order) {
    this.orderService.getOrderDetails(order.orderId).subscribe({
      next: (response) => {
        console.log(response)
        this.orderWithDetails = response
        this.dialog.open(OrderDetailsDialogComponent, {
          minWidth: '1500px',
          data: {
            title: `Thông tin đơn hàng #${order.orderId} - ${order.userEmail}`,
            orderWithDetails: this.orderWithDetails
          }
        })
      },
      error: (error) => {
        console.error('Có lỗi xảy ra:', error)
      }
    })
  }

  async acceptOrder(order: Order) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN TRẠNG THÁI ĐƠN HÀNG',
      `Bạn chắc chắn muốn xác nhận đơn hàng "${order.orderId}" đã được giao thành công?`
    )
    if (confirmed) {
      this.orderService.acceptOrder(order.orderId, order.userEmail).subscribe({
        next: (response) => {
          this.toastr.success(response.message)
          this.getUserOrdersForAdmin()
        },
        error: (error) => {
          console.error('Có lỗi:', error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  async declineOrder(order: Order) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN TRẠNG THÁI ĐƠN HÀNG',
      `Bạn chắc chắn muốn từ chối đơn hàng "${order.orderId}"?`
    )
    if (confirmed) {
      this.orderService.declineOrder(order.orderId, order.userEmail).subscribe({
        next: response => {
          this.toastr.success(response.message)
          this.getUserOrdersForAdmin()
        },
        error: error => {
          console.error('Có lỗi:', error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  onReset() {
    this.adminOrderParams = new OrderParams()
    this.orderService.setAdminOrderParams(this.adminOrderParams)
    this.getUserOrdersForAdmin()
  }

  onPageChange(event: PageEvent) {
    const params = this.orderService.getAdminOrderParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.orderService.setAdminOrderParams(params)
    this.adminOrderParams = params
    this.getUserOrdersForAdmin()
  }
}
