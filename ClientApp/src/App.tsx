import React from 'react';
import { Route, Redirect, Switch } from 'react-router-dom';

import Home from './components/Home';
import Header from './components/Header';
import Footer from './components/Footer';
import ManualRequest from './components/ManualRequest';

export default function App() {
  return (
    <div>
      <Header />
      <Switch>
        <Route exact path='/' component={Home} />
        <Route exact path='/request/manual' component={ManualRequest} />
        <Redirect to='/' />
      </Switch>
      <Footer />
    </div>
  );
}
