import React, { useEffect, useState } from 'react';
import agent from '../../agent';
import { connect } from 'react-redux';
import {
  CHANGE_TAB,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
  CREATE_BOARD,
} from '../../constants/actionTypes';
import AddBoardModal from '../Boards/AddBoardModal';
import { Form, Button, Tooltip, OverlayTrigger } from 'react-bootstrap';
import Board from '../Boards/Board';
import { REMOVE_BOARD } from './../../constants/actionTypes';
import { FaPlus, FaTrash } from 'react-icons/fa';

const mapStateToProps = (state) => ({
  ...state.boards,
  token: state.common.token,
  isShowing: state.boards.isShowing,
  boards: state.boards.boards,
  selectedBoard: state.boards.selectedBoard,
});

const mapDispatchToProps = (dispatch) => ({
  onTabClick: (tab, pager, payload) =>
    dispatch({ type: CHANGE_TAB, tab, pager, payload }),
  onLoad: (payload) => dispatch({ type: MAIN_VIEW_LOAD, payload }),
  onSelectBoard: (id) => {
    const payload = agent.Board.get(id);
    return dispatch({ type: SELECT_BOARD, payload });
  },
  onCreateBord: (name) => {
    if (!name) {
      return;
    }
    let payload = agent.Board.create(name);
    dispatch({ type: CREATE_BOARD, payload });
  },

  onRemoveBoard: (id) => {
    let payload = agent.Board.delete(id);
    dispatch({
      type: REMOVE_BOARD,
      payload: {
        ...payload,
        boardId: id,
      },
    });
  },
});

const MainView = (props) => {
  const [isShowing, setShow] = useState(false);

  useEffect(() => {
    props.onLoad(agent.Board.all());
  }, []);

  const showModal = () => setShow(true);
  const closeModal = () => setShow(false);
  const createBoard = (name) => {
    props.onCreateBord(name);
    setShow(false);
  };

  const removeBoard = () => {
    if (props.selectedBoard) {
      props.onRemoveBoard(props.selectedBoard.id);
    }
  };

  const selectBoard = (ev) => {
    props.onSelectBoard(ev.target.value);
  };

  if (props.boards && props.boards.length > 0 && !props.selectedBoard) {
    props.onSelectBoard(props.boards[0].id);
  }

  return (
    <div className="home">
      <div className="home__board-selector">
        <Form>
          <Form.Group>
            <div className="input-group">
              <Form.Control
                as="select"
                size="sm"
                custom
                onChange={selectBoard}
                value={props.selectedBoard?.id}
                defaultValue={'default'}
              >
                <option value="default" disabled>
                  Choose a board ...
                </option>
                {props.boards?.map((board) => {
                  return (
                    <option key={board.id} value={board.id}>
                      {board.name}
                    </option>
                  );
                })}
              </Form.Control>
              <OverlayTrigger overlay={<Tooltip>Add a board</Tooltip>}>
                <Button
                  size="sm"
                  className="home__add-board"
                  onClick={showModal}
                >
                  <FaPlus />
                </Button>
              </OverlayTrigger>
              <OverlayTrigger overlay={<Tooltip>Remove a board</Tooltip>}>
                <Button
                  size="sm"
                  onClick={removeBoard}
                  className="home__add-board"
                >
                  <FaTrash />
                </Button>
              </OverlayTrigger>
            </div>
          </Form.Group>
        </Form>
      </div>

      {props.selectedBoard ? (
        <div className="home__board">
          <Board />
        </div>
      ) : (
        ''
      )}

      <AddBoardModal
        isShowing={isShowing}
        onHide={closeModal}
        onCreate={createBoard}
      />
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainView);
