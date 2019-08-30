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

import { KeypadIcon } from '../../icons';
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
    background: 'white',
    boxShadow: '0px 0px 100px #2C3E50'
  },
  textField: {
    background: 'white'
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
          <Typography>Scan</Typography> [I]
        </Button>
      </div>
      <Modal
        className={classes.modal}
        open={modalOpen}
        onClose={() => setModalOpen(false)}
      >
        <div className={classes.keypadContainer}>
          {/* This div is needed because Modal requires an Element as the child. Fix for later: Change Keypad to accept a ref so we can get rid of this div. */}
          <Keypad
            initialValue={value}
            onCancel={() => setModalOpen(false)}
            onConfirm={v => {
              setValue(v);
              setModalOpen(false);
            }}
            title={`Enter ${label} Manually`}
          />
        </div>
      </Modal>
    </>
  );
}
