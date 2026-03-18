import { Component, Input, OnInit, Self } from '@angular/core';
import { FormControl, NgControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.css']
})
export class TextAreaComponent implements OnInit {

  @Input() label = '';
  @Input() maxLength!: number;

  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  ngOnInit() {
    const control = this.controlDir.control as FormControl;
    if (control) {
      control.setValidators([
        ...(control.validator ? [control.validator] : []),
        Validators.maxLength(this.maxLength)
      ]);
      control.updateValueAndValidity();
    }
  }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control() {
    return this.controlDir.control as FormControl;
  }

  get characterCount(): number {
    return this.control.value ? this.control.value.length : 0;
  }

}
