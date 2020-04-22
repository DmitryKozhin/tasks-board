import React from 'react';
import { Link } from 'react-router-dom';
import { DropdownButton, ButtonGroup } from 'react-bootstrap';
import { Dropdown } from 'react-bootstrap';

const LoggedOutView = ({ currentUser }) => {
  if (!currentUser) {
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

const LoggedInView = ({ currentUser, onLogout }) => {
  if (currentUser) {
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
            title={currentUser.email}
          >
            <Dropdown.Item>Settings</Dropdown.Item>
            <Dropdown.Item onClick={onLogout}>Logout</Dropdown.Item>
          </DropdownButton>
        </li>
      </ul>
    );
  }

  return null;
};

const Header = ({ appName, currentUser, onLogout }) => {
  return (
    <nav className="navbar navbar-light">
      <Link to="/" className="navbar-brand">
        {appName}
      </Link>

      <LoggedInView currentUser={currentUser} onLogout={onLogout} />
      <LoggedOutView currentUser={currentUser} />
    </nav>
  );
};

export default Header;
