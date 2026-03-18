import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { BookStore } from 'src/app/core/models/book-store.model';
import { ExportInventoriesRequest } from 'src/app/core/models/inventory.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { BookService } from 'src/app/core/services/book.service';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';
import { validateBookExist, validateBookISBN, validateBookInStore } from 'src/app/shared/helpers/validates/validate-exist';
import { validatePastDate, validateQuantityInStore } from 'src/app/shared/helpers/validates/validate-inventory-inputs';

@Component({
  selector: 'app-export-inventories-manual-form',
  templateUrl: './export-inventories-manual-form.component.html',
  styleUrls: ['./export-inventories-manual-form.component.css']
})
export class ExportInventoriesManualFormComponent implements OnInit {

  exportForm!: FormGroup
  headerColumns: string[] = ['Tên sách', 'ISBN', 'Hiệu sách', 'Số lượng', 'ĐVT', 'Ngày xuất kho', 'Ghi chú', '']
  bookStoresList: BookStore[] = []
  measureUnitsList: UnitOfMeasure[] = []
  availableQuantity$ = new BehaviorSubject<number>(0)
  convertedQuantity$ = new BehaviorSubject<number>(0)

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bookStoreService: BookstoreService,
    private bookService: BookService,
    private measureUnitService: UnitOfMeasureService,
    private inventoryService: InventoryService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<ExportInventoriesManualFormComponent>
  ) { }

  ngOnInit(): void {
    this.loadAllBookStores()
    this.loadAllMeasureUnits()
    this.initializeForm()
  }

  initializeForm() {
    this.exportForm = this.fb.group({
      rows: this.fb.array([], [Validators.required])
    })
    this.addRow()
  }

  loadAllBookStores() {
    this.bookStoreService.getAllBookStores().subscribe({
      next: response => this.bookStoresList = response,
      error: error => console.log("Có lỗi xảy ra: ", error)
    })
  }

  loadAllMeasureUnits() {
    this.measureUnitService.getAllUnitOfMeasures().subscribe({
      next: response => this.measureUnitsList = response,
      error: error => console.log("Có lỗi xảy ra: ", error)
    })
  }

  createFormGroup(): FormGroup {
    return this.fb.group({
      bookTitle: ['', [Validators.required], [validateBookExist(this.bookService)]],
      isbn: ['', [Validators.required], [validateBookISBN(this.bookService)]],
      unitOfMeasureId: ['', [Validators.required]],
      quantity: [0, [Validators.required, Validators.min(0)], [validateQuantityInStore(this.inventoryService, this.availableQuantity$, this.convertedQuantity$)]],
      bookStoreId: ['', [Validators.required], [validateBookInStore(this.bookService)]],
      exportDate: ['', [Validators.required, validatePastDate()]],
      exportNotes: ['', [Validators.required, Validators.maxLength(100)]]
    })
  }

  addRow() {
    this.rows.push(this.createFormGroup())
  }

  get rows(): FormArray {
    return this.exportForm.get('rows') as FormArray
  }

  removeFormGroup(index: number) {
    this.rows.removeAt(index)
  }

  submitForm() {
    if (this.exportForm.valid) {
        const requestData: ExportInventoriesRequest[] = this.exportForm.value.rows
        this.inventoryService.exportInventoriesManual(requestData).subscribe({
          next: (response) => {
            this.toastr.success(response.message)
            this.dialogRef.close({ exportSuccess: true })
          },
          error: (error) => {
            console.log("Có lỗi xảy ra: ", error)
            this.toastr.error(error.message)
          }
      })
    } else {
      this.toastr.warning("Dữ liệu nhập vào không hợp lệ")
    }
  }
}
