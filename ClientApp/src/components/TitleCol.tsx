import React from 'react';
import { Col } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';

const useStyles = makeStyles({
  titleRoot: {
    display: 'flex',
    flexDirection: 'column',
    margin: '0 auto',
    textAlign: 'center',
    '& > *': {
      marginBottom: 10
    }
  }
});

interface TitleColProps {
  children?: React.ReactNode;
  title: string;
}

export default function TitleCol({ children, title }: TitleColProps) {
  const classes = useStyles({});
  return (
    <Col className={classes.titleRoot}>
      <Typography variant='h3'>{title}</Typography>
      {children}
    </Col>
  );
}
