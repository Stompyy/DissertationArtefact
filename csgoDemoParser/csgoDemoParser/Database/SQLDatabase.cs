using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if TARGET_LINUX
using Mono.Data.Sqlite;
using sqliteConnection  = Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand = Mono.Data.Sqlite.SqliteCommand;
using sqliteDataReader = Mono.Data.Sqlite.SqliteDataReader;
#endif

#if TARGET_WINDOWS
using System.Data.SQLite;
using sqliteConnection = System.Data.SQLite.SQLiteConnection;
using sqliteCommand = System.Data.SQLite.SQLiteCommand;
using sqliteDataReader = System.Data.SQLite.SQLiteDataReader;
using System.Data.SqlClient;
#endif

namespace csgoDemoParser
{
    /*
     * Database class for the database that holds all the game's tables
     */
    public class SQLDatabase
    {
        String m_DataBaseName;
        sqliteConnection m_Connection;
        List<SQLTable> m_TableList;

        /*
         * Constructor creates a new database
         */
        public SQLDatabase(String dataBaseName)
        {
            m_DataBaseName = dataBaseName;
            m_TableList = new List<SQLTable>();

            /*
            if (CheckDatabaseExists())
            {

            }
            */

            CreateNew();
        }

        /*
         * 
         */
        public static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        /*
         * Creates new database
         */
        public void CreateNew()
        {
            try
            {
                // Creates database
                sqliteConnection.CreateFile(m_DataBaseName);

                m_Connection = new sqliteConnection("Data Source=" + m_DataBaseName + ";Version=3;FailIfMissing=True");
                m_Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create DB failed: " + ex);
            }
        }

        /*
         * Opens the connection
         */
        public void Open()
        {
            try
            {
                m_Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open existing DB failed: " + ex);
            }
        }

        /*
         * Creates, adds, and returns a reference to the master table
         */
        public MasterTable addMasterTable(String tableName, string tableColumns)
        {
            MasterTable newTable = new MasterTable(m_Connection, tableName, tableColumns);
            m_TableList.Add(newTable);
            return newTable;
        }
    }
}
