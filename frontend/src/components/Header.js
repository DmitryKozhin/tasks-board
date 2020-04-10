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

class Header extends React.Component {
  render() {
    return (
      <nav className="navbar navbar-light">
        <div className="container">
          <Link to="/" className="navbar-brand">
            {this.props.appName}
          </Link>

          <LoggedInView currentUser={this.props.currentUser} />
          <LoggedOutView currentUser={this.props.currentUser} />
        </div>
      </nav>
    );
  }
}

export default Header;
