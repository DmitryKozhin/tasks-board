import React from 'react';
import { Link } from 'react-router-dom';

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
          <Link to="/" className="nav-link">
            {props.currentUser.email}
          </Link>
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

      <LoggedInView currentUser={props.currentUser} />
      <LoggedOutView currentUser={props.currentUser} />
    </nav>
  );
};

export default Header;
