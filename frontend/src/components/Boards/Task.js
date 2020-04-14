import React from 'react';
import { Toast } from 'react-bootstrap';

const Task = (props) => {
  return (
    <Toast style={{ maxWidth: 'none' }}>
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
