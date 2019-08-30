import React, { useState } from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import TitleCol from '../../components/TitleCol';
import ScannableTextField from './ScannableTextField';
import { Typography, makeStyles } from '@material-ui/core';

import { SVT_THEME } from '../../components/ThemeProvider';

const useStyles = makeStyles(
  ({ flex, primary, secondary }: typeof SVT_THEME) => ({
    textFieldContainer: {
      alignItems: 'center',
      display: 'flex',
      justifyContent: 'space-between'
    }
  })
);

export default function ManualRequest() {
  const classes = useStyles({});
  const [values, setValues] = useState({
    cartId: '',
    cartLocation: '',
    orderNumber: ''
  });

  const handleChange = (name: keyof typeof values) => (newValue: string) => {
    setValues({ ...values, [name]: newValue });
  };

  return (
    <Container>
      <Row>
        <TitleCol
          title='Manual Request'
          subtitle='Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry’s standard dummy'
        />
      </Row>
      <Row>
        <Col>
          <Typography variant='h5'>Cart Information</Typography>
        </Col>
      </Row>
      <Row>
        <Col className={classes.textFieldContainer}>
          <ScannableTextField
            label='Cart ID'
            handleChange={handleChange('cartId')}
            required
            value={values.cartId}
          />
          <ScannableTextField
            label='Cart Location'
            handleChange={handleChange('cartLocation')}
            required
            value={values.cartLocation}
          />
          <ScannableTextField
            label='Order Number'
            handleChange={handleChange('orderNumber')}
            required
            value={values.orderNumber}
          />
        </Col>
      </Row>
    </Container>
  );
}
