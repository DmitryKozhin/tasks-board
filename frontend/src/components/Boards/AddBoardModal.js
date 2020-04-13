import React, { useCallback } from 'react';
import { connect } from 'react-redux';
import { Modal, Button, Form } from 'react-bootstrap';
import {
  HIDE_ADD_BOARD,
  SHOW_ADD_BOARD,
  CREATE_BOARD,
  UPDATE_ADD_BOARD_NAME,
} from '../../constants/actionTypes';
import agent from '../../agent';

const mapStateToProps = (state) => ({ ...state.boards });

const mapDispatchToProps = (dispatch) => ({
  onCreateBord: (name) => {
    if (!name) {
      return;
    }
    let payload = agent.Board.create(name);
    dispatch({ type: CREATE_BOARD, payload });
  },
  onUpdateName: (name) => dispatch({ type: UPDATE_ADD_BOARD_NAME, name }),
});

const AddBoardModal = (props) => {
  const changeName = (ev) => props.onUpdateName(ev.target.value);
  const saveAndCloseModal = useCallback(() => {
    props.onCreateBord(props.name);
  }, [props]);

  return (
    <Modal show={props.onShowModal} onHide={props.onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Add board</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Board name</Form.Label>
            <Form.Control
              type="input"
              onChange={changeName}
              value={props.name}
            />
            <Form.Text className="text-muted">
              The name of the board must be unique
            </Form.Text>
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={props.onHide}>
          Close
        </Button>
        <Button variant="primary" onClick={saveAndCloseModal}>
          Save
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(AddBoardModal);
