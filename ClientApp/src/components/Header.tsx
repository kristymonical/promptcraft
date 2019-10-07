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

const navItems = [
  { route: '/request/delivery', label: 'Delivery Request' },
  { route: '/request/cart', label: 'Cart Handling' },
  { route: '/request/clean', label: 'Clean Request' },
  { route: '/manage/staging', label: 'Staging Management' },
  { route: '/manage/queue', label: 'Delivery Queue Management' }
];

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
          {navItems.map(({ route, label }, idx) => (
            <Link
              key={`nav-item-${idx}`}
              to={route}
              onClick={() => setNavIsOpen(false)}
            >
              <ListItem button>{label}</ListItem>
            </Link>
          ))}
        </List>
      </Drawer>
    </>
  );
}
