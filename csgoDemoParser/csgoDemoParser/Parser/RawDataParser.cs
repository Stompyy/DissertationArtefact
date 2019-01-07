using System;
using System.IO;
using DemoInfo;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Handles parsing Counter Strike : Global Offensive (csgo) replay files (.dem) using the DemoParser nuget plugin
     */
    class RawDataParser
    {
        /*
         * Allows the user to select .dem csgo replay files to read and parse into a .csv file containg all player's details for every game tick.
         */
        public static void SelectAndParseDemFile()
        {
            // Displays an OpenFileDialog so the user can select a .dem file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Demo files|*.dem";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select .dem file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .dem file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // If multiple files are selected, look at each fileName in turn
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        // Open the current fileName file and parse to .csv file
                        ParseDemFile(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished parsing .dem file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Opens .dem file, then uses the DemoParser plugin to read every frame's information, writing each alive player's details to an output .csv file
         */
        public static void ParseDemFile(string fileName)
        {
            // By using "using" we make sure that the fileStream is properly disposed else will cause memory leak
            using (Stream fileStream = File.OpenRead(fileName))
            {
                // Same using method for disposing of DemoParser
                using (DemoParser parser = new DemoParser(fileStream))
                {
                    // Parses the first few hundred bytes of the demo. Prepares the parser for reading
                    parser.ParseHeader();

                    // Appropriately names an output file stream according to the fileName
                    string outputFileName = fileName.Split('.')[0] + ".rawdata.csv";
                    // and open it. 
                    var outputStream = new StreamWriter(outputFileName);

                    // Writes a csv header line for readability. Describes the contents of each column
                    outputStream.WriteLine(CSVWriter.GenerateCSVHeader(parser.Map));

                    // Starting from the initial tick of the parser, look at each player in that frame
                    do
                    {
                        // Look at each player by name
                        foreach (Player player in parser.PlayingParticipants)
                        {
                            // Only look at that player's data if they are alive in the game
                            if (player.IsAlive)
                            {
                                // Function writes the player's details to the output stream
                                CSVWriter.WritePlayerDetails(outputStream, player);
                            }
                        }

                    // ParseNextTick() returns false at the end of the .dem file
                    } while (parser.ParseNextTick());

                    // Close the output stream
                    outputStream.Close();

                    // Success message
                    MessageBox.Show("Finished parsing .dem file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        /*
         * Returns the name of the map from the demo replay file. Used as a sanity check and debugging by naming and shaming any incorrectly named replay files
         * included in the demo files...
         */
        public static string GetMapName(string fileName)
        {
            string mapName = "";

            // By using "using" we make sure that the fileStream is properly disposed else will cause memory leak
            using (Stream fileStream = File.OpenRead(fileName))
            {
                // Same using method for disposing of DemoParser
                using (DemoParser parser = new DemoParser(fileStream))
                {
                    // Parses the first few hundred bytes of the demo. Prepares the parser for reading
                    parser.ParseHeader();

                    // Gets the map name from the parser
                    mapName = parser.Map;
                }

                // Close the current Stream
                fileStream.Close();
            }
            
            // Returns the map name
            return mapName;
        }
    }
}
