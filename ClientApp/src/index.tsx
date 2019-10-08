import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { ThemeProvider, Header, Footer } from 'components';
import {
  CleanRequest,
  Menu,
  DeliveryRequest,
  CartHandling,
  StagingManagement,
  DeliveryQueueManagement
} from 'views';

const baseUrl =
  document.getElementsByTagName('base')[0].getAttribute('href') ||
  'https://localhost:5001'; // default to localhost for dev. technically this should never happen unless the browser itself is broken.

const rootElement = document.getElementById('root');

// global configuration for toasts
// ToastContainer is mounted on demand so we only need this configuration
toast.configure({
  autoClose: 5 * 1000, // 5 second autoclose delay
  position: toast.POSITION.BOTTOM_LEFT
});

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <ThemeProvider>
      <Container style={{ paddingTop: 15 }}>
        <Header />
        <Switch>
          <Route exact path='/' component={Menu} />
          <Route exact path='/request/cart' component={CartHandling} />
          <Route exact path='/request/delivery' component={DeliveryRequest} />
          <Route exact path='/request/clean' component={CleanRequest} />
          <Route exact path='/manage/staging' component={StagingManagement} />
          <Route
            exact
            path='/manage/queue'
            component={DeliveryQueueManagement}
          />
          <Redirect to='/' />
        </Switch>
        <Footer />
      </Container>
    </ThemeProvider>
  </BrowserRouter>,
  rootElement
);
