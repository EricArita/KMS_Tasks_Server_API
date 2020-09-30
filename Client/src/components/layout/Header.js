import React, { useState } from 'react';
import { FaAffiliatetheme, FaBell } from 'react-icons/fa';
import PropTypes from 'prop-types';
import { AddTask } from '../AddTask';

export const Header = ({ darkMode, setDarkMode }) => {
  const [shouldShowMain, setShouldShowMain] = useState(false);
  const [showQuickAddTask, setShowQuickAddTask] = useState(false);

  return (
    <header className="header" data-testid="header">
      <nav>
        <div className="search">
          <img src="/images/kms-logo-white.svg" alt="KMS Todoist" />
          <div class="search-container">
            <input class="search-box" type="search" placeholder="Search" />
          </div>  
        </div>
        <div className="settings">
          <ul>
            <li className="settings__add">
              <button
                data-testid="quick-add-task-action"
                aria-label="Quick add task"
                type="button"
                onClick={() => {
                  setShowQuickAddTask(true);
                  setShouldShowMain(true);
                }}
              >
                +
              </button>
            </li>
            <li className="settings__darkmode">
              <button
                data-testid="dark-mode-action"
                aria-label="Darkmode on/off"
                type="button"
                onClick={() => setDarkMode(!darkMode)}
              >
                <FaAffiliatetheme />
              </button>
            </li>
            <li className="settings__notification">
              <button
                // data-testid="dark-mode-action"
                // aria-label="Darkmode on/off"
                // type="button"
                // onClick={() => setDarkMode(!darkMode)}
              >
                <FaBell />
              </button>
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
    </header>
  );
};

Header.propTypes = {
  darkMode: PropTypes.bool.isRequired,
  setDarkMode: PropTypes.func.isRequired,
};
