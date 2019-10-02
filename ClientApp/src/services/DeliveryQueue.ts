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
