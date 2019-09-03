import React from 'react';
import logo from './toolkitLogo.png';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles({
  logoRoot: {
    display: 'flex',
    alignItems: 'center'
  },
  label: {
    fontSize: 22,
    fontWeight: 'bold'
  }
});

export default function ToolkitLogo() {
  const classes = useStyles({});
  return (
    <div className={classes.logoRoot}>
      <img src={logo} alt='Toolkit Logo' />
      <span className={classes.label}>Toolkit</span>
    </div>
  );
}
