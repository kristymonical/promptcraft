import React from 'react';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles({
  barcodeLine: {
    fill: 'none',
    stroke: 'white',
    strokeWidth: 2
  }
});

export default function BarcodeIcon() {
  const classes = useStyles({});

  return (
    <svg width='24px' height='20px' xmlns='http://www.w3.org/2000/svg'>
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
