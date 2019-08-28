import React from 'react';
import { Route, Redirect, Switch } from 'react-router-dom';

import Home from './components/Home';
import Header from './components/Header';
import Footer from './components/Footer';

export default function App() {
  return (
    <div>
      <Header />
      <Switch>
        <Route exact path='/' component={Home} />
        <Redirect to='/' />
      </Switch>
      <Footer />
    </div>
  );
}
