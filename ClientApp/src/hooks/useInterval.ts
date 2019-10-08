/*

    Interval hook written by Dan the man himself
    https://overreacted.io/making-setinterval-declarative-with-react-hooks/

 */

import * as React from 'react';

const { useEffect, useRef } = React;

type IntervalFunction = () => unknown | void;

export default function useInterval(callback: IntervalFunction, delay: number) {
  const savedCallback = useRef<IntervalFunction | null>(null);

  // Remember the latest callback.
  useEffect(() => {
    savedCallback.current = callback;
  });

  // Set up the interval.
  useEffect(() => {
    function tick() {
      if (savedCallback.current !== null) {
        savedCallback.current();
      }
    }
    const id = setInterval(tick, delay);
    return () => clearInterval(id);
  }, [delay]);
}
