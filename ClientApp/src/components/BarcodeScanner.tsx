import { useEffect, useState } from 'react';

const timeBetweenCharacters = 5; // Average time (ms) between 2 chars used to do differentiate between keyboard typing and scanning
const terminalCharacter = 13; // Terminal character: 13 === '\r'

interface BarcodeScannerProps {
  onScan: (scanned: string) => void;
}

export default function BarcodeScanner({ onScan }: BarcodeScannerProps) {
  const [firstCharInputTime, setFirstCharInputTime] = useState(0);
  const [lastCharInputTime, setLastCharInputTime] = useState(0);
  const [scannedString, setScannedString] = useState('');

  useEffect(() => {
    const handleKeyPress = (event: KeyboardEvent) => {
      event.preventDefault(); // don't want keypresses to actually do anything here
      if (event.which !== terminalCharacter) {
        const character = String.fromCharCode(event.which); // get character from ascii value
        setScannedString(current => `${current}${character}`); // add character
      } else {
        setLastCharInputTime(Date.now()); // terminal character found
      }
    };

    window.document.addEventListener('keypress', handleKeyPress);

    return () => {
      window.document.removeEventListener('keypress', handleKeyPress);
    };
  }, []);

  useEffect(() => {
    // string was reset, reset our state
    if (scannedString.length === 0) {
      setFirstCharInputTime(0);
      setLastCharInputTime(0);
    } else if (scannedString.length === 1) {
      setFirstCharInputTime(Date.now()); // first character added
    } else {
      const timeDiff = lastCharInputTime - firstCharInputTime;
      if (
        timeDiff >= 0 &&
        timeDiff <= scannedString.length * timeBetweenCharacters
      ) {
        onScan(scannedString);
        setScannedString('');
      }
    }
  }, [scannedString, firstCharInputTime, lastCharInputTime, onScan]);

  return null;
}
