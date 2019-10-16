import React, { useState, useEffect } from 'react';
import { TextField, makeStyles, Typography } from '@material-ui/core';
import { fade } from '@material-ui/core/styles';

import { SVT_THEME } from 'components';
import ScanButton from './ScanButton';

interface ScannableTextFieldProps {
  label: string;
  handleChange: (value: string) => void;
  maxWidth?: number;
  required: boolean;
  value: string;
}

const useStyles = makeStyles(({ secondary }: typeof SVT_THEME) => ({
  scannableTextField: {
    display: 'flex',
    flexDirection: 'column',
    maxWidth: ({ maxWidth }: ScannableTextFieldProps) => maxWidth,
    justifyContent: 'space-between',
    '& > *': {
      marginBottom: 10
    }
  },
  button: {
    background: secondary.background,
    color: 'white',
    maxHeight: 40,
    maxWidth: 110,
    '&:hover': {
      background: fade(secondary.background, 0.75)
    }
  }
}));

export default function ScannableTextField(props: ScannableTextFieldProps) {
  const classes = useStyles(props);
  const { label, handleChange, required, value } = props;
  const [localValue, setValue] = useState(`${value || ''}`);

  useEffect(() => setValue(value), [value]);
  return (
    <div className={classes.scannableTextField}>
      <Typography component='span'>
        {label}{' '}
        {required && (
          <Typography component='span' color='error'>
            *REQUIRED
          </Typography>
        )}
      </Typography>
      <TextField
        value={localValue}
        onChange={({ target: { value } }) => {
          setValue(value);
          handleChange(value);
        }}
        variant='outlined'
      />
      <ScanButton
        onScan={scanned => {
          setValue(scanned);
          handleChange(scanned);
        }}
      />
    </div>
  );
}

ScannableTextField.defaultProps = {
  maxWidth: 400,
  required: false
};
