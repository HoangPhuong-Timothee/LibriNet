import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { Book } from 'src/app/core/models/book.model';
import { BookParams } from 'src/app/core/models/params.model';
import { BookService } from 'src/app/core/services/book.service';
import { DialogService } from 'src/app/core/services/dialog.service';
import { AddBooksFormComponent } from './add-books-form/add-books-form.component';
import { AdminBookDetailsComponent } from './admin-book-details/admin-book-details.component';
import { BookFilterDialogComponent } from './book-filter-dialog/book-filter-dialog.component';
import { ImportBooksFormComponent } from './import-books-form/import-books-form.component';

@Component({
  selector: 'app-admin-book',
  templateUrl: './admin-book.component.html',
  styleUrls: ['./admin-book.component.css'],
})
export class AdminBookComponent implements OnInit {

  book?: Book
  searchTerm: string = ''
  bookList: Book[] = []
  adminBookParams: BookParams
  totalBooks = 0
  columns = [
    { field: 'id', header: 'Mã sách' },
    { field: 'isbn', header: 'ISBN' },
    { field: ['title', 'imageUrl'], header: 'Sách', haveImage: true },
    { field: 'author', header: 'Tác giả' },
    { field: 'genre', header: 'Thể loại' },
    { field: 'publisher', header: 'NXB' },
    { field: 'price', header: 'Giá', pipe: 'currency', pipeArgs: 'VND' }
  ]
  actions = [
    {
      label: 'Xem sách',
      icon: 'visibility',
      tooltip: 'Xem thông tin chi tiết sách',
      action: (row: any) => {
        this.openBookDetailsDialog(row.id)
      },
    },
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa sách khỏi hệ thống',
      action: (row: any) => {
        this.openDeleteBookDialog(row)
      },
    }
  ]

  constructor(
    private bookService: BookService,
    private dialog: MatDialog,
    private dialogService: DialogService,
    private toastr: ToastrService
  )
  {
    this.adminBookParams = bookService.getAdminBookParams()
  }

  ngOnInit(): void {
    this.adminBookParams.search = ''
    this.getAllBooksForAdmin()
  }

  getAllBooksForAdmin() {
    this.bookService.getAllBooksForAdmin().subscribe({
      next: response => {
        this.bookList = response.data
        this.totalBooks = response.count
      },
      error: error => {
        console.log("Có lỗi: ", error)
      }
    })
  }

  openAddNewBookDialog() {
    const dialog = this.dialog.open(AddBooksFormComponent, {
      minWidth: '500px',
      maxHeight: '500px',
      autoFocus: true,
      data: {
        title: 'Thêm sách'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.success) {
          const params = this.bookService.getAdminBookParams()
          params.pageIndex = 1
          this.bookService.setAdminBookParams(params)
          this.adminBookParams = params
          this.getAllBooksForAdmin()
        }
      }
    })
  }

  openImportBooksDialog() {
    const dialog = this.dialog.open(ImportBooksFormComponent, {
      minWidth: '200px',
      data: {
        title: 'Nhập dữ liệu sách'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSuccesss) {
          const params = this.bookService.getAdminBookParams()
          params.pageIndex = 1
          this.bookService.setAdminBookParams(params)
          this.adminBookParams = params
          this.getAllBooksForAdmin()
        }
      }
    })
  }

  openBookDetailsDialog(id: number) {
    this.bookService.getSingleBook(id).subscribe({
      next: (response) => {
        this.book = response
        this.dialog.open(AdminBookDetailsComponent, {
          minWidth: '300px',
          maxWidth: '1000px',
          minHeight: '200px',
          maxHeight: '580px',
          data: {
            title: `Thông tin sách ${this.book.title}`,
            book: this.book
          }
        })
      },
      error: (error) => {
        console.error('Có lỗi xảy ra:', error)
      }
    })
  }

  async openDeleteBookDialog(book: Book) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN XÓA',
      `Bạn chắc chắn muốn xóa sách "${book.title}"?`
    )
    if (confirmed) {
      this.bookService.deleteBook(book.id).subscribe({
        next: () => {
          this.bookList = this.bookList.filter(b => b.id !== book.id)
          this.toastr.success(`Xóa sách "${book.title}" thành công`)
        },
        error: error => {
          console.log("Có lỗi xảy ra: ", error)
        }
      })
    }
  }

  openFilterDialog() {
    let dialog = this.dialog.open(BookFilterDialogComponent, {
      minWidth: '500px',
      data: {
        selectedGenreId: this.adminBookParams.genreId,
        selectedPublisherId: this.adminBookParams.publisherId,
        selectedAuthorId: this.adminBookParams.authorId
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          const params = this.bookService.getAdminBookParams()
          params.genreId = result.selectedGenreId
          params.publisherId = result.selectedPublisherId
          params.authorId = result.selectedAuthorId
          params.pageIndex = 1
          this.bookService.setAdminBookParams(params)
          this.adminBookParams = params
          this.getAllBooksForAdmin()
        }
      }
    })
  }

  onPageChange(event: PageEvent) {
    const params = this.bookService.getAdminBookParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.bookService.setAdminBookParams(params)
    this.adminBookParams = params
    this.getAllBooksForAdmin()
  }

  onSearch() {
    const params = this.bookService.getAdminBookParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.bookService.setAdminBookParams(params)
    this.adminBookParams = params
    this.getAllBooksForAdmin()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.adminBookParams = new BookParams()
    this.bookService.setAdminBookParams(this.adminBookParams)
    this.getAllBooksForAdmin()
  }

}
