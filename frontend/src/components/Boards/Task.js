import React, { useCallback } from 'react';
import { Toast } from 'react-bootstrap';

const Task = (props) => {
  const closeClick = useCallback(() => {
    props.onRemove(props.task.id);
  }, [props]);

  return (
    <Toast
      style={{ maxWidth: 'none', borderColor: props.color }}
      onClose={closeClick}
    >
      <Toast.Header>
        <strong className="mr-auto">{props.task.header}</strong>
      </Toast.Header>
      <Toast.Body>{props.task.description}</Toast.Body>
    </Toast>
  );
};

export default Task;
