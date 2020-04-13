import {
  SHOW_ADD_BOARD,
  HIDE_ADD_BOARD,
  CREATE_BOARD,
  UPDATE_ADD_BOARD_NAME,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
  CREATE_COLUMN,
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case SHOW_ADD_BOARD:
      return {
        ...state,
        isShowing: true,
        name: '',
      };
    case HIDE_ADD_BOARD:
      return {
        ...state,
        isShowing: false,
        name: '',
      };
    case CREATE_BOARD:
      return {
        ...state,
        name: '',
        isShowing: false,
        boards: action.error
          ? null
          : (state.boards || []).concat([action.payload.board]),
        selectedBoard: action.payload.board,
      };

    case UPDATE_ADD_BOARD_NAME:
      return {
        ...state,
        name: action.name,
      };

    case MAIN_VIEW_LOAD: {
      return {
        ...state,
        boards: action.error ? [] : action.payload.boards,
      };
    }

    case SELECT_BOARD: {
      return {
        ...state,
        selectedBoard: action.error ? null : action.payload.board,
      };
    }

    case CREATE_COLUMN: {
      return {
        ...state,
        selectedBoard: {
          ...state.selectedBoard,
          columns: (state.selectedBoard.columns || []).concat([
            action.payload.column,
          ]),
        },
      };
    }

    default:
      return state;
  }
};
