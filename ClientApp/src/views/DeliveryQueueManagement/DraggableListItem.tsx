import React from 'react';
import { makeStyles } from '@material-ui/core';
import { Draggable } from 'react-beautiful-dnd';

import { SVT_THEME } from 'components';

interface DraggableListItemProps {
  index: number;
  item: any;
}

const createStyles = makeStyles<
  typeof SVT_THEME,
  Partial<DraggableListItemProps>
>({});

export default function DraggableListItem({
  index,
  item: { deliveryId, currentLocation }
}: DraggableListItemProps) {
  const classes = createStyles({});
  return (
    <Draggable draggableId={`${deliveryId}`} index={index}>
      {(provided, snapshot) => (
        <div
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
        >
          {deliveryId} - {currentLocation}
        </div>
      )}
    </Draggable>
  );
}
