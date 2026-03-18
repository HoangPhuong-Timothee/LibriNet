import { AbstractControl, AsyncValidatorFn } from "@angular/forms"
import { catchError, debounceTime, finalize, map, of, switchMap, take } from "rxjs"
import { AuthService } from "src/app/core/services/auth.service"
import { AuthorService } from "src/app/core/services/author.service"
import { BookService } from "src/app/core/services/book.service"
import { BookstoreService } from "src/app/core/services/bookstore.service"
import { GenreService } from "src/app/core/services/genre.service"
import { PublisherService } from "src/app/core/services/publisher.service"
import { UnitOfMeasureService } from "src/app/core/services/unit-of-measure.service"

export function validateEmailExist(authService: AuthService): AsyncValidatorFn {

  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap(() => {
        return authService.checkEmailExists(control.value).pipe(
          map((result) => (result ? { emailExists: true } : null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validateAuthorExist(authorService: AuthorService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap(() => {
        return authorService.checkAuthorExist(control.value).pipe(
        map((result) => (result ? { authorExists: true } : null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validateGenreExist(genreService: GenreService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap(() => {
        return genreService.checkGenreExist(control.value).pipe(
        map((result) => (result ? { genreExists: true } : null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validatePublisherExist(publisherService: PublisherService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap(() => {
        return publisherService.checkPublisherExist(control.value).pipe(
        map((result) => (result ? { publisherExists: true } : null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validateBookExist(bookService: BookService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap((bookTitle) => {
        return bookService.checkBookExistByTitle(bookTitle).pipe(
          map((result) => (result ? { bookExists: true } : null)),
          catchError(() => of(null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validateBookInStore(bookService: BookService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    const bookTitle = control.parent?.get('bookTitle')?.value
    const bookStoreId = control.value
    if (bookTitle && bookStoreId) {
      return bookService.checkBookExistInBookStore(bookTitle, bookStoreId).pipe(
        map((result) => result ? null : { bookExistInStore: true }),
        catchError(() => of(null)),
    )} else {
        return of(null)
      }
    }
}

export function validateBookISBN(bookService: BookService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    const bookTitle = control.parent?.get('bookTitle')?.value
    const isbn = control.value
    if (bookTitle && isbn) {
      return bookService.checkBookISBN(isbn, bookTitle).pipe(
        map((result) => result ? null : { isbnMatch: true }),
        catchError(() => of(null))
      )
    } else {
      return of(null)
    }
  }
}

export function validateBookStoreExist(bookStoreService: BookstoreService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap((storeName) => {
        return bookStoreService.checkBookStoreExistByStoreName(storeName).pipe(
          map((result) => (result ? { bookStoreExists: true } : null)),
          catchError(() => of(null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}

export function validateUnitOfMeasureExist(uomService: UnitOfMeasureService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    return control.valueChanges.pipe(
      debounceTime(1000),
      take(1),
      switchMap(() => {
        return uomService.checkUnitOfMeasureExist(control.value).pipe(
        map((result) => (result ? { uomExists: true } : null)),
          finalize(() => control.markAllAsTouched())
        )
      })
    )
  }
}


