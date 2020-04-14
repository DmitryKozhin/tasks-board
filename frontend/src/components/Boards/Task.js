import React, { useCallback } from 'react';
import { Toast } from 'react-bootstrap';

const Task = (props) => {
  const closeClick = useCallback(() => {
    props.onRemove(props.task.id);
  }, [props]);

  return (
    <Toast style={{ maxWidth: 'none' }} onClose={closeClick}>
      <Toast.Header>
        <strong className="mr-auto">
          {props.task.orderNum}. {props.task.header}
        </strong>
        <small>11 mins ago</small>
      </Toast.Header>
      <Toast.Body>{props.task.description}</Toast.Body>
    </Toast>
  );
};

export default Task;
