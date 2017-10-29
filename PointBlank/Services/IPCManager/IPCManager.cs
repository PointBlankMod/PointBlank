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
    internal class IPCManager : PointBlankService
    {
        #region Info
        public static readonly string FileLocation = Directory.GetCurrentDirectory() + "/IPC.ipc";
        #endregion

        #region Variables
        private bool _Update = false;
        #endregion

        #region Properties
        public static Dictionary<string, string> IPC { get; } = new Dictionary<string, string>();

        public override int LaunchIndex => 1;
        #endregion

        public override void Load()
        {
            if (File.Exists(FileLocation))
                File.Delete(FileLocation);

            // Setup the thread
            ExtensionEvents.OnFrameworkTick += UpdateFile;

            // Setup the events
            IPCEvents.OnKeyValueChanged += new IPCEvents.KeyValueChangedHandler(OnKeySet);
            IPCEvents.OnKeyRemoved += new IPCEvents.KeyListChangedHandler(OnKeyUpdated);
            IPCEvents.OnKeyAdded += new IPCEvents.KeyListChangedHandler(OnKeyUpdated);
        }

        public override void Unload()
        {
            ExtensionEvents.OnFrameworkTick -= UpdateFile;

            File.Delete(FileLocation);
        }

        #region Private Functions
        private void UpdateFile()
        {
            if (!_Update || PointBlankIPCManager.IPCType != EIPCType.FILE)
                return;
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
        #endregion

        #region Event Functions
        public static void OnOutput(string text)
        {
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
                PointBlankConsole.WriteLine("0x0:" + text);
        }

        private void OnKeySet(string key, string value)
        {
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + value);
            else if (PointBlankIPCManager.IPCType == EIPCType.FILE)
                _Update = true;
        }

        private void OnKeyUpdated(string key)
        {
            if (PointBlankIPCManager.IPCType == EIPCType.CONSOLE)
                PointBlankConsole.WriteLine("0x1:" + key + ":" + IPC[key]);
            else if (PointBlankIPCManager.IPCType == EIPCType.FILE)
                _Update = true;
        }
        #endregion
    }
}
