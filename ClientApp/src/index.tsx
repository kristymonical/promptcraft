import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import registerServiceWorker from './registerServiceWorker';
import { ThemeProvider, Header, Footer } from './components';
import { Menu, ManualRequest } from './views';

const baseUrl =
  document.getElementsByTagName('base')[0].getAttribute('href') ||
  'https://localhost:5001'; // default to localhost for dev. technically this should never happen unless the browser itself is broken.

const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <ThemeProvider>
      <Header />
      <Switch>
        <Route exact path='/' component={Menu} />
        <Route exact path='/request/manual' component={ManualRequest} />
        <Redirect to='/' />
      </Switch>
      <Footer />
    </ThemeProvider>
  </BrowserRouter>,
  rootElement
);

registerServiceWorker();
