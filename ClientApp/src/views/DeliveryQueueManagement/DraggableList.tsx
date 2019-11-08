import React from 'react';
import { makeStyles, Typography } from '@material-ui/core';
import {
  DragDropContext,
  Droppable,
  DragDropContextProps
} from 'react-beautiful-dnd';

import { SVT_THEME, Overlay } from 'components';
import DraggableListItem from './DraggableListItem';
import { Row, Col } from 'react-bootstrap';

interface DraggableListProps {
  isDragDisabled?: boolean;
  items: any[];
  itemIdKey: string;
  locked?: boolean;
  onDragEnd: DragDropContextProps['onDragEnd'];
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<DraggableListProps>>({
  listContainer: {
    flexDirection: 'column',
    flexGrow: 1,
    margin: '0 auto !important',
    position: 'relative',
    '& .row': {
      justifyContent: 'space-around',
      '& .col': {
        maxWidth: '25%'
      }
    }
  },
  headerRow: {
    padding: 10,
    '& .col > p': {
      fontWeight: 'bold'
    }
  }
});

export default function DraggableList({
  isDragDisabled = false,
  items,
  itemIdKey,
  locked = false,
  onDragEnd
}: DraggableListProps) {
  const classes = createStyles({});
  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <Droppable droppableId='droppable'>
        {(provided, snapshot) => (
          <div
            className={classes.listContainer}
            {...provided.droppableProps}
            ref={provided.innerRef}
          >
            <Row className={classes.headerRow}>
              <Col>
                <Typography>Priority</Typography>
              </Col>
              <Col>
                <Typography>Order Id</Typography>
              </Col>
              <Col>
                <Typography>Cart Id</Typography>
              </Col>
              <Col>
                <Typography>Current Location</Typography>
              </Col>
              <Col>
                <Typography>Destination Area</Typography>
              </Col>
            </Row>
            {items.map((item, idx) => (
              <DraggableListItem
                index={idx}
                isDragDisabled={isDragDisabled}
                item={item}
                key={`drag-delivery-${item[itemIdKey]}`}
              />
            ))}
            {provided.placeholder}
            {locked && <Overlay />}
          </div>
        )}
      </Droppable>
    </DragDropContext>
  );
}
