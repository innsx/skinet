import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  private readonly baseUrl = environment.apiUrl;

  validationErrors: [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe((response: any) => {
      console.log('Response: ', response);
    }, error => {
      console.log('BadRequestErors: ', error);
    });
  }

  get400ValidationError() {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe((response: any) => {
      console.log('Response: ', response);
    }, error => {
      console.log('ValidationErrors: ', error);
      this.validationErrors = error.errors; // errors array
    });
  }

  get404Error() {
    this.http.get(this.baseUrl + 'products/42').subscribe((response) => {
      console.log('Response: ', response);
    }, error => {
      console.log('NotFoundErrors: ', error);
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe((response: any) => {
      console.log('Response: ', response);
    }, error => {
      console.log('ServerErrors: ', error);
    });
  }
}








// export class TestErrorComponent implements OnInit {
//   private readonly baseUrl = environment.apiUrl;

//   validationErrors: [];

//   constructor(private http: HttpClient) {}

//   ngOnInit(): void {
//   }

//   get400Error() {
//     this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
//       next: (response: any) => {
//         console.log(response);
//       }, error: (err) => {
//         console.log(err);
//       }
//     });
//   }

//   get400ValidationError() {
//     this.http.get(this.baseUrl + 'products/fortytwo').subscribe({
//       next: (response: any) => {
//         console.log(response);
//       }, error: (err) => {
//         console.log(err);
//         this.validationErrors = err.errors; // errors array
//       }
//     });
//   }

//   get404Error() {
//     this.http.get(this.baseUrl + 'products/42').subscribe({
//       next: (response) => {
//         console.log(response);
//       }, error: (err) => {
//         console.log(err);
//       }
//     });
//   }

//   get500Error() {
//     this.http.get(this.baseUrl + 'buggy/servererror').subscribe({
//       next: (response: any) => {
//         console.log('Results: ', response);
//       }, error: (err) => {
//         console.log(err);
//       }
//     });
//   }
// }

