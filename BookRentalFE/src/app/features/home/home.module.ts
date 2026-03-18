import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { BookModule } from '../book/book.module';
import { SharedModule } from "../../shared/shared.module";

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    BookModule,
    SharedModule
]
})
export class HomeModule { }
