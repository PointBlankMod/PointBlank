using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using PointBlank.API;
using PointBlank.API.Services;
using PointBlank.API.IPC;
using PointBlank.API.Extension;

namespace PointBlank.Services.IPCManager
{
    internal class IpcManager : PointBlankService
    {
        #region Info
        public static readonly string FileLocation = Directory.GetCurrentDirectory() + "/IPC.ipc";
        #endregion

        #region Variables
<<<<<<< HEAD
        private bool _Update = false;
=======
        private bool _update = false;
>>>>>>> master
        #endregion

        #region Properties
        public static Dictionary<string, string> Ipc { get; } = new Dictionary<string, string>();

        public override int LaunchIndex => 1;
        #endregion

        public override void Load()
        {
            if (File.Exists(FileLocation))
                File.Delete(FileLocation);

            // Setup the thread
            ExtensionEvents.OnFrameworkTick += UpdateFile;

            // Setup the events
            IpcEvents.OnKeyValueChanged += new IpcEvents.KeyValueChangedHandler(OnKeySet);
            IpcEvents.OnKeyRemoved += new IpcEvents.KeyListChangedHandler(OnKeyUpdated);
            IpcEvents.OnKeyAdded += new IpcEvents.KeyListChangedHandler(OnKeyUpdated);
        }

        public override void Unload()
        {
            ExtensionEvents.OnFrameworkTick -= UpdateFile;

            File.Delete(FileLocation);
        }

        #region Private Functions
        private void UpdateFile()
        {
<<<<<<< HEAD
            if (!_Update || PointBlankIPCManager.IPCType != EIPCType.FILE)
                return;
            List<string> contents = new List<string>();

            foreach (string key in IPC.Keys)
                contents.Add(key + ":" + IPC[key]);
=======
            if (!_update || PointBlankIpcManager.IpcType != EipcType.File)
                return;
            List<string> contents = new List<string>();

            foreach (string key in Ipc.Keys)
                contents.Add(key + ":" + Ipc[key]);
>>>>>>> master

            try
            {
                File.WriteAllLines(FileLocation, contents.ToArray());
<<<<<<< HEAD
                _Update = false;
=======
                _update = false;
>>>>>>> master
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                Thread.Sleep(1);
            }
        }
        #endregion

        #region Event Functions
        public static void OnOutput(string text)
        {
<<<<<<< HEAD
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
=======
            if (PointBlankIpcManager.IpcType == EipcType.Console)
>>>>>>> master
                PointBlankConsole.WriteLine("0x0:" + text);
        }

        private void OnKeySet(string key, string value)
        {
<<<<<<< HEAD
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + value);
            else if (PointBlankIPCManager.IPCType == EIPCType.FILE)
                _Update = true;
=======
            if (PointBlankIpcManager.IpcType == EipcType.Console)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + value);
            else if (PointBlankIpcManager.IpcType == EipcType.File)
                _update = true;
>>>>>>> master
        }

        private void OnKeyUpdated(string key)
        {
<<<<<<< HEAD
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + IPC[key]);
            else if (PointBlankIPCManager.IPCType == EIPCType.FILE)
                _Update = true;
=======
            if (PointBlankIpcManager.IpcType == EipcType.Console)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + Ipc[key]);
            else if (PointBlankIpcManager.IpcType == EipcType.File)
                _update = true;
>>>>>>> master
        }
        #endregion
    }
}
