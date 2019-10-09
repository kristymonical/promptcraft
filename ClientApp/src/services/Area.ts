export async function getAreas() {
  try {
    const result = await fetch('/api/areas');
    return (await result.json()) as GetAreasResult[];
  } catch (err) {
    console.error('[getAreas]:', err);
    return [] as GetAreasResult[];
  }
}

export async function getDestinationAreas(locationName: string) {
  try {
    const result = await fetch(
      `/api/areas/destination?locationName=${locationName}`
    );
    return (await result.json()).data as GetAreasResult[];
  } catch (err) {
    console.error('[getDestinationAreas]:', err);
    return [] as GetAreasResult[];
  }
}

// Types and stuff
export interface GetAreasResult {
  areaId: number;
  areaName: string;
}
