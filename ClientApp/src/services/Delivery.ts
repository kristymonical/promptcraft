import { toast } from 'react-toastify';

import ServiceResponse from './ServiceResponse';

export async function createDeliveryRequest({
  cartId,
  cartLocation,
  deliveryType = 'deliver',
  destinationArea,
  orderNumber
}: CreateDeliveryRequest) {
  try {
    const result = await fetch('/api/delivery/queue', {
      method: 'POST',
      body: JSON.stringify({
        deliveries: [
          {
            orderId: orderNumber,
            cartId,
            location: cartLocation,
            deliveryType,
            destinationArea
          }
        ]
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const response: ServiceResponse = await result.json();
    if (response.success) {
      toast.success('Created delivery request!');
    } else {
      toast.error(response.message);
    }

    return response.success;
  } catch (err) {
    console.error('[createDeliveryRequest]:', err);
    throw err;
  }
}

// Types and stuff
export interface CreateDeliveryRequest {
  cartId: string;
  cartLocation: string;
  deliveryType?: string;
  destinationArea: string;
  orderNumber?: string;
}
