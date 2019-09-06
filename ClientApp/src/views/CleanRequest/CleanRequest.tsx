import React, { useState, useEffect } from 'react';
import { makeStyles, Modal, Typography } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import {
  TitleCol,
  ScannableTextField,
  SubmitButton,
  Table,
  SVT_THEME
} from 'components';
import Timer from './Timer';

const createStyles = makeStyles(({ primary }: typeof SVT_THEME) => ({
  modal: {
    alignItems: 'center',
    color: 'white',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    '& h3': {
      color: 'white',
      fontSize: 51,
      fontWeight: 300
    }
  },
  customBackdrop: {
    background: primary.dark,
    boxShadow: '0px 10px 10px #0000006A;',
    height: '90vh',
    margin: '0 auto',
    position: 'absolute',
    width: '90vw',
    zIndex: -1
  }
}));

const initialTableData = {
  deliveryId: '',
  destination: '',
  orderId: ''
};

export default function CleanRequest() {
  const classes = createStyles({});

  const [cartId, setCartId] = useState('');
  const [tableData, setTableData] = useState(initialTableData);
  const [modalOpen, setModalOpen] = useState(false);

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
          onClick={() => {
            setModalOpen(true);
            setCartId('');
          }}
          text='Start Cleaning Process'
          variant='secondary'
        />
      </Row>
      <Modal
        className={classes.modal}
        open={modalOpen}
        onClose={() => setModalOpen(false)}
      >
        <>
          <Row style={{ marginBottom: 25 }}>
            <Col>
              <Typography variant='h3'>Cleaning Progress</Typography>
            </Col>
          </Row>
          <Row>
            <Col>
              <Timer minutes={10} />
            </Col>
          </Row>
          <div className={classes.customBackdrop} />
        </>
      </Modal>
    </>
  );
}
