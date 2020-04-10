import auth from './reducers/auth';
import { combineReducers } from 'redux';
import common from './reducers/common';
import boards from './reducers/boards';
import home from './reducers/home';
import { routerReducer } from 'react-router-redux';

export default combineReducers({
  auth,
  common,
  home,
  boards,
  router: routerReducer,
});
