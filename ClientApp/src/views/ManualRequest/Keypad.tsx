import React, { useState } from 'react';
import { Typography, Button } from '@material-ui/core';
import { makeStyles } from '@material-ui/styles';

import { SVT_THEME } from 'components';
import { XIcon, BackIcon } from 'icons';

interface KeypadProps {
  initialValue?: string;
  onCancel: () => void;
  onConfirm: (value: string) => void;
  title: string;
}

const keySize = 60;

// TOOD: responsiveness instead of pixels
const useStyles = makeStyles(
  ({ flex, primary, secondary }: typeof SVT_THEME) => ({
    keypadRoot: {
      alignItems: 'center',
      display: 'flex',
      flexDirection: 'column',
      margin: '0 auto',
      width: 915,
      '& > *': {
        marginBottom: flex.verticalSpacing
      }
    },
    inputDisplay: {
      background: 'white',
      border: '1px solid #D5D5D5',
      borderRadius: 3,
      height: 50,
      padding: 16,
      width: 250
    },
    keysContainer: {
      color: 'white',
      display: 'flex',
      flexDirection: 'row',
      flexWrap: 'wrap',
      justifyContent: 'space-between',
      maxWidth: 215
    },
    key: {
      alignItems: 'center',
      background: primary.background,
      borderRadius: keySize / 2,
      display: 'flex',
      height: keySize,
      justifyContent: 'center',
      marginBottom: flex.verticalSpacing,
      width: keySize
    },
    buttonContainer: {
      display: 'flex',
      justifyContent: 'space-around',
      width: 250
    },
    button: {
      background: secondary.background,
      color: secondary.color
    }
  })
);

const keys = [1, 2, 3, 4, 5, 6, 7, 8, 9, 'clear', 0, 'backspace'];

export default function Keypad({
  initialValue = '',
  onCancel,
  onConfirm,
  title
}: KeypadProps) {
  const classes = useStyles({});
  const [value, setValue] = useState(initialValue);

  const handleKeyPress = (key: number | string) => {
    if (typeof key === 'number') {
      return setValue(`${value}${key}`);
    }

    if (key === 'clear') {
      return setValue('');
    }

    if (key === 'backspace') {
      return setValue(value.slice(0, -1));
    }
  };

  const renderKey = (key: string | number) => {
    if (typeof key === 'number') {
      return key;
    }

    if (key === 'clear') {
      return <XIcon />;
    }

    if (key === 'backspace') {
      return <BackIcon />;
    }
  };

  return (
    <div className={classes.keypadRoot}>
      <Typography variant='h3'>{title}</Typography>
      <div className={classes.inputDisplay}>{value}</div>
      <div className={classes.keysContainer}>
        {keys.map((key, idx) => (
          <div
            className={classes.key}
            key={`keypad-key-${idx}`}
            onClick={() => handleKeyPress(key)}
          >
            <Typography variant='h5'>{renderKey(key)}</Typography>
          </div>
        ))}
      </div>
      <div className={classes.buttonContainer}>
        <Button className={classes.button} onClick={onCancel}>
          Cancel
        </Button>
        <Button className={classes.button} onClick={() => onConfirm(value)}>
          Confirm
        </Button>
      </div>
    </div>
  );
}
