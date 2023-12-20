import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';
import { IBasketTotals } from '../shared/models/basket';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  basketTotals$: Observable<IBasketTotals>;
  checkoutFormGroup: FormGroup;

  constructor(private fb: FormBuilder, private accountService: AccountService, private basketService: BasketService) { }

  ngOnInit(): void {
    this.createCheckoutForm();
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
    this.basketTotals$ = this.basketService.basketTotal$;
  }

  createCheckoutForm() {
    this.checkoutFormGroup = this.fb.group({
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName:  [null, Validators.required],
        street:    [null, Validators.required],
        city:      [null, Validators.required],
        state:     [null, Validators.required],
        zipcode:   [null, Validators.required],
      }),

      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required]
      }),

      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required]
      })
    });
  }

  getAddressFormValues() {
    this.accountService.getUserAddress().subscribe(address => {
      // console.log('Loaded Address: ', address);
      if (address) {
        this.checkoutFormGroup.get('addressForm').patchValue(address);
      }
    }, error => {
      console.log('Error getting Address: ', error);
    });
  }

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();

    if (basket.deliveryMethodId !== null) {
     this.checkoutFormGroup.get('deliveryForm').get('deliveryMethod').patchValue(basket.deliveryMethodId?.toString());
    }
  }


}
