import React from 'react';
import ReactDOM from 'react-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import NavBar from 'components/navbar';
import { BrowserRouter } from 'react-router-dom';

ReactDOM.render(
  <BrowserRouter>
    <NavBar />
  </BrowserRouter>,
  document.getElementById('root')
);
