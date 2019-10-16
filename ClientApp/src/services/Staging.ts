import { toast } from 'react-toastify';
import ServiceResponse from './ServiceResponse';

export async function getStagedCarts() {
  try {
    const result = await fetch('/api/staging');
    const json: ServiceResponse = await result.json();
    if (!json.success) {
      toast.error(json.message || 'Failed to get staged carts');
      return [] as GetStagedCartsResult[];
    }

    return json.data as GetStagedCartsResult[];
  } catch (err) {
    console.error('[getStagedCarts]:', err);
    throw err;
  }
}

// TYPES AND STUFF
export interface GetStagedCartsResult {
  orderId: string;
  cartId: string;
  stagingLocationId: string;
  deliveryRequestType: string;
  destinationArea: string;
}
