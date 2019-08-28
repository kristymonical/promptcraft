import React from 'react';
import { Col } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';

const useStyles = makeStyles({
  title: {
    display: 'flex',
    flexDirection: 'column',
    margin: '0 auto',
    maxWidth: '675px',
    textAlign: 'center'
  }
});

interface TitleColProps {
  title: string;
  subtitle?: string;
}

export default function TitleCol({ title, subtitle }: TitleColProps) {
  const classes = useStyles({});
  return (
    <Col className={classes.title}>
      <Typography variant='h3'>{title}</Typography>
      {subtitle && <Typography variant='subtitle1'>{subtitle}</Typography>}
    </Col>
  );
}
