import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable, of, tap } from "rxjs";
import { environment } from "src/environments/environment";
import { AddExportReceiptRequest, AddImportReceiptRequest, AddInventoryAuditRequest, BookQuantity, ConductInventoryRequest, Inventory, InventoryAudit, InventoryAuditDetails, InventoryReceipt, InventoryTransaction } from "../models/inventory.model";
import { Pagination } from "../models/pagination.model";
import { InventoryAuditParams, InventoryParams, InventoryTransactionParams, ReceiptParams, ValidateBookQuantityInBookStoreParams } from "../models/params.model";

@Injectable({
    providedIn: 'root'
})
export class InventoryService {

    inventories: Inventory[] = []
    inventoryPagination?: Pagination<Inventory[]>
    inventoryParams = new InventoryParams()
    inventoryCache = new Map<string, Pagination<Inventory[]>>()

    invAuditList: InventoryAudit[] = []
    invAuditPagination?: Pagination<InventoryAudit[]>
    invAuditParams = new InventoryAuditParams()
    invAuditCache = new Map<string, Pagination<InventoryAudit[]>>()

    receiptList: InventoryReceipt[] = []
    receiptPagination?: Pagination<InventoryReceipt[]>
    receiptParams = new ReceiptParams()
    receiptCache = new Map<string, Pagination<InventoryReceipt[]>>()

    constructor(private http: HttpClient) { }

    //Book inventory service
    getAllBookInventories(useCache = true): Observable<Pagination<Inventory[]>> {
      if (!useCache) {
        this.inventoryCache = new Map()
      }
      if (this.inventoryCache.size > 0 && useCache) {
        if (this.inventoryCache.has(Object.values(this.inventoryParams).join('-'))) {
          this.inventoryPagination = this.inventoryCache.get(Object.values(this.inventoryParams).join('-'))

          if(this.inventoryPagination) {
            return of(this.inventoryPagination)
          }
        }
      }
      let params = new HttpParams()
      if (this.inventoryParams.isbnSearch)
        params = params.append('isbn', this.inventoryParams.isbnSearch)
      if (this.inventoryParams.search)
        params = params.append('search', this.inventoryParams.search)
      if (this.inventoryParams.inventoryStatus)
        params = params.append('inventoryStatus', this.inventoryParams.inventoryStatus)
      if (this.inventoryParams.genreId)
        params = params.append('genreId', this.inventoryParams.genreId)
      if (this.inventoryParams.bookStoreId)
        params = params.append('bookStoreId', this.inventoryParams.bookStoreId)
      params = params.append('pageIndex', this.inventoryParams.pageIndex)
      params = params.append('pageSize', this.inventoryParams.pageSize)
      return this.http.get<Pagination<Inventory[]>>(`${environment.baseAPIUrl}/api/Inventories`, { params }).pipe(
        map(response => {
          this.inventories = [...this.inventories, ...response.data]
          this.inventoryPagination = response
          return response
        })
      )
    }

    setInventoryParams(params: InventoryParams) {
      this.inventoryParams = params
    }

    getInventoryParams() {
      return this.inventoryParams
    }

    getConvertedAndRemainingQuatity(validateParams: ValidateBookQuantityInBookStoreParams) {
      let params = new HttpParams()
        .set('bookTitle', validateParams.bookTitle)
        .set('bookStoreId', validateParams.bookStoreId)
        .set('unitOfMeasureId', validateParams.unitOfMeasureId)
        .set('inputQuantity', validateParams.inputQuantity)
        .set('isbn', validateParams.isbn)
      return this.http.get<BookQuantity>(`${environment.baseAPIUrl}/api/Inventories/quantity`, { params })
    }

    getBookInventoryTransactions(bookId: number, storeName: string, invTranParams: InventoryTransactionParams) {
      let params = new HttpParams()
        .set('bookId', bookId)
        .set('storeName', storeName)
      if (invTranParams.transactionType) params = params.append('transactionType', invTranParams.transactionType)
      if (invTranParams.measureUnitId) params = params.append('measureUnitId', invTranParams.measureUnitId)
      if (invTranParams.startDate) params = params.append('startDate', new Date(invTranParams.startDate).toDateString())
      if (invTranParams.endDate) params = params.append('endDate', new Date(invTranParams.endDate).toDateString())
      return this.http.get<InventoryTransaction[]>(`${environment.baseAPIUrl}/api/Inventories/inventory-transactions`, { params })
    }

    //Book receipt service
    getAllInventoryReceipts(useCache = true): Observable<Pagination<InventoryReceipt[]>> {
      if (!useCache) {
        this.receiptCache = new Map()
      }
      if (this.receiptCache.size > 0 && useCache) {
        if (this.receiptCache.has(Object.values(this.receiptParams).join('-'))) {
          this.receiptPagination = this.receiptCache.get(Object.values(this.receiptParams).join('-'))

          if(this.receiptPagination) {
            return of(this.receiptPagination)
          }
        }
      }
      let params = new HttpParams()
      if (this.receiptParams.receiptStatus)
        params = params.append('receiptStatus', this.receiptParams.receiptStatus)
      if (this.receiptParams.search)
        params = params.append('search', this.receiptParams.search)
      if (this.invAuditParams.startDate)
        params = params.append('startDate', new Date(this.invAuditParams.startDate).toDateString())
      if (this.invAuditParams.endDate)
        params = params.append('endDate', new Date(this.invAuditParams.endDate).toDateString())
      if (this.receiptParams.receiptType)
        params = params.append('receiptType', this.receiptParams.receiptType)
      params = params.append('pageIndex', this.inventoryParams.pageIndex)
      params = params.append('pageSize', this.inventoryParams.pageSize)
      return this.http.get<Pagination<InventoryReceipt[]>>(`${environment.baseAPIUrl}/api/Inventories/admin/receipt-list`, { params }).pipe(
        map(response => {
          this.receiptList = [...this.receiptList, ...response.data]
          this.receiptPagination = response
          return response
        })
      )
    }

    getInventoryReceiptParams() {
      return this.receiptParams
    }

    setInventoryReceiptParams(params: ReceiptParams) {
      this.receiptParams = params
    }

    importInventoriesFromFile(file: File): Observable<any> {
      const formData = new FormData()
      formData.append('file', file, file.name)
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/import/file`, formData).pipe(
        tap(() => {
          this.inventoryCache = new Map()
        })
      )
    }

    exportInventoriesFromFile(file: File): Observable<any> {
      const formData = new FormData()
      formData.append('file', file, file.name)
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/export/file`, formData).pipe(
        tap(() => {
          this.inventoryCache = new Map()
        })
      )
    }

    importInventoriesManual(request: AddImportReceiptRequest): Observable<any> {
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/import/manual`, request).pipe(
        tap(() => {
          this.inventoryCache = new Map()
        })
      )
    }

    exportInventoriesManual(request: AddExportReceiptRequest): Observable<any> {
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/export/manual`, request).pipe(
        tap(() => {
          this.inventoryCache = new Map()
        })
      )
    }

    acceptReceipt(receiptId: number, receiptType: string): Observable<any> {
      let params = new HttpParams().set('receiptType', receiptType)
      return this.http.put(`${environment.baseAPIUrl}/api/Inventories/admin/receipt-list/accept/${receiptId}`, { params }).pipe(
        tap(() => {
          this.receiptCache = new Map()
        })
      )
    }

    cancelReceipt(receiptId: number, receiptType: string): Observable<any> {
      let params = new HttpParams().set('receiptType', receiptType)
      return this.http.put(`${environment.baseAPIUrl}/api/Inventories/admin/receipt-list/cancel/${receiptId}`, { params }).pipe(
        tap(() => {
          this.receiptCache = new Map()
        })
      )
    }

    getAllInventoryAudits(useCache = true): Observable<Pagination<InventoryAudit[]>> {
      if (!useCache) {
        this.invAuditCache = new Map()
      }
      if (this.invAuditCache.size > 0 && useCache) {
        if (this.invAuditCache.has(Object.values(this.invAuditParams).join('-'))) {
          this.invAuditPagination = this.invAuditCache.get(Object.values(this.invAuditParams).join('-'))

          if(this.invAuditPagination) {
            return of(this.invAuditPagination)
          }
        }
      }
      let params = new HttpParams()
      if (this.invAuditParams.audittedBy) params = params.append('audittedBy', this.invAuditParams.audittedBy)
      if (this.invAuditParams.search) params = params.append('search', this.invAuditParams.search)
      if (this.invAuditParams.startDate) params = params.append('startDate', new Date(this.invAuditParams.startDate).toDateString())
      if (this.invAuditParams.endDate) params = params.append('endDate', new Date(this.invAuditParams.endDate).toDateString())
      if (this.invAuditParams.auditStatus) params = params.append('auditStatus', this.invAuditParams.auditStatus)
      params = params.append('pageIndex', this.inventoryParams.pageIndex)
      params = params.append('pageSize', this.inventoryParams.pageSize)
      return this.http.get<Pagination<InventoryAudit[]>>(`${environment.baseAPIUrl}/api/Inventories/admin/audit-list`, { params }).pipe(
        map(response => {
          this.invAuditList = [...this.invAuditList, ...response.data]
          this.invAuditPagination = response
          return response
        })
      )
    }

    getInventoryAuditParams() {
      return this.invAuditParams
    }

    setInventoryAuditParams(params: InventoryAuditParams) {
      this.invAuditParams = params
    }

    addNewInventoryAudit(request: AddInventoryAuditRequest): Observable<any> {
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/admin/audit-list`, request).pipe(
        tap(() => {
          this.invAuditCache = new Map()
        })
      )
    }

    getInventoryAuditDetails(id: number) {
      return this.http.get<InventoryAuditDetails[]>(`${environment.baseAPIUrl}/api/Inventories/admin/audit-list/${id}`)
    }

    conductInventoryAudit(id: number, request: ConductInventoryRequest): Observable<any> {
      return this.http.post(`${environment.baseAPIUrl}/api/Inventories/admin/audit-list/${id}/conduct`, request)
    }

    deleteInventoryAudit(id: number): Observable<any> {
      return this.http.delete(`${environment.baseAPIUrl}/api/Inventories/admin/audit-list/soft-delete/${id}`).pipe(
        tap(() => {
          this.invAuditCache = new Map()
        })
      )
    }
}
