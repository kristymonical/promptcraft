import { toast } from 'react-toastify';

import ServiceResponse from './ServiceResponse';
import fetch from './FetchWrapper';
import { createLog } from './Log';

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

    const response: ServiceResponse<any> = await result.json();

    if (response.success) {
      toast.success(response.message || 'Created delivery request!');
    } else {
      const message = response.message || 'Unable to create delivery request';
      toast.error(message);
      createLog({
        action: 'queue',
        deliveryId: -1,
        message,
        method: 'POST',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });
    }

    return response.success;
  } catch (err) {
    console.error('[createDeliveryRequest]:', err);
    throw err;
  }
}

export async function batchCreateDeliveryRequests(
  requests: CreateDeliveryRequest[]
) {
  if (requests.length === 0) return;

  try {
    const deliveries = requests.map(
      ({
        cartId,
        cartLocation,
        deliveryType,
        destinationArea,
        orderNumber
      }) => ({
        orderId: orderNumber,
        cartId,
        location: cartLocation,
        deliveryType,
        destinationArea
      })
    );
    const result = await fetch('/api/delivery/queue', {
      method: 'POST',
      body: JSON.stringify({
        deliveries
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const response: ServiceResponse<any> = await result.json();

    if (response.success) {
      toast.success('Created delivery requests!');
    } else {
      const message =
        response.message || 'Unable to batch create delivery requests';

      toast.error(message);
      createLog({
        action: 'queue',
        deliveryId: -1,
        message,
        method: 'POST',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });
    }
  } catch (err) {
    console.error('[batchCreateDeliveryRequests]:', err);
    throw err;
  }
}

export async function updateDeliveryDestinationArea(
  deliveryId: number,
  newDestination: string
) {
  try {
    const result = await fetch(`/api/delivery/${deliveryId}/destination`, {
      body: JSON.stringify({ destinationArea: newDestination }),
      headers: { 'Content-Type': 'application/json' },
      method: 'PATCH'
    });

    const response: ServiceResponse<any> = await result.json();

    if (response.success) {
      toast.success(
        `Updated delivery ${deliveryId}'s destination area to ${newDestination}`
      );
    } else {
      const message =
        response.message || "Unable to update delivery's destination area";

      toast.error(message);
      createLog({
        action: 'queue',
        deliveryId: -1,
        message,
        method: 'PATCH',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });
    }
  } catch (err) {
    console.error('[updateDeliveryDestinationArea]:', err);
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
