import React, { useState } from 'react';
import { Row } from 'react-bootstrap';
import { makeStyles } from '@material-ui/styles';
import {
  Drawer,
  List,
  ListItem,
  IconButton,
  ListItemText
} from '@material-ui/core';
import { Menu } from '@material-ui/icons';

import { ToolkitLogo } from 'icons';
import { Link } from 'react-router-dom';
import { SVT_THEME } from 'components';

const useStyles = makeStyles<typeof SVT_THEME>(({ primary }) => ({
  flexContainerOverride: {
    alignItems: 'center',
    justifyContent: 'space-between',
    margin: '0 -15px'
  },
  navDrawer: {
    '& a': {
      color: primary.dark,
      '&:hover': {
        textDecorationLine: 'none'
      }
    }
  }
}));

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
        <IconButton onClick={() => setNavIsOpen(true)}>
          <Menu />
        </IconButton>
      </Row>
      <Drawer
        anchor='right'
        className={classes.navDrawer}
        onClose={() => setNavIsOpen(false)}
        open={navIsOpen}
      >
        <List>
          {navItems.map(({ route, label }, idx) => (
            <Link
              key={`nav-item-${idx}`}
              to={route}
              onClick={() => setNavIsOpen(false)}
            >
              <ListItem button>
                <ListItemText primary={label} />
              </ListItem>
            </Link>
          ))}
        </List>
      </Drawer>
    </>
  );
}
