import ServiceResponse from './ServiceResponse';
import { toast } from 'react-toastify';
import fetch from './FetchWrapper';
import { createLog } from './Log';

export async function getOrderAndDestination(
  cartId: string,
  currentLocationName: string
) {
  try {
    const result = await fetch(
      `/api/cleaninfo?MalLocationName=${currentLocationName}&CartId=${cartId}`
    );
    const json: ServiceResponse = await result.json();

    if (!json.success) {
      const message = json.message || 'Unable to verify cart';
      toast.error(message);
      createLog({
        action: 'move',
        deliveryId: -1,
        message,
        method: 'GET',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });

      return null;
    }

    toast.success(
      `Verified cart ${cartId} and moved into ${currentLocationName}`
    );
    return json.data as GetOrderAndDestinationResponse;
  } catch (err) {
    console.error('[getOrderAndDestination]:', err);
    throw err;
  }
}

export async function moveCart(cartId: string, locationName: string) {
  try {
    const result = await fetch('/api/move', {
      method: 'PUT',
      body: JSON.stringify({
        locationName,
        cartId
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const json: ServiceResponse = await result.json();

    if (!json.success) {
      const message = json.message || 'Unable to move cart.';
      toast.error(message);
      createLog({
        action: 'move',
        deliveryId: -1,
        message,
        method: 'PUT',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });
    } else toast.success(`Cart ${cartId} moved to location ${locationName}`);
  } catch (err) {
    console.error('[moveCart]:', err);
    throw err;
  }
}

export interface GetOrderAndDestinationResponse {
  destinationAreaName: string;
  orderId: string;
}
