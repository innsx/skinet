import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Welcome to SkiNet App!';

  constructor(private basketService: BasketService) {
  }

  ngOnInit(): void {
    this.getBasketIdFromStorage();
  }

  getBasketIdFromStorage() {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log(`Current Loaded BasketId:  ${basketId}`);
      }, error => {
        console.log('Error--No basketId from Storage: ', error);
      });
    }
  }
}
