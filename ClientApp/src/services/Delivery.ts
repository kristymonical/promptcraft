export interface CreateDeliveryRequest {
  cartId: string;
  cartLocation: string;
  destinationArea: string;
  orderNumber?: string;
}
export async function createDeliveryRequest({
  cartId,
  cartLocation,
  destinationArea,
  orderNumber
}: CreateDeliveryRequest) {}
