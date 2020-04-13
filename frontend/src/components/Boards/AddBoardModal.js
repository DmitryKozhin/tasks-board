import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const AddBoardModal = (props) => {
  const [name, setName] = useState('');
  const changeName = useCallback((ev) => {
    setName(ev.target.value);
  }, []);
  const saveAndCloseModal = useCallback(() => {
    props.onCreate(name);
  }, [props, name]);

  return (
    <Modal show={props.isShowing} onHide={props.onHide}>
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

export default AddBoardModal;
