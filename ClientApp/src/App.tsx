import React from 'react';
import { Route, Redirect, Switch } from 'react-router-dom';

export default function App() {
  return (
    <Switch>
      <Route exact path='/' render={() => 'Test'} />
      <Redirect to='/' />
    </Switch>
  );
}
