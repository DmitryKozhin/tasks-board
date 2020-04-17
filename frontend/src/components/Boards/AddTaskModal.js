import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { useEffect } from 'react';

const AddTaskModal = (props) => {
  const [header, setHeader] = useState('');
  const [description, setDescription] = useState('');

  useEffect(() => {
    setHeader(props.task?.header);
    setDescription(props.task?.description);
  }, [props, setHeader, setDescription]);

  const clearState = useCallback(() => {
    setHeader('');
    setDescription('');
  }, [setHeader, setDescription]);

  const changeHeader = useCallback((ev) => {
    setHeader(ev.target.value);
  }, []);

  const changeDescription = useCallback((ev) => {
    setDescription(ev.target.value);
  }, []);

  const saveAndCloseModal = useCallback(() => {
    props.onSave(header, description);
    clearState();
  }, [props, header, description, clearState]);

  const onHide = useCallback(() => {
    props.onHide();
    clearState();
  }, [props, clearState]);

  return (
    <Modal show={props.isShowing} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>{`${props.task ? 'Edit' : 'Add'} task`}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Header</Form.Label>
            <Form.Control type="input" onChange={changeHeader} value={header} />
          </Form.Group>
          <Form.Group>
            <Form.Label>Description</Form.Label>
            <Form.Control
              as="textarea"
              style={{
                height: props.task?.description ? '15rem' : null,
              }}
              onChange={changeDescription}
              value={description}
            />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
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
