import React from 'react';
import './navbar.scss';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import { Link, NavLink } from 'react-router-dom';
import Logo from 'components/logo/logo';

export interface NavBarProps {}

const navItems = [
  { label: 'dashboard', link: '/dashboard' },
  { label: 'store', link: '/marketplace' },
  { label: 'tools', link: '/tools' },
  { label: 'metrics', link: '/metrics' },
  { label: 'notifications', link: '/notifications' },
  { label: 'customer success', link: '/help' }
];

const NavBar: React.FC<NavBarProps> = ({}) => {
  return (
    <Navbar>
      <Navbar.Brand as={Link} to='/dashboard'>
        <Logo />
      </Navbar.Brand>
      <Nav>
        {navItems.map(({ label, link }) => (
          <Nav.Link
            key={`link-${label}`}
            className='nav-item'
            as={NavLink}
            to={link}
          >
            {label}
          </Nav.Link>
        ))}
      </Nav>
    </Navbar>
  );
};

export default NavBar;
