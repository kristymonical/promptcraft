import React, { useState } from 'react';
import { makeStyles, TextField, Typography } from '@material-ui/core';

import { SVT_THEME } from 'components';
import ScanButton from './ScanButton';

interface AutoCompleteProps {
  label?: string;
  maxWidth?: string;
  onSelect: (value: string) => void;
  options: Option[];
  required?: boolean;
  scannable?: boolean;
  value: string;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<AutoCompleteProps>>({
  autoCompleteRoot: {
    display: 'flex',
    flexDirection: 'column',
    maxWidth: ({ maxWidth }) => maxWidth || '100%',
    justifyContent: 'space-between',
    '& > *': {
      marginBottom: 10
    }
  }
});

interface Option {
  value: string;
}

export default function AutoComplete({
  label,
  required = false
}: AutoCompleteProps) {
  const [localValue, setLocalValue] = useState('');
  //   const [] = useState(false);
  const classes = createStyles({});

  return (
    <div className={classes.autoCompleteRoot}>
      <Typography component='span'>
        {label}{' '}
        {required && (
          <Typography component='span' color='error'>
            *REQUIRED
          </Typography>
        )}
      </Typography>
      <TextField
        className={classes.textField}
        value={localValue}
        onChange={({ target: { value } }) => setLocalValue(value)}
        variant='outlined'
      />
      <ScanButton onScan={scanned => setLocalValue(scanned)} />
    </div>
  );
}
