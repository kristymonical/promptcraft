import React, { useState } from 'react';
import { makeStyles, Modal, Typography } from '@material-ui/core';
import { Row } from 'react-bootstrap';
import { BarcodeIcon } from 'icons';

import { SVT_THEME, BarcodeScanner, Button } from 'components';

interface ScanButtonProps {
  onScan: (value: string) => void;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<ScanButtonProps>>({
  modal: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'center'
  },
  customBackdrop: {
    alignSelf: 'center',
    background: '#D5D5D5',
    border: '1px solid #363636CC',
    boxShadow: '0px 0px 100px #2C3E50',
    filter: 'blur(15px)',
    height: '90%',
    left: '10%',
    position: 'absolute',
    top: '5%',
    width: '80%',
    zIndex: -1
  },
  modalFlexContainer: {
    alignItems: 'center',
    color: '#555',
    flexDirection: 'column',
    fontWeight: 300
  }
});

export default function ScanButton({ onScan }: ScanButtonProps) {
  const classes = createStyles({});
  const [scanModalOpen, setScanModalOpen] = useState(false);
  return (
    <>
      <Button onClick={() => setScanModalOpen(true)}>
        <Typography>Scan</Typography>
        <BarcodeIcon />
      </Button>
      <Modal
        className={classes.modal}
        open={scanModalOpen}
        onClose={() => setScanModalOpen(false)}
      >
        <Row className={classes.modalFlexContainer}>
          <div className={classes.customBackdrop}></div>
          <BarcodeScanner
            onScan={scanned => {
              onScan(scanned);
              setScanModalOpen(false);
            }}
          />
          <Typography variant='h3' style={{ marginBottom: 10 }}>
            Please Scan Barcode
          </Typography>
          <BarcodeIcon width={100} height={60} color='#555555' />
        </Row>
      </Modal>
    </>
  );
}
