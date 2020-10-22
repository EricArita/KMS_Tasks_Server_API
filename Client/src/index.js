import React from 'react';
import { render } from 'react-dom';
import 'antd/dist/antd.css';
import { App } from './App';
import './App.scss';
import LoginPage from './pages/LoginPage';

render(<App />, document.getElementById('root'));
// render(<LoginPage />, document.getElementById('root'));
