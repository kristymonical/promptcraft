import React, { useEffect, useState } from 'react';
import { makeStyles, Typography } from '@material-ui/core';
import { Button } from 'components';

interface TimerProps {
  minutes: number;
  onTimerEnd?: () => void;
  userCanCancel?: boolean;
  userCancelThreshold?: number;
}

const createStyles = makeStyles({
  timerRoot: {
    alignItems: 'center',
    background:
      'transparent radial-gradient(closest-side at 50% 50%, #2C3E50 0%, #2C3E50 80%, #713F13 85%, #E67E22 100%)',
    borderRadius: '30vh',
    display: 'flex',
    flexDirection: 'column',
    height: '60vh',
    justifyContent: 'center',
    position: 'relative',
    width: '60vh'
  },
  timerCountdown: {
    marginTop: -30,
    '& > :first-child': {
      fontSize: 160
    },
    '& > :not(:first-child)': {
      fontSize: 60
    }
  },
  timerLabel: {
    fontSize: 24,
    textTransform: 'uppercase'
  }
});

export default function Timer({
  minutes,
  onTimerEnd,
  userCanCancel = false,
  userCancelThreshold = 0
}: TimerProps) {
  const classes = createStyles({});
  const [timeRemaining, setTimeRemaining] = useState(minutes * 60); // number of seconds left
  const [, setIntervalId] = useState();

  useEffect(() => {
    if (timeRemaining === 0 && typeof onTimerEnd !== 'undefined') {
      onTimerEnd();
      setTimeRemaining(-1); // prevent a re-render from executing this again
    }
  }, [onTimerEnd, timeRemaining]);

  useEffect(() => {
    const iv = setInterval(() => setTimeRemaining(time => time - 1), 1000);

    setIntervalId(iv);

    return () => clearInterval(iv);
  }, []);

  return (
    <div className={classes.timerRoot}>
      <div className={classes.timerCountdown}>
        <span>{Math.floor(timeRemaining / 60)}</span>
        <span>:{(timeRemaining % 60).toString().padStart(2, '0')}</span>
      </div>
      <Typography className={classes.timerLabel}>Minutes Remaining</Typography>
      <Button>Cancel</Button>
    </div>
  );
}
