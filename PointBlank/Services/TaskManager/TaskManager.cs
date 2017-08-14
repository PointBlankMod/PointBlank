using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Services;
using PointBlank.API.Tasks;
using PointBlank.API.Plugins;

namespace PointBlank.Services.TaskManager
{
    internal class TaskManager : PointBlankService
    {
        #region Variables
        private Thread _tTasker;

        private bool _Running;
        #endregion

        #region Properties
        public static Dictionary<Assembly, List<PointBlankTask>> Tasks { get; private set; }

        public override int LaunchIndex => 2;
        #endregion

        public override void Load()
        {
            // Set the variables
            Tasks = new Dictionary<Assembly, List<PointBlankTask>>();
            _tTasker = new Thread(new ThreadStart(Tasker));
            _Running = true;

            // Set the events
            PointBlankPluginEvents.OnPluginUnloaded += OnPluginUnloaded;

            // Run the code
            _tTasker.Start();
        }

        public override void Unload()
        {
            // Set the variables
            _Running = false;

            // Remove the events
            PointBlankPluginEvents.OnPluginUnloaded -= OnPluginUnloaded;
        }

        #region Threads
        private void Tasker()
        {
            while (_Running)
            {
                lock (Tasks)
                {
                    foreach (Assembly asm in Tasks.Keys)
                    {
                        Queue<PointBlankTask> remove = new Queue<PointBlankTask>();
                        foreach (PointBlankTask task in Tasks[asm])
                        {
                            if (!task.Running)
                                continue;
                            if (!task.IsThreaded)
                                continue;

                            if ((DateTime.Now - task.NextExecution).TotalMilliseconds >= 0)
                            {
                                task.Run();

                                if (!task.Loop)
                                    remove.Enqueue(task);
                            }
                        }

                        while (remove.Count > 0)
                            remove.Dequeue().Stop();
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

            lock (Tasks)
            {
                foreach (Assembly asm in Tasks.Keys)
                {
                    Queue<PointBlankTask> remove = new Queue<PointBlankTask>();
                    foreach (PointBlankTask task in Tasks[asm])
                    {
                        if (!task.Running)
                            continue;
                        if (task.IsThreaded)
                            continue;

                        if ((DateTime.Now - task.NextExecution).TotalMilliseconds >= 0)
                        {
                            task.Run();

                            if (!task.Loop)
                                remove.Enqueue(task);
                        }
                    }

                    while (remove.Count > 0)
                        remove.Dequeue().Stop();
                }
            }
        }
        #endregion

        #region Events
        private void OnPluginUnloaded(PointBlankPlugin plugin)
        {
            Assembly pluginAssembly = plugin.GetType().Assembly;

            if (Tasks.ContainsKey(pluginAssembly))
            {
                Tasks[pluginAssembly].ForEach((task) =>
                {
                    task.Stop();
                });
            }
        }
        #endregion
    }
}
