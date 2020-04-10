import React from 'react';
import { connect } from 'react-redux';
import { Modal, Button, Form } from 'react-bootstrap';
import {
  HIDE_ADD_BOARD,
  SHOW_ADD_BOARD,
  CREATE_BOARD,
  UPDATE_ADD_BOARD_NAME,
} from '../constants/actionTypes';
import agent from '../agent';

const mapStateToProps = (state) => ({ ...state.boards });

const mapDispatchToProps = (dispatch) => ({
  onCreateBord: (newBoardName) => {
    if (!newBoardName) {
      return;
    }
    let payload = agent.Board.create(newBoardName);
    dispatch({ type: CREATE_BOARD, payload });
  },
  onShowModal: () => dispatch({ type: SHOW_ADD_BOARD }),
  onCloseModal: () => dispatch({ type: HIDE_ADD_BOARD }),
  onUpdateName: (newBoardName) =>
    dispatch({ type: UPDATE_ADD_BOARD_NAME, newBoardName }),
});

class AddBoardModal extends React.Component {
  constructor() {
    super();

    this.changeName = (ev) => this.props.onUpdateName(ev.target.value);
    this.saveAndCloseModal = () =>
      this.props.onCreateBord(this.props.newBoardName);
    this.closeModal = (ev) => this.props.onCloseModal();
    this.showModal = (ev) => this.props.onShowModal();
  }

  render() {
    const newBoardName = this.props.newBoardName;
    return (
      <Modal show={this.showModal} onHide={this.closeModal}>
        <Modal.Header closeButton>
          <Modal.Title>Add board</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group>
              <Form.Label>Board name</Form.Label>
              <Form.Control
                type="input"
                onChange={this.changeName}
                value={this.props.newBoardName}
              />
              <Form.Text className="text-muted">
                The name of the board must be unique
              </Form.Text>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={this.closeModal}>
            Close
          </Button>
          <Button variant="primary" onClick={this.saveAndCloseModal}>
            Save
          </Button>
        </Modal.Footer>
      </Modal>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(AddBoardModal);
