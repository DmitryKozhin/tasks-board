import React, { useCallback, useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const AddBoardModal = (props) => {
  const [name, setName] = useState('');

  useEffect(() => {
    setName(props.board?.name || '');
  }, [props, setName]);

  const saveAndCloseModal = useCallback(() => {
    props.onSave(name);
    setName('');
  }, [setName, props, name]);

  const onHide = useCallback(() => {
    props.onHide();
    setName('');
  }, [setName, props]);

  return (
    <Modal show={props.isShowing} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>{`${props.board ? 'Edit' : 'Add'} board`}</Modal.Title>
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

export default AddBoardModal;
