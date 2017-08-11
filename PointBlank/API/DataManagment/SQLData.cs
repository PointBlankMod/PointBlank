using System;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using PointBlank.Framework.Permissions.Ring;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Easy SQL manager
    /// </summary>
    [RingPermission(SecurityAction.Demand, ring = RingPermissionRing.None)]
    public class SQLData
    {
        #region Variables
        private static Thread _tAsync = new Thread(new ThreadStart(RunAsync));

        private static Queue<AsyncCommand> _AsyncCommands = new Queue<AsyncCommand>();
        #endregion

        #region Properties
        /// <summary>
        /// The Server of the SQL
        /// </summary>
        public string Server { get; private set; }
        /// <summary>
        /// The database name of the SQL server
        /// </summary>
        public string Database { get; private set; }
        /// <summary>
        /// The user that is connected
        /// </summary>
        public string Username { get; private set; }
        /// <summary>
        /// The timeout of the connection
        /// </summary>
        public int Timeout { get; private set; }
        /// <summary>
        /// Is the connection trusted
        /// </summary>
        public bool Trusted { get; private set; }

        /// <summary>
        /// The SQL connection to the database
        /// </summary>
        public SqlConnection Connection { get; private set; }
        /// <summary>
        /// The SQL command used to run functions
        /// </summary>
        public SqlCommand Command { get; private set; }

        /// <summary>
        /// Is the connection open
        /// </summary>
        public bool Connected { get; private set; }
        #endregion

        /// <summary>
        /// Easy SQL manager
        /// </summary>
        /// <param name="server">The server IP/URL</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username of the SQL</param>
        /// <param name="password">The password of the SQL</param>
        /// <param name="timeout">The timeout of the connection</param>
        /// <param name="trusted">Is the connection trusted</param>
        public SQLData(string server, string database, string username, string password, int timeout = 30, bool trusted = true)
        {
            if (_tAsync.ThreadState != ThreadState.Running)
                _tAsync.Start();

            this.Server = server; // Set the server
            this.Database = database; // Set the database
            this.Username = username; // Set the username
            this.Timeout = timeout; // Set the timeout
            this.Trusted = trusted; // Set the trusted

            Connection = new SqlConnection(
                $"user id={username};password={password};server={server};Trusted_Connection={(trusted ? "yes" : "no")};database={database};connection timeout={timeout.ToString()}");
            Command.Connection = Connection; // Set the connection

            Enviroment.SQLConnections.Add(this); // Add to the list
        }

        #region Static Functions
        /// <summary>
        /// Converts the ESQLDataType to the SQL string
        /// </summary>
        /// <param name="dataType">The datatype to convert</param>
        /// <returns>Converted string</returns>
        public static string DataTypeToString(ESQLDataType dataType)
        {
            switch (dataType)
            {
                case ESQLDataType.BIGINT:
                    return "BIGINT";
                case ESQLDataType.CHAR_128:
                    return "CHAR(128)";
                case ESQLDataType.CHAR_16:
                    return "CHAR(16)";
                case ESQLDataType.CHAR_255:
                    return "CHAR(255)";
                case ESQLDataType.CHAR_32:
                    return "CHAR(32)";
                case ESQLDataType.CHAR_64:
                    return "CHAR(64)";
                case ESQLDataType.DATE:
                    return "DATE";
                case ESQLDataType.DATETIME:
                    return "DATETIME";
                case ESQLDataType.DECIMAL_128_128:
                    return "DECIMAL(128,128)";
                case ESQLDataType.DECIMAL_128_16:
                    return "DECIMAL(128,16)";
                case ESQLDataType.DECIMAL_128_255:
                    return "DECIMAL(128,255)";
                case ESQLDataType.DECIMAL_128_32:
                    return "DECIMAL(128,32)";
                case ESQLDataType.DECIMAL_128_64:
                    return "DECIMAL(128,64)";
                case ESQLDataType.DECIMAL_16_128:
                    return "DECIMAL(16,128)";
                case ESQLDataType.DECIMAL_16_16:
                    return "DECIMAL(16,16)";
                case ESQLDataType.DECIMAL_16_255:
                    return "DECIMAL(16,255)";
                case ESQLDataType.DECIMAL_16_32:
                    return "DECIMAL(16,32)";
                case ESQLDataType.DECIMAL_16_64:
                    return "DECIMAL(16,64)";
                case ESQLDataType.DECIMAL_255_128:
                    return "DECIMAL(255,128)";
                case ESQLDataType.DECIMAL_255_16:
                    return "DECIMAL(255,16)";
                case ESQLDataType.DECIMAL_255_255:
                    return "DECIMAL(255,255)";
                case ESQLDataType.DECIMAL_255_32:
                    return "DECIMAL(255,32)";
                case ESQLDataType.DECIMAL_255_64:
                    return "DECIMAL(255,64)";
                case ESQLDataType.DECIMAL_32_128:
                    return "DECIMAL(32,128)";
                case ESQLDataType.DECIMAL_32_16:
                    return "DECIMAL(32,16)";
                case ESQLDataType.DECIMAL_32_255:
                    return "DECIMAL(32,255)";
                case ESQLDataType.DECIMAL_32_32:
                    return "DECIMAL(32,32)";
                case ESQLDataType.DECIMAL_32_64:
                    return "DECIMAL(32,64)";
                case ESQLDataType.DECIMAL_64_128:
                    return "DECIMAL(64,128)";
                case ESQLDataType.DECIMAL_64_16:
                    return "DECIMAL(64,16)";
                case ESQLDataType.DECIMAL_64_255:
                    return "DECIMAL(64,255)";
                case ESQLDataType.DECIMAL_64_32:
                    return "DECIMAL(64,32)";
                case ESQLDataType.DECIMAL_64_64:
                    return "DECIMAL(64,64)";
                case ESQLDataType.DOUBLE_128_10:
                    return "DOUBLE(128,10)";
                case ESQLDataType.DOUBLE_128_20:
                    return "DOUBLE(128,20)";
                case ESQLDataType.DOUBLE_128_30:
                    return "DOUBLE(128,30)";
                case ESQLDataType.DOUBLE_128_4:
                    return "DOUBLE(128,4)";
                case ESQLDataType.DOUBLE_128_40:
                    return "DOUBLE(128,40)";
                case ESQLDataType.DOUBLE_128_50:
                    return "DOUBLE(128,50)";
                case ESQLDataType.DOUBLE_128_53:
                    return "DOUBLE(128,53)";
                case ESQLDataType.DOUBLE_16_10:
                    return "DOUBLE(16,10)";
                case ESQLDataType.DOUBLE_16_20:
                    return "DOUBLE(16,20)";
                case ESQLDataType.DOUBLE_16_30:
                    return "DOUBLE(16,30)";
                case ESQLDataType.DOUBLE_16_4:
                    return "DOUBLE(16,4)";
                case ESQLDataType.DOUBLE_16_40:
                    return "DOUBLE(16,40)";
                case ESQLDataType.DOUBLE_16_50:
                    return "DOUBLE(16,50)";
                case ESQLDataType.DOUBLE_16_53:
                    return "DOUBLE(16,53)";
                case ESQLDataType.DOUBLE_255_10:
                    return "DOUBLE(255,10)";
                case ESQLDataType.DOUBLE_255_20:
                    return "DOUBLE(255,20)";
                case ESQLDataType.DOUBLE_255_30:
                    return "DOUBLE(255,30)";
                case ESQLDataType.DOUBLE_255_4:
                    return "DOUBLE(255,4)";
                case ESQLDataType.DOUBLE_255_40:
                    return "DOUBLE(255,40)";
                case ESQLDataType.DOUBLE_255_50:
                    return "DOUBLE(255,50)";
                case ESQLDataType.DOUBLE_255_53:
                    return "DOUBLE(255,53)";
                case ESQLDataType.DOUBLE_32_10:
                    return "DOUBLE(32,10)";
                case ESQLDataType.DOUBLE_32_20:
                    return "DOUBLE(32,20)";
                case ESQLDataType.DOUBLE_32_30:
                    return "DOUBLE(32,30)";
                case ESQLDataType.DOUBLE_32_4:
                    return "DOUBLE(32,4)";
                case ESQLDataType.DOUBLE_32_40:
                    return "DOUBLE(32,40)";
                case ESQLDataType.DOUBLE_32_50:
                    return "DOUBLE(32,50)";
                case ESQLDataType.DOUBLE_32_53:
                    return "DOUBLE(32,53)";
                case ESQLDataType.DOUBLE_64_10:
                    return "DOUBLE(64,10)";
                case ESQLDataType.DOUBLE_64_20:
                    return "DOUBLE(64,20)";
                case ESQLDataType.DOUBLE_64_30:
                    return "DOUBLE(64,30)";
                case ESQLDataType.DOUBLE_64_4:
                    return "DOUBLE(64,4)";
                case ESQLDataType.DOUBLE_64_40:
                    return "DOUBLE(64,40)";
                case ESQLDataType.DOUBLE_64_50:
                    return "DOUBLE(64,50)";
                case ESQLDataType.DOUBLE_64_53:
                    return "DOUBLE(64,53)";
                case ESQLDataType.ENUM:
                    return "ENUM";
                case ESQLDataType.FLOAT_10_10:
                    return "FLOAT(10,10)";
                case ESQLDataType.FLOAT_10_2:
                    return "FLOAT(10,2)";
                case ESQLDataType.FLOAT_10_20:
                    return "FLOAT(10,20)";
                case ESQLDataType.FLOAT_10_24:
                    return "FLOAT(10,24)";
                case ESQLDataType.FLOAT_128_10:
                    return "FLOAT(128,10)";
                case ESQLDataType.FLOAT_128_2:
                    return "FLOAT(128,2)";
                case ESQLDataType.FLOAT_128_20:
                    return "FLOAT(128,20)";
                case ESQLDataType.FLOAT_128_24:
                    return "FLOAT(128,24)";
                case ESQLDataType.FLOAT_255_10:
                    return "FLOAT(255,10)";
                case ESQLDataType.FLOAT_255_2:
                    return "FLOAT(255,2)";
                case ESQLDataType.FLOAT_255_20:
                    return "FLOAT(255,20)";
                case ESQLDataType.FLOAT_255_24:
                    return "FLOAT(255,24)";
                case ESQLDataType.FLOAT_32_10:
                    return "FLOAT(32,10)";
                case ESQLDataType.FLOAT_32_2:
                    return "FLOAT(32,2)";
                case ESQLDataType.FLOAT_32_20:
                    return "FLOAT(32,20)";
                case ESQLDataType.FLOAT_32_24:
                    return "FLOAT(32,24)";
                case ESQLDataType.FLOAT_64_10:
                    return "FLOAT(64,10)";
                case ESQLDataType.FLOAT_64_2:
                    return "FLOAT(64,2)";
                case ESQLDataType.FLOAT_64_20:
                    return "FLOAT(64,20)";
                case ESQLDataType.FLOAT_64_24:
                    return "FLOAT(64,24)";
                case ESQLDataType.INT:
                    return "INT";
                case ESQLDataType.LONGTEXT:
                    return "LONGTEXT";
                case ESQLDataType.MEDIUMINT:
                    return "MEDIUMINT";
                case ESQLDataType.MEDIUMTEXT:
                    return "MEDIUMTEXT";
                case ESQLDataType.SMALLINT:
                    return "SMALLINT";
                case ESQLDataType.TEXT:
                    return "TEXT";
                case ESQLDataType.TIME:
                    return "TIME";
                case ESQLDataType.TIMESTAMP:
                    return "TIMESTAMP";
                case ESQLDataType.TINYINT:
                    return "TINYINT";
                case ESQLDataType.TINYTEXT:
                    return "TINYTEXT";
                case ESQLDataType.VARCHAR_128:
                    return "VARCHAR(128)";
                case ESQLDataType.VARCHAR_16:
                    return "VARCHAR(16)";
                case ESQLDataType.VARCHAR_255:
                    return "VARCHAR(255)";
                case ESQLDataType.VARCHAR_32:
                    return "VARCHAR(32)";
                case ESQLDataType.VARCHAR_64:
                    return "VARCHAR(64)";
                default:
                    return "TEXT";
            }
        }

        private static void RunAsync()
        {
            while (Enviroment.Running)
            {
                while (_AsyncCommands.Count > 0)
                {
                    AsyncCommand command = _AsyncCommands.Dequeue();

                    try
                    {
                        if (command.CallBack == null)
                            command.Command.ExecuteNonQuery();
                        else
                            command.CallBack(command.Command.ExecuteReader(command.Behaviour));
                    }
                    catch (Exception ex)
                    {
                        PointBlankLogging.LogError("Could not send async command to server!", ex, false, false);
                    }
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Connects to the server
        /// </summary>
        /// <returns>Was the connection successful</returns>
        public bool Connect()
        {
            if (Connected)
                return true;

            try
            {
                Connection.Open(); // Open the connection
                Connected = true; // Set the connected

                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error connecting to the SQL server! " + Server, ex, false, false);

                return false;
            }
        }

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        /// <returns>Was the disconnect successful</returns>
        public bool Disconnect()
        {
            if (!Connected)
                return true;

            try
            {
                Connection.Close(); // Close the connection
                Connected = false; // Set the connected

                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error while disconnecting from the SQL server " + Server, ex, false, false);
                return false;
            }
        }

        /// <summary>
        /// Executes an SQL command on the server without returning the output
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="paramaters">The paramaters in the command</param>
        /// <returns>If the command was ran successfully</returns>
        public bool SendCommand(string command, Dictionary<string, string> paramaters = null)
        {
            try
            {
                Command.CommandText = command; // Set the command text
                Command.Parameters.Clear(); // Clear the paramaters

                if (paramaters != null)
                    foreach (KeyValuePair<string, string> kvp in paramaters)
                        Command.Parameters.AddWithValue(kvp.Key, kvp.Value); // Add the paramater

                Command.ExecuteNonQuery(); // Execute the command

                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Could not send command in SQL server: " + Server, ex, false, false);
                return false;
            }
        }

        /// <summary>
        /// Executes an SQL command on the server without returning the output using async
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="paramaters">The paramaters in the command</param>
        public void SendCommandAsync(string command, Dictionary<string, string> paramaters = null)
        {
            try
            {
                Command.CommandText = command; // Set the command text
                Command.Parameters.Clear(); // Clear the paramaters

                if (paramaters != null)
                    foreach (KeyValuePair<string, string> kvp in paramaters)
                        Command.Parameters.AddWithValue(kvp.Key, kvp.Value); // Add the paramater

                _AsyncCommands.Enqueue(new AsyncCommand(Command));
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Could not send command in SQL server: " + Server, ex, false, false);
            }
        }

        /// <summary>
        /// Executes an SQL command on the server and returns the output
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="output">The data returned by the SQL server</param>
        /// <param name="behaviour">The behaviour of the server</param>
        /// <param name="paramaters">The paramaters in the command</param>
        /// <returns>If the command was ran successfully</returns>
        public bool SendCommand(string command, out SqlDataReader output, CommandBehavior behaviour = CommandBehavior.Default, Dictionary<string, string> paramaters = null)
        {
            try
            {
                Command.CommandText = command; // Set the command text
                Command.Parameters.Clear(); // Clear the paramaters

                if (paramaters != null)
                    foreach (KeyValuePair<string, string> kvp in paramaters)
                        Command.Parameters.AddWithValue(kvp.Key, kvp.Value); // Add the paramater

                output = Command.ExecuteReader(behaviour); // Execute the reader

                return true;
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Could not send command in SQL server: " + Server, ex, false, false);
                output = null;
                return false;
            }
        }

        /// <summary>
        /// Executes an SQL command on the server and returns the output through a callback using async
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="output">The data returned by the SQL server</param>
        /// <param name="behaviour">The behaviour of the server</param>
        /// <param name="paramaters">The paramaters in the command</param>
        /// <param name="callback">The callback function that is called when the query is done</param>
        public void SendCommandAsync(string command, Action<SqlDataReader> callback, CommandBehavior behaviour = CommandBehavior.Default, Dictionary<string, string> paramaters = null)
        {
            try
            {
                Command.CommandText = command; // Set the command text
                Command.Parameters.Clear(); // Clear the paramaters

                if (paramaters != null)
                    foreach (KeyValuePair<string, string> kvp in paramaters)
                        Command.Parameters.AddWithValue(kvp.Key, kvp.Value); // Add the paramater

                _AsyncCommands.Enqueue(new AsyncCommand(Command, callback, behaviour));
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Could not send command in SQL server: " + Server, ex, false, false);
            }
        }

        /// <summary>
        /// Creates a table on the SQL server
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columns">The columns of the table</param>
        /// <returns>If the table was successfully created</returns>
        public bool CreateTable(string tableName, Dictionary<string, ESQLDataType> columns)
        {
            string cols = "id INT(16) UNSIGNED AUTO_INCREMENT PRIMARY KEY";

            foreach (KeyValuePair<string, ESQLDataType> kvp in columns)
                cols += "," + kvp.Key + DataTypeToString(kvp.Value);

            return SendCommand("CREATE TABLE " + tableName + " (" + cols + ");");
        }

        /// <summary>
        /// Creates a table on the SQL server using async
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columns">The columns of the table</param>
        public void CreateTableAsync(string tableName, Dictionary<string, ESQLDataType> columns)
        {
            string cols = "id INT(16) UNSIGNED AUTO_INCREMENT PRIMARY KEY";

            foreach (KeyValuePair<string, ESQLDataType> kvp in columns)
                cols += "," + kvp.Key + DataTypeToString(kvp.Value);

            SendCommandAsync("CREATE TABLE " + tableName + " (" + cols + ");");
        }

        /// <summary>
        /// Deletes a table from the SQL server
        /// </summary>
        /// <param name="tableName">The name of the table to remove</param>
        /// <returns>If the table was successful</returns>
        public bool DeleteTable(string tableName) => SendCommand("DROP TABLE " + tableName + ";");

        /// <summary>
        /// Deletes a table from the SQL server using async
        /// </summary>
        /// <param name="tableName">The name of the table to remove</param>
        public void DeleteTableAsync(string tableName) => SendCommandAsync("DROP TABLE " + tableName + ";");

        /// <summary>
        /// Adds an entry to the table
        /// </summary>
        /// <param name="tableName">The name of the table to add the entry to</param>
        /// <param name="data">The column values to set</param>
        /// <returns>If the entry was added successfully</returns>
        public bool AddEntry(string tableName, string[] data)
        {
            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            for (int i = 0; i < data.Length; i++)
                paramaters.Add("@f" + i, data[i]);

            return SendCommand("INSERT INTO " + tableName + " VALUE (" + string.Join(", ", paramaters.Keys.ToArray()) + ");", paramaters);
        }

        /// <summary>
        /// Adds an entry to the table using async
        /// </summary>
        /// <param name="tableName">The name of the table to add the entry to</param>
        /// <param name="data">The column values to set</param>
        public void AddEntryAsync(string tableName, string[] data)
        {
            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            for (int i = 0; i < data.Length; i++)
                paramaters.Add("@f" + i, data[i]);

            SendCommandAsync("INSERT INTO " + tableName + " VALUE (" + string.Join(", ", paramaters.Keys.ToArray()) + ");", paramaters);
        }

        /// <summary>
        /// Adds an entry to the table on specified columns
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="data">The column and value dictionary</param>
        /// <returns>If the entry was added successfully</returns>
        public bool AddEntry(string tableName, Dictionary<string, string> data)
        {
            string[] columns = new string[data.Count];
            string[] values = new string[data.Count];
            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            for (int i = 0; i < data.Count; i++)
            {
                columns[i] = data.Keys.ElementAt(i);
                values[i] = "@" + data.Keys.ElementAt(i);
                paramaters.Add("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }

            return SendCommand("INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") VALUES (" + string.Join(", ", values) + ");", paramaters);
        }

        /// <summary>
        /// Adds an entry to the table on specified columns using async
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="data">The column and value dictionary</param>
        public void AddEntryAsync(string tableName, Dictionary<string, string> data)
        {
            string[] columns = new string[data.Count];
            string[] values = new string[data.Count];
            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            for (int i = 0; i < data.Count; i++)
            {
                columns[i] = data.Keys.ElementAt(i);
                values[i] = "@" + data.Keys.ElementAt(i);
                paramaters.Add("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }

            SendCommandAsync("INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") VALUES (" + string.Join(", ", values) + ");", paramaters);
        }

        /// <summary>
        /// Modifies an entry in the table
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="data">The columns + values to modify</param>
        /// <param name="condition">The condition to find what to modify</param>
        /// <param name="conditionParameters">Parameters for the condition</param>
        /// <returns>If the entry was modified successfully</returns>
        public bool ModifyEntry(string tableName, Dictionary<string, string> data, string condition, Dictionary<string, string> conditionParameters = null)
        {
            string[] sets = new string[data.Count];
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (conditionParameters != null)
                foreach (KeyValuePair<string, string> kvp in conditionParameters)
                    parameters.Add(kvp.Key, kvp.Value);
            for (int i = 0; i < data.Count; i++)
            {
                sets[i] = data.Keys.ElementAt(i) + " = @" + data.Keys.ElementAt(i);
                parameters.Add("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }

            return SendCommand("UPDATE " + tableName + " SET " + string.Join(", ", sets) + " WHERE " + condition, parameters);
        }

        /// <summary>
        /// Modifies an entry in the table using async
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="data">The columns + values to modify</param>
        /// <param name="condition">The condition to find what to modify</param>
        /// <param name="conditionParameters">Parameters for the condition</param>
        public void ModifyEntryAsync(string tableName, Dictionary<string, string> data, string condition, Dictionary<string, string> conditionParameters = null)
        {
            string[] sets = new string[data.Count];
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (conditionParameters != null)
                foreach (KeyValuePair<string, string> kvp in conditionParameters)
                    parameters.Add(kvp.Key, kvp.Value);
            for (int i = 0; i < data.Count; i++)
            {
                sets[i] = data.Keys.ElementAt(i) + " = @" + data.Keys.ElementAt(i);
                parameters.Add("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }

            SendCommandAsync("UPDATE " + tableName + " SET " + string.Join(", ", sets) + " WHERE " + condition, parameters);
        }

        /// <summary>
        /// Deletes an entry from the table
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="condition">The condition to find the correct entry</param>
        /// <param name="parameters">The option parameters for the condition</param>
        /// <returns>If the entry was deleted successfully</returns>
        public bool DeleteEntry(string tableName, string condition, Dictionary<string, string> parameters = null) => SendCommand("DELETE FROM " + tableName + " WHERE " + condition, parameters);

        /// <summary>
        /// Deletes an entry from the table using async
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="condition">The condition to find the correct entry</param>
        /// <param name="parameters">The option parameters for the condition</param>
        public void DeleteEntryAsync(string tableName, string condition, Dictionary<string, string> parameters = null) => SendCommandAsync("DELETE FROM " + tableName + " WHERE " + condition, parameters);
        #endregion

        #region SubClasses
        private class AsyncCommand
        {
            public SqlCommand Command;
            public Action<SqlDataReader> CallBack;
            public CommandBehavior Behaviour;

            public AsyncCommand(SqlCommand Command, Action<SqlDataReader> CallBack = null, CommandBehavior Behaviour = CommandBehavior.Default)
            {
                this.Command = Command;
                this.CallBack = CallBack;
                this.Behaviour = Behaviour;
            }
        }
        #endregion
    }
}
