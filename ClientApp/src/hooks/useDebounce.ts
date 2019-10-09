import { useCallback, useEffect, useRef, useState } from 'react';

function valueEquality<T>(left: T, right: T): boolean {
  return left === right;
}

export function useDebounce<T>(
  value: T,
  delay: number,
  options?: {
    maxWait?: number;
    leading?: boolean;
    equalityFn?: (left: T, right: T) => boolean;
  }
): [T, () => void] {
  const eq = options && options.equalityFn ? options.equalityFn : valueEquality;

  const [state, dispatch] = useState(value);
  const [callback, cancel] = useDebouncedCallback(
    useCallback(value => dispatch(value), []),
    delay,
    options
  );
  const previousValue = useRef(value);

  useEffect(() => {
    // We need to use this condition otherwise we will run debounce timer for the first render (including maxWait option)
    if (!eq(previousValue.current, value)) {
      callback(value);
      previousValue.current = value;
    }
  }, [value, callback, eq]);

  return [state, cancel];
}

const NOOP_TIMEOUT = setTimeout(() => {}, 1e9); // 1e9 ms noop timeout placeholder

export function useDebouncedCallback<T extends (...args: any[]) => any>(
  callback: T,
  delay: number,
  options: { maxWait?: number; leading?: boolean } = {}
): [T, () => void, () => void] {
  const maxWait = options.maxWait;
  const maxWaitHandler = useRef(NOOP_TIMEOUT);
  const maxWaitArgs: { current: any[] } = useRef([]);

  const leading = options.leading;
  const wasLeadingCalled: { current: boolean } = useRef(false);

  const functionTimeoutHandler = useRef(NOOP_TIMEOUT);
  const isComponentUnmounted: { current: boolean } = useRef(false);

  const debouncedFunction = useRef(callback);
  debouncedFunction.current = callback;

  const cancelDebouncedCallback: () => void = useCallback(() => {
    clearTimeout(functionTimeoutHandler.current);
    clearTimeout(maxWaitHandler.current);
    maxWaitHandler.current = NOOP_TIMEOUT;
    maxWaitArgs.current = [];
    functionTimeoutHandler.current = NOOP_TIMEOUT;
    wasLeadingCalled.current = false;
  }, []);

  useEffect(
    () => () => {
      // we use flag, as we allow to call callPending outside the hook
      isComponentUnmounted.current = true;
    },
    []
  );

  const debouncedCallback = useCallback(
    (...args) => {
      maxWaitArgs.current = args;
      clearTimeout(functionTimeoutHandler.current);

      if (
        !functionTimeoutHandler.current &&
        leading &&
        !wasLeadingCalled.current
      ) {
        debouncedFunction.current(...args);
        wasLeadingCalled.current = true;
        return;
      }

      functionTimeoutHandler.current = setTimeout(() => {
        cancelDebouncedCallback();

        if (!isComponentUnmounted.current) {
          debouncedFunction.current(...args);
        }
      }, delay);

      if (maxWait && !maxWaitHandler.current) {
        maxWaitHandler.current = setTimeout(() => {
          const args = maxWaitArgs.current;
          cancelDebouncedCallback();

          if (!isComponentUnmounted.current) {
            debouncedFunction.current.apply(null, args);
          }
        }, maxWait);
      }
    },
    [maxWait, delay, cancelDebouncedCallback, leading]
  );

  const callPending = () => {
    // Call pending callback only if we have anything in our queue
    if (!functionTimeoutHandler.current) {
      return;
    }

    debouncedFunction.current.apply(null, maxWaitArgs.current);
    cancelDebouncedCallback();
  };

  // At the moment, we use 3 args array so that we save backward compatibility
  return [debouncedCallback as T, cancelDebouncedCallback, callPending];
}
