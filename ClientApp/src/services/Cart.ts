export async function moveCart(cartId: string, location: string) {}

export async function getOrderAndDestination(
  cartId: string,
  currentLocationName: string
): Promise<GetOrderAndDestinationResponse> {
  try {
    const result = await fetch(
      `/api/cleaninfo?MalLocationName=${currentLocationName}&CartId=${cartId}`
    );
    return await result.json();
  } catch (err) {
    console.error('[getStagedCarts]:', err);
    return { destinationAreaName: '', orderId: '' };
  }
}

export interface GetOrderAndDestinationResponse {
  destinationAreaName: string;
  orderId: string;
}
