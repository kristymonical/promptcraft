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
    if (!result.destination) return; // attempted to drop outside of droppable area
    if (result.destination.index === result.source.index) return; // dnd to same position

    const newQueue = queue.slice(); // avoid mutation
    const movedItem = newQueue.splice(result.source.index, 1)[0]; // remove item from queue
    newQueue.splice(result.destination.index, 0, movedItem); // insert it in new location

    setQueue(newQueue);

    setHasActiveRequest(true);

    if (result.destination.index === 0) {
      await moveDeliveryToTop(movedItem.deliveryId);
    } else if (result.destination.index === queue.length - 1) {
      await moveDeliveryToBottom(movedItem.deliveryId);
    } else {
      const newIdx = newQueue.findIndex(
        delivery => delivery.deliveryId === movedItem.deliveryId
      );

      await moveDeliveryInQueue(
        movedItem.deliveryId,
        newQueue[newIdx - 1].deliveryId,
        newQueue[newIdx + 1].deliveryId
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
