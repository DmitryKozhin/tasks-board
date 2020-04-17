import { CREATE_TASK } from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case CREATE_TASK: {
      return {
        ...state,
      };
    }

    default:
      return state;
  }
};
