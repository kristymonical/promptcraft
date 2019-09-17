import React, { useState, useEffect } from 'react';
import {
  TextField,
  makeStyles,
  Typography,
  Button,
  Modal
} from '@material-ui/core';
import { fade } from '@material-ui/core/styles';

import { SVT_THEME, BarcodeScanner } from 'components';
import { BarcodeIcon } from 'icons';
import { Row } from 'react-bootstrap';

interface ScannableTextFieldProps {
  label: string;
  handleChange: (value: string) => void;
  maxWidth?: number;
  required: boolean;
  value: string;
}

const useStyles = makeStyles(({ secondary }: typeof SVT_THEME) => ({
  scannableTextField: {
    display: 'flex',
    flexDirection: 'column',
    maxWidth: ({ maxWidth }: ScannableTextFieldProps) => maxWidth,
    justifyContent: 'space-between',
    '& > *': {
      marginBottom: 10
    }
  },
  button: {
    background: secondary.background,
    color: 'white',
    maxHeight: 40,
    maxWidth: 110,
    '&:hover': {
      background: fade(secondary.background, 0.75)
    }
  },
  buttonInternalFlexContainer: {
    alignItems: 'center',
    display: 'flex',
    flexGrow: 1,
    justifyContent: 'space-between'
  },
  modal: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'center'
  },
  textField: {
    background: 'white'
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
}));

export default function ScannableTextField(props: ScannableTextFieldProps) {
  const classes = useStyles(props);
  const { label, handleChange, required, value } = props;
  const [localValue, setValue] = useState(`${value || ''}`);
  const [scanModalOpen, setScanModalOpen] = useState(false);

  useEffect(() => setValue(value), [value]);
  return (
    <>
      <div className={classes.scannableTextField}>
        <Typography component='span'>
          {label}{' '}
          {required && (
            <Typography component='span' color='error'>
              *REQUIRED
            </Typography>
          )}
        </Typography>
        <TextField
          className={classes.textField}
          value={localValue}
          onChange={({ target: { value } }) => {
            setValue(value);
            handleChange(value);
          }}
          variant='outlined'
        />
        <Button
          variant='contained'
          className={classes.button}
          onClick={() => setScanModalOpen(true)}
        >
          <div className={classes.buttonInternalFlexContainer}>
            <Typography>Scan</Typography>
            <BarcodeIcon />
          </div>
        </Button>
      </div>

      <Modal
        className={classes.modal}
        open={scanModalOpen}
        onClose={() => setScanModalOpen(false)}
      >
        <Row className={classes.modalFlexContainer}>
          <div className={classes.customBackdrop}></div>
          <BarcodeScanner
            onScan={scanned => {
              handleChange(scanned);
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

ScannableTextField.defaultProps = {
  maxWidth: 400,
  required: false
};
