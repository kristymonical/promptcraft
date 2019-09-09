import React from 'react';
import { Row } from 'react-bootstrap';
import { makeStyles } from '@material-ui/styles';

import { ToolkitLogo } from 'icons';

const useStyles = makeStyles({
  flexContainerOverride: {
    alignItems: 'center',
    justifyContent: 'space-between',
    margin: '0 -15px'
  }
});

export default function Header() {
  const classes = useStyles();
  return (
    <>
      <Row className={classes.flexContainerOverride}>
        <ToolkitLogo />
        {/* @missing-assets logo goes here */}
        {/* @next Hamburger menu icon goes here */}
      </Row>
    </>
  );
}
