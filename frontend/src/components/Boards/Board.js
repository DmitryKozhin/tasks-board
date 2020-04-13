import React, { useState } from 'react';
import Column from './Column';
import { connect } from 'react-redux';
import { Button, CardGroup } from 'react-bootstrap';
import AddColumnModal from './AddColumnModal';
import { CREATE_COLUMN } from '../../constants/actionTypes';
import agent from '../../agent';

const mapStateToProps = (state) => ({
  isShowing: state.columns.isShowing,
});

const mapDispatchToProps = (dispatch) => ({
  onCreateColumn: (header, color, boardId) => {
    if (!header) {
      return;
    }
    let payload = agent.Column.create(header, color, boardId);
    dispatch({ type: CREATE_COLUMN, payload });
  },
});

const Board = (props) => {
  const [isShowing, setShow] = useState(false);
  const showModal = () => setShow(true);
  const closeModal = () => setShow(false);
  const createColumn = (header, color) => {
    props.onCreateColumn(header, color, props.board.id);
    setShow(false);
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
        {props.board.columns.length !== 0 ? (
          props.board.columns.map((column) => (
            <Column column={column} key={column.id} />
          ))
        ) : (
          <span>Current board doesn't have any columns!</span>
        )}

        <AddColumnModal
          isShowing={isShowing}
          onHide={closeModal}
          onCreate={createColumn}
        />
      </CardGroup>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Board);
