import React, { useState } from 'react';
import { Row, Col } from 'react-bootstrap';
import { Typography, makeStyles, Button } from '@material-ui/core';
import { fade } from '@material-ui/core/styles';

import Select from './Select';
import ScannableTextField from './ScannableTextField';
import { TitleCol, SVT_THEME } from 'components';

const useStyles = makeStyles(
  ({ flex, primary, secondary }: typeof SVT_THEME) => ({
    flexContainer: {
      display: 'flex',
      marginBottom: flex.verticalSpacing,
      '& > *': {
        marginRight: 25
      }
    },
    createButton: {
      background: secondary.background,
      borderRadius: 6,
      color: secondary.color,
      height: 70,
      width: 250,
      '&:hover': {
        background: fade(secondary.background, 0.75)
      }
    }
  })
);

const testData = ['test1', 'test2', 'test3'];

export default function ManualRequest() {
  const classes = useStyles({});
  const [values, setValues] = useState({
    cartId: '',
    cartLocation: '',
    floorLocation: '',
    orderNumber: '',
    suiteMAL: ''
  });

  const handleChange = (name: keyof typeof values) => (newValue: string) => {
    setValues({ ...values, [name]: newValue });
  };

  return (
    <>
      <Row>
        <TitleCol title='Manual Request' subtitle='This is a subtitle' />
      </Row>
      <Row>
        <Col>
          <Typography variant='h5'>Cart Information</Typography>
        </Col>
      </Row>
      <Row>
        <Col className={classes.flexContainer}>
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
      <Row>
        <Col>
          <Typography variant='h5'>Destination</Typography>
        </Col>
      </Row>
      <Row>
        <Col className={classes.flexContainer}>
          <Select
            items={testData}
            label='Floor Location'
            handleChange={handleChange('floorLocation')}
            required
            value={values.floorLocation}
          />
          <Select
            items={testData}
            label='SuiteMAL'
            handleChange={handleChange('suiteMAL')}
            required
            value={values.suiteMAL}
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <Button className={classes.createButton} variant='contained'>
            <Typography variant='body1'>Create Manual Request</Typography>
          </Button>
        </Col>
      </Row>
    </>
  );
}
