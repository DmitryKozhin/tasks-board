import React, { useState } from 'react';
import Column from './Column';
import { connect } from 'react-redux';
import { Button, CardGroup } from 'react-bootstrap';
import AddColumnModal from './AddColumnModal';
import {
  UPDATE_BOARD,
  REMOVE_COLUMN,
  UPDATE_COLUMN,
  REMOVE_TASK,
} from '../../constants/actionTypes';
import agent from '../../agent';
import { DragDropContext } from 'react-beautiful-dnd';
import { useCallback } from 'react';

const mapStateToProps = (state) => ({
  board: state.boards.selectedBoard,
  columns: state.columns.columns || [],
});

const mapDispatchToProps = (dispatch) => ({
  onCreateColumn: async (header, color, boardId) => {
    if (!header) {
      return;
    }

    let columnEnvelope = await agent.Column.create(header, color, boardId);
    let payload = agent.Board.edit(boardId, {
      addedColumns: [columnEnvelope.column.id],
    });

    dispatch({ type: UPDATE_BOARD, payload });
  },

  onRemoveColumn: (id) => {
    const payload = agent.Column.delete(id);
    dispatch({
      type: REMOVE_COLUMN,
      payload: { ...payload, columnId: id },
    });
  },

  onChangeTaskColumn: async (updateTaskData) => {
    dispatch({
      type: REMOVE_TASK,
      payload: {
        taskId: updateTaskData.task.id,
        columnId: updateTaskData.oldColumn,
      },
    });

    await agent.Task.edit(updateTaskData.task.id, {
      columnId: updateTaskData.newColumn,
      orderNum: updateTaskData.task.orderNum,
    });

    let oldColumnPayload = agent.Column.get(updateTaskData.oldColumn);
    dispatch({
      type: UPDATE_COLUMN,
      payload: oldColumnPayload,
    });

    let newColumnPayload = agent.Column.get(updateTaskData.newColumn);
    dispatch({
      type: UPDATE_COLUMN,
      payload: newColumnPayload,
    });
  },

  onChangeTaskOrder: async (updateTaskData) => {
    await agent.Task.edit(updateTaskData.task.id, {
      orderNum: updateTaskData.task.orderNum,
    });

    let payload = agent.Column.get(updateTaskData.column);
    return dispatch({
      type: UPDATE_COLUMN,
      payload,
    });
  },
});

const Board = (props) => {
  const [isShowing, setShow] = useState(false);

  const showModal = () => setShow(true);
  const closeModal = () => setShow(false);
  const createColumn = useCallback(
    (header, color) => {
      props.onCreateColumn(header, color, props.board.id);
      setShow(false);
    },
    [props, setShow]
  );

  const onDragEnd = (result, columns, changeSource, changeOrder) => {
    if (!result.destination) return;
    const { source, destination } = result;

    if (source.droppableId !== destination.droppableId) {
      const sourceColumn = columns.find(
        (column) => column.id === source.droppableId
      );
      const destColumn = columns.find(
        (column) => column.id === destination.droppableId
      );
      const sourceTasks = [...sourceColumn.tasks];
      const [removed] = sourceTasks.splice(source.index, 1);
      changeSource({
        task: { ...removed, orderNum: destination.index },
        oldColumn: sourceColumn.id,
        newColumn: destColumn.id,
      });
    } else {
      const column = columns.find((column) => column.id === source.droppableId);
      const sourceTasks = [...column.tasks];
      const [changed] = sourceTasks.splice(source.index, 1);
      changeOrder({
        task: { ...changed, orderNum: destination.index },
        column: column.id,
      });
    }
  };

  return (
    <div>
      <Button
        className="board__add-column"
        variant="primary"
        onClick={showModal}
        size="sm"
      >
        Add column
      </Button>

      <CardGroup className="board__columns-container ">
        <DragDropContext
          onDragEnd={(result) =>
            onDragEnd(
              result,
              props.columns,
              props.onChangeTaskColumn,
              props.onChangeTaskOrder
            )
          }
        >
          {props.columns.length > 0 ? (
            props.columns.map((column) => (
              <Column
                column={column}
                key={column.id}
                onRemoveColumn={props.onRemoveColumn}
              />
            ))
          ) : (
            <span>Current board doesn't have any columns!</span>
          )}
        </DragDropContext>
      </CardGroup>

      <AddColumnModal
        isShowing={isShowing}
        onHide={closeModal}
        onCreate={createColumn}
      />
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Board);
