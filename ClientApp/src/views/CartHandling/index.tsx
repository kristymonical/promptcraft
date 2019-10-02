import React, { useState } from 'react';
import { Typography } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import {
  Table,
  TitleCol,
  ScannableTextField,
  SubmitButton
  //   AutoComplete
} from 'components';
import { moveCart } from 'services/Cart';

const tableShape = [
  { label: 'Order ID', key: 'orderId' },
  { label: 'Destination SuiteMAL', key: 'destination' },
  { label: 'Current Location', key: 'currentLocation' }
];

export default function CartHandling() {
  const [tableData] = useState([]);
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
          <ScannableTextField
            handleChange={newCartId => setFloorLocation(newCartId)}
            label='Cart Location'
            required
            value={floorLocation}
          />
          {/* <AutoComplete
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
          /> */}
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
            disabled={false}
            text='Confirm Move'
            variant='secondary'
            onClick={() => {
              moveCart(cartId, floorLocation);
              setCartId('');
              setFloorLocation('');
            }}
          />
        </Col>
      </Row>
    </>
  );
}
