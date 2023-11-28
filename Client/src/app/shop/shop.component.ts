import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { HttpClient } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { ShopService } from './shop.service';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  // added a searchTerm property as a 'ElementRef' type
  // which the searchTerm is decorated as @ViewChild DECORATOR for
  // the PARENT ShopComponent to ACCESS
  // the CHILD INPUT control that has a search string referenced as 'searchTemplateRefVariable'
  @ViewChild('searchTemplateRefVariable', {static: true}) searchTerm: ElementRef;

  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];

  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'},
  ];

  shopParams = new ShopParams();

  totalCount: number;

  constructor(private shopService: ShopService) {
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe((response) => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;
    }, (error) => {
      console.log(error);
    });
  }

  getBrands() {
     this.shopService.getBrands().subscribe((response) => {
      this.brands = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  getTypes() {
     this.shopService.getTypes().subscribe((response) => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  onBrandSelected(brandId: number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event: any) {
    // "event" is a NUMBER passing OUT
    // to the PARENT ShopComponent & assigned to pageNumber
    if (this.shopParams.pageNumber !== event) {
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }

  onSearch() {
    // assign the user's entered "search string value" to the search property
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    // clear the search input control
    this.searchTerm.nativeElement.value = '';

    // reset all ShopParams model Properties to EMPTY strings
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}








// export class ShopComponent implements OnInit {
//   products: IProduct[] = [];
//   brands: IBrand[] = [];
//   types: IType[] = [];

//   brandIdSelected = 0;
//   typeIdSelected = 0;

//   sortSelected = 'name';  // default selected value
//   sortOptions = [
//     {name: 'Alphabetical', value: 'name'},
//     {name: 'Price: Low to High', value: 'priceAsc'},
//     {name: 'Price: High to Low', value: 'priceDesc'},
//   ];

//   useCache = false;

//   shopParams: ShopParams;

//   totalCount: number;

//   constructor(private shopService: ShopService) {
//     this.shopParams = this.shopService.getShopParams();
//   }

//   ngOnInit(): void {
//     this.getProducts(true);
//     this.getBrands();
//     this.getTypes();
//   }

//   getProducts(useCache) {
//     this.shopService.getProducts(useCache).subscribe((response) => {
//       this.products = response.data;
//       this.totalCount = response.count;
//     }, (error) => {
//       console.log(error);
//     });
//   }

//   getBrands() {
//      this.shopService.getBrands().subscribe((response) => {
//       this.brands = [{id: 0, name: 'All'}, ...response];
//     }, error => {
//       console.log(error);
//     });
//   }

//   getTypes() {
//      this.shopService.getTypes().subscribe((response) => {
//       this.types = [{id: 0, name: 'All'}, ...response];
//     }, error => {
//       console.log(error);
//     });
//   }

//   onBrandSelected(brandId: number){
//     // this.brandIdSelected = brandId;
//     const params = this.shopService.getShopParams();
//     params.brandId = brandId;
//     params.pageNumber = 1;
//     this.shopService.setShopParams(params);
//     this.getProducts(this.useCache);
//   }

//   onTypeSelected(typeId: number){
//     // this.typeIdSelected = typeId;
//     const params = this.shopService.getShopParams();
//     params.typeId = typeId;
//     params.pageNumber = 1;
//     this.shopService.setShopParams(params);
//     this.getProducts(this.useCache);
//   }

//   onSortSelected(sort: string) {
//     // this.sortSelected = sort;
//     const params = this.shopService.getShopParams();
//     params.sort = sort;
//     this.shopService.setShopParams(params);
//     this.getProducts(this.useCache);
//   }

//   onPageChanged(event: any) {
//     const params = this.shopService.getShopParams();

//     if (params.pageNumber !== event) {
//       params.pageNumber = event;
//       this.shopService.setShopParams(params);

//       this.useCache = true;
//       this.getProducts(this.useCache);
//     }
//   }
// }
