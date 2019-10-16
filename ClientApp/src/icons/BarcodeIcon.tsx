import React from 'react';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles({
  barcodeLine: {
    fill: 'none',
    stroke: ({ color }: { color?: string }) => color,
    strokeWidth: 2
  }
});

interface BarcodeIconProps {
  color?: string;
  height?: number;
  width?: number;
}

export default function BarcodeIcon({
  color,
  width,
  height
}: BarcodeIconProps) {
  const classes = useStyles({ color });
  return (
    <svg
      width={`${width}px`}
      height={`${height}px`}
      preserveAspectRatio='xMidYMid meet'
      viewBox='0 0 24 20'
      xmlns='http://www.w3.org/2000/svg'
    >
      <polyline className={classes.barcodeLine} points='4,0 0,0 0,20 4,20' />
      <polyline className={classes.barcodeLine} points='4,6 4,14' />
      <polyline className={classes.barcodeLine} points='8,4 8,16' />
      <polyline className={classes.barcodeLine} points='12,6 12,14' />
      <polyline className={classes.barcodeLine} points='16,4 16,16' />
      <polyline className={classes.barcodeLine} points='20,6 20,14' />
      <polyline
        className={classes.barcodeLine}
        points='20,0 24,0 24,20 20,20'
      />
    </svg>
  );
}

BarcodeIcon.defaultProps = {
  color: 'white',
  height: 20,
  width: 24
};
