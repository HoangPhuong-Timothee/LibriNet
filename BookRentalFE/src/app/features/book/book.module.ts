import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookRoutingModule } from './book-routing.module';
import { BookcaseComponent } from './bookcase.component';
import { BookDetailsComponent } from './book-details/book-details.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    BookcaseComponent,
    BookDetailsComponent
  ],
  imports: [
    CommonModule,
    BookRoutingModule,
    FormsModule,
    SharedModule
  ]
})
export class BookModule { }
