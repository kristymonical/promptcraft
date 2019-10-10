import React from 'react';
import { makeStyles, Typography } from '@material-ui/core';
import { DragDropContext, Droppable } from 'react-beautiful-dnd';

import { SVT_THEME, Overlay } from 'components';
import DraggableListItem from './DraggableListItem';
import { Row, Col } from 'react-bootstrap';

interface DraggableListProps {
  items: any[];
  itemIdKey: string;
  locked?: boolean;
  onDragEnd: (result: any) => void;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<DraggableListProps>>({
  listContainer: {
    flexDirection: 'column',
    flexGrow: 1,
    margin: '0 auto',
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
                key={`drag-delivery-${item[itemIdKey]}`}
                item={item}
                index={idx}
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
