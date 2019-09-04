import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';
import { TitleCol, ScannableTextField, SubmitButton, Table } from 'components';

interface CleanRequestProps {}

const createStyles = makeStyles({});

const initialTableData = {
  deliveryId: '',
  destination: '',
  orderId: ''
};

export default function CleanRequest({  }: CleanRequestProps) {
  const classes = createStyles({});

  const [cartId, setCartId] = useState('');
  const [tableData, setTableData] = useState(initialTableData);

  useEffect(() => {
    // @hookup real data
    setTableData({
      deliveryId: `#${Math.random()
        .toString()
        .slice(-10)}`,
      destination: '1513',
      orderId: `#${Math.random()
        .toString()
        .slice(-10)}`
    });
  }, [cartId]);

  return (
    <>
      <Row>
        <TitleCol title='Clean Request' />
      </Row>
      <Row>
        <ScannableTextField
          handleChange={newCartId => setCartId(newCartId)}
          label='Cart ID'
          required
          value={cartId}
        />
      </Row>
      {cartId.length > 0 && (
        <Row>
          <Table
            data={tableData}
            shape={[
              { label: 'Delivery Instruction ID', key: 'deliveryId' },
              { label: 'Lonza Order ID', key: 'orderId' },
              { label: 'Destination SuiteMAL', key: 'destination' }
            ]}
          />
        </Row>
      )}
      <Row>
        <SubmitButton
          disabled={!cartId}
          onClick={() => setCartId('')}
          text='Create Manual Request'
          variant='secondary'
        />
      </Row>
    </>
  );
}
