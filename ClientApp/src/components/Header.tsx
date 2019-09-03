import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';

export default function Header() {
  return (
    <Container>
      <Row>
        <Col>Toolkit logo</Col>
        <Col>Other logo</Col>
        <Col></Col>
      </Row>
    </Container>
  );
}
