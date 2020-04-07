import React from 'react';
import agent from '../../agent';
import { connect } from 'react-redux';
import { CHANGE_TAB } from '../../constants/actionTypes';

const mapStateToProps = (state) => ({
  token: state.common.token,
});

const mapDispatchToProps = (dispatch) => ({
  onTabClick: (tab, pager, payload) =>
    dispatch({ type: CHANGE_TAB, tab, pager, payload }),
});

const MainView = (props) => {
  return (
    <div className="col-md-9">
      <p>Boards</p>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainView);
