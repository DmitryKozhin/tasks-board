import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const AddTaskModal = (props) => {
  const [header, setHeader] = useState('');
  const [description, setDescription] = useState('');

  const changeHeader = useCallback((ev) => {
    setHeader(ev.target.value);
  }, []);
  const changeDescription = useCallback((ev) => {
    setDescription(ev.target.value);
  }, []);
  const saveAndCloseModal = useCallback(() => {
    props.onCreate(header, description);
  }, [props, header, description]);

  return (
    <Modal show={props.isShowing} onHide={props.onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Add task</Modal.Title>
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
            <Form.Label>Description</Form.Label>
            <Form.Control
              type="input"
              onChange={changeDescription}
              value={props.description}
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

export default AddTaskModal;