import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem, IBasket } from '../../models/basket';
import { IOrderItem } from '../../models/order';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;

  @Input() items: IOrderItem[] | IBasketItem[] = [];  // our items can be either an Array of IBasketItem or IOrderItem
  @Input() isOrder = false;  // isOrder flag is used for styling the basket-summary table in the order-detailed

  constructor() { }

  ngOnInit(): void {
  }

  decrementItemQuantity(item: IBasketItem) {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    this.increment.emit(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.remove.emit(item);
  }
}



//   @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
//   @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
//   @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();

//   @Input() isBasket = true;
//   // @Input() items: IBasket[];
//   @Input() items: IBasketItem[] | IOrderItem[] = [];

//   basket$: Observable<IBasket>;

//   constructor(private basketService: BasketService) { }

//   ngOnInit(): void {
//     this.basket$ = this.basketService.basket$;
//   }

//   decrementItemQuantity(item: IBasketItem) {
//     this.decrement.emit(item);
//   }

//   incrementItemQuantity(item: IBasketItem) {
//     this.increment.emit(item);
//   }

//   removeBasketItem(item: IBasketItem) {
//     this.remove.emit(item);
//   }
// }
