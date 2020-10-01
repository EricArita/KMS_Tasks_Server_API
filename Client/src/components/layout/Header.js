import React, { useState } from 'react';
import { FaRegMoon, FaBell, FaPlus } from 'react-icons/fa';
import { CgProfile } from 'react-icons/cg';
import { FiSettings, FiLogOut } from 'react-icons/fi';
import DropdownButton from 'react-bootstrap/DropdownButton';
import Dropdown from 'react-bootstrap/Dropdown';
import PropTypes from 'prop-types';
import { AddTask } from '../AddTask';

export const Header = ({ darkMode, setDarkMode }) => {
  const [shouldShowMain, setShouldShowMain] = useState(false);
  const [showQuickAddTask, setShowQuickAddTask] = useState(false);

  return (
    <header className="header" data-testid="header">
      <nav>
        <div className="left-navbar">
          <img src="/images/kms-logo-white.svg" alt="KMS Todoist" />
          <div class="search-container">
            <input class="search-box" type="search" placeholder="Search" />
          </div>
        </div>
        <div className="right-navbar">
          <ul>
            <li className="right-navbar-add">
              <button
                data-testid="quick-add-task-action"
                aria-label="Quick add task"
                type="button"
                onClick={() => {
                  setShowQuickAddTask(true);
                  setShouldShowMain(true);
                }}
              >
                <FaPlus size={21} />
              </button>
            </li>
            <li className="right-navbar-darkmode">
              <button
                data-testid="dark-mode-action"
                aria-label="Darkmode on/off"
                type="button"
                onClick={() => setDarkMode(!darkMode)}
              >
                <FaRegMoon size={22} />
              </button>
            </li>
            <li className="right-navbar-notification">
              <button
                // data-testid="dark-mode-action"
                // aria-label="Darkmode on/off"
                type="button"
              // onClick={() => setDarkMode(!darkMode)}
              >
                <FaBell size={21} />
              </button>
            </li>
            <li className="right-navbar-dropdown">
              <DropdownButton
                alignRight
                id="dropdown-menu-align-right"
              >
                <Dropdown.Item>
                  <CgProfile />
                  <span> Profile</span>
                </Dropdown.Item>
                <Dropdown.Divider />
                <Dropdown.Item>
                  <FiSettings />
                  <span> Settings</span>
                  </Dropdown.Item>
                <Dropdown.Divider />
                <Dropdown.Item>
                  <FiLogOut />
                  <span> Log out</span>
                </Dropdown.Item>
              </DropdownButton>
            </li>
          </ul>
        </div>
      </nav>

      <AddTask
        showAddTaskMain={false}
        shouldShowMain={shouldShowMain}
        showQuickAddTask={showQuickAddTask}
        setShowQuickAddTask={setShowQuickAddTask}
      />
    </header >
  );
};

Header.propTypes = {
  darkMode: PropTypes.bool.isRequired,
  setDarkMode: PropTypes.func.isRequired,
};
