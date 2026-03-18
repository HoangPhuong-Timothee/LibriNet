import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AddAuthorFormComponent } from './admin-author/add-author-form/add-author-form.component';
import { AdminAuthorComponent } from './admin-author/admin-author.component';
import { EditAuthorFormComponent } from './admin-author/edit-author-form/edit-author-form.component';
import { ImportAuthorFormComponent } from './admin-author/import-author-form/import-author-form.component';
import { AddBooksFormComponent } from './admin-book/add-books-form/add-books-form.component';
import { AdminBookDetailsComponent } from './admin-book/admin-book-details/admin-book-details.component';
import { AdminBookComponent } from './admin-book/admin-book.component';
import { BookFilterDialogComponent } from './admin-book/book-filter-dialog/book-filter-dialog.component';
import { ImportBooksFormComponent } from './admin-book/import-books-form/import-books-form.component';
import { AddBookstoreFormComponent } from './admin-bookstore/add-bookstore-form/add-bookstore-form.component';
import { AdminBookstoreComponent } from './admin-bookstore/admin-bookstore.component';
import { EditBookstoreFormComponent } from './admin-bookstore/edit-bookstore-form/edit-bookstore-form.component';
import { ImportBookstoreFormComponent } from './admin-bookstore/import-bookstore-form/import-bookstore-form.component';
import { AddGenreFormComponent } from './admin-genre/add-genre-form/add-genre-form.component';
import { AdminGenreComponent } from './admin-genre/admin-genre.component';
import { EditGenreFormComponent } from './admin-genre/edit-genre-form/edit-genre-form.component';
import { ImportGenreFormComponent } from './admin-genre/import-genre-form/import-genre-form.component';
import { AdminInventoryComponent } from './admin-inventory/admin-inventory.component';
import { AddInventoryAuditFormComponent } from './admin-inventory/inventory-audit/add-inventory-audit-form/add-inventory-audit-form.component';
import { ConductInventoryFormComponent } from './admin-inventory/inventory-audit/conduct-inventory-form/conduct-inventory-form.component';
import { FilterInventoryAuditDialogComponent } from './admin-inventory/inventory-audit/filter-inventory-audit-dialog/filter-inventory-audit-dialog.component';
import { InventoryAuditResultComponent } from './admin-inventory/inventory-audit/inventory-audit-result/inventory-audit-result.component';
import { InventoryAuditComponent } from './admin-inventory/inventory-audit/inventory-audit.component';
import { InventoryFilterDialogComponent } from './admin-inventory/inventory-list/inventory-filter-dialog/inventory-filter-dialog.component';
import { InventoryListComponent } from './admin-inventory/inventory-list/inventory-list.component';
import { InventoryTransactionDialogComponent } from './admin-inventory/inventory-list/inventory-transaction-dialog/inventory-transaction-dialog.component';
import { AddExportReceiptFileFormComponent } from './admin-inventory/inventory-receipt-list/add-export-receipt-file-form/add-export-receipt-file-form.component';
import { AddExportReceiptFormComponent } from './admin-inventory/inventory-receipt-list/add-export-receipt-form/add-export-receipt-form.component';
import { AddImportReceiptFileFormComponent } from './admin-inventory/inventory-receipt-list/add-import-receipt-file-form/add-import-receipt-file-form.component';
import { AddImportReceiptFormComponent } from './admin-inventory/inventory-receipt-list/add-import-receipt-form/add-import-receipt-form.component';
import { InventoryReceiptListComponent } from './admin-inventory/inventory-receipt-list/inventory-receipt-list.component';
import { ReceiptFilterDialogComponent } from './admin-inventory/inventory-receipt-list/receipt-filter-dialog/receipt-filter-dialog.component';
import { AddUnitOfMeasureFormComponent } from './admin-inventory/unit-of-measure-list/add-unit-of-measure-form/add-unit-of-measure-form.component';
import { EditUnitOfMeasureFormComponent } from './admin-inventory/unit-of-measure-list/edit-unit-of-measure-form/edit-unit-of-measure-form.component';
import { ImportMeasureUnitFormComponent } from './admin-inventory/unit-of-measure-list/import-measure-unit-form/import-measure-unit-form.component';
import { UnitOfMeasureListComponent } from './admin-inventory/unit-of-measure-list/unit-of-measure-list.component';
import { AdminOrderComponent } from './admin-order/admin-order.component';
import { DeliveryMethodListComponent } from './admin-order/delivery-method-list/delivery-method-list.component';
import { ImportDeliveryMethodFormComponent } from './admin-order/delivery-method-list/import-delivery-method-form/import-delivery-method-form.component';
import { OrderDetailsDialogComponent } from './admin-order/order-list/order-details-dialog/order-details-dialog.component';
import { OrderFilterDialogComponent } from './admin-order/order-list/order-filter-dialog/order-filter-dialog.component';
import { OrderListComponent } from './admin-order/order-list/order-list.component';
import { AddPublisherFormComponent } from './admin-publisher/add-publisher-form/add-publisher-form.component';
import { AdminPublisherComponent } from './admin-publisher/admin-publisher.component';
import { EditPublisherFormComponent } from './admin-publisher/edit-publisher-form/edit-publisher-form.component';
import { ImportPublisherFormComponent } from './admin-publisher/import-publisher-form/import-publisher-form.component';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminUserComponent } from './admin-user/admin-user.component';
import { AdminComponent } from './admin.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminBookComponent,
    AdminAuthorComponent,
    AdminGenreComponent,
    AdminPublisherComponent,
    AdminOrderComponent,
    AdminInventoryComponent,
    AddPublisherFormComponent,
    EditPublisherFormComponent,
    AddGenreFormComponent,
    EditGenreFormComponent,
    AddAuthorFormComponent,
    EditAuthorFormComponent,
    AddBooksFormComponent,
    AdminBookDetailsComponent,
    InventoryFilterDialogComponent,
    BookFilterDialogComponent,
    AdminUserComponent,
    InventoryTransactionDialogComponent,
    AdminBookstoreComponent,
    AddBookstoreFormComponent,
    EditBookstoreFormComponent,
    OrderFilterDialogComponent,
    OrderDetailsDialogComponent,
    AddUnitOfMeasureFormComponent,
    EditUnitOfMeasureFormComponent,
    ImportGenreFormComponent,
    ImportAuthorFormComponent,
    ImportPublisherFormComponent,
    ImportMeasureUnitFormComponent,
    ImportBookstoreFormComponent,
    ImportBooksFormComponent,
    InventoryListComponent,
    UnitOfMeasureListComponent,
    InventoryAuditComponent,
    AddInventoryAuditFormComponent,
    OrderListComponent,
    DeliveryMethodListComponent,
    ImportDeliveryMethodFormComponent,
    ConductInventoryFormComponent,
    InventoryAuditResultComponent,
    FilterInventoryAuditDialogComponent,
    InventoryReceiptListComponent,
    AddExportReceiptFormComponent,
    AddImportReceiptFormComponent,
    AddImportReceiptFileFormComponent,
    AddExportReceiptFileFormComponent,
    ReceiptFilterDialogComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule
  ]
})
export class AdminModule { }
