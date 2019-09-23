import React, { useState } from 'react';
import { makeStyles, Typography } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import {
  Table,
  TitleCol,
  ScannableTextField,
  SubmitButton,
  AutoComplete
} from 'components';

const createStyles = makeStyles({});

const tableShape = [
  { label: 'Order ID', key: 'orderId' },
  { label: 'Destination SuiteMAL', key: 'destination' },
  { label: 'Current Location', key: 'currentLocation' }
];

export default function CartHandling() {
  const classes = createStyles({});

  const [tableData, setTableData] = useState([]);
  const [cartId, setCartId] = useState('');
  const [floorLocation, setFloorLocation] = useState('');

  return (
    <>
      <Row>
        <TitleCol title='Cart Handling'>
          <Typography>
            Use this screen to confirm the new location of a cart that you have
            manually moved.
          </Typography>
          <Typography>
            Scan Cart ID and floor location you have moved the cart to and press
            the CONFIRM MOVE command button.
          </Typography>
        </TitleCol>
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
          <AutoComplete
            label='Cart Location'
            onSelect={value => console.log(value)}
            options={[
              { value: 'test1' },
              { value: 'aaaaa1' },
              { value: 'aaaaa2' },
              { value: 'aaaaa3' }
            ]}
            required
            value={''}
          />
        </Col>
        <Col></Col>
      </Row>
      {tableData.length > 0 && (
        <Row>
          <Table data={tableData} shape={tableShape} />
        </Row>
      )}
      <Row>
        <Col>
          <SubmitButton
            disabled={!cartId || !floorLocation}
            text='Confirm Move'
            variant='secondary'
          />
        </Col>
      </Row>
    </>
  );
}
