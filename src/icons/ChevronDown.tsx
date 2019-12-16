import React from 'react';

export interface ChevronDownProps {
  size?: number;
}

const ChevronDown: React.FC<ChevronDownProps> = ({ size = 20 }) => {
  return (
    <span
      style={{
        borderLeft: `${size}px solid transparent`,
        borderRight: `${size}px solid transparent`,
        borderTop: `${size * 2}px solid #1D1D2C`,
        height: 0,
        width: 0
      }}
    ></span>
  );
};

export default ChevronDown;
