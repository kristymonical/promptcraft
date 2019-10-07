import React from 'react';
import { makeStyles, CircularProgress } from '@material-ui/core';

import { SVT_THEME } from 'components';
import { fade } from '@material-ui/core/styles';

interface OverlayProps {}

const createStyles = makeStyles<typeof SVT_THEME, Partial<OverlayProps>>(
  ({ primary, secondary }) => ({
    overlayRoot: {
      alignItems: 'center',
      background: fade(primary.background, 0.85),
      borderRadius: 5,
      display: 'flex',
      justifyContent: 'center',
      position: 'absolute',
      left: 0,
      top: 0,
      right: 0,
      bottom: 0,
      width: 'inherit',
      height: 'inherit',
      margin: -5
    },
    spinner: {
      color: secondary.background
    }
  })
);

export default function Overlay({  }: OverlayProps) {
  const classes = createStyles({});
  return (
    <div className={classes.overlayRoot}>
      <CircularProgress className={classes.spinner} />
    </div>
  );
}
