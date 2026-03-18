import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookDetailsComponent } from './book-details/book-details.component';
import { BookcaseComponent } from './bookcase.component';

const routes: Routes = [
  {
    path: '',
    component: BookcaseComponent
  },
  {
    path: ':id',
    component: BookDetailsComponent,
    data: { breadcrumb: { alias: 'bookDetails' } }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookRoutingModule { }
