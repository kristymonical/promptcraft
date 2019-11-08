import React, { ReactNode } from 'react';
import { Typography, makeStyles, MenuItem, TextField } from '@material-ui/core';

import { SVT_THEME } from 'components';

interface SelectProps {
  className?: string;
  direction?: 'column' | 'row';
  handleChange: (value: string) => void;
  items: string[];
  label: string | ReactNode;
  maxWidth?: number;
  required?: boolean;
  value: string;
}

const useStyles = makeStyles<typeof SVT_THEME, Partial<SelectProps>>(() => ({
  selectRoot: {
    display: 'flex',
    flexDirection: ({ direction }) => direction || 'column',
    maxWidth: ({ maxWidth }) => maxWidth,
    '& > *': {
      marginBottom: 10
    }
  },
  select: {
    background: 'white'
  }
}));

export default function Select({
  className,
  direction,
  handleChange,
  items,
  label,
  maxWidth,
  required,
  value
}: SelectProps) {
  const classes = useStyles({ maxWidth, direction });

  return (
    <div className={`${classes.selectRoot} ${className}`}>
      <Typography component='span'>
        {label}{' '}
        {required && (
          <Typography component='span' color='error'>
            *REQUIRED
          </Typography>
        )}
      </Typography>
      <TextField
        select
        className={classes.select}
        onChange={({ target: { value } }) => handleChange(value)}
        value={value}
        variant='outlined'
      >
        {items.map((item, idx) => (
          <MenuItem key={`select-item-${idx}`} value={item} dense>
            {item}
          </MenuItem>
        ))}
      </TextField>
    </div>
  );
}

Select.defaultProps = {
  required: false,
  maxWidth: 275
};
