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
        <TitleCol title='MacGregor Toolkit' />
      </Row>
      <Row>
        <Col className={classes.buttonGroup}>
          <Link className={classes.link} to='/request/delivery'>
            <BigButton>Delivery Request</BigButton>
          </Link>
          <Link className={classes.link} to='/manage/queue'>
            <BigButton>Delivery Queue</BigButton>
          </Link>
          <Link className={classes.link} to='/request/cart'>
            <BigButton variant='secondary'>Cart Handling</BigButton>
          </Link>
          <Link className={classes.link} to='/request/clean'>
            <BigButton variant='secondary'>Clean Request</BigButton>
          </Link>
          <Link className={classes.link} to='/manage/staging'>
            <BigButton>Staging Management</BigButton>
          </Link>
        </Col>
      </Row>
    </>
  );
}
