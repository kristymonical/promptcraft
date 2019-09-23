import React, { useState } from 'react';
import { makeStyles, Modal, Typography } from '@material-ui/core';
import { Row, Col } from 'react-bootstrap';

import {
  TitleCol,
  ScannableTextField,
  SubmitButton,
  Table,
  SVT_THEME,
  Button
} from 'components';
import Timer from './Timer';

const createStyles = makeStyles(
  ({ flex: { horizontalSpacing }, primary }: typeof SVT_THEME) => ({
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
    },
    fieldContainer: {
      alignItems: 'center',
      '& > div': {
        marginRight: horizontalSpacing,
        minWidth: '30%'
      },
      '& > button': {
        marginLeft: horizontalSpacing * 2
      }
    }
  })
);

const tableShape = [
  { label: 'Order ID', key: 'orderId' },
  { label: 'Destination SuiteMAL', key: 'destination' }
];

export default function CleanRequest() {
  const classes = createStyles({});

  const [cartId, setCartId] = useState('');
  const [mal, setMal] = useState('');
  const [tableData, setTableData] = useState<
    { orderId: string; destination: string }[]
  >([]);
  const [verified, setVerified] = useState(false);
  const [modalOpen, setModalOpen] = useState(false);

  return (
    <>
      <Row>
        <TitleCol title='Clean Request'>
          <Typography>
            Use this screen to confirm Cart is moved into MAL and to start TIMER
            PROCESS once all cleaning solutions have been applied.
          </Typography>
          <Typography>
            Scan Cart ID and MAL location. Start the cleaning process. Once Cart
            has final cleaning solution applied, press START TIMER PROCESS
            command button.
          </Typography>
        </TitleCol>
      </Row>
      <Row className={classes.fieldContainer}>
        <ScannableTextField
          handleChange={newCartId => setCartId(newCartId)}
          label='Cart ID'
          required
          value={cartId}
        />
        <ScannableTextField
          handleChange={newMal => setMal(newMal)}
          label='MAL'
          required
          value={mal}
        />
        <Button
          disabled={cartId.length === 0 || mal.length === 0}
          scale={1.25}
          onClick={() => {
            // @hookup real data
            setVerified(true);
            setTableData([
              {
                destination: '1513',
                orderId: `#${Math.random()
                  .toString()
                  .slice(-10)}`
              }
            ]);
          }}
        >
          Verify MAL
        </Button>
      </Row>
      {tableData.length > 0 && (
        <Row>
          <Table data={tableData} shape={tableShape} maxWidth='50%' />
        </Row>
      )}
      <Row>
        <SubmitButton
          disabled={!verified}
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
              <Timer
                minutes={0.1}
                onTimerEnd={() => {
                  setVerified(false);
                  setTableData([]);
                  setCartId('');
                  setModalOpen(false);
                }}
              />
            </Col>
          </Row>
          <div className={classes.customBackdrop} />
        </>
      </Modal>
    </>
  );
}
