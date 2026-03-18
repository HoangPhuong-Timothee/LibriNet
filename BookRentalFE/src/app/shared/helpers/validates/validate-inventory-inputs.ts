import { AbstractControl, AsyncValidatorFn, ValidatorFn } from "@angular/forms"
import { BehaviorSubject, catchError, debounceTime, map, of, switchMap, take, tap } from "rxjs"
import { ValidateBookQuantityInBookStoreParams } from "src/app/core/models/params.model"
import { InventoryService } from "src/app/core/services/inventory.service"

export function validateQuantityInStore(inventoryService: InventoryService, availableQuantity$: BehaviorSubject<number>, convertedQuantity$: BehaviorSubject<number>): AsyncValidatorFn {
  return (control: AbstractControl) => {
    const bookTitle = control.parent?.get('bookTitle')?.value
    const bookStoreId = control.parent?.get('bookStoreId')?.value
    const unitOfMeasureId = control.parent?.get('unitOfMeasureId')?.value
    const isbn = control.parent?.get('isbn')?.value
    const inputQuantity = control.value
    const validateParams = new ValidateBookQuantityInBookStoreParams()
    validateParams.bookStoreId = bookStoreId
    validateParams.unitOfMeasureId = unitOfMeasureId
    validateParams.inputQuantity = inputQuantity
    validateParams.bookTitle = bookTitle
    validateParams.isbn = isbn
    if (bookTitle && bookStoreId) {
        return control.valueChanges.pipe(
         debounceTime(1000),
         take(1),
         switchMap(() => {
          return inventoryService.getConvertedAndRemainingQuatity(validateParams).pipe(
            debounceTime(1000),
            tap(result => {
              availableQuantity$.next(result.remainingQuantity)
              convertedQuantity$.next(result.convertedInputQuantity)
            }),
            map((result) => result.remainingQuantity >= result.convertedInputQuantity ? null : { validQuantity: true }),
            catchError(() => of(null))
          )
         })
        )
    } else {
        return of(null)
      }
    }
}

export function validatePastDate(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    if (!control.value) {
      return null
    }
    const today = new Date()
    const inputDate = new Date(control.value)
    return inputDate <= today ? null : { 'futureDate': { value: control.value } }
  }
}
