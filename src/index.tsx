import React from 'react';
import ReactDOM from 'react-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import NavBar from 'components/navbar/Navbar';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';

import './index.scss';
import {
  Dashboard,
  Lab,
  Marketplace,
  Login,
  Metrics,
  Notifications,
  Help,
  Tools
} from 'views';

const routes = [
  { path: '/login', component: Login },
  { path: '/dashboard', component: Dashboard },
  { path: '/lab', component: Lab },
  { path: '/tools', component: Tools },
  { path: '/marketplace', component: Marketplace },
  { path: '/metrics', component: Metrics },
  { path: '/notifications', component: Notifications },
  { path: '/help', component: Help }
];

ReactDOM.render(
  <BrowserRouter>
    <NavBar />
    <Switch>
      {routes.map(({ path, component }) => (
        <Route key={`route-${path}`} path={path} component={component} />
      ))}
      <Redirect to='/dashboard' />
    </Switch>
  </BrowserRouter>,
  document.getElementById('root')
);
