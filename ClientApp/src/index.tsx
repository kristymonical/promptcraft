import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import { Container } from 'react-bootstrap';

import { ThemeProvider, Header, Footer } from 'components';
import {
  CleanRequest,
  Menu,
  DeliveryRequest,
  CartHandling,
  StagingManagement
} from 'views';

const baseUrl =
  document.getElementsByTagName('base')[0].getAttribute('href') ||
  'https://localhost:5001'; // default to localhost for dev. technically this should never happen unless the browser itself is broken.

const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <ThemeProvider>
      <Container style={{ paddingTop: 15 }}>
        <Header />
        <Switch>
          <Route exact path='/' component={Menu} />
          <Route exact path='/staging' component={StagingManagement} />
          <Route exact path='/request' component={CartHandling} />
          <Route exact path='/request/delivery' component={DeliveryRequest} />
          <Route exact path='/request/clean' component={CleanRequest} />
          <Redirect to='/' />
        </Switch>
        <Footer />
      </Container>
    </ThemeProvider>
  </BrowserRouter>,
  rootElement
);
