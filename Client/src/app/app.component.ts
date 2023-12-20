import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { AccountService } from './account/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Welcome to SkiNet App!';

  constructor(
    private basketService: BasketService,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.getBasketIdFromStorage();
    this.loadCurrentUser();
  }

  getBasketIdFromStorage() {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(
        () => {
          // console.log(`Current Loaded BasketId:  ${basketId}`);
        },
        (error) => {
          console.log('Error--No basketId from Storage: ', error);
        }
      );
    }
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');

    this.accountService.loadCurrentUser(token).subscribe(
      (response) => {
        // console.log('User is Loaded or reloaded');
      },
      (error) => {
        console.error('Error: Failed to Load Logged User');
      }
    );
  }
}
