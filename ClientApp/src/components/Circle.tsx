import React from 'react';
import { makeStyles } from '@material-ui/core';

import { SVT_THEME } from './ThemeProvider';

interface CircleProps {
  color: string;
  size: number;
}

const useStyles = makeStyles(({ primary, secondary }: typeof SVT_THEME) => ({
  circle: {
    background: (props: CircleProps) => props.color,
    height: (props: CircleProps) => props.size,
    width: (props: CircleProps) => props.size,
    borderRadius: (props: CircleProps) => props.size / 2 // border radius is half of size to make circle
  }
}));

export default function Circle(props: CircleProps) {
  const classes = useStyles(props);
  return <span className={classes.circle} />;
}
