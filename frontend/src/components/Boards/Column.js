import React from 'react';
import { connect } from 'react-redux';
import { Card, Button } from 'react-bootstrap';

const mapStateToProps = (state) => ({});

const mapDispatchToProps = (dispatch) => ({});

const Column = (props) => {
  return (
    <Card className="column" style={{ borderColor: '' }}>
      <Card.Header as="h5">{props.column.header}</Card.Header>
      <Card.Body>
        <Card.Title>Special title treatment</Card.Title>
        <Card.Text>
          With supporting text below as a natural lead-in to additional content.
        </Card.Text>
      </Card.Body>
      <Button variant="outline-primary column__add-task" size="sm">
        +
      </Button>
    </Card>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(Column);
