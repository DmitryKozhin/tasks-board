import {
  UPDATE_ADD_COLUMN_FIELD,
  SHOW_ADD_COLUMN,
  HIDE_ADD_COLUMN,
  SELECT_BOARD,
  CREATE_COLUMN,
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case CREATE_COLUMN: {
      return {
        ...state,
        isShowing: false,
      };
    }
    case SHOW_ADD_COLUMN:
      return {
        ...state,
        isShowing: true,
        header: '',
        color: '',
      };
    case HIDE_ADD_COLUMN:
      return {
        ...state,
        isShowing: false,
        header: '',
        color: '',
      };
    case UPDATE_ADD_COLUMN_FIELD:
      return { ...state, [action.key]: action.value };

    case SELECT_BOARD: {
      return {
        ...state,
        isShowing: false,
      };
    }
    default:
      return state;
  }
};
