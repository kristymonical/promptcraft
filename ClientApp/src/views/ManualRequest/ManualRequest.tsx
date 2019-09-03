import React, { useState, useEffect } from 'react';
import { Row } from 'react-bootstrap';
import { Typography, makeStyles, Button } from '@material-ui/core';
import { fade } from '@material-ui/core/styles';

import Select from './Select';
import ScannableTextField from './ScannableTextField';
import { SVT_THEME, Table, TitleCol } from 'components';

const useStyles = makeStyles(({ flex, secondary }: typeof SVT_THEME) => ({
  createButton: {
    background: secondary.background,
    borderRadius: 6,
    color: secondary.color,
    height: 70,
    width: 250,
    '&:hover': {
      background: fade(secondary.background, 0.75)
    }
  },
  flexFormContainer: {
    justifyContent: 'space-between',
    '& > *': {
      minWidth: '30% !important'
    }
  }
}));

const tableShape = [
  { label: 'Floor Location', key: 'floorLocation' },
  { label: 'MAL', key: 'mal' },
  { label: 'Destination SuiteMAL', key: 'suiteMAL' }
];

const tableInitialData = [
  { floorLocation: '1011A', mal: '1501', suiteMAL: '1513' }
];

const initialFormValues = {
  cartId: '',
  cartLocation: '',
  floorLocation: '',
  orderNumber: '',
  suiteMAL: ''
};

export default function ManualRequest() {
  const classes = useStyles({});
  const [formValues, setFormValues] = useState(initialFormValues);
  const [submitDisabled, setSubmitDisabled] = useState(true);
  const [requests, setRequests] = useState(tableInitialData);

  // "reducer" for form value state
  const handleChange = (name: keyof typeof formValues) => (
    newValue: string
  ) => {
    setFormValues({ ...formValues, [name]: newValue });
  };

  useEffect(() => {
    // current calculated value based on form values
    const calculatedDisabledValue = !Object.values(formValues).every(
      value => value && value.length > 0
    );

    // if calculated value is different than current one, update it
    if (submitDisabled !== calculatedDisabledValue) {
      setSubmitDisabled(calculatedDisabledValue);
    }
  }, [formValues, submitDisabled]);

  return (
    <>
      <Row>
        <TitleCol title='Manual Request' subtitle='This is a subtitle' />
      </Row>
      <Row>
        <Typography variant='h5'>Cart Information</Typography>
      </Row>
      <Row className={classes.flexFormContainer}>
        <ScannableTextField
          label='Cart ID'
          handleChange={handleChange('cartId')}
          required
          value={formValues.cartId}
        />
        <ScannableTextField
          label='Cart Location'
          handleChange={handleChange('cartLocation')}
          required
          value={formValues.cartLocation}
        />
        <ScannableTextField
          label='Order Number'
          handleChange={handleChange('orderNumber')}
          required
          value={formValues.orderNumber}
        />
      </Row>
      <Row>
        <Typography variant='h5'>Destination</Typography>
      </Row>
      <Row className={classes.flexFormContainer}>
        <Select
          items={['1011A', '1011B', '1001C']}
          label='Floor Location'
          handleChange={handleChange('floorLocation')}
          required
          value={formValues.floorLocation}
        />
        {formValues.floorLocation && (
          <Select
            items={['1513', '1535']}
            label='SuiteMAL'
            handleChange={handleChange('suiteMAL')}
            required
            value={formValues.suiteMAL}
          />
        )}
        <span>{/* Placeholder */}</span>
      </Row>
      <Row>
        <Table data={requests} shape={tableShape} />
      </Row>
      <Row>
        <Button
          className={classes.createButton}
          variant='contained'
          disabled={submitDisabled}
          onClick={() => {
            setFormValues(initialFormValues);
            const newRequest = {
              floorLocation: formValues.floorLocation,
              mal: '1501',
              suiteMAL: formValues.suiteMAL
            };
            setRequests([...requests, newRequest]);
          }}
        >
          <Typography variant='body1'>Create Manual Request</Typography>
        </Button>
      </Row>
    </>
  );
}
