import MainView from './MainView';
import React, { useEffect } from 'react';
import { Redirect } from 'react-router-dom';
import { connect } from 'react-redux';
import {
  HOME_PAGE_LOADED,
  HOME_PAGE_UNLOADED,
} from '../../constants/actionTypes';

const mapStateToProps = (state) => ({
  ...state.home,
  appName: state.common.appName,
  token: state.common.token,
  currentUser: state.common.currentUser,
});

const mapDispatchToProps = (dispatch) => ({
  onLoad: (tab, pager, payload) =>
    dispatch({ type: HOME_PAGE_LOADED, tab, pager, payload }),
  onUnload: () => dispatch({ type: HOME_PAGE_UNLOADED }),
});

const Home = (props) => {
  useEffect(() => {
    props.onLoad();
  }, [props]);

  useEffect(() => {
    return () => {
      props.onUnload();
    };
  }, [props]);

  return (
    <div className="home-page">
      <div>
        <div>{props.currentUser ? <MainView /> : <Redirect to="/login" />}</div>
      </div>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Home);
