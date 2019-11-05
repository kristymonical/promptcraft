import ServiceResponse from './ServiceResponse';
import { toast } from 'react-toastify';
import fetch from './FetchWrapper';

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
    const result = await fetch(
      `/api/delivery/${deliveryId}/queue/priority?newParentDeliveryId=${newParentId}&newChildDeliveryId=${newChildId}&poolId=${poolId}`,
      { method: 'PUT' }
    );
    const response: ServiceResponse = await result.json();

    if (!response.success) {
      toast.error(response.message || 'Unable to move item in queue');
    } else {
      toast.success('Delivery moved successfully');
    }
  } catch (err) {
    console.error('[moveDeliveryInQueue]:', err);
    throw err;
  }
}

export async function moveDeliveryToTop(deliveryId: number, poolId = 1) {
  try {
    const result = await fetch(
      `/api/delivery/${deliveryId}/queue/priority/top?poolId=${poolId}`,
      { method: 'PUT' }
    );

    const response: ServiceResponse = await result.json();

    if (!response.success) {
      toast.error(response.message || 'Unable to move item to top of queue');
    } else {
      toast.success('Delivery moved successfully');
    }
  } catch (err) {
    console.error('[moveDeliveryToTop]:', err);
    throw err;
  }
}

export async function moveDeliveryToBottom(deliveryId: number, poolId = 1) {
  try {
    const result = await fetch(
      `/api/delivery/${deliveryId}/queue/priority/bottom?poolId=${poolId}`,
      { method: 'PUT' }
    );

    const response: ServiceResponse = await result.json();

    if (!response.success) {
      toast.error(response.message || 'Unable to move item to bottom of queue');
    } else {
      toast.success('Delivery moved successfully');
    }
  } catch (err) {
    console.error('[moveDeliveryToBottom]:', err);
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
