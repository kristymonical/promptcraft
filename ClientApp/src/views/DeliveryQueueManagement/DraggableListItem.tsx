import React from 'react';
import { makeStyles, Typography, Card } from '@material-ui/core';
import { Draggable } from 'react-beautiful-dnd';

import { SVT_THEME } from 'components';
import { Row, Col } from 'react-bootstrap';

interface DraggableListItemProps {
  index: number;
  item: any;
  isDragDisabled?: boolean;
}

const createStyles = makeStyles<
  typeof SVT_THEME,
  Partial<DraggableListItemProps>
>({
  listItem: {
    padding: 10,
    marginBottom: 5,
    '& > .row': {
      margin: 'initial !important'
    }
  }
});

export default function DraggableListItem({
  index,
  item: { deliveryId, cartId, currentLocation, destinationArea, orderId },
  isDragDisabled: dragIsDisabled = false
}: DraggableListItemProps) {
  const classes = createStyles({});
  return (
    <Draggable
      draggableId={`${deliveryId}`}
      index={index}
      isDragDisabled={dragIsDisabled}
    >
      {(provided, snapshot) => (
        <Card
          className={classes.listItem}
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
        >
          <Row>
            <Col>
              <Typography>{index + 1}</Typography>
            </Col>
            <Col>
              <Typography>{orderId || <i>NONE</i>}</Typography>
            </Col>
            <Col>
              <Typography>{cartId}</Typography>
            </Col>
            <Col>
              <Typography>{currentLocation}</Typography>
            </Col>
            <Col>
              <Typography>{destinationArea}</Typography>
            </Col>
          </Row>
        </Card>
      )}
    </Draggable>
  );
}
