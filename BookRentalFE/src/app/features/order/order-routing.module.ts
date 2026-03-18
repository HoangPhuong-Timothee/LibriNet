import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/guards/auth.guard';
import { CheckoutSuccessComponent } from '../checkout/checkout-success/checkout-success.component';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderComponent } from './order.component';

const routes: Routes = [
  {
    path: '',
    component: OrderComponent,
    canActivate: [AuthGuard]
  },
  {
    path: ':id',
    component: OrderDetailsComponent,
    canActivate: [AuthGuard],
    data: { breadcrumb: { alias: 'orderDetails' } }
  },
  {
    path: 'success',
    component: CheckoutSuccessComponent,
    data: { breadcrumb: 'Đặt đơn thành công' }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
