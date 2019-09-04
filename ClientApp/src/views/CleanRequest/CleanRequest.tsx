import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';
import { TitleCol, ScannableTextField, SubmitButton } from 'components';

interface CleanRequestProps {}

const createStyles = makeStyles({});

export default function CleanRequest({  }: CleanRequestProps) {
  const classes = createStyles({});

  const [cartId, setCartId] = useState('');

  return (
    <>
      <Row>
        <TitleCol title='Clean Request' />
      </Row>
      <Row>
        <ScannableTextField
          handleChange={newCartId => setCartId(newCartId)}
          label='Cart ID'
          required
          value={cartId}
        />
      </Row>
      <Row></Row>
      <Row>
        <SubmitButton text='Create Manual Request' variant='secondary' />
      </Row>
    </>
  );
}
