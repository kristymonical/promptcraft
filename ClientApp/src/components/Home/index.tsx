import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { makeStyles, Typography } from '@material-ui/core';

import BigButton from './BigButton';

const useStyles = makeStyles({
  buttonGroup: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'center',
    margin: '0 auto',
    maxWidth: '800px'
  },
  title: {
    display: 'flex',
    flexDirection: 'column',
    margin: '0 auto',
    maxWidth: '675px',
    textAlign: 'center'
  }
});

export default function Home() {
  const classes = useStyles({});
  return (
    <Container>
      <Row>
        <Col className={classes.title}>
          <Typography variant='h3'>Title</Typography>
          <Typography variant='subtitle1'>
            Lorem Ipsum is simply dummy text of the printing and typesetting
            industry. Lorem Ipsum has been the industry’s standard dummy
          </Typography>
        </Col>
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
