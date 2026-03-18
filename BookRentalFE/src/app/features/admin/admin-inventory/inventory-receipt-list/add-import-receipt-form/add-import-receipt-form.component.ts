import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BookStore } from 'src/app/core/models/book-store.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { BookService } from 'src/app/core/services/book.service';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';
import { validateBookExist, validateBookInStore, validateBookISBN } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-import-receipt-form',
  templateUrl: './add-import-receipt-form.component.html',
  styleUrls: ['./add-import-receipt-form.component.css']
})
export class AddImportReceiptFormComponent implements OnInit {

  addImportReceiptForm!: FormGroup
  headerColumns: string[] = ['Tên sách', 'ISBN', 'Hiệu sách', 'Số lượng', 'ĐVT', '']
  bookStoresList: BookStore[] = []
  measureUnitsList: UnitOfMeasure[] = []

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bookStoreService: BookstoreService,
    private measureUnitService: UnitOfMeasureService,
    private bookService: BookService,
    private inventoryService: InventoryService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<AddImportReceiptFormComponent>
  ) { }

  ngOnInit(): void {
    this.initializeForm()
    this.loadAllBookStores()
    this.loadAllMeasureUnits()
  }

  initializeForm() {
    this.addImportReceiptForm = this.fb.group({
      exportNotes: ['', [Validators.required, Validators.maxLength(100)]],
      items: this.fb.array([], [Validators.required])
    })
    this.addItem()
  }

  createFormGroup(): FormGroup {
    return this.fb.group({
      bookTitle: ['', [Validators.required], [validateBookExist(this.bookService)]],
      isbn: ['', [Validators.required], [validateBookISBN(this.bookService)]],
      unitOfMeasureId: ['', [Validators.required]],
      quantity: [0, [Validators.required, Validators.min(0)]],
      bookStoreId: ['', [Validators.required], [validateBookInStore(this.bookService)]]
    })
  }

  get items(): FormArray {
    return this.addImportReceiptForm.get('items') as FormArray
  }

  addItem() {
    this.items.push(this.createFormGroup())
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
    if(this.addImportReceiptForm.valid) {
      console.log(this.addImportReceiptForm.value.items)
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
