import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-custom-table',
  templateUrl: './custom-table.component.html',
  styleUrls: ['./custom-table.component.css']
})
export class CustomTableComponent {

  @Input() columns: {
    field: string | string[],
    header: string,
    haveImage?: boolean,
    imageUrl?: string,
    pipe?: string,
    pipeArgs?: any,
    class?: (row: any) => string,
    colClass?: string
  }[] = []
  @Input() dataSource: any[] = []
  @Input() actions: {
    label: string,
    icon: string,
    tooltip: string,
    action: (row: any) => void,
    disabled?: (row: any) => boolean
  }[] = []
  @Input() totalItems: number = 0
  @Input() pageSize: number = 20
  @Input() pageIndex: number = 0
  @Input() isErrorData?: boolean
  @Output() pageChange = new EventEmitter<PageEvent>()

  constructor() { }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex
    this.pageSize = event.pageSize
    this.pageChange.emit(event)
  }

  onAction(action: (row: any) => void, row: any) {
    action(row)
  }

  getCellClass(row: any, column: any): string {
    return column.class ? column.class(row) : ''
  }

  getColClass(column: any): string {
    return column.colClass ? column.colClass : ''
  }

  getCellValue(row: any, column: any) {
    const value = row[column.field];
    if (column.pipe === 'currency') {
      return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: column.pipeArgs || 'VND' }).format(value)
    }
    if (column.pipe === 'date') {
      return new Date(value).toLocaleDateString('en-US', column.pipeArgs)
    }
    return value
  }
}
