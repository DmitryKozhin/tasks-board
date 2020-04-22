import React, { useCallback, useState, useEffect } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { SketchPicker } from 'react-color';

const defaultColor = '#3486eb';

const AddColumnModal = ({ column, isShowing, onSave, onHide }) => {
  const [header, setHeader] = useState('');
  const [color, setColor] = useState(defaultColor);

  useEffect(() => {
    setHeader(column?.header || '');
    setColor(column?.color || defaultColor);
  }, [column, setHeader, setColor]);

  const clearState = useCallback(() => {
    setHeader('');
    setColor(defaultColor);
  }, [setHeader, setColor]);

  const saveAndCloseModal = useCallback(() => {
    onSave(header, color.hex || column?.color || defaultColor);
    clearState();
  }, [header, color, onSave, clearState]);

  const hide = useCallback(() => {
    onHide();
    clearState();
  }, [onHide, clearState]);

  return (
    <Modal show={isShowing} onHide={hide}>
      <Modal.Header closeButton>
        <Modal.Title>{`${column ? 'Edit' : 'Add'} column`}</Modal.Title>
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

export default AddColumnModal;
