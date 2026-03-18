import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BookStore } from 'src/app/core/models/book-store.model';
import { AddInventoryAuditRequest, InventoryAuditItem } from 'src/app/core/models/inventory.model';
import { BookService } from 'src/app/core/services/book.service';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { validateBookExist, validateBookInStore, validateBookISBN } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-inventory-audit-form',
  templateUrl: './add-inventory-audit-form.component.html',
  styleUrls: ['./add-inventory-audit-form.component.css']
})
export class AddInventoryAuditFormComponent implements OnInit {

  addInventoryAuditForm!: FormGroup
  headerColumns: string[] = ['Tên sách', 'ISBN', 'Hiệu sách', 'Số lượng', 'ĐVT', '']
  bookStoresList: BookStore[] = []

  constructor(
   @Inject(MAT_DIALOG_DATA) public data: any,
    private bookStoreService: BookstoreService,
    private bookService: BookService,
    private inventoryService: InventoryService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<AddInventoryAuditFormComponent>
  ) { }

  ngOnInit(): void {
    this.loadAllBookStores()
    this.initializeForm()
  }

  initializeForm() {
    this.addInventoryAuditForm = this.fb.group({
      audittedBy: ['', [Validators.required]],
      auditDate: ['', [Validators.required]],
      auditNotes: ['', [Validators.required, Validators.maxLength(255)]],
      items: this.fb.array([], [Validators.required])
    })
    this.addItem()
  }

  loadAllBookStores() {
    this.bookStoreService.getAllBookStores().subscribe({
      next: response => this.bookStoresList = response,
      error: error => console.log("Có lỗi xảy ra: ", error)
    })
  }

  createFormGroup(): FormGroup {
    return this.fb.group({
      bookTitle: ['', [Validators.required], [validateBookExist(this.bookService)]],
      isbn: ['', [Validators.required], [validateBookISBN(this.bookService)]],
      bookStoreId: ['', [Validators.required], [validateBookInStore(this.bookService)]]
    })
  }

  get items(): FormArray {
    return this.addInventoryAuditForm.get('items') as FormArray
  }

  addItem() {
    this.items.push(this.createFormGroup())
  }

  removeFormGroup(index: number) {
    this.items.removeAt(index)
  }

  submitForm() {
    if(this.addInventoryAuditForm.valid) {
      const addInvAuditRequest: AddInventoryAuditRequest = {
        audittedBy: this.addInventoryAuditForm.value.audittedBy,
        auditDate: this.addInventoryAuditForm.value.auditDate,
        auditNotes: this.addInventoryAuditForm.value.auditNotes,
        inventoryAuditItems: this.addInventoryAuditForm.value.items.map((item: InventoryAuditItem)  => ({
          bookTitle: item.bookTitle,
          isbn: item.isbn,
          bookStoreId: item.bookStoreId
        }))
      }
      this.inventoryService.addNewInventoryAudit(addInvAuditRequest).subscribe({
        next: (response) => {
          this.toastr.success(response.message)
          this.dialogRef.close({ success: true })
        },
        error: (error) => {
          console.log("Có lỗi: ", error)
          this.toastr.error(error.message)
          this.dialogRef.close()
        }
      })
    }
  }
}
