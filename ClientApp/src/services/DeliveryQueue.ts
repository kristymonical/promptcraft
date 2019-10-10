import ServiceResponse from './ServiceResponse';
import { toast } from 'react-toastify';

export async function getDeliveryQueue(
  poolId = 1
): Promise<GetDeliveryQueueResponse[]> {
  try {
    const res = await fetch(`/api/delivery-queue?poolId=${poolId}`);
    const json: ServiceResponse = await res.json();
    if (!json.success) {
      toast.error(`Unable to retrieve delivery queue for pool ${poolId}`);
      return [];
    }

    return json.data;
  } catch (err) {
    console.error('[getDeliveryQueue]:', err);
    throw err;
  }
}

export async function moveDeliveryInQueue(
  deliveryId: number,
  newParentId: number,
  newChildId: number,
  poolId = 1
) {
  try {
    await fetch(
      `/api/delivery/${deliveryId}/queue/priority?newParentDeliveryId=${newParentId}&newChildDeliveryId=${newChildId}&poolId=${poolId}`,
      { method: 'PUT' }
    );
  } catch (err) {
    console.error('[moveDeliveryInQueue]:', err);
    throw err;
  }
}

export async function moveDeliveryToTop(deliveryId: number, poolId = 1) {
  try {
    await fetch(
      `/api/delivery/${deliveryId}/queue/priority/top?poolId=${poolId}`,
      { method: 'PUT' }
    );
  } catch (err) {
    console.error('[moveDeliveryToTop]:', err);
    throw err;
  }
}

export async function moveDeliveryToBottom(deliveryId: number, poolId = 1) {
  try {
    await fetch(
      `/api/delivery/${deliveryId}/queue/priority/bottom?poolId=${poolId}`,
      { method: 'PUT' }
    );
  } catch (err) {
    console.error('[moveDeliveryToTop]:', err);
    throw err;
  }
}

export interface GetDeliveryQueueResponse {
  deliveryId: number;
  userId: string;
  cartId: string;
  orderId: string;
  currentLocation: string;
  reservedLocation: string;
  destinationArea: string;
  deliveryType: string;
}
