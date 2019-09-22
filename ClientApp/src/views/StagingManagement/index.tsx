import React, { useState, useEffect } from 'react';
import { Typography } from '@material-ui/core';
import { Row } from 'react-bootstrap';

import { TitleCol, ScannableTextField, SubmitButton, Table } from 'components';

const cartsTableShape = [
  { label: 'Order ID', key: 'orderId', filter: true },
  { label: 'Cart ID', key: 'cartId' },
  { label: 'Staging Location ID', key: 'locationId' },
  { label: 'Delivery Request Type', key: 'requestType', filter: true },
  { label: 'Destination Area', key: 'destinationArea' }
];

interface StagingTableDataItem {
  orderId: string;
  cartId: string;
  locationId: string;
}

const getStagingTableTestData = async () => [
  {
    orderId: '1A',
    cartId: '1300',
    locationId: '9AB',
    requestType: '1',
    destinationArea: '1010A'
  },
  {
    orderId: '101',
    cartId: '1309',
    locationId: '9AC',
    requestType: '1',
    destinationArea: '1012B'
  },
  {
    orderId: '101',
    cartId: '1310',
    locationId: '8AB',
    requestType: '2',
    destinationArea: '1012C'
  }
];

export default function StagingManagement() {
  const [finalDestination, setFinalDestination] = useState('');
  const [stagingTableData, setStagingTableData] = useState<
    StagingTableDataItem[]
  >([]);

  // get data on mount
  useEffect(() => {
    getStagingTableTestData() // @hookup real data
      .then(cartsData => setStagingTableData(cartsData))
      .catch(err => console.error(err)); // @error handling
  }, []);

  return (
    <>
      <Row>
        <TitleCol title='Staging Management'>
          <Typography>
            Use this screen to add or modify the Destination Area for staged
            carts and create new Delivery Requests for loaded and unloaded
            carts.
          </Typography>
          <Typography>
            <b>Suite MAL delivery requests:</b> Filter list of staged carts by
            Delivery Request Type and/or Order ID and select new Suite MAL
            destination to create new Delivery Request.
          </Typography>
          <Typography>
            <b>Return Cart Delivery Requests:</b> Filter list of staged carts by
            Delivery Request Type and select staged Delivery Requests to modify
            and select FPA or CARWASH destination to create new Delivery Request
            for a cart return.
          </Typography>
        </TitleCol>
      </Row>
      <Row>
        <Table
          shape={cartsTableShape}
          data={stagingTableData}
          onSelectRow={selectedRows => console.table(selectedRows)}
        />
      </Row>
      <Row>
        <ScannableTextField
          handleChange={value => setFinalDestination(value)}
          label='Final Destination'
          value={finalDestination}
          required
        />
      </Row>
      <Row>
        <SubmitButton
          disabled={finalDestination.length === 0}
          onClick={() => setFinalDestination('')}
          text='Create Staging Request'
          variant='secondary'
        />
      </Row>
    </>
  );
}
