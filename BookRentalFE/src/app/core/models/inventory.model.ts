export interface Inventory {
  bookId: number
  bookTitle: string
  isbn: string
  quantity: number
  inventoryStatus: string
  unitOfMeasure: string
  storeName: string
  createInfo: string
  updateInfo: string
}

export interface InventoryReceipt {
  receiptId: number
  receiptCode: string
  totalAmount: number
  totalPrice: number
  importNotes: string
  receiptStatus: string
  receiptType: string
  createInfo: string
  updateInfo: string
}

export interface InventoryTransaction {
  transactionId: number
  storeName: string
  storeAddress: string
  transactionType: string
  quantity: number
  measureUnit: string
  transactionDate: string
  performedBy: string
  transactionNotes: string
  showDetails?: boolean
}

export interface AddImportReceiptRequest {
  importNotes: string
  importReceiptItems: ImportReceiptItem[]
}

export interface ImportReceiptItem {
  bookTitle: string
  isbn: string
  unitOfMeasureId: number
  quantity: number
  bookStoreId: number
}

export interface AddExportReceiptRequest {
  exportNotes: string
  exportReceiptItems: ExportReceiptItem[]
}

export interface ExportReceiptItem {
  bookTitle: string
  isbn: string
  unitOfMeasureId: number
  quantity: number
  bookStoreId: number
}

export interface BookQuantity {
  convertedInputQuantity: number
  remainingQuantity: number
}

export interface InventoryAudit {
  id: number
  auditDate: string
  audittedBy: string
  auditNotes: string
  auditStatus: string
  createdAt: string
  auditCode: string
}

export interface AddInventoryAuditRequest {
  audittedBy: string
  auditDate: Date
  auditNotes: string
  inventoryAuditItems: InventoryAuditItem[]
}

export interface InventoryAuditItem {
  bookTitle: string
  isbn: string
  bookStoreId: number
}

export interface InventoryAuditDetails {
  id: number
  bookTitle: string
  isbn: string
  storeName: string
  inventoryQuantity: number
  measureUnit: string
  updateInfo: string
}

export interface ConductInventoryRequest {
  id: number
  bookTitle: string
  isbn: string
  storeName: string
  inventoryQuantity: number
  actualQuantity: number
  unitOfMeasureId: number
}
