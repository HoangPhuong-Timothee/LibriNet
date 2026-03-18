import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-admin-book-details',
  templateUrl: './admin-book-details.component.html',
  styleUrls: ['./admin-book-details.component.css']
})
export class AdminBookDetailsComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

}
