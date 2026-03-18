import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrderRoutingModule } from './order-routing.module';
import { OrderComponent } from './order.component';


@NgModule({
  declarations: [
    OrderComponent,
    OrderDetailsComponent
  ],
  imports: [
    CommonModule,
    OrderRoutingModule,
    SharedModule
  ]
})
export class OrderModule { }
