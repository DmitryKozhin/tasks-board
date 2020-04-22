import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { useEffect } from 'react';

const AddTaskModal = ({ task, isShowing, onSave, onHide }) => {
  const [header, setHeader] = useState('');
  const [description, setDescription] = useState('');

  useEffect(() => {
    setHeader(task?.header || '');
    setDescription(task?.description || '');
  }, [task, setHeader, setDescription]);

  const clearState = useCallback(() => {
    setHeader('');
    setDescription('');
  }, [setHeader, setDescription]);

  const saveAndCloseModal = useCallback(() => {
    onSave(header, description);
    clearState();
  }, [header, description, onSave, clearState]);

  const hide = useCallback(() => {
    onHide();
    clearState();
  }, [onHide, clearState]);

  return (
    <Modal show={isShowing} onHide={hide}>
      <Modal.Header closeButton>
        <Modal.Title>{`${task ? 'Edit' : 'Add'} task`}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Header</Form.Label>
            <Form.Control
              type="input"
              onChange={(ev) => setHeader(ev.target.value)}
              value={header}
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Description</Form.Label>
            <Form.Control
              as="textarea"
              style={{
                height: task?.description ? '15rem' : null,
              }}
              onChange={(ev) => setDescription(ev.target.value)}
              value={description}
            />
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

export default AddTaskModal;
