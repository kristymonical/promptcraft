import React, { useState, useEffect, useCallback } from 'react';
import { Row } from 'react-bootstrap';
import {
  Typography,
  makeStyles,
  RadioGroup,
  FormControlLabel,
  Radio
} from '@material-ui/core';

import { ScannableTextField, Select, TitleCol, SubmitButton } from 'components';
import { createDeliveryRequest } from 'services/Delivery';
import { getDestinationAreas, GetAreasResult } from 'services/Area';
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

type DeliveryType = 'deliver' | 'return' | 'stage';

export default function DeliveryRequest() {
  const classes = useStyles({});
  const [formValues, setFormValues] = useState(initialFormValues);
  const [submitDisabled, setSubmitDisabled] = useState(true);
  const [debouncedCartLocation] = useDebounce(formValues.cartLocation, 1.5e3); // 1.5 second debounce for cart location
  const [areas, setAreas] = useState<GetAreasResult[]>([]);
  const [deliveryType, setDeliveryType] = useState<DeliveryType>('deliver');

  // "reducer" for form value state
  const handleChange = useCallback(
    (name: keyof typeof initialFormValues) => (newValue: string) => {
      setFormValues(old => ({ ...old, [name]: newValue }));
    },
    []
  );

  // get data on debounced value change or delivery type change
  useEffect(() => {
    if (!debouncedCartLocation || debouncedCartLocation.length === 0) return;
    getDestinationAreas(debouncedCartLocation, deliveryType).then(
      returnedAreas => {
        setAreas(_.sortBy(_.uniqBy(returnedAreas, 'areaName'), 'areaName'));
        handleChange('area')('');
      }
    );
  }, [debouncedCartLocation, deliveryType, handleChange]);

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
        deliveryType,
        destinationArea: formValues.area,
        orderNumber: formValues.orderNumber
      });

      // reset form on success
      if (success) {
        setFormValues(initialFormValues);
        setAreas([]);
      }
    } catch (err) {}
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
            Enter Cart Id, Cart's current location, and optional Order Number.
            Choose a destination in the resulting dropdown to create Delivery
            Request.
          </Typography>
        </TitleCol>
      </Row>
      <Row>
        <Typography variant='h5'>Delivery Type</Typography>
      </Row>
      <Row>
        <RadioGroup
          value={deliveryType}
          onChange={evt => setDeliveryType(evt.target.value as DeliveryType)}
        >
          <FormControlLabel
            value='deliver'
            control={<Radio />}
            label={
              <span>
                <b>Delivery</b> - <i>Deliver a cart to another area</i>
              </span>
            }
          />
          <FormControlLabel
            value='stage'
            control={<Radio />}
            label={
              <span>
                <b>Staging</b> -{' '}
                <i>Stage a cart in another area for future use</i>
              </span>
            }
          />
          <FormControlLabel
            value='return'
            control={<Radio />}
            label={
              <span>
                <b>Cart Return</b> - <i>Send an empty cart to another area</i>
              </span>
            }
          />
        </RadioGroup>
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
      {areas.length > 0 && (
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
