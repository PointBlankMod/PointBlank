using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Services;
using PointBlank.API.IPC;

namespace PointBlank.Services.IPCManager
{
    [Service("IPCManager", true)]
    internal class IPCManager : Service
    {
        #region Info
        public static readonly string FileLocation = Directory.GetCurrentDirectory() + "/IPC.ipc";
        #endregion

        #region Variables
        private bool _Update = false;
        private bool _Running = true;
        private Thread _FileUpdaterThread;
        #endregion

        #region Properties
        public static Dictionary<string, string> IPC { get; } = new Dictionary<string, string>();
        #endregion

        #region Override Functions
        public override void Load()
        {
            if (File.Exists(FileLocation))
                File.Delete(FileLocation);

            // Setup the thread
            _FileUpdaterThread = new Thread(new ThreadStart(UpdateFile));
            _FileUpdaterThread.Start();

            // Setup the events
            IPCEvents.OnKeyValueChanged += new IPCEvents.KeyValueChangedHandler(OnKeySet);
            IPCEvents.OnKeyRemoved += new IPCEvents.KeyListChangedHandler(OnKeyUpdated);
            IPCEvents.OnKeyAdded += new IPCEvents.KeyListChangedHandler(OnKeyUpdated);
        }

        public override void Unload()
        {
            _Running = false;
            _FileUpdaterThread.Abort();
            File.Delete(FileLocation);
        }
        #endregion

        #region Private Functions
        private void UpdateFile()
        {
            while(_Running)
            {
                if (!_Update)
                    continue;
                List<string> contents = new List<string>();

                foreach (string key in IPC.Keys)
                    contents.Add(key + ":" + IPC[key]);

                try
                {
                    File.WriteAllLines(FileLocation, contents.ToArray());
                    _Update = false;
                }
#pragma warning disable CS0168 // Variable is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    Thread.Sleep(1);
                }
            }
        }
        #endregion

        #region Event Functions
        private void OnKeySet(string key, string value) => _Update = true;

        private void OnKeyUpdated(string key) => _Update = true;
        #endregion
    }
}
