import React from 'react';
import { Row, Col } from 'react-bootstrap';
import { makeStyles } from '@material-ui/core';
import { Link } from 'react-router-dom';

import BigButton from './BigButton';
import { TitleCol } from 'components';

const useStyles = makeStyles({
  buttonGroup: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'center',
    margin: '0 auto',
    maxWidth: '800px'
  },
  link: {
    '&:hover': {
      textDecoration: 'none'
    }
  }
});

export default function Menu() {
  const classes = useStyles({});
  return (
    <>
      <Row>
        <TitleCol title='Title' />
      </Row>
      <Row>
        <Col className={classes.buttonGroup}>
          <Link className={classes.link} to='/request/manual'>
            <BigButton>Request</BigButton>
          </Link>
          <Link className={classes.link} to='/deliveries'>
            <BigButton>Deliveries</BigButton>
          </Link>
          <Link className={classes.link} to='/mapping'>
            <BigButton variant='secondary'>Mapping</BigButton>
          </Link>
          <Link className={classes.link} to='/configure'>
            <BigButton variant='secondary'>Configuration</BigButton>
          </Link>
        </Col>
      </Row>
    </>
  );
}
