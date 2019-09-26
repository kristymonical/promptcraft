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

export async function moveCart(cartId: string, malLocationName: string) {
  try {
    await fetch('/api/move', {
      method: 'POST',
      body: JSON.stringify({
        malLocationName,
        cartId
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });
  } catch (err) {
    console.error('[moveCart]:', err);
  }
}

export interface GetOrderAndDestinationResponse {
  destinationAreaName: string;
  orderId: string;
}
