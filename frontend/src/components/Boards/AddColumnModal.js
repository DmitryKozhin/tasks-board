import React, { useCallback, useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { SketchPicker } from 'react-color';

const defaultColor = '#3486eb';

const AddColumnModal = (props) => {
  const [header, setHeader] = useState('');
  const [color, setColor] = useState(defaultColor);

  const clearState = useCallback(() => {
    setHeader('');
    setColor(defaultColor);
  }, [setHeader, setColor]);

  const changeHeader = useCallback((ev) => {
    setHeader(ev.target.value);
  }, []);

  const changeColor = useCallback((color) => {
    setColor(color);
  }, []);

  const handleSubmit = useCallback(() => {
    props.onCreate(header, color.hex || defaultColor);
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
              onChange={changeHeader}
              value={header}
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Color</Form.Label>
            <SketchPicker color={color} onChangeComplete={changeColor} />
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
