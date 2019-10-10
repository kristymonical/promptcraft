import React, { useState, useEffect } from 'react';
import { Typography } from '@material-ui/core';
import { Row } from 'react-bootstrap';

import * as Staging from 'services/Staging';
import { TitleCol, ScannableTextField, SubmitButton, Table } from 'components';

const cartsTableShape = [
  { label: 'Order ID', key: 'orderId', filter: true },
  { label: 'Cart ID', key: 'cartId' },
  { label: 'Staging Location ID', key: 'stagingLocationId' },
  { label: 'Delivery Request Type', key: 'deliveryRequestType', filter: true },
  { label: 'Destination Area', key: 'destinationArea' }
];

export default function StagingManagement() {
  const [finalDestination, setFinalDestination] = useState('');
  const [stagingTableData, setStagingTableData] = useState<
    Staging.GetStagedCartsResult[]
  >([]);

  // get data on mount
  useEffect(() => {
    Staging.getStagedCarts()
      .then(cartsData => setStagingTableData(cartsData))
      .catch(err => console.error(err)); // @error-handling FE
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
          selectable
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
          onClick={() => {
            setFinalDestination('');
            setStagingTableData(stagingTableData);
          }}
          text='Create Staging Request'
          variant='secondary'
        />
      </Row>
    </>
  );
}
