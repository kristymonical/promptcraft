import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import ThemeProvider from './components/ThemeProvider';

const baseUrl =
  document.getElementsByTagName('base')[0].getAttribute('href') ||
  'https://localhost:5001';

const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <ThemeProvider>
      <App />
    </ThemeProvider>
  </BrowserRouter>,
  rootElement
);

registerServiceWorker();
