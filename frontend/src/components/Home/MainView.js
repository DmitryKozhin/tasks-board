import React, { useEffect, useState } from 'react';
import agent from '../../agent';
import { connect } from 'react-redux';
import {
  CHANGE_TAB,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
  CREATE_BOARD,
  UPDATE_BOARD,
} from '../../constants/actionTypes';
import AddBoardModal from '../Board/AddBoardModal';
import { Form, Button, Tooltip, OverlayTrigger } from 'react-bootstrap';
import Board from '../Board/Board';
import { REMOVE_BOARD } from '../../constants/actionTypes';
import { FaPlus, FaTrash, FaPen } from 'react-icons/fa';
import { useCallback } from 'react';

const mapStateToProps = (state) => ({
  ...state.boards,
  token: state.common.token,
  isModalShowing: state.boards.isModalShowing,
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

  onEditBoard: (id, name) => {
    if (!name) {
      return;
    }

    let payload = agent.Board.edit(id, { name });
    dispatch({ type: UPDATE_BOARD, payload });
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
  const [isModalShowing, setModalShow] = useState(false);
  const [isEditState, setEdit] = useState(false);

  useEffect(() => {
    props.onLoad(agent.Board.all());
    // eslint-disable-next-line
  }, [props.onLoad]);

  const showModal = () => setModalShow(true);
  const closeModal = () => setModalShow(false);
  const createBoard = useCallback(
    (name) => {
      props.onCreateBord(name);
      setModalShow(false);
    },
    [props.onCreateBord, setModalShow]
  );

  const removeBoard = useCallback(() => {
    if (props.selectedBoard) {
      props.onRemoveBoard(props.selectedBoard.id);
    }
  }, [props.selectedBoard, props.onRemoveBoard]);

  const editBoard = () => {
    setEdit(true);
    setModalShow(true);
  };

  const updateBoard = useCallback(
    (name) => {
      props.onEditBoard(props.selectedBoard.id, name);
      setEdit(false);
      setModalShow(false);
    },
    [props.selectedBoard, props.onEditBoard]
  );

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
              <OverlayTrigger overlay={<Tooltip>Edit a board</Tooltip>}>
                <Button
                  disabled={!props.selectedBoard}
                  size="sm"
                  onClick={editBoard}
                  className="home__add-board"
                >
                  <FaPen />
                </Button>
              </OverlayTrigger>
              <OverlayTrigger overlay={<Tooltip>Remove a board</Tooltip>}>
                <Button
                  disabled={!props.selectedBoard}
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
        isShowing={isModalShowing}
        board={isEditState ? props.selectedBoard : null}
        onHide={closeModal}
        onSave={isEditState ? updateBoard : createBoard}
      />
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainView);
