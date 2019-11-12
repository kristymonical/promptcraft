import { toast } from 'react-toastify';
import fetch from './FetchWrapper';
import ServiceResponse from './ServiceResponse';
import { createLog } from './Log';

export async function getDestinationAreas(
  locationName: string,
  type: 'deliver' | 'stage' | 'return' = 'deliver'
) {
  try {
    const result = await fetch(
      `/api/areas/destination?locationName=${locationName}&deliveryType=${type}`
    );

    const json: ServiceResponse<GetAreasResult[]> = await result.json();

    if (!json.success) {
      const message = `No valid destination areas for '${locationName}' found.`;
      toast.error(message);
      createLog({
        action: 'queue',
        deliveryId: -1,
        message,
        method: 'GET',
        route: result.url,
        statusCode: result.status,
        success: false,
        trackingId: result.headers.get('trackingId') || 'unknown'
      });
      return [] as GetAreasResult[];
    }

    return json.data;
  } catch (err) {
    console.error('[getDestinationAreas]:', err);
    throw err;
  }
}

// Types and stuff
export interface GetAreasResult {
  areaId: number;
  areaName: string;
}
