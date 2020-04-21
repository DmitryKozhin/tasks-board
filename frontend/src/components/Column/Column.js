import React, { useState } from 'react';
import { Card, Button, Tooltip, OverlayTrigger } from 'react-bootstrap';
import Task from '../Task/Task';
import AddTaskModal from '../Task/AddTaskModal';
import { connect } from 'react-redux';
import {
  REMOVE_TASK,
  CREATE_TASK,
  UPDATE_COLUMN,
} from '../../constants/actionTypes';
import agent from '../../agent';
import { FaTimes, FaPen } from 'react-icons/fa';
import { Droppable, Draggable } from 'react-beautiful-dnd';
import { useCallback } from 'react';
import AddColumnModal from './AddColumnModal';

const mapStateToProps = (state) => ({});

const mapDispatchToProps = (dispatch) => ({
  onEditColumn: (id, header, color) => {
    const payload = agent.Column.edit(id, { header, color });
    dispatch({
      type: UPDATE_COLUMN,
      payload,
    });
  },

  onCreateTask: (header, description, columnId) => {
    if (!header) {
      return;
    }

    let payload = agent.Task.create(header, description, columnId);
    dispatch({
      type: CREATE_TASK,
      payload,
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
  const [isTaskModalShowing, setTaskModalShow] = useState(false);
  const [isColumnModalShowing, setColumnModalShow] = useState(false);
  const [isEditVisible, setEditVisible] = useState(false);

  const createTask = useCallback(
    (header, description) => {
      props.onCreateTask(header, description, props.column.id);
      setTaskModalShow(false);
    },
    [props, setTaskModalShow]
  );

  const updateColumn = useCallback(
    (header, color) => {
      props.onEditColumn(props.column.id, header, color);
      setColumnModalShow(false);
    },
    [props, setColumnModalShow]
  );

  const showModal = () => {
    setEditVisible(false);
    return setColumnModalShow(true);
  };

  return (
    <Card className="column">
      <Card.Header
        as="h5"
        onMouseEnter={() => setEditVisible(true)}
        onMouseLeave={() => setEditVisible(false)}
      >
        <div className="column__header">
          <span
            className="column__header-name"
            style={{ color: props.column.color }}
          >
            {props.column.header}
          </span>
          <div>
            {isEditVisible ? (
              <OverlayTrigger overlay={<Tooltip>Edit a column</Tooltip>}>
                <Button variant="link" size="sm" onClick={showModal}>
                  <FaPen />
                </Button>
              </OverlayTrigger>
            ) : null}
            <OverlayTrigger overlay={<Tooltip>Remove a column</Tooltip>}>
              <Button
                variant="link"
                onClick={() => props.onRemoveColumn(props.column.id)}
              >
                <FaTimes />
              </Button>
            </OverlayTrigger>
          </div>
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
                            onRemove={() =>
                              props.onRemoveTask(task.id, props.column.id)
                            }
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

      <AddColumnModal
        isShowing={isColumnModalShowing}
        column={props.column}
        onHide={() => setColumnModalShow(false)}
        onSave={updateColumn}
      />

      <AddTaskModal
        isShowing={isTaskModalShowing}
        onHide={() => setTaskModalShow(false)}
        onSave={createTask}
      />

      <Button
        variant="link column__add-task"
        size="sm"
        onClick={() => setTaskModalShow(true)}
      >
        Add new task...
      </Button>
    </Card>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Column);
