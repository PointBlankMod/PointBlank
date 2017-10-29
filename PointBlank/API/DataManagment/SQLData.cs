using System;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace PointBlank.API.DataManagment
{
    /// <summary>
    /// Easy SQL manager
    /// </summary>
<<<<<<< HEAD
    public class SQLData
=======
    public class SqlData
>>>>>>> master
    {
        #region Variables
        private static Thread _tAsync = new Thread(new ThreadStart(RunAsync));

        private static Queue<AsyncCommand> _asyncCommands = new Queue<AsyncCommand>();
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
        public SqlData(string server, string database, string username, string password, int timeout = 30, bool trusted = true)
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

            PointBlankEnvironment.SqlConnections.Add(this); // Add to the list
        }

        #region Static Functions
        /// <summary>
        /// Converts the ESQLDataType to the SQL string
        /// </summary>
        /// <param name="dataType">The datatype to convert</param>
        /// <returns>Converted string</returns>
        public static string DataTypeToString(EsqlDataType dataType)
        {
            switch (dataType)
            {
                case EsqlDataType.Bigint:
                    return "BIGINT";
                case EsqlDataType.Char128:
                    return "CHAR(128)";
                case EsqlDataType.Char16:
                    return "CHAR(16)";
                case EsqlDataType.Char255:
                    return "CHAR(255)";
                case EsqlDataType.Char32:
                    return "CHAR(32)";
                case EsqlDataType.Char64:
                    return "CHAR(64)";
                case EsqlDataType.Date:
                    return "DATE";
                case EsqlDataType.Datetime:
                    return "DATETIME";
                case EsqlDataType.Decimal128128:
                    return "DECIMAL(128,128)";
                case EsqlDataType.Decimal12816:
                    return "DECIMAL(128,16)";
                case EsqlDataType.Decimal128255:
                    return "DECIMAL(128,255)";
                case EsqlDataType.Decimal12832:
                    return "DECIMAL(128,32)";
                case EsqlDataType.Decimal12864:
                    return "DECIMAL(128,64)";
                case EsqlDataType.Decimal16128:
                    return "DECIMAL(16,128)";
                case EsqlDataType.Decimal1616:
                    return "DECIMAL(16,16)";
                case EsqlDataType.Decimal16255:
                    return "DECIMAL(16,255)";
                case EsqlDataType.Decimal1632:
                    return "DECIMAL(16,32)";
                case EsqlDataType.Decimal1664:
                    return "DECIMAL(16,64)";
                case EsqlDataType.Decimal255128:
                    return "DECIMAL(255,128)";
                case EsqlDataType.Decimal25516:
                    return "DECIMAL(255,16)";
                case EsqlDataType.Decimal255255:
                    return "DECIMAL(255,255)";
                case EsqlDataType.Decimal25532:
                    return "DECIMAL(255,32)";
                case EsqlDataType.Decimal25564:
                    return "DECIMAL(255,64)";
                case EsqlDataType.Decimal32128:
                    return "DECIMAL(32,128)";
                case EsqlDataType.Decimal3216:
                    return "DECIMAL(32,16)";
                case EsqlDataType.Decimal32255:
                    return "DECIMAL(32,255)";
                case EsqlDataType.Decimal3232:
                    return "DECIMAL(32,32)";
                case EsqlDataType.Decimal3264:
                    return "DECIMAL(32,64)";
                case EsqlDataType.Decimal64128:
                    return "DECIMAL(64,128)";
                case EsqlDataType.Decimal6416:
                    return "DECIMAL(64,16)";
                case EsqlDataType.Decimal64255:
                    return "DECIMAL(64,255)";
                case EsqlDataType.Decimal6432:
                    return "DECIMAL(64,32)";
                case EsqlDataType.Decimal6464:
                    return "DECIMAL(64,64)";
                case EsqlDataType.Double12810:
                    return "DOUBLE(128,10)";
                case EsqlDataType.Double12820:
                    return "DOUBLE(128,20)";
                case EsqlDataType.Double12830:
                    return "DOUBLE(128,30)";
                case EsqlDataType.Double1284:
                    return "DOUBLE(128,4)";
                case EsqlDataType.Double12840:
                    return "DOUBLE(128,40)";
                case EsqlDataType.Double12850:
                    return "DOUBLE(128,50)";
                case EsqlDataType.Double12853:
                    return "DOUBLE(128,53)";
                case EsqlDataType.Double1610:
                    return "DOUBLE(16,10)";
                case EsqlDataType.Double1620:
                    return "DOUBLE(16,20)";
                case EsqlDataType.Double1630:
                    return "DOUBLE(16,30)";
                case EsqlDataType.Double164:
                    return "DOUBLE(16,4)";
                case EsqlDataType.Double1640:
                    return "DOUBLE(16,40)";
                case EsqlDataType.Double1650:
                    return "DOUBLE(16,50)";
                case EsqlDataType.Double1653:
                    return "DOUBLE(16,53)";
                case EsqlDataType.Double25510:
                    return "DOUBLE(255,10)";
                case EsqlDataType.Double25520:
                    return "DOUBLE(255,20)";
                case EsqlDataType.Double25530:
                    return "DOUBLE(255,30)";
                case EsqlDataType.Double2554:
                    return "DOUBLE(255,4)";
                case EsqlDataType.Double25540:
                    return "DOUBLE(255,40)";
                case EsqlDataType.Double25550:
                    return "DOUBLE(255,50)";
                case EsqlDataType.Double25553:
                    return "DOUBLE(255,53)";
                case EsqlDataType.Double3210:
                    return "DOUBLE(32,10)";
                case EsqlDataType.Double3220:
                    return "DOUBLE(32,20)";
                case EsqlDataType.Double3230:
                    return "DOUBLE(32,30)";
                case EsqlDataType.Double324:
                    return "DOUBLE(32,4)";
                case EsqlDataType.Double3240:
                    return "DOUBLE(32,40)";
                case EsqlDataType.Double3250:
                    return "DOUBLE(32,50)";
                case EsqlDataType.Double3253:
                    return "DOUBLE(32,53)";
                case EsqlDataType.Double6410:
                    return "DOUBLE(64,10)";
                case EsqlDataType.Double6420:
                    return "DOUBLE(64,20)";
                case EsqlDataType.Double6430:
                    return "DOUBLE(64,30)";
                case EsqlDataType.Double644:
                    return "DOUBLE(64,4)";
                case EsqlDataType.Double6440:
                    return "DOUBLE(64,40)";
                case EsqlDataType.Double6450:
                    return "DOUBLE(64,50)";
                case EsqlDataType.Double6453:
                    return "DOUBLE(64,53)";
                case EsqlDataType.Enum:
                    return "ENUM";
                case EsqlDataType.Float1010:
                    return "FLOAT(10,10)";
                case EsqlDataType.Float102:
                    return "FLOAT(10,2)";
                case EsqlDataType.Float1020:
                    return "FLOAT(10,20)";
                case EsqlDataType.Float1024:
                    return "FLOAT(10,24)";
                case EsqlDataType.Float12810:
                    return "FLOAT(128,10)";
                case EsqlDataType.Float1282:
                    return "FLOAT(128,2)";
                case EsqlDataType.Float12820:
                    return "FLOAT(128,20)";
                case EsqlDataType.Float12824:
                    return "FLOAT(128,24)";
                case EsqlDataType.Float25510:
                    return "FLOAT(255,10)";
                case EsqlDataType.Float2552:
                    return "FLOAT(255,2)";
                case EsqlDataType.Float25520:
                    return "FLOAT(255,20)";
                case EsqlDataType.Float25524:
                    return "FLOAT(255,24)";
                case EsqlDataType.Float3210:
                    return "FLOAT(32,10)";
                case EsqlDataType.Float322:
                    return "FLOAT(32,2)";
                case EsqlDataType.Float3220:
                    return "FLOAT(32,20)";
                case EsqlDataType.Float3224:
                    return "FLOAT(32,24)";
                case EsqlDataType.Float6410:
                    return "FLOAT(64,10)";
                case EsqlDataType.Float642:
                    return "FLOAT(64,2)";
                case EsqlDataType.Float6420:
                    return "FLOAT(64,20)";
                case EsqlDataType.Float6424:
                    return "FLOAT(64,24)";
                case EsqlDataType.Int:
                    return "INT";
                case EsqlDataType.Longtext:
                    return "LONGTEXT";
                case EsqlDataType.Mediumint:
                    return "MEDIUMINT";
                case EsqlDataType.Mediumtext:
                    return "MEDIUMTEXT";
                case EsqlDataType.Smallint:
                    return "SMALLINT";
                case EsqlDataType.Text:
                    return "TEXT";
                case EsqlDataType.Time:
                    return "TIME";
                case EsqlDataType.Timestamp:
                    return "TIMESTAMP";
                case EsqlDataType.Tinyint:
                    return "TINYINT";
                case EsqlDataType.Tinytext:
                    return "TINYTEXT";
                case EsqlDataType.Varchar128:
                    return "VARCHAR(128)";
                case EsqlDataType.Varchar16:
                    return "VARCHAR(16)";
                case EsqlDataType.Varchar255:
                    return "VARCHAR(255)";
                case EsqlDataType.Varchar32:
                    return "VARCHAR(32)";
                case EsqlDataType.Varchar64:
                    return "VARCHAR(64)";
                default:
                    return "TEXT";
            }
        }

        private static void RunAsync()
        {
            while (PointBlankEnvironment.Running)
            {
                while (_asyncCommands.Count > 0)
                {
                    AsyncCommand command = _asyncCommands.Dequeue();

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

                _asyncCommands.Enqueue(new AsyncCommand(Command));
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

                _asyncCommands.Enqueue(new AsyncCommand(Command, callback, behaviour));
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
        public bool CreateTable(string tableName, Dictionary<string, EsqlDataType> columns)
        {
            string cols = "id INT(16) UNSIGNED AUTO_INCREMENT PRIMARY KEY";

            foreach (KeyValuePair<string, EsqlDataType> kvp in columns)
                cols += "," + kvp.Key + DataTypeToString(kvp.Value);

            return SendCommand("CREATE TABLE " + tableName + " (" + cols + ");");
        }

        /// <summary>
        /// Creates a table on the SQL server using async
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columns">The columns of the table</param>
        public void CreateTableAsync(string tableName, Dictionary<string, EsqlDataType> columns)
        {
            string cols = "id INT(16) UNSIGNED AUTO_INCREMENT PRIMARY KEY";

            foreach (KeyValuePair<string, EsqlDataType> kvp in columns)
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

            public AsyncCommand(SqlCommand command, Action<SqlDataReader> callBack = null, CommandBehavior behaviour = CommandBehavior.Default)
            {
                this.Command = command;
                this.CallBack = callBack;
                this.Behaviour = behaviour;
            }
        }
        #endregion
    }
}
