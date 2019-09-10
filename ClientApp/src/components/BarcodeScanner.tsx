// OG lifted from https://github.com/kybarg/react-barcode-reader (MIT License)

import React, { useEffect, useState } from 'react';

function isContentEditable(element: HTMLElement) {
  if (typeof element.getAttribute !== 'function') {
    return false;
  }

  return !!element.getAttribute('contenteditable');
}

function isInput(element: HTMLElement) {
  if (!element) {
    return false;
  }

  const { tagName } = element;
  const editable = isContentEditable(element);

  return tagName === 'INPUT' || tagName === 'TEXTAREA' || editable;
}

function inIframe() {
  try {
    return window.self !== window.top;
  } catch (e) {
    return true;
  }
}

interface BarcodeScannerProps {
  avgTimeByChar: number; // Average time (ms) between 2 chars. Used to do difference between keyboard typing and scanning
  endChar: number[]; // ASCII value of Chars to remove and means end of scanning
  minLength: number; // Minimum length for a scanning
  onScan: (scanned: string, numScanned: number) => void; // Callback after detection of a successfull scanning (scanned string in parameter)
  onError: (scanned: string, error: string) => void; // Callback after detection of a unsuccessfull scanning (scanned string in parameter)
  onKeyDetect: (event: KeyboardEvent) => void; // Callback after detecting a keyDown (key char in parameter) - in contrast to onReceive, this fires for non-character keys like tab, arrows, etc. too!
  onReceive: (event: KeyboardEvent) => void; // Callback after receiving and processing a char (scanned char in parameter)
  onScanButtonLongPressed: (scanned: string, numScanned: number) => void; // Callback after detection of a successfull scan while the scan button was pressed and held down
  preventDefault: boolean; // Prevent default action on keypress event
  scanButtonKeyCode: number; // Key code of the scanner hardware button (if the scanner button a acts as a key itself)
  scanButtonLongPressThreshold: number; // How many times the hardware button should issue a pressed event before a barcode is read to detect a longpress
  startChar: number[]; // ASCII value of Chars to remove and means start of scanning
  stopPropagation: boolean; // Stop immediate propagation on keypress event
  timeBeforeScanTest: number; // Wait duration (ms) after keypress event to check if scanning is finished
}

export default function BarcodeScanner({
  avgTimeByChar,
  endChar,
  minLength,
  onScanButtonLongPressed,
  onKeyDetect,
  onReceive,
  onScan,
  onError,
  preventDefault,
  scanButtonKeyCode,
  scanButtonLongPressThreshold,
  startChar,
  stopPropagation
}: BarcodeScannerProps) {
  const [callIsScanner, setCallIsScanner] = useState(false);
  const [firstCharTime, setFirstCharTime] = useState(0);
  const [lastCharTime, setLastCharTime] = useState(0);
  const [stringWriting, setStringWriting] = useState('');
  const [scanButtonCounter, setScanButtonCounter] = useState(0);

  useEffect(() => {
    const scannerDetectionTest = (testCode: string) => {
      // If string is given, test it
      if (testCode) {
        setFirstCharTime(0);
        setLastCharTime(0);
        setStringWriting(testCode);
      }

      if (!scanButtonCounter) {
        setScanButtonCounter(1);
      }

      // If all condition are good (length, time...), call the callback and re-initialize the plugin for next scanning
      // Else, just re-initialize
      if (
        stringWriting.length >= minLength &&
        lastCharTime - firstCharTime < stringWriting.length * avgTimeByChar
      ) {
        if (
          onScanButtonLongPressed &&
          scanButtonCounter > scanButtonLongPressThreshold
        )
          onScanButtonLongPressed(stringWriting, scanButtonCounter);
        else if (onScan) onScan(stringWriting, scanButtonCounter);

        initScannerDetection();
        return true;
      }

      let errorMsg = '';
      if (stringWriting.length < minLength) {
        errorMsg = `String length should be greater or equal ${minLength}`;
      } else {
        if (
          lastCharTime - firstCharTime >
          stringWriting.length * avgTimeByChar
        ) {
          errorMsg = `Average key character time should be less or equal ${avgTimeByChar}ms`;
        }
      }

      if (onError) onError(stringWriting, errorMsg);
      initScannerDetection();
      return false;
    };
    const handleKeyPress = (event: KeyboardEvent) => {
      if (event.target instanceof HTMLElement && isInput(event.target)) {
        return;
      }

      // If it's just the button of the scanner, ignore it and wait for the real input
      if (scanButtonKeyCode && event.which === scanButtonKeyCode) {
        setScanButtonCounter(counter => counter + 1);
        // Cancel default
        event.preventDefault();
        event.stopImmediatePropagation();
      }
      // Fire keyDetect event in any case!
      if (onKeyDetect) onKeyDetect(event);

      if (stopPropagation) event.stopImmediatePropagation();
      if (preventDefault) event.preventDefault();

      if (firstCharTime && endChar.includes(event.which)) {
        event.preventDefault();
        event.stopImmediatePropagation();
        setCallIsScanner(true);
      } else if (!firstCharTime && startChar.includes(event.which)) {
        event.preventDefault();
        event.stopImmediatePropagation();
        setCallIsScanner(false);
      } else {
        if (typeof event.which !== 'undefined') {
          setStringWriting(sw => `${sw}${String.fromCharCode(event.which)}`);
        }
        setCallIsScanner(false);
      }

      if (!firstCharTime) {
        setFirstCharTime(Date.now());
      }

      setLastCharTime(Date.now());

      if (callIsScanner) {
        scannerDetectionTest('');
      }

      if (onReceive) onReceive(event);
    };

    if (inIframe)
      window.parent.document.addEventListener('keypress', handleKeyPress);
    window.document.addEventListener('keypress', handleKeyPress);

    return () => {
      if (inIframe)
        window.parent.document.removeEventListener('keypress', handleKeyPress);
      window.document.removeEventListener('keypress', handleKeyPress);
    };
  }, []);

  const initScannerDetection = () => {
    setFirstCharTime(0);
    setStringWriting('');
    setScanButtonCounter(0);
  };

  return null;
}

BarcodeScanner.defaultProps = {
  avgTimeByChar: 30,
  endChar: [9, 13],
  minLength: 6,
  preventDefault: false,
  timeBeforeScanTest: 100,
  scanButtonLongPressThreshold: 3,
  startChar: [],
  stopPropagation: false
};
