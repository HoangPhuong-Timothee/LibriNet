import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Book } from 'src/app/core/models/book.model';
import { BasketService } from 'src/app/core/services/basket.service';

@Component({
  selector: 'app-book-item',
  templateUrl: './book-item.component.html',
  styleUrls: ['./book-item.component.css']
})
export class BookItemComponent {

  @Input() book?: Book

  constructor(
    private basketService: BasketService,
    private toastr: ToastrService
  ) { }

  addItemToBasket() {
    this.book && this.basketService.addItemToBasket(this.book)
    this.toastr.success('Đã thêm sách vào giỏ')
  }
}
