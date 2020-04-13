import { SELECT_BOARD, CREATE_COLUMN } from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case CREATE_COLUMN: {
      return {
        ...state,
        isShowing: false,
      };
    }
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
