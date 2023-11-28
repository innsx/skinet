import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;

  constructor(private shopService: ShopService, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    this.loadProduct();
  }

  loadProduct() {
    const id: number = +this.activeRoute.snapshot.paramMap.get('id');

    if (isNaN(id)) {
      return;
    }

    this.shopService.getProduct(id).subscribe((response: IProduct) => {
        this.product = response;
      }, error => {
        console.log('error: ', error);
    });
  }
}
