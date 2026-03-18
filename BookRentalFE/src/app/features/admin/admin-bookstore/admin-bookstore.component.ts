import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { BookStore } from 'src/app/core/models/book-store.model';
import { BookStoreParams } from 'src/app/core/models/params.model';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { DialogService } from 'src/app/core/services/dialog.service';
import { AddBookstoreFormComponent } from './add-bookstore-form/add-bookstore-form.component';
import { EditBookstoreFormComponent } from './edit-bookstore-form/edit-bookstore-form.component';
import { ImportBookstoreFormComponent } from './import-bookstore-form/import-bookstore-form.component';

@Component({
  selector: 'app-admin-bookstore',
  templateUrl: './admin-bookstore.component.html',
  styleUrls: ['./admin-bookstore.component.css']
})
export class AdminBookstoreComponent implements OnInit {

  searchTerm: string = ''
  bookStoreList: BookStore[] = []
  adminBookStoreParams: BookStoreParams
  totalBookStores: number = 0
  columns = [
    { field: 'id', header: 'Mã hiệu sách' },
    { field: 'storeName' , header: 'Hiệu sách' },
    { field: 'storeAddress', header: 'Địa chỉ' },
    { field: 'totalQuantity', header: 'Tổng số sách' },
    {
      field: 'createInfo',
      header: 'Tạo bởi',
      class: () => {
        return 'fst-italic'
      }
    },
    {
      field: 'updateInfo',
      header: 'Cập nhật bởi',
      class: () => {
        return 'fst-italic'
      }
    }
  ]
  actions = [
    {
      label: 'Cập nhật',
      icon: 'edit',
      tooltip: 'Chỉnh sửa thông tin hiệu sách',
      action: (row: any) => {
        this.openUpdateBookStoreDialog(row)
      }
    },
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa hiệu sách',
      action: (row: any) => {
        this.openDeleteBookStoreDialog(row)
      }
    }
  ]

  constructor(
    private bookStoreService: BookstoreService,
    private dialog: MatDialog,
    private dialogService: DialogService,
    private toastr: ToastrService
  ) { this.adminBookStoreParams = bookStoreService.getBookStoreParams() }

  ngOnInit(): void {
    this.getAllBookStoresForAdmin()
  }

  getAllBookStoresForAdmin() {
    this.bookStoreService.getAllBookStoresForAdmin().subscribe({
      next: response => {
        this.bookStoreList = response.data
        this.totalBookStores = response.count
      },
      error: error => {
        console.log("Có lỗi: ", error)
      }
    })
  }

  openAddNewBookStoreDialog() {
    const dialog = this.dialog.open(AddBookstoreFormComponent, {
      minWidth: '500px',
      data: {
          title: 'Thêm hiệu sách'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.success) {
          const params = this.bookStoreService.getBookStoreParams()
          params.pageIndex = 1
          this.bookStoreService.setBookStoreParams(params)
          this.adminBookStoreParams = params
          this.getAllBookStoresForAdmin()
        }
      }
    })
  }

  openImportBookStoreDialog() {
    const dialog = this.dialog.open(ImportBookstoreFormComponent, {
      minWidth: '200px',
      data: {
          title: 'Nhập dữ liệu hiệu sách'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSuccess) {
          const params = this.bookStoreService.getBookStoreParams()
          params.pageIndex = 1
          this.bookStoreService.setBookStoreParams(params)
          this.adminBookStoreParams = params
          this.getAllBookStoresForAdmin()
        }
      }
    })
  }

  openUpdateBookStoreDialog(bookStore: BookStore) {
    const dialog = this.dialog.open(EditBookstoreFormComponent, {
      minWidth: '500px',
      data: {
        title: 'Chỉnh sửa thông tin hiệu sách',
        bookStore
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          if (result && result.success) {
            const params = this.bookStoreService.getBookStoreParams()
            params.pageIndex = 1
            this.bookStoreService.setBookStoreParams(params)
            this.adminBookStoreParams = params
            this.getAllBookStoresForAdmin()
          }
        }
      }
    })
  }

  async openDeleteBookStoreDialog(bookStore: BookStore) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN XÓA',
      `Bạn chắc chắn muốn xóa hiệu sách "${bookStore.storeName}"?`
    )
    if (confirmed) {
      this.bookStoreService.deleteBookStore(bookStore.id).subscribe({
        next: async () => {
          this.bookStoreList = this.bookStoreList.filter(bs => bs.id !== bs.id)
          this.toastr.success(`Xóa hiệu sách "${bookStore.storeName}" thành công`)
        },
        error: error => {
          console.log("Có lỗi xảy ra: ", error)
        }
      })
    }
  }

  onPageChange(event: PageEvent) {
    const params = this.bookStoreService.getBookStoreParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.bookStoreService.setBookStoreParams(params)
    this.adminBookStoreParams = params
    this.getAllBookStoresForAdmin()
  }

  onSearch() {
    const params = this.bookStoreService.getBookStoreParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.bookStoreService.setBookStoreParams(params)
    this.adminBookStoreParams = params
    this.getAllBookStoresForAdmin()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.adminBookStoreParams = new BookStoreParams()
    this.bookStoreService.setBookStoreParams(this.adminBookStoreParams)
    this.getAllBookStoresForAdmin()
  }

}
