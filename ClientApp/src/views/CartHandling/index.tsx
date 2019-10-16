import React, { useState } from 'react';
import { Typography } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import { TitleCol, ScannableTextField, SubmitButton } from 'components';
import { moveCart } from 'services/Cart';

export default function CartHandling() {
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
        </Col>
        <Col></Col>
      </Row>
      <Row>
        <Col>
          <SubmitButton
            disabled={cartId && floorLocation ? false : true}
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
