import React, { useState, useEffect, useCallback } from 'react';
import { Row } from 'react-bootstrap';
import { Typography, makeStyles } from '@material-ui/core';

import { ScannableTextField, Select, TitleCol, SubmitButton } from 'components';
import { createDeliveryRequest } from 'services/Delivery';
import { getDestinationAreas, GetAreasResult } from 'services/Area';
import { createLog } from 'services/Log';
import { useDebounce } from 'hooks';
import _ from 'lodash';

const useStyles = makeStyles(() => ({
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

export default function DeliveryRequest() {
  const classes = useStyles({});
  const [formValues, setFormValues] = useState(initialFormValues);
  const [submitDisabled, setSubmitDisabled] = useState(true);
  const [debouncedCartLocation] = useDebounce(formValues.cartLocation, 1e3); // 1 second debounce for cart location

  const [areas, setAreas] = useState<GetAreasResult[]>([]);

  // "reducer" for form value state
  const handleChange = useCallback(
    (name: keyof typeof initialFormValues) => (newValue: string) => {
      setFormValues(old => ({ ...old, [name]: newValue }));
    },
    []
  );

  // get data on debounced value change
  useEffect(() => {
    if (!debouncedCartLocation || debouncedCartLocation.length === 0) return;
    getDestinationAreas(debouncedCartLocation).then(returnedAreas => {
      setAreas(_.uniqBy(returnedAreas, 'areaName'));
      handleChange('area')('');
    });
  }, [debouncedCartLocation, handleChange]);

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

  const onSubmit = async () => {
    try {
      const success = await createDeliveryRequest({
        cartId: formValues.cartId,
        cartLocation: formValues.cartLocation,
        destinationArea: formValues.area,
        orderNumber: formValues.orderNumber
      });

      // reset form on success
      if (success) setFormValues(initialFormValues);
    } catch (err) {
      console.log(err);
      // attempt to log on error
      // createLog({
      //     action: 'Create Delivery Request',
      //     deliveryId: -1,
      //     message: 'Failed to create delivery request',
      //     method: 'POST',
      //     route: ''
      // });
    }
  };

  return (
    <>
      <Row>
        <TitleCol title='Delivery Request'>
          <Typography>
            Use this screen to create delivery requests for loaded and unloaded
            carts
          </Typography>
          <Typography>
            <b>Loaded Carts:</b> Enter Cart ID, Cart's current location, Order
            Number, and Destination of Suite MAL or Staging to create Delivery
            Request.
          </Typography>
          <Typography>
            <b>Unloaded Carts:</b> Enter Cart ID, Cart's current floor location
            and destination of FPA or CARWASH to create delivery request to
            return a cart.
          </Typography>
        </TitleCol>
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
      {areas && areas.length > 0 && formValues.cartId.length > 0 && (
        <>
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
        </>
      )}
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
