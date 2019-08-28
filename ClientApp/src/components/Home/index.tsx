import React from 'react';
import { Typography } from '@material-ui/core';
import { Container, Row, Col } from 'react-bootstrap';

export default function Home() {
  return (
    <Container>
      <Row>
        <Col>Title</Col>
      </Row>
      <Row>
        <Col>Request Button</Col>
        <Col>Deliveries Button</Col>
      </Row>
      <Row>
        <Col>Mapping Button</Col>
        <Col>Configuration Button</Col>
      </Row>
    </Container>
  );
}
