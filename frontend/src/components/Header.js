import React from 'react';
import { Link } from 'react-router-dom';
import { DropdownButton, ButtonGroup } from 'react-bootstrap';
import { Dropdown } from 'react-bootstrap';

const LoggedOutView = (props) => {
  if (!props.currentUser) {
    return (
      <ul className="nav">
        <li className="nav-item">
          <Link to="/login" className="nav-link">
            Sign in
          </Link>
        </li>

        <li className="nav-item">
          <Link to="/register" className="nav-link">
            Sign up
          </Link>
        </li>
      </ul>
    );
  }
  return null;
};

const LoggedInView = (props) => {
  if (props.currentUser) {
    return (
      <ul className="nav">
        <li className="nav-item">
          <Link to="/" className="nav-link">
            Boards
          </Link>
        </li>

        <li className="nav-item">
          <DropdownButton
            variant="link"
            as={ButtonGroup}
            title={props.currentUser.email}
          >
            <Dropdown.Item>Settings</Dropdown.Item>
            <Dropdown.Item onClick={props.onLogout}>Logout</Dropdown.Item>
          </DropdownButton>
          {/* <Link to="/" className="nav-link">
            
          </Link> */}
        </li>
      </ul>
    );
  }

  return null;
};

const Header = (props) => {
  return (
    <nav className="navbar navbar-light">
      <Link to="/" className="navbar-brand">
        {props.appName}
      </Link>

      <LoggedInView currentUser={props.currentUser} onLogout={props.onLogout} />
      <LoggedOutView currentUser={props.currentUser} />
    </nav>
  );
};

export default Header;
