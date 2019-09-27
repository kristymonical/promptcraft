import React, { useState } from 'react';
import { Row } from 'react-bootstrap';
import { makeStyles } from '@material-ui/styles';

import { ToolkitLogo } from 'icons';
import Button from './Button';
import { Drawer, List, ListItem } from '@material-ui/core';
import { Link } from 'react-router-dom';

const useStyles = makeStyles({
  flexContainerOverride: {
    alignItems: 'center',
    justifyContent: 'space-between',
    margin: '0 -15px'
  }
});

export default function Header() {
  const classes = useStyles();
  const [navIsOpen, setNavIsOpen] = useState(false);
  return (
    <>
      <Row className={classes.flexContainerOverride}>
        <ToolkitLogo />
        {/* @missing-assets logo goes here */}
        <Button variant='primary' onClick={() => setNavIsOpen(true)}>
          Nav
        </Button>
      </Row>
      <Drawer
        open={navIsOpen}
        anchor='right'
        onClose={() => setNavIsOpen(false)}
      >
        <List>
          <ListItem button>
            <Link to='/request/delivery'>Delivery Request</Link>
          </ListItem>
          <ListItem button>
            <Link to='/request/cart'>Cart Handling</Link>
          </ListItem>
          <ListItem button>
            <Link to='/request/staging'>Staging Management</Link>
          </ListItem>
          <ListItem button>
            <Link to='/request/clean'>Clean Request</Link>
          </ListItem>
        </List>
      </Drawer>
    </>
  );
}
