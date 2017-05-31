using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Services;
using PointBlank.API.Implements;
using PointBlank.API.DataManagment;
using SM = PointBlank.Framework.ServiceManager;
using SDG.Unturned;

namespace PointBlank.Services.RCON
{
    [Service("RCON", false)]
    internal class RCON : Service
    {
        #region Info
        public static string Version { get { return "0.1"; } }
        #endregion

        #region Properties
        private static UniversalData UniRCONConfig { get; set; } // The universal RCON data
        private static JsonData RCONConfig { get; set; } // The JSON RCON config

        public static List<RCONClient> Clients { get; private set; } // The list of RCON clients
        public static Thread ListenThread { get; private set; } // The listener thread
        public static TcpListener Listener { get; private set; } // The TCP listener

        public static bool Running { get; private set; } // Is the listener running
        #endregion

        #region Override Functions
        public override void Load()
        {
            // Setup the variables
            UniRCONConfig = new UniversalData(SM.ConfigurationPath + "/RCON"); // Get the file
            RCONConfig = UniRCONConfig.GetData(EDataType.JSON) as JsonData; // Get the JSON
            Clients = new List<RCONClient>(); // Create a list of clients
            ListenThread = new Thread(new ThreadStart(RunListen)); // Create the thread

            // Add events
            CommandWindow.onCommandWindowOutputted += new CommandWindowOutputted(OnConsoleOutput); // Set the console output event
            CommandWindow.onCommandWindowInputted += new CommandWindowInputted(OnConsoleInput); // Set the console input event

            // Set the variables
            Running = true;

            // Run important functions
            LoadConfig(); // Load the config
            Listener = new TcpListener(IPAddress.Any, RCONConfiguration.Port); // Create the TCP listener
            Listener.Start(); // Start the listener
            ListenThread.Start(); // Start the listen thread

        }

        public override void Unload()
        {
            // Set the variables
            Running = false;

            // Run important code
            foreach (RCONClient client in Clients)
                client.Disconnect(); // Disconnect the clients

            // Run important functions
            SaveConfig(); // Save the config
            ListenThread.Abort(); // Abort the thread
            Listener.Stop(); // Stop the listener
        }
        #endregion

        #region Mono Functions
        public void UpdateX()
        {
            if (!RCONConfiguration.CanSendCommands)
                return;

            lock (Clients)
            {
                foreach(RCONClient client in Clients)
                {
                    lock (client.Commands)
                    {

                    }
                }
            }
        }
        #endregion

        #region Event Functions
        private void OnConsoleOutput(object text, ConsoleColor color)
        {
            if (!RCONConfiguration.CanReadLogs)
                return;

            lock(Clients)
                foreach (RCONClient client in Clients)
                    if(client.InConsole)
                        client.SendMessage(text.ToString(), true);
        }

        private void OnConsoleInput(string text, ref bool shouldExecute)
        {
            if(RCONConfiguration.ShowExecutedCommands)
                Logging.Log("Executing command: " + text);
        }
        #endregion

        #region Private Functions
        private void LoadConfig()
        {
            if (UniRCONConfig.CreatedNew)
            {
                RCONConfiguration.CanReadLogs = true;
                RCONConfiguration.CanSendCommands = true;
                RCONConfiguration.Password = PBTools.RandomString();
                RCONConfiguration.Port = 27115;
                RCONConfiguration.MaxClients = 2;
                RCONConfiguration.ShowExecutedCommands = true;

                RCONConfig.Document.Add("Port", RCONConfiguration.Port.ToString());
                RCONConfig.Document.Add("Password", RCONConfiguration.Password);
                RCONConfig.Document.Add("MaxClients", RCONConfiguration.MaxClients.ToString());
                RCONConfig.Document.Add("CanReadLogs", (RCONConfiguration.CanReadLogs ? "true" : "false"));
                RCONConfig.Document.Add("CanSendCommands", (RCONConfiguration.CanSendCommands ? "true" : "false"));
                RCONConfig.Document.Add("ShowExecutedCommands", (RCONConfiguration.ShowExecutedCommands ? "true" : "false"));
            }
            else
            {
                RCONConfiguration.CanReadLogs = ((string)RCONConfig.Document["CanReadLogs"] == "true");
                RCONConfiguration.CanSendCommands = ((string)RCONConfig.Document["CanSendCommands"] == "true");
                RCONConfiguration.Password = (string)RCONConfig.Document["Password"];
                RCONConfiguration.Port = int.Parse((string)RCONConfig.Document["Port"]);
                RCONConfiguration.MaxClients = int.Parse((string)RCONConfig.Document["MaxClients"]);
                RCONConfiguration.ShowExecutedCommands = ((string)RCONConfig.Document["ShowExecutedCommands"] == "true");
            }
        }

        private void SaveConfig()
        {
            UniRCONConfig.Save();
        }
        #endregion

        #region Thread Functions
        private void RunListen()
        {
            while (Running)
            {
                TcpClient tcp = Listener.AcceptTcpClient(); // Get the client connect

                if (tcp == null || Clients.Count >= RCONConfiguration.MaxClients)
                    continue;

                RCONClient client = new RCONClient(tcp); // Create a new RCON client

                client.SendMessage(PointBlankInfo.Name + " v" + PointBlankInfo.Version, true); // Print the pointblank info
                client.SendMessage("RCON v" + Version, true); // Print the RCON info
                Clients.Add(client); // Add the client to the clients list

                Logging.Log("Client connected! IP: " + client.IP, false); // Log the connection
            }
        }
        #endregion
    }
}
