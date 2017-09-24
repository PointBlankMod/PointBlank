using System;
using System.Collections.Generic;
using PointBlank.API.Services;
using PointBlank.API.Tasks;
using PointBlank.API.Extension;

namespace PointBlank.Services.TaskManager
{
    internal class TaskManager : PointBlankService
    {
        #region Variables
<<<<<<< HEAD
        private Queue<PointBlankTask> _Remove = new Queue<PointBlankTask>();
=======
        private Queue<PointBlankTask> _remove = new Queue<PointBlankTask>();
>>>>>>> master

        private bool _running = false;
        #endregion

        #region Properties
        public static List<PointBlankTask> Tasks { get; private set; }

        public override int LaunchIndex => 2;
        #endregion

        public override void Load()
        {
            // Set the variables
            Tasks = new List<PointBlankTask>();
<<<<<<< HEAD
            _Running = true;

            // Set the events
            ExtensionEvents.OnAPITick += Tasker;
=======
            _running = true;

            // Set the events
            ExtensionEvents.OnApiTick += Tasker;
>>>>>>> master
        }

        public override void Unload()
        {
            // Set the variables
<<<<<<< HEAD
            _Running = false;

            // Remove the events
            ExtensionEvents.OnAPITick -= Tasker;
=======
            _running = false;

            // Remove the events
            ExtensionEvents.OnApiTick -= Tasker;
>>>>>>> master
        }

        #region Threads
        private void Tasker()
        {
            if (Tasks.Count < 1)
                return;
            foreach (PointBlankTask task in Tasks)
            {
                if (!task.Running)
<<<<<<< HEAD
                    continue;
                if (!task.IsThreaded)
                    continue;
=======
                    continue;
                if (!task.IsThreaded)
                    continue;
>>>>>>> master
                if (task.NextExecution == null)
                    continue;

                if ((DateTime.Now - task.NextExecution).TotalMilliseconds >= 0)
                {
                    task.Run();

                    if (!task.Loop)
<<<<<<< HEAD
                        _Remove.Enqueue(task);
=======
                        _remove.Enqueue(task);
>>>>>>> master
                }
            }
        }
        #endregion

        #region Mono Functions
        void Update()
        {
            if (!_running)
                return;

            if (Tasks.Count < 1)
                return;
            foreach (PointBlankTask task in Tasks)
            {
                if (!task.Running)
                    continue;
                if (task.IsThreaded)
                    continue;
                if (task.NextExecution == null)
                    continue;

                if ((DateTime.Now - task.NextExecution).TotalMilliseconds >= 0)
                {
                    task.Run();

                    if (!task.Loop)
                        _remove.Enqueue(task);
                }
            }

            lock (Tasks)
            {
                lock (_remove)
                {
                    while (_remove.Count > 0)
                        _remove.Dequeue().Stop();
                }
            }
        }
        #endregion
    }
}
