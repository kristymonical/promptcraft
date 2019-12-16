import React from 'react';
import './widget.scss';

export interface WidgetProps {
  children: React.ReactNode;
}

const Widget: React.FC<WidgetProps> = ({}) => {
  return <div>Widget</div>;
};

export default Widget;
