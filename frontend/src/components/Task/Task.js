import React, { useCallback } from 'react';
import { Toast, Button, OverlayTrigger, Tooltip } from 'react-bootstrap';
import { FaPen } from 'react-icons/fa';
import { useState } from 'react';
import { UPDATE_TASK } from '../../constants/actionTypes';
import { connect } from 'react-redux';
import agent from '../../agent';
import AddTaskModal from './AddTaskModal';

const mapStateToProps = (state) => ({});

const mapDispatchToProps = (dispatch) => ({
  onEditTask: (id, header, description) => {
    const payload = agent.Task.edit(id, { header, description });
    dispatch({
      type: UPDATE_TASK,
      payload,
    });
  },
});

const Task = (props) => {
  const [isModalShowing, setModalShow] = useState(false);
  const [isEditVisible, setEditVisible] = useState(false);

  const closeClick = useCallback(() => {
    props.onRemove(props.task.id);
  }, [props]);

  const showModal = () => {
    setEditVisible(false);
    return setModalShow(true);
  };

  const updateTask = useCallback(
    (header, description) => {
      if (
        header !== props.task.header ||
        description !== props.task.description
      ) {
        props.onEditTask(props.task.id, header, description);
      }
      setModalShow(false);
    },
    [props, setModalShow]
  );

  return (
    <Toast
      className="task"
      style={{ borderColor: props.color }}
      onClose={closeClick}
      onMouseEnter={() => setEditVisible(true)}
      onMouseLeave={() => setEditVisible(false)}
    >
      <Toast.Header className="task__header">
        <strong className="mr-auto">{props.task.header}</strong>
        {isEditVisible ? (
          <OverlayTrigger overlay={<Tooltip>Edit a task</Tooltip>}>
            <Button variant="link" size="sm" onClick={showModal}>
              <FaPen />
            </Button>
          </OverlayTrigger>
        ) : null}
      </Toast.Header>
      <Toast.Body>{props.task.description}</Toast.Body>

      <AddTaskModal
        isShowing={isModalShowing}
        task={props.task}
        onHide={() => setModalShow(false)}
        onSave={updateTask}
      />
    </Toast>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Task);
