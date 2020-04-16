import React, { useState } from 'react';
import { Card, Button, Tooltip, OverlayTrigger } from 'react-bootstrap';
import Task from './Task';
import AddTaskModal from './AddTaskModal';
import { connect } from 'react-redux';
import {
  UPDATE_COLUMN,
  REMOVE_TASK,
  CREATE_TASK,
} from '../../constants/actionTypes';
import agent from '../../agent';
import { FaTimes } from 'react-icons/fa';
import { Droppable, Draggable } from 'react-beautiful-dnd';

const mapStateToProps = (state) => ({});

const mapDispatchToProps = (dispatch) => ({
  onCreateTask: async (header, description, columnId) => {
    if (!header) {
      return;
    }

    let payload = await agent.Task.create(header, description, columnId);
    dispatch({
      type: CREATE_TASK,
      payload: { ...payload, columnId },
    });
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
      <Droppable droppableId={props.column.id} key={props.column.id}>
        {(provided, snapshot) => {
          return (
            <Card.Body
              className="overflow-auto"
              {...provided.droppableProps}
              ref={provided.innerRef}
            >
              {props.column.tasks?.map((task) => {
                return (
                  <Draggable
                    key={task.id}
                    draggableId={task.id}
                    index={task.orderNum}
                  >
                    {(provided, snapshot) => {
                      return (
                        <div
                          ref={provided.innerRef}
                          {...provided.draggableProps}
                          {...provided.dragHandleProps}
                          style={{
                            userSelect: 'none',
                            marginBottom: '5px',
                            ...provided.draggableProps.style,
                          }}
                        >
                          <Task
                            task={task}
                            key={task.id}
                            color={props.column.color}
                            onRemove={removeTask}
                          />
                        </div>
                      );
                    }}
                  </Draggable>
                );
              })}
              {provided.placeholder}
            </Card.Body>
          );
        }}
      </Droppable>

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
