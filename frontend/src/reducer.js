import auth from './reducers/auth';
import { combineReducers } from 'redux';
import common from './reducers/common';
import boards from './reducers/boards';
import columns from './reducers/columns';
import home from './reducers/home';
import tasks from './reducers/tasks';
import { routerReducer } from 'react-router-redux';

export default combineReducers({
  auth,
  common,
  home,
  boards,
  columns,
  tasks,
  router: routerReducer,
});
