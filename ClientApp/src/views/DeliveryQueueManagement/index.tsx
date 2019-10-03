import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core';
import { Row } from 'react-bootstrap';
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';

import { SVT_THEME, TitleCol } from 'components';
import {
  getDeliveryQueue,
  GetDeliveryQueueResponse
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

  useEffect(() => {
    getDeliveryQueue().then(setQueue);
  }, []);

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
            onDragEnd={result => {
              if (!result.destination) return;

              const newQueue = queue.slice();
              const movedItem = newQueue.splice(result.source.index, 1);
              newQueue.splice(result.destination.index, 0, ...movedItem);

              setQueue(newQueue);
            }}
          />
        )}
      </Row>
    </>
  );
}
