import agent from '../agent';
import Header from './Header';
import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { APP_LOAD, REDIRECT, LOGOUT } from '../constants/actionTypes';
import { Route, Switch } from 'react-router-dom';
import Home from '../components/Home';
import Login from '../components/Login';
import Register from '../components/Register';
import { store } from '../store';
import { push } from 'react-router-redux';

const mapStateToProps = (state) => {
  return {
    appLoaded: state.common.appLoaded,
    appName: state.common.appName,
    currentUser: state.common.currentUser,
    redirectTo: state.common.redirectTo,
  };
};

const mapDispatchToProps = (dispatch) => ({
  onLoad: (payload, token) =>
    dispatch({ type: APP_LOAD, payload, token, skipTracking: true }),
  onRedirect: () => dispatch({ type: REDIRECT }),
  onLogout: () => dispatch({ type: LOGOUT }),
});

const App = (props) => {
  useEffect(() => {
    const token = window.localStorage.getItem('jwt');
    if (token) {
      agent.setToken(token);
    }

    props.onLoad(token ? agent.Auth.current() : null, token);
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    if (props.redirectTo) {
      store.dispatch(push(props.redirectTo));
      props.onRedirect();
    }
  }, [props]);

  return props.appLoaded ? (
    <div>
      <Header
        appName={props.appName}
        currentUser={props.currentUser}
        onLogout={props.onLogout}
      />
      <Switch>
        <Route exact path="/" component={Home} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
      </Switch>
    </div>
  ) : (
    <div>
      <Header appName={props.appName} currentUser={props.currentUser} />
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
