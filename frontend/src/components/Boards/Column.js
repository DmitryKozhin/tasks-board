import React, { useState } from 'react';
import { Card, Button, Tooltip, OverlayTrigger } from 'react-bootstrap';
import Task from './Task';
import AddTaskModal from './AddTaskModal';
import { connect } from 'react-redux';
import { UPDATE_COLUMN, REMOVE_TASK } from '../../constants/actionTypes';
import agent from '../../agent';
import { FaTimes } from 'react-icons/fa';

const mapStateToProps = (state) => ({});

const mapDispatchToProps = (dispatch) => ({
  onCreateTask: async (header, description, columnId) => {
    if (!header) {
      return;
    }

    let taskEnvelope = await agent.Task.create(header, description, columnId);
    let payload = agent.Column.edit(columnId, {
      addedTasks: [taskEnvelope.task.id],
    });

    dispatch({ type: UPDATE_COLUMN, payload });
  },

  onRemoveTask: (id, columnId) => {
    const payload = agent.Task.delete(id);
    dispatch({
      type: REMOVE_TASK,
      payload: { ...payload, taskId: id, columnId },
    });
  },
});

const Column = (props) => {
  const [isShowing, setShow] = useState(false);

  const showModal = () => setShow(true);
  const closeModal = () => setShow(false);
  const createTask = (header, description) => {
    props.onCreateTask(header, description, props.column.id);
    setShow(false);
  };

  const removeClick = () => {
    props.onRemoveColumn(props.column.id);
  };

  const removeTask = (id) => {
    props.onRemoveTask(id, props.column.id);
  };

  return (
    <Card className="column">
      <Card.Header as="h5">
        <div className="column__header">
          <div
            className="column__header-name"
            style={{ color: props.column.color }}
          >
            {props.column.header}
          </div>
          <OverlayTrigger overlay={<Tooltip>Remove a column</Tooltip>}>
            <Button variant="link" onClick={removeClick}>
              <FaTimes />
            </Button>
          </OverlayTrigger>
        </div>
      </Card.Header>
      <Card.Body className="overflow-auto">
        {props.column.tasks?.map((task) => (
          <Task
            task={task}
            key={task.id}
            color={props.column.color}
            onRemove={removeTask}
          />
        ))}
      </Card.Body>
      <AddTaskModal
        isShowing={isShowing}
        onHide={closeModal}
        onCreate={createTask}
      />
      <Button
        variant="outline-primary column__add-task"
        size="sm"
        onClick={showModal}
      >
        +
      </Button>
    </Card>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Column);
