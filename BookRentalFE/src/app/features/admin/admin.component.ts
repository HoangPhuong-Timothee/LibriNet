import { Component, OnInit } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  selectedTabIndex: number = 0

  constructor() { }

  ngOnInit(): void {
    const savedTabIndex = localStorage.getItem('selectedTabIndex')
    if (savedTabIndex !== null) {
      this.selectedTabIndex = +savedTabIndex
    }
  }

  onTabChange(event: MatTabChangeEvent) {
    this.selectedTabIndex = event.index
    localStorage.setItem('selectedTabIndex', this.selectedTabIndex.toString());
  }
}
