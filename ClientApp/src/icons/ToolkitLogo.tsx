import React from 'react';
import logo from './toolkitLogo.png';
import { makeStyles } from '@material-ui/core';
import { Link } from 'react-router-dom';

const useStyles = makeStyles({
  logoRoot: {
    alignItems: 'center',
    cursor: 'pointer',
    color: 'inherit',
    display: 'flex',
    '&:hover': {
      color: 'inherit',
      textDecoration: 'none'
    }
  },
  label: {
    fontSize: 22,
    fontWeight: 'bold'
  }
});

export default function ToolkitLogo() {
  const classes = useStyles({});
  return (
    <Link to='/' className={classes.logoRoot}>
      <img src={logo} alt='Toolkit Logo' />
      <span className={classes.label}>Toolkit</span>
    </Link>
  );
}
