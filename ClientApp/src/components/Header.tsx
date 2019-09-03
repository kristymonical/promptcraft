import React from 'react';
import { Row } from 'react-bootstrap';
import { makeStyles } from '@material-ui/styles';

import { ToolkitLogo } from 'icons';

const useStyles = makeStyles({
  flexContainerOverriede: {
    alignItems: 'center',
    justifyContent: 'space-between'
  }
});

export default function Header() {
  const classes = useStyles();
  return (
    <>
      <Row className={classes.flexContainerOverriede}>
        <ToolkitLogo />
        <div>Other logo</div>
        {/* Hamburger menu icon placeholder */}
        <div>&nbsp;</div>
      </Row>
    </>
  );
}
