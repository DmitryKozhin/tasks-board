import React from 'react';
import agent from '../../agent';
import { connect } from 'react-redux';
import {
  CHANGE_TAB,
  HIDE_ADD_BOARD,
  SHOW_ADD_BOARD,
  MAIN_VIEW_LOAD,
  SELECT_BOARD,
} from '../../constants/actionTypes';
import AddBoardModal from '../AddBoardModal';
import { Form, Button } from 'react-bootstrap';

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

class MainView extends React.Component {
  constructor() {
    super();

    this.showModal = (ev) => this.props.onShowModal();
    this.closeModal = (ev) => this.props.onCloseModal();
    this.selectBoard = (ev) => this.props.onSelectBoard(ev.target.value);
  }

  componentWillMount() {
    this.props.onLoad(agent.Board.all());
  }

  render() {
    if (!this.props.selectedBoard && this.props.boards) {
      this.props.onSelectBoard(this.props.boards[0].id);
    }

    return (
      <div className="col-md-9 board">
        <Form>
          <Form.Group>
            <div className="input-group mb-3">
              <Form.Control
                as="select"
                size="sm"
                custom
                onChange={this.selectBoard}
              >
                {this.props.boards ? (
                  this.props.boards.map((board) => {
                    return this.props.selectedBoard &&
                      this.props.selectedBoard.id === board.id ? (
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
                className="control-button"
                onClick={this.showModal}
              >
                Add
              </Button>
            </div>
          </Form.Group>
        </Form>

        {this.props.isShowing ? (
          <AddBoardModal show={this.showModal} onHide={this.closeModal} />
        ) : (
          ''
        )}
      </div>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(MainView);
