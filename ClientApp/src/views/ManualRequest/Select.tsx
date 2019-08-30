import React from 'react';
import { Typography, makeStyles, MenuItem, TextField } from '@material-ui/core';

import { SVT_THEME } from '../../components/ThemeProvider';

interface SelectProps {
  handleChange: (value: string) => void;
  items: string[];
  label: string;
  maxWidth?: number;
  required?: boolean;
  value: string;
}

const useStyles = makeStyles(({ secondary }: typeof SVT_THEME) => ({
  selectRoot: {
    display: 'flex',
    flexDirection: 'column',
    maxWidth: ({ maxWidth }: SelectProps) => maxWidth,
    '& > *': {
      marginBottom: 10
    }
  },
  select: {
    background: 'white'
  }
}));

function Select(props: SelectProps) {
  const classes = useStyles(props);
  const { handleChange, items, label, required, value } = props;

  return (
    <div className={classes.selectRoot}>
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

export default Select;
