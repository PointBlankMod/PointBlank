using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Services;
using PointBlank.API.Tasks;
using PointBlank.API.Plugins;

namespace PointBlank.Services.TaskManager
{
    internal class TaskManager : PointBlankService
    {
        #region Variables
        private Thread _tTasker;
        private Queue<PointBlankTask> _Remove = new Queue<PointBlankTask>();

        private bool _Running = false;
        #endregion

        #region Properties
        public static List<PointBlankTask> Tasks { get; private set; }

        public override int LaunchIndex => 2;
        #endregion

        public override void Load()
        {
            // Set the variables
            Tasks = new List<PointBlankTask>();
            _tTasker = new Thread(new ThreadStart(Tasker));
            _Running = true;

            // Run the code
            _tTasker.Start();
        }

        public override void Unload()
        {
            // Set the variables
            _Running = false;
        }

        #region Threads
        private void Tasker()
        {
            while (_Running)
            {
                Thread.Sleep(1);

                if (Tasks.Count < 1)
                    continue;
                foreach (PointBlankTask task in Tasks)
                {
                    if (!task.Running)
                        continue;
                    if (!task.IsThreaded)
                        continue;
                    if (task.NextExecution == null)
                        continue;

                    if ((DateTime.Now - task.NextExecution).TotalMilliseconds >= 0)
                    {
                        task.Run();

                        if (!task.Loop)
                            _Remove.Enqueue(task);
                    }
                }
            }
        }
        #endregion

        #region Mono Functions
        void Update()
        {
            if (!_Running)
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
                        _Remove.Enqueue(task);
                }
            }

            lock (Tasks)
            {
                lock (_Remove)
                {
                    while (_Remove.Count > 0)
                        _Remove.Dequeue().Stop();
                }
            }
        }
        #endregion
    }
}
