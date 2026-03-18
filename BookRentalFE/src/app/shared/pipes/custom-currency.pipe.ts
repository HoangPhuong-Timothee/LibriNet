import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customCurrency'
})
export class CustomCurrencyPipe implements PipeTransform {

  transform(value: number, currency: string = 'VND'): string {
    return `${value.toLocaleString()}${currency === 'VND' ? 'đ' : ''}`
  }

}
