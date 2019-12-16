import React from 'react';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import './dashboard.scss';
import { ChevronDown } from 'icons';

export interface DashboardProps {}

const Dashboard: React.FC<DashboardProps> = ({}) => {
  return (
    <>
      <Row>
        <Col>Location Name</Col>
        <Col>Dashboard Overview</Col>
        <Col>
          <span>Select Location</span>
          <ChevronDown size={5} />
        </Col>
      </Row>
    </>
  );
};

export default Dashboard;
