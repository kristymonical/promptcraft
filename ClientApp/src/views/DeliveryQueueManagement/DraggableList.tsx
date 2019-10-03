import React from 'react';
import { makeStyles } from '@material-ui/core';
import { DragDropContext, Droppable } from 'react-beautiful-dnd';

import { SVT_THEME } from 'components';
import DraggableListItem from './DraggableListItem';

interface DraggableListProps {
  items: any[];
  itemIdKey: string;
  onDragEnd: (result: any) => void;
}

const createStyles = makeStyles<typeof SVT_THEME, Partial<DraggableListProps>>(
  {}
);

export default function DraggableList({
  items,
  itemIdKey,
  onDragEnd
}: DraggableListProps) {
  const classes = createStyles({});
  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <Droppable droppableId='droppable'>
        {(provided, snapshot) => (
          <div {...provided.droppableProps} ref={provided.innerRef}>
            {items.map((item, idx) => (
              <DraggableListItem
                key={`drag-delivery-${item[itemIdKey]}`}
                item={item}
                index={idx}
              />
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </DragDropContext>
  );
}
