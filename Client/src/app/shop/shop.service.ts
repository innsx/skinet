import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { env } from 'process';
import { environment } from 'src/environments/environment';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  getTypes() {
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }

  getProduct(id: number) {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

}







// export class ShopService {
//   private readonly baseUrl = environment.apiUrl;

//   pagination = new Pagination();
//   shopParams = new ShopParams();

//   products: IProduct[] = [];
//   brands: IBrand[] = [];
//   types: IType[] = [];

//   constructor(private http: HttpClient) {
//   }

//   getProducts(useCache: boolean) {

//     if (useCache === false){
//       this.products = [];
//     }

//     if (this.products.length > 0 && useCache === true){
//       const pageSize = this.products.length / this.shopParams.pageSize;

//       const pagesReceived = Math.ceil(pageSize);

//       if (this.shopParams.pageNumber <= pagesReceived){
//         const startingPage = ((this.shopParams.pageNumber - 1) * (this.shopParams.pageSize));

//         const endingPage = (this.shopParams.pageNumber * this.shopParams.pageSize);

//         this.pagination.data = this.products.slice(startingPage, endingPage);

//         return of(this.pagination);
//       }
//     }

//     let params = new HttpParams();

//     if (this.shopParams.brandId !== 0){
//       params = params.append('brandId', this.shopParams.brandId.toString());
//     }

//     if (this.shopParams.typeId !== 0) {
//       params = params.append('typeId', this.shopParams.typeId.toString());
//     }

//     if (this.shopParams.search) {
//       params = params.append('search', this.shopParams.search);
//     }

//     params = params.append('sort', this.shopParams.sort);
//     params = params.append('pageIndex', this.shopParams.pageNumber.toString());
//     params = params.append('pageSize', this.shopParams.pageSize.toString());

//     return this.http.get<IPagination>(this.baseUrl + 'products', {observe: 'response', params})
//       .pipe(
//         map(response => {
//           this.products = [...this.products, ...response.body.data];
//           this.pagination = response.body as any;
//           return this.pagination;
//         })
//       );
//   }


//   getBrands() {
//     return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
//   }

//   getTypes() {
//     return this.http.get<IType[]>(this.baseUrl + 'products/types');
//   }

//   setShopParams(params: ShopParams) {
//     this.shopParams = params;
//   }

//   getShopParams() {
//     return this.shopParams;
//   }
// }
