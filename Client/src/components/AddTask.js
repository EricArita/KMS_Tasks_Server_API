import React, { useState } from 'react';
import { FaRegListAlt } from 'react-icons/fa';
import { FcPlanner, FcLandscape, FcGallery, FcRadarPlot, FcRating } from 'react-icons/fc';
import { BsFlag, BsAlarm } from 'react-icons/bs';
import { AiOutlineTag } from 'react-icons/ai';
import moment from 'moment';
import PropTypes from 'prop-types';
import { useSelectedProjectValue } from '../context';
import { ProjectOverlay } from './ProjectOverlay';
import { TaskDate } from './TaskDate';
import DropdownButton from 'react-bootstrap/DropdownButton';
import Dropdown from 'react-bootstrap/Dropdown';
import InfiniteCalendar from 'react-infinite-calendar';
import 'react-infinite-calendar/styles.css';

export const AddTask = ({
  showAddTaskMain = true,
  shouldShowMain = false,
  showQuickAddTask,
  setShowQuickAddTask,
}) => {
  const [task, setTask] = useState('');
  const [taskDate, setTaskDate] = useState('');
  const [project, setProject] = useState('');
  const [showMain, setShowMain] = useState(shouldShowMain);
  const [showProjectOverlay, setShowProjectOverlay] = useState(false);
  const [showTaskDate, setShowTaskDate] = useState(false);

  const { selectedProject } = useSelectedProjectValue();

  const addNewTask = () => {
    //call API to .net core backend
    const projectId = project || selectedProject;
    let collatedDate = '';

    if (projectId === 'TODAY') {
      collatedDate = moment().format('DD/MM/YYYY');
    } else if (projectId === 'NEXT_7') {
      collatedDate = moment().add(7, 'days').format('DD/MM/YYYY');
    }

    return (
      task &&
      projectId //&&
      // firebase
      //   .firestore()
      //   .collection('tasks')
      //   .add({
      //     archived: false,
      //     projectId,
      //     task,
      //     date: collatedDate || taskDate,
      //     userId: 'jlIFXIwyAL3tzHMtzRbw',
      //   })
      //   .then(() => {
      //     setTask('');
      //     setProject('');
      //     setShowMain('');
      //     setShowProjectOverlay(false);
      //   })
    );
  };

  return (
    <div
      className={showQuickAddTask ? 'add-task add-task__overlay' : 'add-task'}
      data-testid="add-task-comp"
    >
      {showAddTaskMain && (
        <div
          className="add-task__shallow"
          data-testid="show-main-action"
          onClick={() => setShowMain(!showMain)}
          onKeyDown={(e) => {
            if (e.key === 'Enter') setShowMain(!showMain);
          }}
          tabIndex={0}
          aria-label="Add task"
          role="button"
        >
          <span className="add-task__plus">+</span>
          <span className="add-task__text">Add Task</span>
        </div>
      )}

      {(showMain || showQuickAddTask) && (
        <div className="add-task__main" data-testid="add-task-main">
          {showQuickAddTask && (
            <>
              <div data-testid="quick-add-task">
                <h6 className="header">Quick Add Task</h6>
                <span
                  className="add-task__cancel-x"
                  data-testid="add-task-quick-cancel"
                  aria-label="Cancel adding task"
                  onClick={() => {
                    setShowMain(false);
                    setShowProjectOverlay(false);
                    setShowQuickAddTask(false);
                  }}
                  tabIndex={0}
                  role="button"
                >
                  X
                </span>
              </div>
            </>
          )}
          <ProjectOverlay
            setProject={setProject}
            showProjectOverlay={showProjectOverlay}
            setShowProjectOverlay={setShowProjectOverlay}
          />
          <TaskDate
            setTaskDate={setTaskDate}
            showTaskDate={showTaskDate}
            setShowTaskDate={setShowTaskDate}
          />
          <input
            className="add-task__content"
            placeholder="ex: Fix bug..."
            aria-label="Enter your task"
            data-testid="add-task-content"
            type="text"
            value={task}
            onChange={(e) => setTask(e.target.value)}
          />
          <button
            type="button"
            className="add-task__submit"
            data-testid="add-task"
            onClick={() =>
              showQuickAddTask
                ? addNewTask() && setShowQuickAddTask(false)
                : addNewTask()
            }
          >
            Submit
          </button>
          {!showQuickAddTask && (
            <span
              className="add-task__cancel"
              data-testid="add-task-main-cancel"
              onClick={() => {
                setShowMain(false);
                setShowProjectOverlay(false);
              }}
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  setShowMain(false);
                  setShowProjectOverlay(false);
                }
              }}
              aria-label="Cancel adding a task"
              tabIndex={0}
              role="button"
            >
              Cancel
            </span>
          )}
          <span
            className="add-task__project"
            data-testid="show-project-overlay"
            // onClick={() => setShowProjectOverlay(!showProjectOverlay)}
            // onKeyDown={(e) => {
            //   if (e.key === 'Enter') setShowProjectOverlay(!showProjectOverlay);
            // }}
            tabIndex={0}
            role="button"
          >
            <Dropdown>
              <Dropdown.Toggle id="dropdown-project-toggle">
                <FaRegListAlt size={22} />
              </Dropdown.Toggle>
              <Dropdown.Menu id="dropdown-project-menu">
                <Dropdown.Item eventKey="Emergency" active>
                  <span class="priority-item-text">Project A</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="High">
                  <span class="priority-item-text">Project B</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="Medium">
                  <span class="priority-item-text">Project C</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="Low">
                  <span class="priority-item-text">Project D</span>
                </Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </span>
          <span className="add-task__alarm">
            <BsAlarm size={20} />
          </span>
          <span className="add-task__priority">
            <Dropdown>
              <Dropdown.Toggle id="dropdown-priority-level-toggle">
                <BsFlag size={22} />
              </Dropdown.Toggle>
              <Dropdown.Menu>
                <Dropdown.Item eventKey="Emergency">
                  <BsFlag color="red" size={22} />
                  <span class="priority-item-text">Emergency</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="High">
                  <BsFlag color="#EFEC56" size={22} />
                  <span class="priority-item-text">High</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="Medium">
                  <BsFlag color="green" size={22} />
                  <span class="priority-item-text">Medium</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="Low">
                  <BsFlag color="grey" size={22} />
                  <span class="priority-item-text">Low</span>
                </Dropdown.Item>
                <Dropdown.Item eventKey="Anytime">
                  <BsFlag color="blue" size={22} />
                  <span class="priority-item-text">Anytime</span>
                </Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          </span>
          <span className="add-task__tag">
            <AiOutlineTag size={22} />
          </span>
          <DropdownButton
            alignRight
            title={taskDate === '' ? 'Today' : taskDate}
            id="dropdown-menu-align-right"
            className="add-task__date"
            data-testid="show-task-date-overlay"
            onSelect={(val) => {
              if (val !== 'calendar') setTaskDate(val.trim());
            }}
          >
            <Dropdown.Item>
              <span> Selected date</span>
            </Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item eventKey="Today">
              <FcPlanner size={21} />
              <span className="calendar-option-text"> Today</span>
            </Dropdown.Item>
            <Dropdown.Item eventKey="Tomorrow">
              <FcLandscape size={21} />
              <span className="calendar-option-text"> Tomorrow</span>
            </Dropdown.Item>
            <Dropdown.Item eventKey="Weekend">
              <FcGallery size={21} />
              <span className="calendar-option-text"> Weekend </span>
            </Dropdown.Item>
            <Dropdown.Item eventKey="This week">
              <FcRadarPlot size={21} />
              <span className="calendar-option-text"> This week</span>
            </Dropdown.Item>
            <Dropdown.Item eventKey="Next week">
              <FcRating size={21} />
              <span className="calendar-option-text"> Next week</span>
            </Dropdown.Item>
            <Dropdown.Divider />
            <Dropdown.Item eventKey="calendar">
              <InfiniteCalendar
                rowHeight={45}
                width={280}
                height={170}
                selected={new Date()}
                min={new Date()}
                onSelect={(date) => {
                  var convertedDate = `${date.toLocaleString('default', { month: 'short' })}  ${date.getDate()} ${date.getFullYear()}`;
                  setTaskDate(convertedDate);
                }}
              />
            </Dropdown.Item>
          </DropdownButton>
        </div>
      )}
    </div>
  );
};

AddTask.propTypes = {
  showAddTaskMain: PropTypes.bool,
  shouldShowMain: PropTypes.bool,
  showQuickAddTask: PropTypes.bool,
  setShowQuickAddTask: PropTypes.func,
};
