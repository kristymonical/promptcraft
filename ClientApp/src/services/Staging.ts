export interface CreateStagingRequest {
  cartIds: string[];
  destinationArea: string;
  requestType: string;
}
export async function createStagingRequest({
  cartIds,
  destinationArea,
  requestType
}: CreateStagingRequest) {}

export async function getStagedCarts() {
  try {
    const result = await fetch('/api/staging');
    return (await result.json()) as GetStagedCartsResult[];
  } catch (err) {
    console.error('[getStagedCarts]:', err);
    return [] as GetStagedCartsResult[];
  }
}

export interface GetStagedCartsResult {
  orderId: string;
  cartId: string;
  stagingLocationId: string;
  deliveryRequestType: string;
  destinationArea: string;
}
