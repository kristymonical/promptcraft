import React from 'react';
import { makeStyles, Button as MuiButton } from '@material-ui/core';
import { ButtonProps as MuiButtonProps } from '@material-ui/core/Button';
import { fade } from '@material-ui/core/styles';

import { SVT_THEME } from 'components';

interface ButtonProps {
  children: React.ReactNode;
  disabled?: boolean;
  fontSize?: number;
  maxHeight?: number;
  maxWidth?: number;
  onClick?: MuiButtonProps['onClick'];
  scale?: number;
  variant?: 'primary' | 'secondary';
}

type RequiredStyleProps = 'variant' | 'scale';
type StyleProps = Required<Pick<ButtonProps, RequiredStyleProps>> &
  Omit<Partial<ButtonProps>, RequiredStyleProps>;

const createStyles = makeStyles<typeof SVT_THEME, StyleProps>(theme => ({
  buttonRoot: {
    background: ({ variant }) => theme[variant].background,
    color: 'white',
    fontSize: ({ fontSize, scale }) =>
      fontSize || theme.initial.fontSize * scale,
    maxHeight: ({ maxHeight, scale }) => maxHeight || 40 * scale,
    maxWidth: ({ maxWidth, scale }) => maxWidth || 110 * scale,
    '&:hover': {
      background: ({ variant }) => fade(theme[variant].background, 0.75)
    }
  }
}));

export default function Button({
  children,
  disabled,
  fontSize,
  maxHeight,
  maxWidth,
  onClick,
  scale = 1,
  variant = 'secondary'
}: ButtonProps) {
  const classes = createStyles({
    fontSize,
    maxHeight,
    maxWidth,
    scale,
    variant
  });
  return (
    <MuiButton
      className={classes.buttonRoot}
      disabled={disabled}
      onClick={onClick}
      variant='contained'
    >
      {children}
    </MuiButton>
  );
}
