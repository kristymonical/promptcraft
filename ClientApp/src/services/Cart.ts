import ServiceResponse from './ServiceResponse';
import { toast } from 'react-toastify';
import fetch from './FetchWrapper';

export async function getOrderAndDestination(
  cartId: string,
  currentLocationName: string
) {
  try {
    const res = await fetch(
      `/api/cleaninfo?MalLocationName=${currentLocationName}&CartId=${cartId}`
    );
    const json: ServiceResponse = await res.json();

    if (!json.success) {
      toast.error(json.message || 'Unable to verify cart');

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
    const res = await fetch('/api/move', {
      method: 'PUT',
      body: JSON.stringify({
        locationName,
        cartId
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const json: ServiceResponse = await res.json();

    if (!json.success) toast.error(json.message || 'Unable to move cart.');
    else toast.success(`Cart ${cartId} moved to location ${locationName}`);
  } catch (err) {
    console.error('[moveCart]:', err);
    throw err;
  }
}

export interface GetOrderAndDestinationResponse {
  destinationAreaName: string;
  orderId: string;
}
