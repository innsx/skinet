import uuid from 'uuid/v4';

export interface IBasket {
    id: string;
    items: IBasketItem[];
    deliveryMethodId?: number;
    shippingPrice?: number;
    paymentIntentId?: string;
  }

export interface IBasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}

// whenever we create a new INSTANCE of the CLASS
// basket, it will have an ID and empty array for items.
export class Basket implements IBasket {
    id = uuid();
    items: IBasketItem[] = [];
}


export interface IBasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}
