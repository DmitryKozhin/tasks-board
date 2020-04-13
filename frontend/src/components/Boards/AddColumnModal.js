import React, { useCallback } from 'react';
import { connect } from 'react-redux';
import { Modal, Button, Form } from 'react-bootstrap';
import {
  HIDE_ADD_COLUMN,
  SHOW_ADD_COLUMN,
  UPDATE_ADD_COLUMN_FIELD,
  CREATE_COLUMN,
} from '../../constants/actionTypes';
import agent from '../../agent';

const mapStateToProps = (state) => ({
  ...state.columns,
  selectedBoard: state.boards.selectedBoard.id,
});

const mapDispatchToProps = (dispatch) => ({
  onCreateColumn: (header, color, boardId) => {
    if (!header) {
      return;
    }
    let payload = agent.Column.create(header, color, boardId);
    dispatch({ type: CREATE_COLUMN, payload });
  },
  onChangeHeader: (value) =>
    dispatch({ type: UPDATE_ADD_COLUMN_FIELD, key: 'header', value }),
  onChangeColor: (value) =>
    dispatch({ type: UPDATE_ADD_COLUMN_FIELD, key: 'color', value }),
});

const AddColumnModal = (props) => {
  // use callback после того, как поменять на FC
  // + bind action creators
  const changeHeader = (ev) => {
    props.onChangeHeader(ev.target.value);
  };
  const changeColor = (ev) => {
    props.onChangeColor(ev.target.value);
  };
  const saveAndCloseModal = useCallback(() => {
    props.onCreateColumn(props.header, props.color, props.selectedBoard);
  }, [props]);

  return (
    <Modal show={props.onShowModal} onHide={props.onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Add column</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Header</Form.Label>
            <Form.Control
              type="input"
              onChange={changeHeader}
              value={props.header}
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Color</Form.Label>
            <Form.Control
              type="input"
              onChange={changeColor}
              value={props.color}
            />
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

export default connect(mapStateToProps, mapDispatchToProps)(AddColumnModal);
