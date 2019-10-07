import React from 'react';
import { makeStyles } from '@material-ui/core';
import { DragDropContext, Droppable } from 'react-beautiful-dnd';

import { SVT_THEME, Overlay } from 'components';
import DraggableListItem from './DraggableListItem';

interface DraggableListProps {
  items: any[];
  itemIdKey: string;
  locked?: boolean;
  onDragEnd: (result: any) => void;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<DraggableListProps>>({
  listContainer: {
    alignItems: 'center',
    flexDirection: 'column',
    justifyContent: 'center',
    minWidth: '100%',
    position: 'relative'
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
