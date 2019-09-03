import React from 'react';
import { makeStyles } from '@material-ui/core';

import { SVT_THEME } from '../components/ThemeProvider';

const iconSize = 40;
// const iconPadding = iconSize / 7;
// const dotSpacing = 2; // maybe do math better to make this dynamic?
// const dotSize = (iconSize - 2 * iconPadding - 3 * dotSpacing) / 3;

// console.log(iconPadding);
// console.log(dotSize);
// console.log(dotSpacing);

// console.log(2 * iconPadding + 3 * dotSize + 3 * dotSpacing);

const useStyles = makeStyles(({ primary, secondary }: typeof SVT_THEME) => ({
  keypadIconRoot: {
    alignItems: 'center',
    background: primary.background,
    borderRadius: 3,
    cursor: 'pointer',
    display: 'flex',
    flexFlow: 'row wrap',
    height: iconSize,
    justifyContent: 'space-between',
    padding: 6,
    width: iconSize
  },
  dot: {
    background: 'white',
    borderRadius: 4,
    height: 8,
    width: 8
  }
}));

export default function KeypadIcon() {
  const classes = useStyles({});
  return (
    <span className={classes.keypadIconRoot}>
      {[...Array(9)].map((_, idx) => (
        <span key={idx} className={classes.dot}></span>
      ))}
    </span>
  );
}
