import React, { useState, useEffect } from 'react';
import { Row } from 'react-bootstrap';
import { Typography, makeStyles } from '@material-ui/core';

import {
  ScannableTextField,
  Select,
  SVT_THEME,
  TitleCol,
  SubmitButton
} from 'components';
import {
  GetAreasResult,
  getAreas,
  createDeliveryRequest
} from 'services/Delivery';

const useStyles = makeStyles(({ flex, secondary }: typeof SVT_THEME) => ({
  flexFormContainer: {
    justifyContent: 'space-between',
    '& > *': {
      minWidth: '30% !important'
    }
  }
}));

const initialFormValues = {
  cartId: '',
  cartLocation: '',
  area: '',
  orderNumber: ''
};

export default function ManualRequest() {
  const classes = useStyles({});
  const [formValues, setFormValues] = useState(initialFormValues);
  const [submitDisabled, setSubmitDisabled] = useState(true);

  const [areas, setAreas] = useState<GetAreasResult[]>([]);

  // "reducer" for form value state
  const handleChange = (name: keyof typeof formValues) => (
    newValue: string
  ) => {
    setFormValues({ ...formValues, [name]: newValue });
  };

  // get data on mount
  useEffect(() => {
    getAreas()
      .then(returnedAreas => setAreas(returnedAreas))
      .catch(err => console.error(err)); // @error handling
  }, []);

  // determine if create button should be disabled
  useEffect(() => {
    // Disabled is false if all fields (except order number) are filled in
    // NOTE: If there's more complex logic in the future, it may be beneficial to switch to Yup and Formik
    const calculatedDisabledValue = !Object.entries(formValues).every(
      ([key, value]) => key === 'orderNumber' || value.length > 0
    );

    // if calculated value is different than current one, update it
    if (submitDisabled !== calculatedDisabledValue) {
      setSubmitDisabled(calculatedDisabledValue);
    }
  }, [formValues, submitDisabled]);

  const onSubmit = () => {
    createDeliveryRequest({
      cartId: formValues.cartId,
      cartLocation: formValues.cartLocation,
      destinationArea: formValues.area,
      orderNumber: formValues.orderNumber
    });
    setFormValues(initialFormValues);
  };

  return (
    <>
      <Row>
        <TitleCol title='Delivery Request' />
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
          value={formValues.orderNumber}
        />
      </Row>
      <Row>
        <Typography variant='h5'>Destination</Typography>
      </Row>
      <Row className={classes.flexFormContainer}>
        <Select
          items={areas.map(area => area.areaName)}
          label='Area'
          handleChange={handleChange('area')}
          required
          value={formValues.area}
        />
        <span>{/* Placeholder */}</span>
        <span>{/* Placeholder */}</span>
      </Row>
      <Row>
        <SubmitButton
          disabled={submitDisabled}
          onClick={onSubmit}
          text='Create Delivery Request'
          variant='secondary'
        />
      </Row>
    </>
  );
}
