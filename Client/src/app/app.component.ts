import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IProduct } from './shared/models/product';
import { IPagination } from './shared/models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Welcome to SkiNet App!';

  products: IProduct[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.http.get<IPagination>('https://localhost:5001/api/products').subscribe((response: IPagination) => {
      console.log('Products: ', response);
      this.products = response.data;
    }, error => {
      console.error('Error: ', error);
    });
  }

}
