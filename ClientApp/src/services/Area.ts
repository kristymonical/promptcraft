import { toast } from 'react-toastify';
import fetch from './FetchWrapper';

export async function getAreas() {
  try {
    const result = await fetch('/api/areas');
    return (await result.json()) as GetAreasResult[];
  } catch (err) {
    console.error('[getAreas]:', err);
    throw err;
  }
}

export async function getDestinationAreas(locationName: string) {
  try {
    const result = await fetch(
      `/api/areas/destination?locationName=${locationName}`
    );

    const json = await result.json();

    if (!json.success) {
      toast.error(`No valid destination areas for '${locationName}' found.`);
      return [] as GetAreasResult[];
    }

    return json.data as GetAreasResult[];
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
