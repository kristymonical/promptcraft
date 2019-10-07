import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';

import { SVT_THEME, TitleCol } from 'components';
import {
  getDeliveryQueue,
  GetDeliveryQueueResponse,
  moveDeliveryToTop,
  moveDeliveryToBottom,
  moveDeliveryInQueue
} from 'services/DeliveryQueue';
import DraggableList from './DraggableList';

interface DeliveryQueueManagementProps {}

const createStyles = makeStyles<
  typeof SVT_THEME,
  Partial<DeliveryQueueManagementProps>
>({});

export default function DeliveryQueueManagement({

}: DeliveryQueueManagementProps) {
  const classes = createStyles({});

  const [queue, setQueue] = useState<GetDeliveryQueueResponse[]>([]);
  const [hasActiveRequest, setHasActiveRequest] = useState(false);

  useEffect(() => {
    getDeliveryQueue().then(setQueue);
  }, []);

  const onDragEnd = async (result: any) => {
    if (!result.destination) return;
    if (result.destination.index === result.source.index) return;

    const newQueue = queue.slice();
    const movedItem = newQueue.splice(result.source.index, 1)[0];
    newQueue.splice(result.destination.index, 0, movedItem);

    setQueue(newQueue);

    setHasActiveRequest(true);

    if (result.destination.index === 0) {
      await moveDeliveryToTop(movedItem.deliveryId);
    } else if (result.destination.index === queue.length - 1) {
      await moveDeliveryToBottom(movedItem.deliveryId);
    } else {
      const newParent = queue[result.destination.index];
      const newChild = queue[result.destination.index + 1];
      console.table([newParent, newChild]);
      await moveDeliveryInQueue(
        movedItem.deliveryId,
        newParent.deliveryId,
        newChild.deliveryId
      );
    }

    setHasActiveRequest(false);
  };

  return (
    <>
      <Row>
        <TitleCol title='Delivery Queue Management' />
      </Row>
      <Row>
        {queue.length > 0 && (
          <DraggableList
            items={queue}
            itemIdKey='deliveryId'
            locked={hasActiveRequest}
            onDragEnd={onDragEnd}
          />
        )}
      </Row>
    </>
  );
}
