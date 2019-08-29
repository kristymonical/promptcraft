import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import TitleCol from '../../components/TitleCol';

export default function ManualRequest() {
  return (
    <Container>
      <Row>
        <TitleCol
          title='Manual Request'
          subtitle='Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry’s standard dummy'
        />
      </Row>
      <Row>
        <Col>Test</Col>
      </Row>
    </Container>
  );
}
