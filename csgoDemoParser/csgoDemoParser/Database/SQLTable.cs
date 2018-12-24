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
            m_Connection = connection;
            m_TableName = tableName;
            m_TableColumns = tableColumns;

            sqliteCommand command = new sqliteCommand("create table " + m_TableName + " (" + m_TableColumns + ")", m_Connection);

            command.Parameters.Add("@name", System.Data.DbType.String).Value = m_TableName;
            command.ExecuteNonQuery();
        }

        /*
         * 
         */
        public SQLTable(string fileName, string tableName)
        {
            m_TableName = tableName;

            m_Connection = new sqliteConnection("Data Source=" + fileName + ";Version=3;FailIfMissing=True");
            m_Connection.Open();

            MessageBox.Show("Successfully connected.");
        }

        /*
         * Returns the name of the table
         */
        public String getName()
        {
            return m_TableName;
        }

        /*
         * Returns true if the query exists somewhere in the field in this table
         */
        public bool queryExists(String query, String field)
        {
            sqliteCommand command = new sqliteCommand("select * from " + m_TableName + " where " + field + " = @query", m_Connection);

            command.Parameters.Add("@query", System.Data.DbType.String).Value = query;
            command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

            sqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * Returns true if the query exists in the field in this table for entry name in column Name
         */
        public bool queryOtherFieldFromName(String name, String query, String field)
        {
            sqliteCommand command = new sqliteCommand("select " + field + " from " + m_TableName + " where name = @name", m_Connection);

            command.Parameters.Add("@name", System.Data.DbType.String).Value = name;
            command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

            sqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[field].ToString() == query)
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Returns the string found at the field column for 'name' name
         */
        public String getStringFieldFromName(String name, String field)
        {
            sqliteCommand command = new sqliteCommand("select " + field + " from " + m_TableName + " where name = @name", m_Connection);

            command.Parameters.Add("@name", System.Data.DbType.String).Value = name;
            command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

            sqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                String Debug = reader[field].ToString();
                return reader[field].ToString();
            }
            return null;
        }

        /*
         * Returns the int found at the field column for 'name' name
         */
        public int getIntFieldFromName(String name, String field)
        {
            sqliteCommand command = new sqliteCommand("select " + field + " from " + m_TableName + " where name = @name", m_Connection);

            command.Parameters.Add("@name", System.Data.DbType.String).Value = name;
            command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

            sqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (int.TryParse(reader[field].ToString(), out int j))
                    {
                        return j;
                    }
                }
            }
            return -1;
        }

        /*
         * Returns the string found at the field column for 'id' id
         */
        public String getStringFieldFromId(int id, String field)
        {
            sqliteCommand command = new sqliteCommand("select " + field + " from " + m_TableName + " where id = @id", m_Connection);

            command.Parameters.Add("@id", System.Data.DbType.UInt32).Value = id;
            command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

            sqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                return reader[field].ToString();
            }
            return null;
        }

        /*
         * Returns all the names of entrys where the field == query
         */
        public List<String> getNamesFromField(String field, String query)
        {
            List<String> returnList = new List<string>();

            try
            {
                sqliteCommand command = new sqliteCommand("select name from " + m_TableName + " where " + field + " = @query", m_Connection);

                command.Parameters.Add("@query", System.Data.DbType.String).Value = query;
                command.Parameters.Add("@field", System.Data.DbType.String).Value = field;

                sqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    returnList.Add(reader["name"].ToString());
                }
            }
            catch { }

            return returnList;
        }

        /*
         * Returns all the names of entrys where the field == query for two queries
         */
        public List<String> getNamesFromTwoFields(String fieldOne, String queryOne, String fieldTwo, String queryTwo)
        {
            List<String> returnList = new List<string>();

            try
            {
                sqliteCommand command = new sqliteCommand("select name from " + m_TableName + " where " + fieldOne + " = @query1 and " + fieldTwo + " = @query2", m_Connection);

                command.Parameters.Add("@query1", System.Data.DbType.String).Value = queryOne;
                command.Parameters.Add("@query2", System.Data.DbType.String).Value = queryTwo;
                command.Parameters.Add("@field1", System.Data.DbType.String).Value = fieldOne;
                command.Parameters.Add("@field2", System.Data.DbType.String).Value = fieldTwo;

                sqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    returnList.Add(reader["name"].ToString());
                }
            }
            catch { }
            return returnList;
        }

        /*
         * Returns all the names of entrys where the field == query for two queries
         */
        public List<PlayerNamePosition> GetPlayerNamePositionDataBetweenConstraints(double minX, double maxX, double minY, double maxY)
        {
            List<PlayerNamePosition> returnList = new List<PlayerNamePosition>();

            try
            {
                sqliteCommand command = new sqliteCommand("select * from " + m_TableName + " where positionX > @minX and positionX < @maxX and positionY > @minY and positionY < @maxY", m_Connection);

                command.Parameters.Add("@minX", System.Data.DbType.Double).Value = minX;
                command.Parameters.Add("@maxX", System.Data.DbType.Double).Value = maxX;
                command.Parameters.Add("@minY", System.Data.DbType.Double).Value = minY;
                command.Parameters.Add("@maxY", System.Data.DbType.Double).Value = maxY;

                sqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    double x, y, z;
                    double.TryParse(reader["positionX"].ToString(), out x);
                    double.TryParse(reader["positionY"].ToString(), out y);
                    double.TryParse(reader["positionZ"].ToString(), out z);
                    returnList.Add(new PlayerNamePosition(reader["name"].ToString(), x, y, z));
                }
            }
            catch
            {

            }
            return returnList;
        }

        /*
         * 
         */
        public double[] GetMaxMin()
        {
            double[] returnList = new double[] { };

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
                
                returnList = new double[] { minX, maxX, minY, maxY };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // For master table, returns -2859, 3323.807, -2480, 3600
            return returnList;
        }

        /*
         * Overridden in derived classes to exactly match the parameters to the specific table's fields
         */
        public virtual void AddEntry(string csvLine) { }

        /*
         * 
         */
        public virtual void BulkInsert(StreamReader stringReader) { }
    }

    /*
     * 
     */
    public class MasterTable : SQLTable
    {
        /*
        // Found by performing a min max query on master table, returns -2859, 3323.807, -2480, 3600
        const double minimumXValue = -2859.0;
        const double maximumXValue = 3323.807;
        const double minimumYValue = -2480.0;
        const double maximumYValue = 3600.0;
        */

        // Constructor for creating a new database table
        public MasterTable(sqliteConnection connection, String tableName, String tableColumns) : base(connection, tableName, tableColumns) { }

        // Constructor for referencing an existing database table
        public MasterTable(string fileName, string tableName) : base(fileName, tableName) { }

        /*
         * Specific parameters for this table's fields and their data types
         */
        public override void AddEntry(string csvLine)
        {
            string[] splitString = csvLine.Split(',');

            try
            {
                sqliteCommand command = new sqliteCommand("insert into " + m_TableName +
                    " values (@name, @team, @positionX, @positionY, @positionZ, @activeWeapon, @hp, @armour, @velocityX, @velocityY, @velocityZ, @isDucking, @hasHelmet, @hasDefuseKit, @viewDirectionX, @viewDirectionY)", m_Connection);

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

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add: " + splitString[0] + " to DB " + ex);
            }
        }

        /*
         * 
         */
        public override void BulkInsert(StreamReader stringReader)
        {
            using (var command = new SQLiteCommand(m_Connection))
            {
                using (var transaction = m_Connection.BeginTransaction())
                {
                    string currentLine;

                    // 100,000 inserts
                    while ((currentLine = stringReader.ReadLine()) != null )// && count++ < 100000)
                    {
                        try 
                        {
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

                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            // Else failed message
                            //MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }

                        transaction.Commit();


                }
            }

        //    m_Connection.Close();

            // Success message
            MessageBox.Show("Finished datatable.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * 
         */
        public Vector[,] SegmentDatabaseIntoArray(int segments, int x)
        {
            Vector[,] returnArray = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions]; 

            //for (int x = 0; x < segments; x++)//(double x = minimumXValue; x < maximumXValue; x += deltaX)
            {
                for (int y = 0; y < segments; y++)//(double y = minimumYValue; y < maximumYValue; y += deltaY)
                {
                    try
                    {
                        sqliteCommand command = new sqliteCommand("select * from " + m_TableName + " where positionX > @minX and positionX < @maxX and positionY > @minY and positionY < @maxY", m_Connection);

                        command.Parameters.Add("@minX", System.Data.DbType.Double).Value = InfernoLevelData.minimumXValue + ((double)x * InfernoLevelData.deltaX);
                        command.Parameters.Add("@maxX", System.Data.DbType.Double).Value = InfernoLevelData.minimumXValue + ((double)(x+1) * InfernoLevelData.deltaX);
                        command.Parameters.Add("@minY", System.Data.DbType.Double).Value = InfernoLevelData.minimumYValue + ((double)y * InfernoLevelData.deltaY);
                        command.Parameters.Add("@maxY", System.Data.DbType.Double).Value = InfernoLevelData.minimumYValue + ((double)(y + 1) * InfernoLevelData.deltaY);

                        sqliteDataReader reader = command.ExecuteReader();

                        int count = 0;
                        double totalVelX = 0.0,
                            totalVelY = 0.0,
                            totalVelZ = 0.0;

                        while (reader.Read())
                        {
                            count++;

                        //    double.TryParse(reader["positionX"].ToString(), out double positionX);
                        //    double.TryParse(reader["positionY"].ToString(), out double positionY);
                        //    double.TryParse(reader["positionZ"].ToString(), out double positionZ);
                            double.TryParse(reader["velocityX"].ToString(), out double velocityX);
                            double.TryParse(reader["velocityY"].ToString(), out double velocityY);
                            double.TryParse(reader["velocityZ"].ToString(), out double velocityZ);

                            totalVelX += velocityX;
                            totalVelY += velocityY;
                            totalVelZ += velocityZ;
                        }

                        if (count != 0)
                        {
                            returnArray[x, y] = new Vector(
                                totalVelX / count,
                                totalVelY / count,
                                totalVelZ / count);
                        }
                        else
                        {
                            returnArray[x, y] = new Vector(0.0f);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fuck: " + ex.ToString());
                    }
                }
            }

            return returnArray;
        }
    }
}
