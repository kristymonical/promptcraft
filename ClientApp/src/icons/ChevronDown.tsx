import React from 'react';
import { makeStyles } from '@material-ui/core';

import { SVT_THEME } from 'components';

interface ChevronDownProps {
  color?: string;
  size?: number;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<ChevronDownProps>>({
  chevronDownRoot: {
    background: ({ color }) => color || 'white',
    height: ({ size }) => size || 10,
    width: ({ size }) => size || 10,
    transform: 'matrix(-1, 0, 0, -1, 0, 0)'
  }
});

export default function ChevronDown(props: ChevronDownProps) {
  const classes = createStyles(props);
  return <span className={classes.chevronDownRoot}></span>;
}
