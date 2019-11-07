import uuid from 'uuid/v4';

export default async function(input: RequestInfo, options: RequestInit = {}) {
  // add trackingId header
  if (!options.headers) {
    options.headers = new Headers({ trackingId: uuid() });
  } else {
    options.headers = new Headers({ ...options.headers, trackingId: uuid() });
  }

  return fetch(input, options);
}
