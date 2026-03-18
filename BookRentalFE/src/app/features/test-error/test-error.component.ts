import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UpdateAuthorRequest } from 'src/app/core/models/author.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent {

  validateErrors: string[] = []

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getNotFoundError() {
    this.http.get(`${environment.baseAPIUrl}/api/Bugs/not-found`).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  getServerError() {
    this.http.get(`${environment.baseAPIUrl}/api/Bugs/server-error`).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  getBadRequestError() {
    this.http.get(`${environment.baseAPIUrl}/api/Bugs/bad-request`).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    })
  }

  getValidationError() {
    const request: UpdateAuthorRequest = {
      id: 1,
      name: 'fdsjflkdsflkdsfjlskdfjlksdfjlksfjklsjlfkdsjlkfjdslkfjldskfjlkdsfjdsfkldsjflkdsfjlkdsfjlkdsfjlkdsjflkdsflkds'
    }
    this.http.put(`${environment.baseAPIUrl}/api/Genres/${request.id}`, request).subscribe({
      next: response => console.log(response),
      error: error => {
        this.toastr.error(error.message, error.statusCode)
        this.validateErrors = error.errors
      }
    })
  }


}
