export async function createDeliveryRequest({
  cartId,
  cartLocation,
  destinationArea,
  orderNumber
}: CreateDeliveryRequest) {
  try {
    await fetch('/api/delivery', {
      method: 'POST',
      body: JSON.stringify([
        {
          orderId: orderNumber,
          cartId,
          location: cartLocation,
          destinationArea
        }
      ]),
      headers: {
        'Content-Type': 'application/json'
      }
    });
  } catch (err) {
    console.error('[createDeliveryRequest]:', err);
  }
}

// Types and stuff
export interface CreateDeliveryRequest {
  cartId: string;
  cartLocation: string;
  destinationArea: string;
  orderNumber?: string;
}
