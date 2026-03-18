import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { BookStore } from 'src/app/core/models/book-store.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { BookService } from 'src/app/core/services/book.service';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';
import { validateBookExist, validateBookInStore, validateBookISBN } from 'src/app/shared/helpers/validates/validate-exist';
import { validateQuantityInStore } from 'src/app/shared/helpers/validates/validate-inventory-inputs';

@Component({
  selector: 'app-add-export-receipt-form',
  templateUrl: './add-export-receipt-form.component.html',
  styleUrls: ['./add-export-receipt-form.component.css']
})
export class AddExportReceiptFormComponent implements OnInit {

  addExportReceiptForm!: FormGroup
  headerColumns: string[] = ['Tên sách', 'ISBN', 'Hiệu sách', 'Số lượng', 'ĐVT', '']
  bookStoresList: BookStore[] = []
  measureUnitsList: UnitOfMeasure[] = []
  availableQuantity$ = new BehaviorSubject<number>(0)
  convertedQuantity$ = new BehaviorSubject<number>(0)

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bookStoreService: BookstoreService,
    private measureUnitService: UnitOfMeasureService,
    private bookService: BookService,
    private inventoryService: InventoryService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<AddExportReceiptFormComponent>
  ) { }

  ngOnInit(): void {
    this.loadAllBookStores()
    this.loadAllMeasureUnits()
    this.initializeForm()
  }

  initializeForm() {
    this.addExportReceiptForm = this.fb.group({
      exportNotes: ['', Validators.maxLength(100)],
      items: this.fb.array([], [Validators.required])
    })
    this.addItem()
  }

  createFormGroup(): FormGroup {
    return this.fb.group({
      bookTitle: ['', [Validators.required], [validateBookExist(this.bookService)]],
      isbn: ['', [Validators.required], [validateBookISBN(this.bookService)]],
      unitOfMeasureId: ['', [Validators.required]],
      quantity: [0, [Validators.required, Validators.min(0)], [validateQuantityInStore(this.inventoryService, this.availableQuantity$, this.convertedQuantity$)]],
      bookStoreId: ['', [Validators.required], [validateBookInStore(this.bookService)]]
    })
  }

  addItem() {
    this.items.push(this.createFormGroup())
  }

  get items(): FormArray {
    return this.addExportReceiptForm.get('items') as FormArray
  }

  removeFormGroup(index: number) {
    this.items.removeAt(index)
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

  submitForm() {
    if(this.addExportReceiptForm.valid) {
      console.log(this.addExportReceiptForm.value.items)
      // const addInvAuditRequest: AddInventoryAuditRequest = {
      //   audittedBy: this.addImportReceiptForm.value.audittedBy,
      //   auditDate: this.addImportReceiptForm.value.auditDate,
      //   auditNotes: this.addImportReceiptForm.value.auditNotes,
      //   inventoryAuditItems: this.addImportReceiptForm.value.items.map((item: InventoryAuditItem)  => ({
      //     bookTitle: item.bookTitle,
      //     isbn: item.isbn,
      //     bookStoreId: item.bookStoreId
      //   }))
      // }
      // this.inventoryService.addNewInventoryAudit(addInvAuditRequest).subscribe({
      //   next: (response) => {
      //     this.toastr.success(response.message)
      //     this.dialogRef.close({ success: true })
      //   },
      //   error: (error) => {
      //     console.log("Có lỗi: ", error)
      //     this.toastr.error(error.message)
      //     this.dialogRef.close()
      //   }
      // })
    }
  }
}
