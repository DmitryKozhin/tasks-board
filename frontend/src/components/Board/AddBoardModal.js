import React, { useCallback, useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const AddBoardModal = ({ board, isShowing, onSave, onHide }) => {
  const [name, setName] = useState('');

  useEffect(() => {
    setName(board?.name || '');
  }, [board, setName]);

  const saveAndCloseModal = useCallback(() => {
    onSave(name);
    setName('');
  }, [setName, onSave, name]);

  const hide = useCallback(() => {
    onHide();
    setName('');
  }, [setName, onHide]);

  return (
    <Modal show={isShowing} onHide={hide}>
      <Modal.Header closeButton>
        <Modal.Title>{`${board ? 'Edit' : 'Add'} board`}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Board name</Form.Label>
            <Form.Control
              type="input"
              onChange={(ev) => setName(ev.target.value)}
              value={name}
            />
            <Form.Text className="text-muted">
              The name of the board must be unique
            </Form.Text>
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={hide}>
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
