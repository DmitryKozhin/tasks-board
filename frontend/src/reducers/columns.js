import {
  SELECT_BOARD,
  CREATE_COLUMN,
  UPDATE_COLUMN,
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case CREATE_COLUMN: {
      return {
        ...state,
      };
    }
    case SELECT_BOARD: {
      return {
        ...state,
        columns: action.error ? null : action.payload.board.columns,
      };
    }

    case UPDATE_COLUMN: {
      let columns = state.columns.map((column) =>
        column.id === action.payload.column.id ? action.payload.column : column
      );
      return {
        ...state,
        columns: [...columns],
      };
    }

    default:
      return state;
  }
};
