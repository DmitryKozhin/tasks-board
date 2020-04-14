import {
  SELECT_BOARD,
  CREATE_COLUMN,
  UPDATE_COLUMN,
  REMOVE_TASK,
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

    case REMOVE_TASK: {
      let columns = state.columns.map((column) =>
        column.id === action.payload.columnId
          ? {
              ...column,
              tasks: column.tasks.filter(
                (task) => task.id !== action.payload.taskId
              ),
            }
          : column
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
