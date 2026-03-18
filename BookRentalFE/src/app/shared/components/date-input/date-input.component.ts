import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.css']
})
export class DateInputComponent implements ControlValueAccessor {
  @Input() label = ''
  @Input() maxDate: Date | undefined
  bsConfig: Partial<BsDatepickerConfig> = { }
  @Input() dateMode: 'date' | 'year' = 'date'
  isYearOnly = false

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this
    this.updateBsConfig()
   }

  toggleDateMode() {
    this.isYearOnly = !this.isYearOnly
    this.updateBsConfig()
  }

  updateBsConfig() {
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: this.isYearOnly ? 'YYYY' : 'DD MMMM YYYY',
      minMode: this.isYearOnly ? 'year' : 'day'
    }
  }

  writeValue(obj: any): void {
    if (this.isYearOnly && typeof obj === 'number') {
      this.control.setValue(new Date(obj, 0, 1))
    } else {
      this.control.setValue(obj)
    }
  }

  registerOnChange(fn: any): void {
    this.control.valueChanges.subscribe(
      value => {
        if (this.isYearOnly && value instanceof Date) {
          fn(value.getFullYear())
        } else {
          fn(value)
        }
      })
  }

  registerOnTouched(fn: any): void {

  }

  get control(): FormControl {
    return this.ngControl.control as FormControl
  }

}
