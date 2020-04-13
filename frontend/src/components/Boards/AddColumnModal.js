import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

const AddColumnModal = (props) => {
  const [header, setHeader] = useState('');
  const [color, setColor] = useState('');
  // use callback после того, как поменять на FC
  // + bind action creators
  const changeHeader = useCallback((ev) => {
    setHeader(ev.target.value);
  }, []);
  const changeColor = useCallback((ev) => {
    setColor(ev.target.value);
  }, []);
  const saveAndCloseModal = useCallback(() => {
    props.onCreate(header, color);
  }, [props, header, color]);

  return (
    <Modal show={props.isShowing} onHide={props.onHide}>
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

export default AddColumnModal;
