import { toast } from 'react-toastify';
import ServiceResponse from './ServiceResponse';
import fetch from './FetchWrapper';
import { createLog } from './Log';

export async function getStagedCarts() {
  try {
    const result = await fetch('/api/staging');

    const response: ServiceResponse<GetStagedCartsResult[]> = await result.json();

    if (!response.success) {
      const message = response.message || 'Failed to get staged carts';
      toast.error(message);
      createLog({
        action: 'unknown',
        deliveryId: -1,
        message,
        method: 'PUT',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });

      return [] as GetStagedCartsResult[];
    }

    return response.data;
  } catch (err) {
    console.error('[getStagedCarts]:', err);
    throw err;
  }
}

// TYPES AND STUFF
export interface GetStagedCartsResult {
  cartId: string;
  deliveryId: number;
  deliveryRequestType: string;
  destinationArea: string;
  orderId: string;
  stagingLocationId: string;
}
