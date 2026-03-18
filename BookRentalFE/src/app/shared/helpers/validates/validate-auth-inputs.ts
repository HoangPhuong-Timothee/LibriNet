import { AbstractControl, ValidatorFn } from "@angular/forms"

export function validateMatchValues(matchTo: string): ValidatorFn {
  return (control: AbstractControl) => {
    const parent = control?.parent
    const matchControl = parent ? parent.get(matchTo) : null
    if (!parent || !matchControl) {
      return null
    }
    return control.value === matchControl.value ? null : { isMatching: true }
  }
}
