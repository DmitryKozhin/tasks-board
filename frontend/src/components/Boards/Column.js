import React, { useState, useCallback } from 'react';
import { connect } from 'react-redux';
import { Card, Button } from 'react-bootstrap';
import Task from './Task';
import AddTaskModal from './AddTaskModal';
import { UPDATE_COLUMN, REMOVE_TASK } from '../../constants/actionTypes';
import agent from '../../agent';

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

  const removeTask = (id) => {
    props.onRemoveTask(id, props.column.id);
  };

  return (
    <Card className="column" style={{ borderColor: '' }}>
      <Card.Header as="h5">{props.column.header}</Card.Header>
      <Card.Body className="overflow-auto">
        {props.column.tasks?.map((task) => (
          <Task task={task} key={task.id} onRemove={removeTask} />
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
