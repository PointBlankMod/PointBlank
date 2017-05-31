using System;
using System.Data;
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
            this.Server = server; // Set the server
            this.Database = database; // Set the database
            this.Username = username; // Set the username
            this.Timeout = timeout; // Set the timeout
            this.Trusted = trusted; // Set the trusted

            Connection = new SqlConnection(string.Format("user id={0};password={1};server={2};Trusted_Connection={3};database={4};connection timeout={5}", // Create the connection
                username,
                password,
                server,
                (trusted ? "yes" : "no"),
                database,
                timeout.ToString()
            ));
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
            if (dataType == ESQLDataType.BIGINT)
                return "BIGINT";
            else if (dataType == ESQLDataType.CHAR_128)
                return "CHAR(128)";
            else if (dataType == ESQLDataType.CHAR_16)
                return "CHAR(16)";
            else if (dataType == ESQLDataType.CHAR_255)
                return "CHAR(255)";
            else if (dataType == ESQLDataType.CHAR_32)
                return "CHAR(32)";
            else if (dataType == ESQLDataType.CHAR_64)
                return "CHAR(64)";
            else if (dataType == ESQLDataType.DATE)
                return "DATE";
            else if (dataType == ESQLDataType.DATETIME)
                return "DATETIME";
            else if (dataType == ESQLDataType.DECIMAL_128_128)
                return "DECIMAL(128,128)";
            else if (dataType == ESQLDataType.DECIMAL_128_16)
                return "DECIMAL(128,16)";
            else if (dataType == ESQLDataType.DECIMAL_128_255)
                return "DECIMAL(128,255)";
            else if (dataType == ESQLDataType.DECIMAL_128_32)
                return "DECIMAL(128,32)";
            else if (dataType == ESQLDataType.DECIMAL_128_64)
                return "DECIMAL(128,64)";
            else if (dataType == ESQLDataType.DECIMAL_16_128)
                return "DECIMAL(16,128)";
            else if (dataType == ESQLDataType.DECIMAL_16_16)
                return "DECIMAL(16,16)";
            else if (dataType == ESQLDataType.DECIMAL_16_255)
                return "DECIMAL(16,255)";
            else if (dataType == ESQLDataType.DECIMAL_16_32)
                return "DECIMAL(16,32)";
            else if (dataType == ESQLDataType.DECIMAL_16_64)
                return "DECIMAL(16,64)";
            else if (dataType == ESQLDataType.DECIMAL_255_128)
                return "DECIMAL(255,128)";
            else if (dataType == ESQLDataType.DECIMAL_255_16)
                return "DECIMAL(255,16)";
            else if (dataType == ESQLDataType.DECIMAL_255_255)
                return "DECIMAL(255,255)";
            else if (dataType == ESQLDataType.DECIMAL_255_32)
                return "DECIMAL(255,32)";
            else if (dataType == ESQLDataType.DECIMAL_255_64)
                return "DECIMAL(255,64)";
            else if (dataType == ESQLDataType.DECIMAL_32_128)
                return "DECIMAL(32,128)";
            else if (dataType == ESQLDataType.DECIMAL_32_16)
                return "DECIMAL(32,16)";
            else if (dataType == ESQLDataType.DECIMAL_32_255)
                return "DECIMAL(32,255)";
            else if (dataType == ESQLDataType.DECIMAL_32_32)
                return "DECIMAL(32,32)";
            else if (dataType == ESQLDataType.DECIMAL_32_64)
                return "DECIMAL(32,64)";
            else if (dataType == ESQLDataType.DECIMAL_64_128)
                return "DECIMAL(64,128)";
            else if (dataType == ESQLDataType.DECIMAL_64_16)
                return "DECIMAL(64,16)";
            else if (dataType == ESQLDataType.DECIMAL_64_255)
                return "DECIMAL(64,255)";
            else if (dataType == ESQLDataType.DECIMAL_64_32)
                return "DECIMAL(64,32)";
            else if (dataType == ESQLDataType.DECIMAL_64_64)
                return "DECIMAL(64,64)";
            else if (dataType == ESQLDataType.DOUBLE_128_10)
                return "DOUBLE(128,10)";
            else if (dataType == ESQLDataType.DOUBLE_128_20)
                return "DOUBLE(128,20)";
            else if (dataType == ESQLDataType.DOUBLE_128_30)
                return "DOUBLE(128,30)";
            else if (dataType == ESQLDataType.DOUBLE_128_4)
                return "DOUBLE(128,4)";
            else if (dataType == ESQLDataType.DOUBLE_128_40)
                return "DOUBLE(128,40)";
            else if (dataType == ESQLDataType.DOUBLE_128_50)
                return "DOUBLE(128,50)";
            else if (dataType == ESQLDataType.DOUBLE_128_53)
                return "DOUBLE(128,53)";
            else if (dataType == ESQLDataType.DOUBLE_16_10)
                return "DOUBLE(16,10)";
            else if (dataType == ESQLDataType.DOUBLE_16_20)
                return "DOUBLE(16,20)";
            else if (dataType == ESQLDataType.DOUBLE_16_30)
                return "DOUBLE(16,30)";
            else if (dataType == ESQLDataType.DOUBLE_16_4)
                return "DOUBLE(16,4)";
            else if (dataType == ESQLDataType.DOUBLE_16_40)
                return "DOUBLE(16,40)";
            else if (dataType == ESQLDataType.DOUBLE_16_50)
                return "DOUBLE(16,50)";
            else if (dataType == ESQLDataType.DOUBLE_16_53)
                return "DOUBLE(16,53)";
            else if (dataType == ESQLDataType.DOUBLE_255_10)
                return "DOUBLE(255,10)";
            else if (dataType == ESQLDataType.DOUBLE_255_20)
                return "DOUBLE(255,20)";
            else if (dataType == ESQLDataType.DOUBLE_255_30)
                return "DOUBLE(255,30)";
            else if (dataType == ESQLDataType.DOUBLE_255_4)
                return "DOUBLE(255,4)";
            else if (dataType == ESQLDataType.DOUBLE_255_40)
                return "DOUBLE(255,40)";
            else if (dataType == ESQLDataType.DOUBLE_255_50)
                return "DOUBLE(255,50)";
            else if (dataType == ESQLDataType.DOUBLE_255_53)
                return "DOUBLE(255,53)";
            else if (dataType == ESQLDataType.DOUBLE_32_10)
                return "DOUBLE(32,10)";
            else if (dataType == ESQLDataType.DOUBLE_32_20)
                return "DOUBLE(32,20)";
            else if (dataType == ESQLDataType.DOUBLE_32_30)
                return "DOUBLE(32,30)";
            else if (dataType == ESQLDataType.DOUBLE_32_4)
                return "DOUBLE(32,4)";
            else if (dataType == ESQLDataType.DOUBLE_32_40)
                return "DOUBLE(32,40)";
            else if (dataType == ESQLDataType.DOUBLE_32_50)
                return "DOUBLE(32,50)";
            else if (dataType == ESQLDataType.DOUBLE_32_53)
                return "DOUBLE(32,53)";
            else if (dataType == ESQLDataType.DOUBLE_64_10)
                return "DOUBLE(64,10)";
            else if (dataType == ESQLDataType.DOUBLE_64_20)
                return "DOUBLE(64,20)";
            else if (dataType == ESQLDataType.DOUBLE_64_30)
                return "DOUBLE(64,30)";
            else if (dataType == ESQLDataType.DOUBLE_64_4)
                return "DOUBLE(64,4)";
            else if (dataType == ESQLDataType.DOUBLE_64_40)
                return "DOUBLE(64,40)";
            else if (dataType == ESQLDataType.DOUBLE_64_50)
                return "DOUBLE(64,50)";
            else if (dataType == ESQLDataType.DOUBLE_64_53)
                return "DOUBLE(64,53)";
            else if (dataType == ESQLDataType.ENUM)
                return "ENUM";
            else if (dataType == ESQLDataType.FLOAT_10_10)
                return "FLOAT(10,10)";
            else if (dataType == ESQLDataType.FLOAT_10_2)
                return "FLOAT(10,2)";
            else if (dataType == ESQLDataType.FLOAT_10_20)
                return "FLOAT(10,20)";
            else if (dataType == ESQLDataType.FLOAT_10_24)
                return "FLOAT(10,24)";
            else if (dataType == ESQLDataType.FLOAT_128_10)
                return "FLOAT(128,10)";
            else if (dataType == ESQLDataType.FLOAT_128_2)
                return "FLOAT(128,2)";
            else if (dataType == ESQLDataType.FLOAT_128_20)
                return "FLOAT(128,20)";
            else if (dataType == ESQLDataType.FLOAT_128_24)
                return "FLOAT(128,24)";
            else if (dataType == ESQLDataType.FLOAT_255_10)
                return "FLOAT(255,10)";
            else if (dataType == ESQLDataType.FLOAT_255_2)
                return "FLOAT(255,2)";
            else if (dataType == ESQLDataType.FLOAT_255_20)
                return "FLOAT(255,20)";
            else if (dataType == ESQLDataType.FLOAT_255_24)
                return "FLOAT(255,24)";
            else if (dataType == ESQLDataType.FLOAT_32_10)
                return "FLOAT(32,10)";
            else if (dataType == ESQLDataType.FLOAT_32_2)
                return "FLOAT(32,2)";
            else if (dataType == ESQLDataType.FLOAT_32_20)
                return "FLOAT(32,20)";
            else if (dataType == ESQLDataType.FLOAT_32_24)
                return "FLOAT(32,24)";
            else if (dataType == ESQLDataType.FLOAT_64_10)
                return "FLOAT(64,10)";
            else if (dataType == ESQLDataType.FLOAT_64_2)
                return "FLOAT(64,2)";
            else if (dataType == ESQLDataType.FLOAT_64_20)
                return "FLOAT(64,20)";
            else if (dataType == ESQLDataType.FLOAT_64_24)
                return "FLOAT(64,24)";
            else if (dataType == ESQLDataType.INT)
                return "INT";
            else if (dataType == ESQLDataType.LONGTEXT)
                return "LONGTEXT";
            else if (dataType == ESQLDataType.MEDIUMINT)
                return "MEDIUMINT";
            else if (dataType == ESQLDataType.MEDIUMTEXT)
                return "MEDIUMTEXT";
            else if (dataType == ESQLDataType.SMALLINT)
                return "SMALLINT";
            else if (dataType == ESQLDataType.TEXT)
                return "TEXT";
            else if (dataType == ESQLDataType.TIME)
                return "TIME";
            else if (dataType == ESQLDataType.TIMESTAMP)
                return "TIMESTAMP";
            else if (dataType == ESQLDataType.TINYINT)
                return "TINYINT";
            else if (dataType == ESQLDataType.TINYTEXT)
                return "TINYTEXT";
            else if (dataType == ESQLDataType.VARCHAR_128)
                return "VARCHAR(128)";
            else if (dataType == ESQLDataType.VARCHAR_16)
                return "VARCHAR(16)";
            else if (dataType == ESQLDataType.VARCHAR_255)
                return "VARCHAR(255)";
            else if (dataType == ESQLDataType.VARCHAR_32)
                return "VARCHAR(32)";
            else if (dataType == ESQLDataType.VARCHAR_64)
                return "VARCHAR(64)";
            else
                return "TEXT";
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
                return false;
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
                output = null;
                return false;
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

            foreach(KeyValuePair<string, ESQLDataType> kvp in columns)
                cols += "," + kvp.Key + DataTypeToString(kvp.Value);

            return SendCommand("CREATE TABLE " + tableName + " (" + cols + ");");
        }

        /// <summary>
        /// Deletes a table from the SQL server
        /// </summary>
        /// <param name="tableName">The name of the table to remove</param>
        /// <returns>If the table was successful</returns>
        public bool DeleteTable(string tableName)
        {
            return SendCommand("DROP TABLE " + tableName + ";");
        }

        /// <summary>
        /// Adds an entry to the table
        /// </summary>
        /// <param name="tableName">The name of the table to add the entry to</param>
        /// <param name="data">The column values to set</param>
        /// <returns>If the entry was added successfully</returns>
        public bool AddEntry(string tableName, string[] data)
        {
            Dictionary<string, string> paramaters = new Dictionary<string, string>();

            for(int i = 0; i < data.Length; i++)
                paramaters.Add("@f" + i, data[i]);

            return SendCommand("INSERT INTO " + tableName + " VALUE (" + string.Join(", ", paramaters.Keys.ToArray()) + ");", paramaters);
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

            for(int i = 0; i < data.Count; i++)
            {
                columns[i] = data.Keys.ElementAt(i);
                values[i] = "@" + data.Keys.ElementAt(i);
                paramaters.Add("@" + data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }

            return SendCommand("INSERT INTO " + tableName + " (" + string.Join(", ", columns) + ") VALUES (" + string.Join(", ", values) + ");", paramaters);
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
        /// Deletes an entry from the table
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="condition">The condition to find the correct entry</param>
        /// <param name="parameters">The option parameters for the condition</param>
        /// <returns>If the entry was deleted successfully</returns>
        public bool DeleteEntry(string tableName, string condition, Dictionary<string, string> parameters = null)
        {
            return SendCommand("DELETE FROM " + tableName + " WHERE " + condition, parameters);
        }
        #endregion
    }
}
