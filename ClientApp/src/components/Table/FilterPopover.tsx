import React, { useState, useRef } from 'react';
import {
  makeStyles,
  Popover,
  FormControlLabel,
  Checkbox
} from '@material-ui/core';
import { ArrowDropDown } from '@material-ui/icons';

interface FilterPopoverProps {
  filters: { value: string; checked: boolean }[];
  onToggleFilter: (filter: string) => void;
}

const createStyles = makeStyles({});

export default function FilterPopover({
  filters,
  onToggleFilter
}: FilterPopoverProps) {
  const classes = createStyles({});

  const [open, setOpen] = useState(false);
  const arrowRef = useRef(null);

  return (
    <>
      <ArrowDropDown onClick={() => setOpen(open => !open)} ref={arrowRef} />
      <Popover
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center'
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left'
        }}
        anchorEl={arrowRef.current}
        onClose={() => setOpen(false)}
        open={open}
      >
        {filters.map(({ value, checked }, idx) => (
          <FormControlLabel
            key={`filter-${idx}`}
            label={value}
            control={
              <Checkbox
                value={checked}
                onChange={() => onToggleFilter(value)}
              />
            }
          />
        ))}
      </Popover>
    </>
  );
}
