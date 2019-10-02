import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';

import { SVT_THEME, TitleCol } from 'components';
import { getDeliveryQueue } from 'services/DeliveryQueue';

interface DeliveryQueueManagementProps {}

const createStyles = makeStyles<
  typeof SVT_THEME,
  Partial<DeliveryQueueManagementProps>
>({});

export default function DeliveryQueueManagement({

}: DeliveryQueueManagementProps) {
  const classes = createStyles({});

  const [queue, setQueue] = useState<any[]>([]);

  useEffect(() => {
    getDeliveryQueue().then(res => setQueue(res));
  }, []);

  return (
    <>
      <Row>
        <TitleCol title='Delivery Queue Management' />
      </Row>
    </>
  );
}
