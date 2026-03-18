import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { InventoryTransaction } from 'src/app/core/models/inventory.model';
import { InventoryTransactionParams } from 'src/app/core/models/params.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';

@Component({
  selector: 'app-inventory-transaction-dialog',
  templateUrl: './inventory-transaction-dialog.component.html',
  styleUrls: ['./inventory-transaction-dialog.component.css']
})
export class InventoryTransactionDialogComponent implements OnInit {

  transactions: InventoryTransaction[] = []
  transactionTypes = [
    { name: 'Tất cả', value: '' },
    { name: 'Nhập kho', value: 'import' },
    { name: 'Xuất kho', value: 'export' }
  ]
  measureUnitsList: UnitOfMeasure[] = []
  invTranParams = new InventoryTransactionParams()
  selectedTransactionType: string = ''
  selectedMeasureUnitId: number | string = 0
  startDate?: Date
  endDate?: Date

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private toastr: ToastrService,
    private uomService: UnitOfMeasureService,
    private inventoryService: InventoryService
  ) { }

  ngOnInit(): void {
    this.loadInventoryTransactions(this.data.inventory.bookId, this.data.inventory.storeName)
    this.loadAllMeasureUnits()
  }

  loadInventoryTransactions(bookId: number, storeName: string) {
    this.invTranParams.startDate = this.startDate
    this.invTranParams.endDate = this.endDate
    this.invTranParams.transactionType = this.selectedTransactionType
    this.invTranParams.measureUnitId = this.selectedMeasureUnitId === '0' ? undefined : +(this.selectedMeasureUnitId)
    return this.inventoryService.getBookInventoryTransactions(bookId, storeName, this.invTranParams).subscribe({
      next: response => {
        this.transactions = response.map(transaction => ({
          ...transaction,
          showDetails: false
        }))
      },
      error: error => console.log("Có lỗi: ", error)
    })
  }

  loadAllMeasureUnits() {
    this.uomService.getAllUnitOfMeasures().subscribe({
      next: response => this.measureUnitsList = [{ id: 0, name: 'Tất cả' },...response],
      error: error => console.log("Có lỗi xảy ra: ", error)
    })
  }

  toggleShowDetails(transaction: InventoryTransaction) {
    transaction.showDetails = !transaction.showDetails
    const index = this.transactions.findIndex(t => t.transactionId === transaction.transactionId)
    if (index !== -1) {
      this.transactions[index] = { ...transaction }
    }
  }

  applyFilters() {
    if (this.startDate && this.endDate && this.startDate >= this.endDate) {
      this.toastr.error("Ngày bắt đầu phải nhỏ hơn ngày kết thúc")
      return
    }
    this.invTranParams.transactionType = this.selectedTransactionType
    this.invTranParams.measureUnitId = +(this.selectedMeasureUnitId)
    this.invTranParams.startDate = this.startDate
    this.invTranParams.endDate = this.endDate
    this.loadInventoryTransactions(this.data.inventory.bookId, this.data.inventory.storeName)
  }

  onReset() {
    if (this.selectedMeasureUnitId) {
      this.selectedMeasureUnitId = 0
    }
    if (this.selectedTransactionType) {
      this.selectedTransactionType = ''
    }
    if (this.startDate) {
      this.startDate = undefined
    }
    if (this.endDate) {

      this.endDate = undefined
    }
    this.invTranParams = new InventoryTransactionParams()
    this.loadInventoryTransactions(this.data.inventory.bookId, this.data.inventory.storeName)
  }

}
