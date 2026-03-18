import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookStore } from 'src/app/core/models/book-store.model';
import { Genre } from 'src/app/core/models/genre.model';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { GenreService } from 'src/app/core/services/genre.service';

@Component({
  selector: 'app-invnetory-filter-dialog',
  templateUrl: './inventory-filter-dialog.component.html',
  styleUrls: ['./inventory-filter-dialog.component.css']
})
export class InventoryFilterDialogComponent implements OnInit {

  genresList: Genre[] = []
  bookStoresList: BookStore[] = []
  selectedGenreId: number = this.data.selectedGenreId
  selectedBookStoreId: number = this.data.selectedBookStoresId
  selectedInventoryStatus: string = this.data.selectedInventoryStatus
  inventoryStatusOptions = [
    { name: 'Tất cả', value: '' },
    { name: 'Bình thường', value: 'normal' },
    { name: 'Sắp hết hàng', value: 'soon' },
    { name: 'Hết hàng', value: 'out' }
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public genreService: GenreService,
    private bookStoreService: BookstoreService,
    private filterDialogRef: MatDialogRef<InventoryFilterDialogComponent>
  ) { }

  ngOnInit(): void {
      this.loadAllBookStores()
      this.loadAllGenres()
  }

  loadAllGenres() {
    this.genreService.getAllGenres().subscribe({
      next: response => this.genresList = response,
      error: error => console.log(error)
    })
  }

  loadAllBookStores() {
    this.bookStoreService.getAllBookStores().subscribe({
      next: response => this.bookStoresList = response,
      error: error => console.log(error)
    })
  }

  applyFilters() {
    this.filterDialogRef.close({
      selectedGenreId: this.selectedGenreId,
      selectedBookStoreId: this.selectedBookStoreId,
      selectedInventoryStatus: this.selectedInventoryStatus
    })
  }

}
