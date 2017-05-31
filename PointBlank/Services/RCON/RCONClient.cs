using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;

namespace PointBlank.Services.RCON
{
    internal class RCONClient
    {
        #region Properties
        public TcpClient Client { get; private set; } // The TCP client
        public Thread ClientThread { get; private set; } // The client thread
        public NetworkStream Stream { get; private set; } // The network stream

        public string IP { get; private set; } // The IP of the client

        public Queue<string> Commands { get; private set; } // The commands to execute
        public bool Authed { get; private set; } // Is the client authorized
        public bool InConsole { get; private set; } // Is the client in the console
        #endregion

        public RCONClient(TcpClient client)
        {
            // Set the variables
            this.Client = client; // Set the TCP client
            this.Authed = false; // Set authed
            this.InConsole = false; // Set in console

            // Setup the variables
            ClientThread = new Thread(new ThreadStart(RunCommands)); // Create the client thread
            Commands = new Queue<string>(); // Create the commands queue
            IP = Client.Client.RemoteEndPoint.ToString(); // Set the IP
            Stream = Client.GetStream();

            // Execute functions
            ClientThread.Start();
        }

        #region Public Functions
        public string ReceiveMessage()
        {
            try
            {
                byte[] messageBytes = new byte[Client.ReceiveBufferSize]; // Create the byte array for the message
                int readBytes = Stream.Read(messageBytes, 0, messageBytes.Length); // Read the stream

                if (readBytes <= 0)
                    return null;

                return Encoding.Unicode.GetString(messageBytes, 0, readBytes); // Convert the bytes to string and return
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SendMessage(string message, bool newLine = true)
        {
            try
            {
                if (newLine)
                    message = message + (!message.Contains('\n') ? "\r\n" : ""); // Add the enter
                if (string.IsNullOrEmpty(message))
                    return true;

                byte[] bytes = Encoding.Unicode.GetBytes(message); // Get the bytes
                Stream.Write(bytes, 0, bytes.Length); // Write the message

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendLog(string message)
        {
            return SendMessage("[RCON] " + message, true);
        }

        public void Disconnect()
        {
            try
            {
                Client.Client.Disconnect(false); // Disconnect
                ClientThread.Abort(); // Abort the thread
                Client.Close(); // Close the connection
            }
            catch(Exception ex) { }
        }
        #endregion

        #region Thread Functions
        private void RunCommands()
        {
            while(Client != null && Client.Connected)
            {
                try
                {
                    string command = ReceiveMessage(); // Receive the message
                    string[] commands = null;
                    command.TrimEnd('\n', '\r', ' ', '\t'); // Trim any extra characters

                    if (string.IsNullOrEmpty(command))
                        continue;
                    commands = command.Split(' '); // Split the command

                    if(commands[0].ToLower() == "console")
                    {
                        if (!Authed)
                        {
                            SendLog("You aren't logged in! Please login!");
                            continue; // Not authorized
                        }

                        InConsole = !InConsole; // Switch the InConsole
                        SendLog((InConsole ? "You are now in console mode! Use the same command to exit console mode!" : "You are no longer in console mode"));
                        continue;
                    }
                    if (!InConsole)
                    {
                        if(commands[0].ToLower() == "exit" || commands[0].ToLower() == "shutdown" || commands[0].ToLower() == "quit")
                        {
                            break;
                        }
                        else if(commands[0].ToLower() == "help" || commands[0].ToLower() == "?")
                        {
                            SendLog("help/? - Returns this text");
                            SendLog("exit/shutdown/quit - Exits the RCON");
                            SendLog("login <password> - Login to the RCON");
                            SendLog("logout - Logout of the RCON");
                            SendLog("console - Activates console mode");
                            continue;
                        }
                        else if(commands[0].ToLower() == "login" && commands.Length > 1)
                        {
                            if (Authed)
                            {
                                SendLog("You are already logged in! Do logout to log out of the RCON!");
                                continue;
                            }
                            if(commands[1] != RCONConfiguration.Password)
                            {
                                SendLog("Invalid password! Please try again!");
                                continue;
                            }

                            Authed = true;
                            SendLog("You are now logged in!");
                            continue;
                        }
                        else if(commands[0].ToLower() == "logout")
                        {
                            if (!Authed)
                            {
                                SendLog("You aren't logged in!");
                                continue;
                            }

                            Authed = false;
                            SendLog("You are now logged out!");
                            continue;
                        }
                    }
                    else
                    {
                        lock (Commands)
                            Commands.Enqueue(command); // Add the command to queue
                    }
                }
                catch (Exception ex) { }
            }
            SendLog("You have now disconnected!");
            Disconnect(); // Close the connection
        }
        #endregion
    }
}
