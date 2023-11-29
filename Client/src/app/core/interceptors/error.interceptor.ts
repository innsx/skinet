import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
          catchError(error => {
              if (error){
                  if (error.status === 400) {
                    if (error.error.errors) {
                      throw error.error;
                    } else {
                      this.toastr.error(error.error.message, error.error.statusCode);
                    }
                  }
                  if (error.status === 401) {
                    this.toastr.error(error.error.message, error.error.statusCode);
                  }
                  if (error.status === 404){
                    this.router.navigateByUrl('/not-found');
                  }

                  if (error.status === 500){
                    const navigationExtras: NavigationExtras = {state: {error: error.error}};
                    this.router.navigateByUrl('/server-error', navigationExtras);
                  }
              }
              return throwError(error);
          })
      );
  }
}




// @Injectable()
// export class ErrorInterceptor implements HttpInterceptor {

//   constructor(private router: Router, private toastr: ToastrService) {}

//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//       return next.handle(req).pipe(
//           catchError(errorResponse => {
//               if (errorResponse){
//                   if (errorResponse.status === 400){
//                       if (errorResponse.error.errors) {
//                           throw errorResponse.error;
//                       } else {
//                           this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                           const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                           this.router.navigateByUrl('/badrequest', navigationExtras);
//                       }
//                   }

//                   if (errorResponse.status === 401){
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.router.navigateByUrl('/unauthorized', navigationExtras);
//                   }

//                   if (errorResponse.status === 403){
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.router.navigateByUrl('/forbidden', navigationExtras);
//                   }

//                   if (errorResponse.status === 404){
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       this.router.navigateByUrl('/not-found', navigationExtras);
//                   }

//                   if (errorResponse.status === 500){
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       this.router.navigateByUrl('/server-error', navigationExtras);
//                   }

//                   if (errorResponse.status === 501){
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.router.navigateByUrl('/notimplemented', navigationExtras);
//                   }

//                   if (errorResponse.status === 511){
//                       this.toastr.error(errorResponse.error.message, errorResponse.error.statusCode);
//                       const navigationExtras: NavigationExtras = {state: {error: errorResponse.error}};
//                       this.router.navigateByUrl('/network-auth-required', navigationExtras);
//                   }
//               }
//               return throwError(errorResponse);
//           })
//       );
//   }
// }








