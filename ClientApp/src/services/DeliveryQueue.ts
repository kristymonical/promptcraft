export async function getDeliveryQueue(
  poolId = 1
): Promise<GetDeliveryQueueResponse[]> {
  try {
    const res = await fetch(`/api/delivery-queue?poolId=${poolId}`);
    return await res.json();
  } catch (err) {
    console.error('[getDeliveryQueue]:', err);
  }

  return [];
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
