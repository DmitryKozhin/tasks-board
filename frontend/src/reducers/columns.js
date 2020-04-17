import {
  SELECT_BOARD,
  CREATE_COLUMN,
  UPDATE_COLUMN,
  REMOVE_TASK,
  UPDATE_BOARD,
  REMOVE_COLUMN,
  CREATE_BOARD,
  CREATE_TASK,
  LOGOUT,
  UPDATE_TASK,
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case CREATE_COLUMN: {
      return {
        ...state,
      };
    }

    case CREATE_BOARD:
    case SELECT_BOARD: {
      return {
        ...state,
        columns: action.error ? null : action.payload.board.columns || [],
      };
    }

    case UPDATE_BOARD: {
      return {
        ...state,
        columns: action.payload.board.columns || [],
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

    case REMOVE_COLUMN: {
      return {
        ...state,
        columns: [
          ...state.columns.filter(
            (column) => column.id !== action.payload.columnId
          ),
        ],
      };
    }

    case UPDATE_TASK: {
      let columns = state.columns.map((column) =>
        column.id === action.payload.task.columnId
          ? {
              ...column,
              tasks: column.tasks.map((task) =>
                task.id === action.payload.task.id ? action.payload.task : task
              ),
            }
          : column
      );

      return {
        ...state,
        columns: [...columns],
      };
    }

    case CREATE_TASK: {
      let columns = state.columns.map((column) =>
        column.id === action.payload.task.columnId
          ? {
              ...column,
              tasks: (column.tasks || []).concat([action.payload.task]),
            }
          : column
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

    case LOGOUT: {
      return {
        ...state,
        columns: [],
      };
    }

    default:
      return state;
  }
};
