import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncateTitle'
})
export class TruncateTitlePipe implements PipeTransform {

  transform(title: string, maxLength: number): string {
    if (!title) return ''
    if (title.length > maxLength)
    {
      return title.substring(0, maxLength) + '...'
    }
    return title;
  }

}
