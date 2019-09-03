import React from 'react';
import { makeStyles } from '@material-ui/core';

import { SVT_THEME } from '../components/ThemeProvider';
import Circle from '../components/Circle';

interface KeypadIconProps {
  size: number;
}

// TODO: Test if these scale responsively
// ratios for responsiveness (values derived from original static component: 40px icon size, 6px padding, 8px dot size)
const paddingRatio = 40 / 6;
const dotSizeRatio = 40 / 8;

const useStyles = makeStyles(({ primary, secondary }: typeof SVT_THEME) => ({
  keypadIconRoot: {
    alignItems: 'center',
    background: primary.background,
    borderRadius: 3,
    cursor: 'pointer',
    display: 'flex',
    flexFlow: 'row wrap',
    height: (props: KeypadIconProps) => props.size,
    justifyContent: 'space-between',
    padding: (props: KeypadIconProps) => props.size / paddingRatio,
    width: (props: KeypadIconProps) => props.size
  }
}));

export default function KeypadIcon(props: KeypadIconProps) {
  const classes = useStyles(props);
  return (
    <span className={classes.keypadIconRoot}>
      {/* spread thing is so TS transpiles this correctly since mapping directly off of Array() doesn't seem to work */}
      {[...Array(9)].map((_, idx) => (
        <Circle key={idx} color='white' size={props.size / dotSizeRatio} />
      ))}
    </span>
  );
}

KeypadIcon.defaultProps = {
  size: 40
};
