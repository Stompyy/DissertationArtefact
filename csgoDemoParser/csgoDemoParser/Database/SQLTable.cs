using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

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
#endif


namespace csgoDemoParser
{
    /*
     * Base class for the SQLTable derivisions
     * Contains all querying functions that create sql queries and sends them to the database
     */
    public class SQLTable
    {
        // The connection to the table
        public sqliteConnection m_Connection;

        // The table's self reference within the database
        public String m_TableName;

        // The string representation of the columns as appear in the SQL table constructing command
        String m_TableColumns;

        /*
         * Constructor
         */
        public SQLTable(sqliteConnection connection, String tableName, String tableColumns)
        {
            // Set the variable names and reference the connection
            m_Connection = connection;
            m_TableName = tableName;
            m_TableColumns = tableColumns;

            // Create the table
            sqliteCommand command = new sqliteCommand("create table " + m_TableName + " (" + m_TableColumns + ")", m_Connection);
            command.Parameters.Add("@name", System.Data.DbType.String).Value = m_TableName;
            command.ExecuteNonQuery();
        }

        /*
         * Constructor to connect to an existing database
         */
        public SQLTable(string fileName, string tableName)
        {
            // Set the table name
            m_TableName = tableName;

            // Create the connection and open it
            m_Connection = new sqliteConnection("Data Source=" + fileName + ";Version=3;FailIfMissing=True");
            m_Connection.Open();

            // Success message
            MessageBox.Show("Successfully connected.");
        }

        /*
         * Overridden in derived classes to exactly match the parameters to the specific table's fields
         */
        public virtual void AddEntry(string csvLine) { }

        /*
         * Bulk inserts are necessary when dealing with large amounts of data
         */
        public virtual void BulkInsert(StreamReader stringReader) { }
    }

    /*
     * The currently used database table type for storing the .dem replay file parsed data
     * Have derived from a base class as other table types may be deemed necessary
     */
    public class MasterTable : SQLTable
    {
        // Constructor for creating a new database table
        public MasterTable(sqliteConnection connection, String tableName, String tableColumns) : base(connection, tableName, tableColumns) { }

        // Constructor for referencing an existing database table
        public MasterTable(string fileName, string tableName) : base(fileName, tableName) { }

        /*
         * Bulk inserts are necessary when inserting large amounts of data into a database table at once
         * Single inserts are a very inefficient method! I found this out the hard way.
         */
        public override void BulkInsert(StreamReader stringReader)
        {
            // Using disposable variables clean up memory as they are completed
            // Set up the command
            using (SQLiteCommand command = new SQLiteCommand(m_Connection))
            {
                // Bulk inerts are performed through a transaction
                using (SQLiteTransaction transaction = m_Connection.BeginTransaction())
                {
                    string currentLine;

                    // The stringReader will return null at the end of the file
                    while ((currentLine = stringReader.ReadLine()) != null)
                    {
                        try
                        {
                            // Parse the currentLine into an insert command
                            string[] splitString = currentLine.Split(',');

                            command.CommandText = "insert into " + m_TableName +
                                " values (@name, @team, @positionX, @positionY, @positionZ, @activeWeapon, @hp, @armour, @velocityX, @velocityY, @velocityZ, @isDucking, @hasHelmet, @hasDefuseKit, @viewDirectionX, @viewDirectionY)";

                            command.Parameters.Add("@name",         System.Data.DbType.String).Value = splitString[0];
                            command.Parameters.Add("@team",         System.Data.DbType.String).Value = splitString[1];
                            command.Parameters.Add("@positionX",    System.Data.DbType.Double).Value = splitString[2];
                            command.Parameters.Add("@positionY",    System.Data.DbType.Double).Value = splitString[3];
                            command.Parameters.Add("@positionZ",    System.Data.DbType.Double).Value = splitString[4];
                            command.Parameters.Add("@activeWeapon", System.Data.DbType.String).Value = splitString[5];
                            command.Parameters.Add("@hp",           System.Data.DbType.Int32).Value = splitString[6];
                            command.Parameters.Add("@armour",       System.Data.DbType.Int32).Value = splitString[7];
                            command.Parameters.Add("@velocityX",    System.Data.DbType.Double).Value = splitString[8];
                            command.Parameters.Add("@velocityY",    System.Data.DbType.Double).Value = splitString[9];
                            command.Parameters.Add("@velocityZ",    System.Data.DbType.Double).Value = splitString[10];
                            command.Parameters.Add("@isDucking",    System.Data.DbType.Boolean).Value = splitString[11];
                            command.Parameters.Add("@hasHelmet",    System.Data.DbType.Boolean).Value = splitString[12];
                            command.Parameters.Add("@hasDefuseKit", System.Data.DbType.Boolean).Value = splitString[13];
                            command.Parameters.Add("@viewDirectionX", System.Data.DbType.Double).Value = splitString[14];
                            command.Parameters.Add("@viewDirectionY", System.Data.DbType.Double).Value = splitString[15];

                            // Execute the command
                            command.ExecuteNonQuery();
                        }
                        catch
                        {

                        }
                    }

                    // Execute the transaction
                    transaction.Commit();
                }
            }

            // Success message
            MessageBox.Show("Finished datatable.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * Queries the database for minimum and maximum positions from the positionX and positionY columns
         * I know this is highly coupled, but it's a single use function that is necessary
         */
        public void GetMaxMin()
        {
            try
            {
                double minX = 0.0,
                    maxX = 0.0,
                    minY = 0.0,
                    maxY = 0.0;
                sqliteCommand command;
                sqliteDataReader reader;

                command = new sqliteCommand("select min(positionX) from " + m_TableName, m_Connection);
                reader = command.ExecuteReader();
                reader.Read();
                minX = reader.GetDouble(0);

                command = new sqliteCommand("select max(positionX) from " + m_TableName, m_Connection);
                reader = command.ExecuteReader();
                reader.Read();
                maxX = reader.GetDouble(0);

                command = new sqliteCommand("select min(positionY) from " + m_TableName, m_Connection);
                reader = command.ExecuteReader();
                reader.Read();
                minY = reader.GetDouble(0);

                command = new sqliteCommand("select max(positionY) from " + m_TableName, m_Connection);
                reader = command.ExecuteReader();
                reader.Read();
                maxY = reader.GetDouble(0);

                // Show the information to the user
                MessageBox.Show("minX = " + minX + ", maxX = " + maxX + ", minY = " + minY + ", maxY = " + maxY,
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /*
         * Steps through the database, querying for position values between constraints, and returnng the average velocity values
         * 
         * Currently set up as the user friendly stepped approach, described in mainForm.cs at implementation
         */
        public Vector[,] SegmentDatabaseIntoArray(int segments, int x)
        {
            // Initialise the return array to the correct size
            Vector[,] returnArray = new Vector[segments, segments]; 

            // The next line is commented to pursue the stepped approach described in mainForm.cs
            // Uncommenting will attempt all queries and suffers from inefficient time usage for the current ~8GB database size
            //for (int x = 0; x < segments; x++)
            {
                for (int y = 0; y < segments; y++)
                {
                    try
                    {
                        // Set up the command
                        sqliteCommand command = new sqliteCommand("select * from " + m_TableName + " where positionX > @minX and positionX < @maxX and positionY > @minY and positionY < @maxY", m_Connection);

                        // The parameters are derived from the min and max values, the step amount, and the current iteration
                        command.Parameters.Add("@minX", System.Data.DbType.Double).Value = InfernoLevelData.minimumXValue + ((double)x * InfernoLevelData.subdivisionSizeX);
                        command.Parameters.Add("@maxX", System.Data.DbType.Double).Value = InfernoLevelData.minimumXValue + ((double)(x+1) * InfernoLevelData.subdivisionSizeX);
                        command.Parameters.Add("@minY", System.Data.DbType.Double).Value = InfernoLevelData.minimumYValue + ((double)y * InfernoLevelData.subdivisionSizeY);
                        command.Parameters.Add("@maxY", System.Data.DbType.Double).Value = InfernoLevelData.minimumYValue + ((double)(y + 1) * InfernoLevelData.subdivisionSizeY);

                        // Execute the command
                        sqliteDataReader reader = command.ExecuteReader();

                        // Initialise the values to zero
                        int count = 0;

                        // Running totals
                        double totalVelX = 0.0,
                            totalVelY = 0.0,
                            totalVelZ = 0.0;

                        // Look through each returned query result one by one
                        while (reader.Read())
                        {
                            // Increment the count used for the averaging
                            count++;
                            
                            // Look up the velocity values of the current item, then parse the string values into doubles
                            double.TryParse(reader["velocityX"].ToString(), out double velocityX);
                            double.TryParse(reader["velocityY"].ToString(), out double velocityY);
                            double.TryParse(reader["velocityZ"].ToString(), out double velocityZ);

                            // Add the values onto the running totals
                            totalVelX += velocityX;
                            totalVelY += velocityY;
                            totalVelZ += velocityZ;
                        }

                        // If some results were found
                        if (count != 0)
                        {
                            // Return the average 
                            returnArray[x, y] = new Vector(
                                totalVelX / count,
                                totalVelY / count,
                                totalVelZ / count);
                        }
                        else
                        {
                            // If no results at that position query, return Vector(0.0f) instead of null
                            returnArray[x, y] = new Vector(0.0f);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("This should not happen: " + ex.ToString());
                    }
                }
            }

            // Return the array
            return returnArray;
        }
    }
}
