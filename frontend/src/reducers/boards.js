import {
  CREATE_BOARD,
  REMOVE_BOARD,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
  UPDATE_BOARD,
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case REMOVE_BOARD:
      return {
        ...state,
        boards: state.boards.filter(
          (board) => board.id !== action.payload.boardId
        ),
        selectedBoard: null,
      };
    case CREATE_BOARD:
      return {
        ...state,
        boards: action.error
          ? null
          : (state.boards || []).concat([action.payload.board]),
        selectedBoard: action.payload.board,
      };
    case MAIN_VIEW_LOAD: {
      return {
        ...state,
        boards: action.error ? [] : action.payload.boards,
      };
    }

    case UPDATE_BOARD:
    case SELECT_BOARD: {
      return {
        ...state,
        selectedBoard: action.error ? null : action.payload.board,
      };
    }
    default:
      return state;
  }
};
