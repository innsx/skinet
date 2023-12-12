import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private readonly baseUrl: string = environment.apiUrl;

  // set an initial value of 'null'
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();

  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }

  getBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id).pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        // console.log('CurrentBasketValue: ', this.getCurrentBasketValue());
        this.calculateTotals();
      })
    );
  }

  setBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
    }, error => {
      console.log(error);
    });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemBasketItem(item, quantity);

    // ternary operator will ONLY work with TypeScript 3.7.0 or higher
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    // console.log('CurrentBasket/Create Basket: ', basket);

    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

    this.setBasket(basket);

  }

  private mapProductItemBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    };
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);

    return basket;
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);

    if (index === -1) { // index not found
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }

    return items;
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    // const subtotal = basket ? (basket.items.reduce((result, item) => (item.price * item.quantity) + result, 0)) : null;
    // console.log('subtotal: ', subtotal);
    const subtotal = basket ? basket.items.reduce((result, item) => (item.price * item.quantity) + result, 0) : 0;
    // console.log('subtotal: ', subtotal);
    const total = subtotal ? subtotal + shipping : 0;

    this.basketTotalSource.next({shipping, total, subtotal});
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    // const foundItemIndex = basket?.items.findIndex(x => x.id === item.id);

    // basket ? (basket.items[foundItemIndex ? foundItemIndex : 0].quantity++) : null;

    basket.items[foundItemIndex].quantity ++;
    // basket.items[foundItemIndex ? foundItemIndex : 0].quantity ++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();

    const foundItemIndex = basket?.items.findIndex(x => x.id === item.id);

    if (basket.items[foundItemIndex ? foundItemIndex : 0].quantity > 1) {
      basket.items[foundItemIndex ? foundItemIndex : 0].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }

  // remove the item in the basket
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();

    if (basket?.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id);

      if (basket.items.length > 0){
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  // go to API and remove the basket with this basket's Id
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }
}

