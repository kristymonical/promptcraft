import React from 'react';
import { makeStyles, Typography } from '@material-ui/core';

import { SVT_THEME } from '../../components/ThemeProvider';
import { fade } from '@material-ui/core/styles';

interface BigButtonProps {
  variant?: 'primary' | 'secondary';
  children: any;
}

const useStyles = makeStyles(({ primary, secondary }: typeof SVT_THEME) => ({
  bigButtonRoot: {
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
    width: '330px',
    '&:hover': {
      background: ({ variant }: BigButtonProps) =>
        fade(
          variant === 'primary' ? primary.background : secondary.background,
          0.75
        )
    }
  }
}));

export default function BigButton(props: BigButtonProps) {
  const classes = useStyles(props);
  return (
    <div className={classes.bigButtonRoot}>
      <Typography variant='h5'>{props.children}</Typography>
    </div>
  );
}

BigButton.defaultProps = {
  variant: 'primary'
};
