import { Component } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-admin-inventory',
  templateUrl: './admin-inventory.component.html',
  styleUrls: ['./admin-inventory.component.css']
})
export class AdminInventoryComponent {

  innerSelectedTabIndex: number = 0

  constructor () { }

  onInnerTabChange(event: MatTabChangeEvent) {
    this.innerSelectedTabIndex = event.index
  }
}
