import React from 'react';
import { makeStyles, Button, Typography } from '@material-ui/core';
import { fade } from '@material-ui/core/styles';

import { SVT_THEME } from '.';

type Variant = 'primary' | 'secondary';

interface SubmitButtonProps {
  disabled?: boolean;
  onClick?: () => void;
  text: string;
  variant: Variant;
}

const useStyles = makeStyles((theme: typeof SVT_THEME) => ({
  buttonRoot: {
    background: ({ variant }: { variant: Variant }) =>
      theme[variant].background,
    borderRadius: 6,
    color: ({ variant }: { variant: Variant }) => theme[variant].color,
    height: 70,
    width: 250,
    '&:hover': {
      background: ({ variant }: { variant: Variant }) =>
        fade(theme[variant].background, 0.75)
    }
  }
}));

export default function SubmitButton({
  disabled,
  onClick,
  text,
  variant
}: SubmitButtonProps) {
  const classes = useStyles({ variant });
  return (
    <Button
      className={classes.buttonRoot}
      variant='contained'
      disabled={disabled}
      onClick={onClick}
    >
      <Typography variant='body1'>{text}</Typography>
    </Button>
  );
}

SubmitButton.defaultProps = {
  disabled: false,
  onClick: Function,
  variant: 'primary'
};
