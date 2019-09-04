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
        <div>Other logo</div>
        {/* Hamburger menu icon placeholder */}
        <div>&nbsp;</div>
      </Row>
    </>
  );
}
