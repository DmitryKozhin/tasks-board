import React, { useCallback, useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { SketchPicker } from 'react-color';

const defaultColor = '#3486eb';

const AddColumnModal = (props) => {
  const [header, setHeader] = useState('');
  const [color, setColor] = useState(defaultColor);

  useEffect(() => {
    setHeader(props.column?.header || '');
    setColor(props.column?.color || defaultColor);
  }, [props, setHeader, setColor]);

  const clearState = useCallback(() => {
    setHeader('');
    setColor(defaultColor);
  }, [setHeader, setColor]);

  const handleSubmit = useCallback(() => {
    props.onSave(header, color.hex || defaultColor);
    clearState();
  }, [props, header, color, clearState]);

  const onHide = useCallback(() => {
    props.onHide();
    clearState();
  }, [props, clearState]);

  return (
    <Modal show={props.isShowing} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Add column</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group>
            <Form.Label>Header</Form.Label>
            <Form.Control
              type="input"
              required
              onChange={(ev) => setHeader(ev.target.value)}
              value={header}
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Color</Form.Label>
            <SketchPicker
              color={color}
              onChangeComplete={(color) => setColor(color)}
            />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Close
        </Button>
        <Button variant="primary" onClick={handleSubmit}>
          Save
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default AddColumnModal;
