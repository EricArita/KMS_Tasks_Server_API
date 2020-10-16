import * as React from 'react';
import PropTypes from 'prop-types';
import './AuthLayout.scss';
import { Divider } from 'antd';

export class AuthLayout extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      clientOnly: false,
      forgotPasswordModal: false,
    };
  }

  renderChild = () => {
    return this.props.children;
  };

  render() {
    return (
      <div className='auth-container'>
        <div className='form-wrapper' style={{ display: '' }}>
          <img className="logo" src="/images/kms-logo.png" width={'30%'} />
          {this.renderChild()}          
        </div>
        <Divider></Divider>
        <div className="footer">
          <p className={'copyright'}>Developed By I554 © 2020 KMS Technology</p>    
        </div>
      </div>
    );
  }
}

AuthLayout.propTypes = {
  pageName: 'login' | 'register'
};

AuthLayout.propTypes = {
  clientOnly: PropTypes.bool,
  forgotPasswordModal: PropTypes.bool
};