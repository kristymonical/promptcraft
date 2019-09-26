export async function createDeliveryRequest({
  cartId,
  cartLocation,
  destinationArea,
  orderNumber
}: CreateDeliveryRequest) {}

export async function getAreas() {
  try {
    const result = await fetch('/api/areas');
    return (await result.json()) as GetAreasResult[];
  } catch (err) {
    console.error('[getStagedCarts]:', err);
    return [] as GetAreasResult[];
  }
}

// Types and stuff
export interface GetAreasResult {
  areaId: number;
  areaName: string;
}

export interface CreateDeliveryRequest {
  cartId: string;
  cartLocation: string;
  destinationArea: string;
  orderNumber?: string;
}
