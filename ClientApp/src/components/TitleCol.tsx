import React from 'react';
import { Col } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';

const useStyles = makeStyles({
  titleRoot: {
    display: 'flex',
    flexDirection: 'column',
    margin: '0 auto',
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
    <Col className={classes.titleRoot}>
      <Typography variant='h3'>{title}</Typography>
      {subtitle && <Typography variant='subtitle1'>{subtitle}</Typography>}
    </Col>
  );
}
