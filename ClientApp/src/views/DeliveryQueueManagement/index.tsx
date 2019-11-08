import React, { useState, useEffect, useCallback } from 'react';
import { Row } from 'react-bootstrap';
import { makeStyles } from '@material-ui/styles';

import { TitleCol, AutoRefresh, SVT_THEME, Select } from 'components';
import {
  getDeliveryQueue,
  GetDeliveryQueueResponse,
  moveDeliveryToTop,
  moveDeliveryToBottom,
  moveDeliveryInQueue
} from 'services/DeliveryQueue';
import DraggableList from './DraggableList';
import { Typography, Card } from '@material-ui/core';

const createStyles = makeStyles<typeof SVT_THEME>({
  queueContainer: {
    maxWidth: 850,
    margin: '0 auto !important'
  },
  emptyQueueCard: {
    padding: 10,
    width: '100%',
    textAlign: 'center'
  },
  poolSelectLabel: {
    alignItems: 'center',
    '& > span': {
      marginRight: 15
    }
  }
});

export default function DeliveryQueueManagement() {
  const [queue, setQueue] = useState<GetDeliveryQueueResponse[]>([]);
  const [hasActiveRequest, setHasActiveRequest] = useState(false);
  const [pool, setPool] = useState('1');
  const classes = createStyles({});

  const refreshQueue = useCallback(() => {
    if (pool !== 'all') {
      getDeliveryQueue(pool).then(setQueue);
      setHasActiveRequest(false);
      return;
    }

    const promises = ['1', '2', '3'].map(p => getDeliveryQueue(p));

    Promise.all(promises)
      .then(results => results.reduce((acc, cur) => acc.concat(cur), []))
      .then(setQueue)
      .then(() => setHasActiveRequest(false));
  }, [pool]);

  useEffect(refreshQueue, [pool]);

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
      <Row className={classes.queueContainer}>
        <AutoRefresh callback={refreshQueue}>
          <Select
            className={classes.poolSelectLabel}
            direction='row'
            handleChange={newPool => setPool(newPool)}
            items={['all', '1', '2', '3']}
            label={<Typography>Pool</Typography>}
            value={pool}
          />
        </AutoRefresh>
        {queue.length > 0 ? (
          <DraggableList
            isDragDisabled={pool == 'all'}
            items={queue}
            itemIdKey='deliveryId'
            locked={hasActiveRequest}
            onDragEnd={onDragEnd}
          />
        ) : (
          <Card className={classes.emptyQueueCard}>
            <Typography>No deliveries in queue</Typography>
          </Card>
        )}
      </Row>
    </>
  );
}
