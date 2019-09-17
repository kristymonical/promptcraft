import React, { useState, useEffect } from 'react';
// import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';

import { TitleCol, ScannableTextField, SubmitButton, Table } from 'components';

// const createStyles = makeStyles({});

const cartsTableShape = [
  { label: 'Order ID', key: 'orderId' },
  { label: 'Cart ID', key: 'cartId' },
  { label: 'Staging Location ID', key: 'locationId' }
];

const stagingRequestShape = [
  { label: 'Floor Location', key: 'floorLocation' },
  { label: 'Destination SuiteMAL', key: 'suiteMAL' }
];

interface CartTableDataItem {
  orderId: string;
  cartId: string;
  locationId: string;
}

const getCartsTableTestData = async () => [
  { orderId: '1A', cartId: '1300', locationId: '9AB' },
  { orderId: '101', cartId: '1309', locationId: '9AC' },
  { orderId: '101', cartId: '1310', locationId: '8AB' }
];

export default function StagingManagement() {
  //   const classes = createStyles({});
  const [finalDestination, setFinalDestination] = useState('');
  const [cartsTableData, setCartsTableData] = useState(
    [] as CartTableDataItem[]
  );

  // get data on mount
  useEffect(() => {
    getCartsTableTestData() // @hookup real data
      .then(cartsData => setCartsTableData(cartsData))
      .catch(err => console.error(err));
  }, []);

  return (
    <>
      <Row>
        <TitleCol title='Staging Management' />
      </Row>
      <Row>
        <Table shape={cartsTableShape} data={cartsTableData} maxWidth='75%' />
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
        <Table
          shape={stagingRequestShape}
          data={{ floorLocation: '1011A', suiteMAL: '1513' }}
          maxWidth='50%'
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
