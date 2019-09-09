import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import {
  Table,
  TitleCol,
  ScannableTextField,
  Select,
  SubmitButton
} from 'components';

const createStyles = makeStyles({
  cartHandlingRoot: {
    '& > *': {
      marginBottom: 30
    }
  },
  buttonRow: {
    justifyContent: 'space-around'
  }
});

const initialTableData = {
  activeTransport: true,
  deliveryId: '',
  destination: '',
  orderId: ''
};

export default function CartHandling() {
  const classes = createStyles({});

  const [tableData] = useState(initialTableData);
  const [cartId, setCartId] = useState('');
  const [floorLocation, setFloorLocation] = useState('');

  return (
    <div className={classes.cartHandlingRoot}>
      <Row>
        <TitleCol title='Cart Handling' />
      </Row>
      <Row>
        <Col>
          <ScannableTextField
            handleChange={newCartId => setCartId(newCartId)}
            label='Cart ID'
            required
            value={cartId}
          />
        </Col>
        <Col>
          <Select
            items={['Floor 1 Section 2']}
            label='Location'
            handleChange={value => setFloorLocation(value)}
            required
            value={floorLocation}
          />
        </Col>
        <Col></Col>
      </Row>
      <Row>
        <Table
          data={tableData}
          shape={[
            { label: 'Delivery Instruction ID', key: 'deliveryId' },
            { label: 'Lonza Order ID', key: 'orderId' },
            { label: 'Destination SuiteMAL', key: 'destination' },
            { label: 'Active Transport', key: 'activeTransport' }
          ]}
        />
      </Row>
      <Row className={classes.buttonRow}>
        <SubmitButton text='Manually Move Cart' variant='secondary' />
        <SubmitButton text='Cancel Transport' variant='secondary' />
        <SubmitButton text='Return Cart' variant='secondary' />
      </Row>
    </div>
  );
}
