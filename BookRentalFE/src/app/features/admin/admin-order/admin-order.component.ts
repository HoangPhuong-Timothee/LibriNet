import { Component } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-admin-order',
  templateUrl: './admin-order.component.html',
  styleUrls: ['./admin-order.component.css']
})
export class AdminOrderComponent {

  innerSelectedTabIndex: number = 0

  constructor () { }

  onInnerTabChange(event: MatTabChangeEvent) {
    this.innerSelectedTabIndex = event.index
  }
}
