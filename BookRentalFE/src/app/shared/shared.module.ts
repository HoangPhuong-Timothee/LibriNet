import { ScrollingModule } from '@angular/cdk/scrolling';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatOptionModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDivider } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { BasketItemComponent } from './components/basket-item/basket-item.component';
import { BookItemComponent } from './components/book-item/book-item.component';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { CustomTableComponent } from './components/custom-table/custom-table.component';
import { DateInputComponent } from './components/date-input/date-input.component';
import { FileInputComponent } from './components/file-input/file-input.component';
import { InputTextComponent } from './components/input-text/input-text.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { PagingFooterComponent } from './components/paging-footer/paging-footer.component';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { SectionHeaderComponent } from './components/section-header/section-header.component';
import { ServerErrorComponent } from './components/server-error/server-error.component';
import { StepperComponent } from './components/stepper/stepper.component';
import { TextAreaComponent } from './components/text-area/text-area.component';
import { CustomCurrencyPipe } from './pipes/custom-currency.pipe';
import { TruncateTitlePipe } from './pipes/truncate-title.pipe';

@NgModule({
  declarations: [
    NavbarComponent,
    PagingHeaderComponent,
    PagingFooterComponent,
    SectionHeaderComponent,
    NotFoundComponent,
    ServerErrorComponent,
    OrderTotalsComponent,
    InputTextComponent,
    CustomCurrencyPipe,
    TruncateTitlePipe,
    BookItemComponent,
    BasketItemComponent,
    CustomTableComponent,
    StepperComponent,
    ConfirmationDialogComponent,
    DateInputComponent,
    TextAreaComponent,
    FileInputComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxSpinnerModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    ReactiveFormsModule,
    FormsModule,
    BreadcrumbModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatRadioModule,
    MatPaginatorModule,
    MatTooltipModule,
    CdkStepperModule,
    MatTabsModule,
    MatDialogModule,
    MatSelectModule,
    MatProgressBarModule,
    NgxMatSelectSearchModule,
    BsDatepickerModule,
    MatOptionModule,
    MatListModule,
    ScrollingModule,
    MatTableModule,
    MatCardModule,
    MatAutocompleteModule,
    MatMenuModule
  ],
  exports: [
    NavbarComponent,
    PagingHeaderComponent,
    PagingFooterComponent,
    ReactiveFormsModule,
    BsDropdownModule,
    SectionHeaderComponent,
    BreadcrumbModule,
    NgxSpinnerModule,
    ToastrModule,
    OrderTotalsComponent,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    InputTextComponent,
    FormsModule,
    CustomCurrencyPipe,
    TruncateTitlePipe,
    MatCheckboxModule,
    MatRadioModule,
    BasketItemComponent,
    BookItemComponent,
    MatTooltipModule,
    MatPaginatorModule,
    CustomTableComponent,
    CdkStepperModule,
    StepperComponent,
    MatTabsModule,
    MatDialogModule,
    MatSelectModule,
    ConfirmationDialogComponent,
    MatProgressBarModule,
    NgxMatSelectSearchModule,
    DateInputComponent,
    BsDatepickerModule,
    MatOptionModule,
    MatListModule,
    MatDivider,
    ScrollingModule,
    MatTableModule,
    MatCardModule,
    MatAutocompleteModule,
    MatMenuModule,
    TextAreaComponent,
    FileInputComponent
  ]
})
export class SharedModule { }
