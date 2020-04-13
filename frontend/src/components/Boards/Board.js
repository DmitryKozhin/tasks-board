import React from 'react';
import Column from './Column';
import { connect } from 'react-redux';
import { Button, ListGroup, CardGroup } from 'react-bootstrap';
import AddColumnModal from './AddColumnModal';
import { SHOW_ADD_COLUMN, HIDE_ADD_COLUMN } from '../../constants/actionTypes';

const mapStateToProps = (state) => ({
  isShowing: state.columns.isShowing,
});

const mapDispatchToProps = (dispatch) => ({
  onShowModal: () => dispatch({ type: SHOW_ADD_COLUMN }),
  onCloseModal: () => dispatch({ type: HIDE_ADD_COLUMN }),
});

const Board = (props) => {
  const showModal = (ev) => props.onShowModal();
  const closeModal = (ev) => props.onCloseModal();

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

        {props.isShowing ? (
          <AddColumnModal onShowModal={showModal} onHide={closeModal} />
        ) : (
          ''
        )}
      </CardGroup>
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Board);
