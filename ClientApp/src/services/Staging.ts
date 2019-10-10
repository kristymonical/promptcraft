export async function createStagingRequest({
  cartIds,
  destinationArea,
  requestType
}: CreateStagingRequest) {
  try {
    const result = await fetch('/api/staging', {
      method: 'POST',
      body: JSON.stringify({}),
      headers: {
        'Content-Type': 'application/json'
      }
    });
    return (await result.json()) as GetStagedCartsResult[];
  } catch (err) {
    console.error('[createStagingRequest]:', err);
    throw err;
  }
}

export async function getStagedCarts() {
  try {
    const result = await fetch('/api/staging');
    return (await result.json()) as GetStagedCartsResult[];
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

export interface CreateStagingRequest {
  cartIds: string[];
  destinationArea: string;
  requestType: string;
}
