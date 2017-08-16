using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.Services.TaskManager;

namespace PointBlank.API.Tasks
{
    /// <summary>
    /// Used for tasking events and actions
    /// </summary>
    public class PointBlankTask // I won't lie I liked the one from uEssentials so I stole the idea
    {
        #region Properties
        /// <summary>
        /// Should the task be executed in a thread
        /// </summary>
        public bool IsThreaded { get; internal set; }
        /// <summary>
        /// Should the task be looped(milliseconds)
        /// </summary>
        public bool Loop { get; internal set; }
        /// <summary>
        /// The task delay
        /// </summary>
        public int Delay { get; internal set; }
        /// <summary>
        /// The code to be executed
        /// </summary>
        public Action<PointBlankTask> Action { get; internal set; }

        /// <summary>
        /// Is the task running
        /// </summary>
        public bool Running { get; private set; } = false;
        /// <summary>
        /// The next time the code will be executed
        /// </summary>
        public DateTime NextExecution { get; internal set; }
        #endregion

        private PointBlankTask()
        {
            // Set the default properties
            IsThreaded = false;
            Loop = false;
            Delay = 1000;
            Action = (a =>
            {
                // Do nothing
            });
        }

        #region Static Functions
        /// <summary>
        /// Creates the task and returns it's instance
        /// </summary>
        /// <returns>The instance of the task</returns>
        public static Builder Create()
        {
            PointBlankTask task = new PointBlankTask();

            return new Builder(task);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Starts the task
        /// </summary>
        public void Start()
        {
            // Set the variables
            Running = true;
            NextExecution = DateTime.Now.AddMilliseconds(Delay);
        }
        /// <summary>
        /// Stops the task
        /// </summary>
        public void Stop()
        {
            // Set the variables
            Running = false;

            // Run the code
            TaskManager.Tasks.Remove(this);
        }

        /// <summary>
        /// Executes the task's code once
        /// </summary>
        public void Run() => Action(this);
        #endregion

        #region Sub Classes
        /// <summary>
        /// Used for building the tasks
        /// </summary>
        public class Builder
        {
            #region Properties
            /// <summary>
            /// The task instance that's being built
            /// </summary>
            public PointBlankTask Task { get; private set; }
            #endregion

            internal Builder(PointBlankTask task)
            {
                // Set the variables
                Task = task;
            }

            #region Functions
            /// <summary>
            /// Toggle threading of the task
            /// </summary>
            /// <returns>The builder instance</returns>
            public Builder Threaded()
            {
                Task.IsThreaded = !Task.IsThreaded;
                return this;
            }

            /// <summary>
            /// Toggle looping of the task
            /// </summary>
            /// <returns>The builder instance</returns>
            public Builder Loop()
            {
                Task.Loop = !Task.Loop;
                return this;
            }

            /// <summary>
            /// Time until task code gets executed
            /// </summary>
            /// <param name="ms">The amount of time in milliseconds</param>
            /// <returns>The builder instance</returns>
            public Builder Delay(int ms)
            {
                Task.Delay = ms;
                return this;
            }
            /// <summary>
            /// Time until task code gets executed
            /// </summary>
            /// <param name="span">The amount of time with TimeSpan instance</param>
            /// <returns>The builder instance</returns>
            public Builder Delay(TimeSpan span)
            {
                Task.Delay = (int)span.TotalMilliseconds;
                return this;
            }

            /// <summary>
            /// Sets the task code that gets executed
            /// </summary>
            /// <param name="action">The code that gets executed</param>
            /// <returns>The builder instance</returns>
            public Builder Action(Action action)
            {
                Task.Action = (a => action());
                return this;
            }
            /// <summary>
            /// Sets the task code that gets executed
            /// </summary>
            /// <param name="action">The code that gets executed</param>
            /// <returns>The builder instance</returns>
            public Builder Action(Action<PointBlankTask> action)
            {
                Task.Action = action;
                return this;
            }

            /// <summary>
            /// Build the task and return it's instance
            /// </summary>
            /// <param name="AutoStart">Should the task be ran on build</param>
            /// <returns>The task instance</returns>
            public PointBlankTask Build(bool AutoStart = false)
            {
                TaskManager.Tasks.Add(Task);

                if (AutoStart)
                    Task.Start();
                return Task;
            }
            #endregion
        }
        #endregion
    }
}
