import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { IOrder, IOrderToCreate } from 'src/app/shared/models/order';
import { IBasket } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService,
              private toastr: ToastrService, private router: Router
             ) { }

  ngOnInit(): void {
  }

  async submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    console.log('Basket: ', basket);

    const orderToCreate = this.getOrderToCreate(basket);

    this.checkoutService.creatOrder(orderToCreate).subscribe((order: IOrder) => {
      this.basketService.deleteLocalBasket(basket.id);
      console.log('Order created: ', order);
      this.toastr.success('Order created successfully!');

      const navigationExtras: NavigationExtras = {state: order};
      this.router.navigate(['checkout/success'], navigationExtras);
    }, error => {
      this.toastr.error(error.message);
      console.log('Error in Submit Order: ', error);
    });
  }

  private getOrderToCreate(basket: IBasket): IOrderToCreate {
    // create an Order object and return Order object
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm.get('deliveryForm').get('deliveryMethod').value,
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
  }
}










  // async submitOrder() {
  //   this.loading = true;
  //   const basket = this.basketService.getCurrentBasketValue();
  //   console.log('Basket: ', basket);

    // try {
      // const createdOrder = await this.createOrder(basket);
      // const paymentResult = await this.confirmPaymentWithStripe(basket);

      // if (paymentResult.paymentIntent) {
      //   this.basketService.deleteBasket(basket);
      //   const navigationExtras: NavigationExtras = {state: createdOrder};
      //   this.router.navigate(['checkout/success'], navigationExtras);
      // } else {
      //   this.toastr.error(paymentResult.error.message);
      // }

    //   this.loading = false;

    // } catch (error) {
    //   console.log(error);
    //   this.loading = false;
    // }
  // }

