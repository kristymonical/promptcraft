import React from 'react';
import { Col } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';

import { SVT_THEME } from 'components';

const useStyles = makeStyles<typeof SVT_THEME, Partial<TitleColProps>>(
  theme => ({
    titleRoot: {
      display: 'flex',
      flexDirection: 'column',
      margin: '0 auto',
      textAlign: 'center',
      '& > *': {
        marginBottom: theme.flex.verticalSpacing
      },
      '& > :last-child': {
        marginBottom: theme.flex.verticalSpacing * 3
      }
    }
  })
);

interface TitleColProps {
  children?: React.ReactNode;
  title: string;
}

export default function TitleCol({ children, title }: TitleColProps) {
  const classes = useStyles({ children });
  return (
    <Col className={classes.titleRoot}>
      <Typography variant='h3'>{title}</Typography>
      {children}
    </Col>
  );
}
