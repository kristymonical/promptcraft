import React from 'react';
import { makeStyles, Typography, Card } from '@material-ui/core';
import { Draggable } from 'react-beautiful-dnd';

import { SVT_THEME } from 'components';

interface DraggableListItemProps {
  index: number;
  item: any;
}

const createStyles = makeStyles<
  typeof SVT_THEME,
  Partial<DraggableListItemProps>
>({
  listItem: {
    padding: 10,
    marginBottom: 5
  }
});

export default function DraggableListItem({
  index,
  item: { deliveryId, currentLocation }
}: DraggableListItemProps) {
  const classes = createStyles({});
  return (
    <Draggable draggableId={`${deliveryId}`} index={index}>
      {(provided, snapshot) => (
        <Card
          className={classes.listItem}
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
        >
          <Typography>
            {deliveryId} - {currentLocation}
          </Typography>
        </Card>
      )}
    </Draggable>
  );
}
