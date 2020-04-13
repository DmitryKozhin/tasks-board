import React, { useEffect, useCallback } from 'react';
import agent from '../../agent';
import { connect } from 'react-redux';
import {
  CHANGE_TAB,
  HIDE_ADD_BOARD,
  SHOW_ADD_BOARD,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
} from '../../constants/actionTypes';
import AddBoardModal from '../Boards/AddBoardModal';
import { Form, Button } from 'react-bootstrap';
import Board from '../Boards/Board';

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
  onShowModal: () => dispatch({ type: SHOW_ADD_BOARD }),
  onCloseModal: () => dispatch({ type: HIDE_ADD_BOARD }),
  onLoad: (payload) => dispatch({ type: MAIN_VIEW_LOAD, payload }),
  onSelectBoard: (id) => {
    const payload = agent.Board.get(id);
    return dispatch({ type: SELECT_BOARD, payload });
  },
});

const MainView = (props) => {
  useEffect(() => {
    props.onLoad(agent.Board.all());

    return () => {
      if (!props.selectedBoard && props.boards) {
        props.onSelectBoard(props.boards[0].id);
      }
    };
  });

  const showModal = () => props.onShowModal();
  const closeModal = () => props.onCloseModal();
  const selectBoard = (ev) => {
    props.onSelectBoard(ev.target.value);
  };

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
                value={props.selectedBoard?.id}
                onChange={selectBoard}
              >
                {props.boards ? (
                  props.boards.map((board) => {
                    return props.selectedBoard &&
                      props.selectedBoard.id === board.id ? (
                      <option value={board.id} selected>
                        {board.name}
                      </option>
                    ) : (
                      <option value={board.id}>{board.name}</option>
                    );
                  })
                ) : (
                  <option>none</option>
                )}
              </Form.Control>
              <Button
                size="sm"
                className="home__add-board"
                onClick={props.onShowModal}
              >
                Add
              </Button>
            </div>
          </Form.Group>
        </Form>
      </div>

      {props.selectedBoard ? (
        <div className="home__board">
          <Board board={props.selectedBoard} />
        </div>
      ) : (
        ''
      )}

      {props.isShowing ? (
        <AddBoardModal onShowModal={showModal} onHide={closeModal} />
      ) : (
        ''
      )}
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MainView);
