import React, { useState, useRef } from 'react';
import {
  makeStyles,
  Popover,
  FormControlLabel,
  Checkbox
} from '@material-ui/core';
import { ArrowDropDown } from '@material-ui/icons';
import { Container } from 'react-bootstrap';

interface FilterPopoverProps {
  filters: {
    [value: string]: boolean;
  };
  onToggleFilter: (filter: string) => void;
}

const createStyles = makeStyles({
  popoverContainer: {
    display: 'flex',
    flexDirection: 'column'
  }
});

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
        <Container className={classes.popoverContainer}>
          {Object.keys(filters)
            .filter(key => !['undefined', 'null'].includes(key)) // filter out blank options
            .map((value: string, idx) => (
              <FormControlLabel
                key={`filter-${idx}`}
                label={value}
                control={
                  <Checkbox
                    checked={filters[value]}
                    onChange={() => onToggleFilter(value)}
                  />
                }
              />
            ))}
        </Container>
      </Popover>
    </>
  );
}
