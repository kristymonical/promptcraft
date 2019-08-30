import React, { useState } from 'react';
import {
  TextField,
  makeStyles,
  Typography,
  InputAdornment,
  Button,
  Modal
} from '@material-ui/core';
import { TextFieldProps } from '@material-ui/core/TextField';

import { KeypadIcon, BarcodeIcon } from '../../icons';
import { SVT_THEME } from '../../components/ThemeProvider';
import Keypad from './Keypad';

type ScannableTextFieldProps = TextFieldProps & {
  maxWidth?: number;
};

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
    justifyContent: 'space-between',
    maxHeight: 40,
    maxWidth: 100
  },
  modal: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'center'
  },
  input: {
    // remove the stupid number spinner
    '& ::-webkit-inner-spin-button': {
      margin: 0,
      WebkitAppearance: 'none'
    },
    '& ::-webkit-outer-spin-button': {
      margin: 0,
      WebkitAppearance: 'none'
    }
  },
  keypadContainer: {
    display: 'flex',
    position: 'relative'
  },
  textField: {
    background: 'white'
  },
  customBackdrop: {
    background: 'white',
    border: '1px solid #363636CC',
    boxShadow: '0px 0px 100px #2C3E50',
    filter: 'blur(15px)',
    height: '125%',
    marginTop: '-6.25%',
    opacity: 0.96,
    position: 'absolute',
    width: '100%',
    zIndex: -1
  }
}));

export default function ScannableTextField({
  label,
  maxWidth = 275,
  required = false
}: ScannableTextFieldProps) {
  const classes = useStyles({ maxWidth });
  const [value, setValue] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
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
          value={value}
          onChange={({ target: { value } }) => setValue(value)}
          variant='outlined'
          InputProps={{
            endAdornment: (
              <InputAdornment position='end' onClick={() => setModalOpen(true)}>
                <KeypadIcon />
              </InputAdornment>
            ),
            type: 'number',
            className: classes.input
          }}
        />
        <Button variant='contained' className={classes.button}>
          <Typography>Scan</Typography>
          <BarcodeIcon />
        </Button>
      </div>
      <Modal
        className={classes.modal}
        open={modalOpen}
        onClose={() => setModalOpen(false)}
      >
        <div className={classes.keypadContainer}>
          <Keypad
            initialValue={value}
            onCancel={() => setModalOpen(false)}
            onConfirm={v => {
              setValue(v);
              setModalOpen(false);
            }}
            title={`Enter ${label} Manually`}
          />
          <div className={classes.customBackdrop} />
        </div>
      </Modal>
    </>
  );
}
