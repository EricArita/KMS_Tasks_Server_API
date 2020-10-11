import * as React from 'react';
import { Form, message, Input, Button, Divider } from 'antd';
import { AuthLayout } from '../components/layout/AuthLayout';
import './LoginPage.scss';
import { FormInstance } from 'antd/lib/form';

export default class LoginPage extends React.Component {
  state = {
    loading: {
      login: false,
      getVerifyCode: false,
      requestResetPassword: false,
      changePassword: false,
    },
  };
  formRef = React.createRef();

  constructor(props) {
    super(props);
  }

  async login() {   
    const info = await this.formRef.current.getFieldsValue();
    // firebase.auth().signInWithEmailAndPassword(info.email, info.password).catch(function (error) {
    //   message.error(error.message);
    // });
  }

  render() {
    return (
      <AuthLayout pageName='login'>
        <div className={'login-screen'}>
          <div className={'content-wrapper'}>
            <Form
              ref={this.formRef}
              onFinish={() => {
                this.login();
              }}
              key={'login-form'}
            >
              <Form.Item name='email'>
                <Input placeholder='Email' type='email' />
              </Form.Item>
              <Form.Item name='password'>
                <Input.Password placeholder={'Mật khẩu'} type='password'
                />
              </Form.Item>
              <Form.Item>
                <span className={'space-between-item button-accept-props'}>
                  <Button type='primary' loading={this.state.loading.login} htmlType='submit' style={{backgroundColor: '#14b1e7'}}>
                    Đăng nhập
                          </Button>
                </span>
              </Form.Item>
            </Form>
            <span className={'space-between-item'}>
              <Divider>
                <span>Hoặc</span>
              </Divider>
            </span>
            <span className={'space-between-item button-back-props'}>
              <Button>
                Quên mật khẩu
              </Button>
            </span>
          </div>
        </div>
      </AuthLayout>
    );
  }
}