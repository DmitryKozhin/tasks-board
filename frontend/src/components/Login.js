import { Link, Redirect } from 'react-router-dom';
import React, { useState, useCallback } from 'react';
import agent from '../agent';
import { connect } from 'react-redux';
import { LOGIN } from '../constants/actionTypes';

const mapStateToProps = (state) => ({
  inProgress: state.auth.inProgress,
  currentUser: state.common.currentUser,
  errors: state.auth.errors,
});

const mapDispatchToProps = (dispatch) => ({
  onSubmit: (email, password) =>
    dispatch({ type: LOGIN, payload: agent.Auth.login(email, password) }),
});

const Login = (props) => {
  let [email, setEmail] = useState('');
  let [password, setPassword] = useState('');

  const submitForm = useCallback(
    (ev) => {
      ev.preventDefault();
      props.onSubmit(email, password);
    },
    [props.onSubmit, email, password]
  );

  if (props.currentUser) {
    return <Redirect to="/" />;
  }

  return (
    <div className="auth">
      <div className="container auth__page">
        <div className="row auth__form">
          <div className="col-md-6 offset-md-3 col-xs-12">
            <h1 className="text-xs-center">Sign In</h1>
            <p className="text-xs-center">
              <Link to="/register">Need an account?</Link>
            </p>
            {props.errors ? (
              <p className="auth__error">{props.errors.Error}</p>
            ) : null}

            <form onSubmit={submitForm}>
              <fieldset>
                <fieldset className="form-group">
                  <input
                    className="form-control form-control-lg"
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(ev) => setEmail(ev.target.value)}
                  />
                </fieldset>

                <fieldset className="form-group">
                  <input
                    className="form-control form-control-lg"
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(ev) => setPassword(ev.target.value)}
                  />
                </fieldset>

                <button
                  className="btn btn-lg btn-primary pull-xs-right"
                  type="submit"
                  disabled={props.inProgress}
                >
                  Sign in
                </button>
              </fieldset>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Login);
