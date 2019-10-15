import React, { useState } from 'react';
import {
  makeStyles,
  FormControlLabel,
  Checkbox,
  Typography,
  Slider,
  IconButton
} from '@material-ui/core';
import { Refresh } from '@material-ui/icons';
import { Col, Row } from 'react-bootstrap';

import { SVT_THEME } from 'components';
import { useInterval } from 'hooks';
import Button from './Button';

interface CallbackStuff {
  intervalCount: number;
}

interface AutoRefreshProps {
  callback: (stuff: CallbackStuff) => Promise<any> | any;
  children?: React.ReactNode;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<AutoRefreshProps>>({
  controlRow: {
    width: '100%',
    '& > *': {
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center'
    }
  }
});

export default function AutoRefresh({
  callback,
  children = null
}: AutoRefreshProps) {
  const classes = createStyles({});
  const [intervalCount, setIntervalCount] = useState(0);
  const [paused, setPaused] = useState(false);
  const [seconds, setSeconds] = useState(30);

  useInterval(() => {
    if (paused) return;
    setIntervalCount(old => old + 1);
    callback({
      intervalCount
    });
  }, seconds * 1000);

  return (
    <>
      <Row className={classes.controlRow}>
        <Col>
          <FormControlLabel
            control={
              <IconButton onClick={() => callback({ intervalCount })}>
                <Refresh />
              </IconButton>
            }
            label=''
          />

          <FormControlLabel
            control={
              <Checkbox
                checked={paused}
                onChange={() => setPaused(old => !old)}
              />
            }
            label='Pause Auto-Refresh'
          />
        </Col>
        <Col style={{ flexDirection: 'column' }}>
          <Typography>Auto-Refresh Rate: {seconds} seconds</Typography>
          <Slider
            min={15}
            max={120}
            step={1}
            value={seconds}
            onChange={(_, value) => setSeconds(value as number)}
          />
        </Col>
        {children && <Col>{children}</Col>}
      </Row>
    </>
  );
}
