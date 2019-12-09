import React from 'react';
import Image from 'react-bootstrap/Image';

import logoImage from './logo.png';
import logoBigImage from './logobig.png';

export interface LogoProps {
  variant?: 'sm' | 'lg';
}

const Logo: React.FC<LogoProps> = ({ variant = 'sm' }) => {
  return (
    <Image
      style={{ background: 'black' }}
      src={variant === 'sm' ? logoImage : logoBigImage}
    ></Image>
  );
};

export default Logo;
