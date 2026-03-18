import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-order-details-dialog',
  templateUrl: './order-details-dialog.component.html',
  styleUrls: ['./order-details-dialog.component.css']
})
export class OrderDetailsDialogComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  get userInfo() {
    return this.data.orderWithDetails?.fullName + ", "
      + this.data.orderWithDetails?.street + ", "
      + this.data.orderWithDetails?.ward + ", "
      + this.data.orderWithDetails?.district + ", "
      + this.data.orderWithDetails?.city + ", "
      + this.data.orderWithDetails?.postalCode
  }

}
