import fetch from './FetchWrapper';

export async function createLog({
  trackingId,
  deliveryId,
  action,
  ...data
}: LogRequest) {
  try {
    await fetch('/api/log', {
      method: 'POST',
      body: JSON.stringify({
        action,
        data,
        deliveryId,
        trackingId
      }),
      headers: {
        'Content-Type': 'application/json'
      }
    });
  } catch (err) {
    console.error('[createLog]:', err);
    throw err;
  }
}

// Types and stuff
export interface LogRequest {
  action: 'clean' | 'move' | 'queue' | 'schedule' | 'status' | 'unknown';
  deliveryId: number;
  message: string;
  method: 'GET' | 'POST' | 'PUT' | 'DELETE';
  route: string;
  statusCode: number;
  success: boolean;
  trackingId: string;
}
