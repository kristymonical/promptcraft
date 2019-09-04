import React, { useState, useEffect } from 'react';
import { Row } from 'react-bootstrap';
import { Typography, makeStyles } from '@material-ui/core';

import Select from './Select';
import {
  ScannableTextField,
  SVT_THEME,
  Table,
  TitleCol,
  SubmitButton
} from 'components';

const useStyles = makeStyles(({ flex, secondary }: typeof SVT_THEME) => ({
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

const requestInitialValues = {
  floorLocation: '',
  mal: '',
  suiteMAL: ''
};

const initialFormValues = {
  cartId: '',
  cartLocation: '',
  floorLocation: '',
  orderNumber: '',
  suiteMAL: ''
};

const testFloorLocations = ['1011A', '1011B', '2011A']; // @hookup real data
const getFloorLocations = () => testFloorLocations;

export default function ManualRequest() {
  const classes = useStyles({});
  const [formValues, setFormValues] = useState(initialFormValues);
  const [submitDisabled, setSubmitDisabled] = useState(true);
  const [requestPreview, setRequestPreview] = useState(requestInitialValues);

  const [floorLocations] = useState(() => getFloorLocations());
  const [suiteMALs, setSuiteMALs] = useState([] as string[]);

  // "reducer" for form value state
  const handleChange = (name: keyof typeof formValues) => (
    newValue: string
  ) => {
    setFormValues({ ...formValues, [name]: newValue });
  };

  // determine if create button should be disabled
  useEffect(() => {
    // current calculated value based on if form values all are filled in
    // NOTE: If there's more complex logic in the future, it may be beneficial to switch to Yup and Formik
    const calculatedDisabledValue = !Object.values(formValues).every(
      value => value && value.length > 0
    );

    // if calculated value is different than current one, update it
    if (submitDisabled !== calculatedDisabledValue) {
      setSubmitDisabled(calculatedDisabledValue);
    }
  }, [formValues, submitDisabled]);

  const { floorLocation, suiteMAL } = formValues;

  // update suiteMALs based on floor location
  useEffect(() => {
    // if floorLocation is reset, then reset suiteMAL and suiteMAL list as well
    if (!floorLocation || floorLocation.length === 0) {
      setSuiteMALs([]);
    } else if (floorLocation[0] === '1') {
      setSuiteMALs(['1513', '1550']); // @hookup real data
    } else {
      setSuiteMALs(['2513', '2550']); // @hookup real data
    }

    setFormValues(current => ({ ...current, suiteMAL: '' }));
  }, [floorLocation]);

  // update request preview
  useEffect(() => {
    if (!floorLocation || !suiteMAL) {
      setRequestPreview(requestInitialValues);
      return;
    }

    setRequestPreview({
      floorLocation,
      mal: 'test', // @hookup real data
      suiteMAL
    });
  }, [floorLocation, suiteMAL]);

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
          items={floorLocations}
          label='Floor Location'
          handleChange={handleChange('floorLocation')}
          required
          value={formValues.floorLocation}
        />
        {formValues.floorLocation && (
          <Select
            items={suiteMALs}
            label='SuiteMAL'
            handleChange={handleChange('suiteMAL')}
            required
            value={formValues.suiteMAL}
          />
        )}
        <span>{/* Placeholder */}</span>
      </Row>
      {formValues.floorLocation && formValues.suiteMAL && (
        <Row>
          <Table data={requestPreview} shape={tableShape} />
        </Row>
      )}
      <Row>
        <SubmitButton
          disabled={submitDisabled}
          onClick={() => setFormValues(initialFormValues)}
          text='Create Manual Request'
          variant='secondary'
        />
      </Row>
    </>
  );
}
