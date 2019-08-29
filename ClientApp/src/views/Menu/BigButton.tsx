import React from 'react';
import { makeStyles, Typography } from '@material-ui/core';

import { SVT_THEME } from '../../components/ThemeProvider';

interface BigButtonProps {
  variant?: 'primary' | 'secondary';
  children: any;
}

const useStyles = makeStyles(({ primary, secondary }: typeof SVT_THEME) => ({
  root: {
    alignItems: 'center',
    background: ({ variant }: BigButtonProps) =>
      variant === 'primary' ? primary.background : secondary.background,
    borderRadius: '6px',
    color: 'white',
    cursor: 'pointer',
    display: 'flex',
    height: '130px',
    justifyContent: 'center',
    margin: '10px',
    textTransform: 'uppercase',
    userSelect: 'none',
    width: '330px'
  }
}));

function BigButton(props: BigButtonProps) {
  const classes = useStyles(props);
  return (
    <div className={classes.root}>
      <Typography variant='h5'>{props.children}</Typography>
    </div>
  );
}

BigButton.defaultProps = {
  variant: 'primary'
};

export default BigButton;
