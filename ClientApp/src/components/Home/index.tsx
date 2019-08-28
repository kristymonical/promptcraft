import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { makeStyles } from '@material-ui/core';

import BigButton from './BigButton';

const useStyles = makeStyles({
  buttonGroup: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'center',
    margin: '0 auto',
    maxWidth: '800px'
  }
});

export default function Home() {
  const classes = useStyles({});
  return (
    <Container>
      <Row>
        <Col>Title</Col>
      </Row>
      <Row>
        <Col className={classes.buttonGroup}>
          <BigButton>Request</BigButton>
          <BigButton>Deliveries</BigButton>
          <BigButton variant='secondary'>Mapping</BigButton>
          <BigButton variant='secondary'>Configuration</BigButton>
        </Col>
      </Row>
    </Container>
  );
}
