import { Link, Redirect } from 'react-router-dom';
import React, { useEffect, useCallback, useState } from 'react';
import agent from '../agent';
import { connect } from 'react-redux';
import { REGISTER, REGISTER_PAGE_UNLOADED } from '../constants/actionTypes';

const mapStateToProps = (state) => ({
  inProgress: state.auth.inProgress,
  currentUser: state.common.currentUser,
});

const mapDispatchToProps = (dispatch) => ({
  onSubmit: (username, email, password) => {
    const payload = agent.Auth.register(username, email, password);
    dispatch({ type: REGISTER, payload });
  },
  onUnload: () => dispatch({ type: REGISTER_PAGE_UNLOADED }),
});

const Register = (props) => {
  let [username, setUsername] = useState('');
  let [email, setEmail] = useState('');
  let [password, setPassword] = useState('');

  useEffect(() => {
    return () => {
      props.onUnload();
    };
  }, [props]);

  const changeEmail = useCallback((ev) => {
    setEmail(ev.target.value);
  }, []);
  const changePassword = useCallback((ev) => {
    setPassword(ev.target.value);
  }, []);
  const changeUsername = useCallback((ev) => {
    setUsername(ev.target.value);
  }, []);
  const submitForm = useCallback(
    (ev) => {
      ev.preventDefault();
      props.onSubmit(username, email, password);
    },
    [props, username, email, password]
  );

  if (props.currentUser) {
    return <Redirect to="/" />;
  }

  return (
    <div className="auth">
      <div className="container auth__page">
        <div className="row auth__form">
          <div className="col-md-6 offset-md-3 col-xs-12">
            <h1 className="text-xs-center">Sign Up</h1>
            <p className="text-xs-center">
              <Link to="/login">Have an account?</Link>
            </p>

            <form onSubmit={submitForm}>
              <fieldset>
                <fieldset className="form-group">
                  <input
                    className="form-control form-control-lg"
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={changeUsername}
                  />
                </fieldset>

                <fieldset className="form-group">
                  <input
                    className="form-control form-control-lg"
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={changeEmail}
                  />
                </fieldset>

                <fieldset className="form-group">
                  <input
                    className="form-control form-control-lg"
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={changePassword}
                  />
                </fieldset>

                <button
                  className="btn btn-lg btn-primary pull-xs-right"
                  type="submit"
                  disabled={props.inProgress}
                >
                  Sign up
                </button>
              </fieldset>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Register);
