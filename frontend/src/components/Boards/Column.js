import React, { useState } from 'react';
import { connect } from 'react-redux';
import { Card, Button } from 'react-bootstrap';
import Task from './Task';
import AddTaskModal from './AddTaskModal';
import { UPDATE_COLUMN } from '../../constants/actionTypes';
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
});

const Column = (props) => {
  const [isShowing, setShow] = useState(false);

  const showModal = () => setShow(true);
  const closeModal = () => setShow(false);
  const createTask = (header, description) => {
    props.onCreateTask(header, description, props.column.id);
    setShow(false);
  };

  return (
    <Card className="column" style={{ borderColor: '' }}>
      <Card.Header as="h5">{props.column.header}</Card.Header>
      <Card.Body className="overflow-auto">
        {props.column.tasks?.map((task) => (
          <Task task={task} key={task.id} />
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
