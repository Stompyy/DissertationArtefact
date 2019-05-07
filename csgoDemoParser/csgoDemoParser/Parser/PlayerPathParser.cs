using System;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Handles parsing path data from player data
     */
    class PlayerPathParser
    {
        // The positions of these values in a comma seperated split list from a playerPath.csv file ReadLine()
        const int listPositionX = 2;
        const int listPositionY = 3;
        const int listPositionZ = 4;

        string[] splitFileName;
        string headerLine;
        StreamWriter outputStream;
        int currentPath;

        // Describes an unnacceptable step distance for a player's movement.
        // Any distance greater than this is decided to describe a respawn event or reposition between matches
        // Generously decided, as the max player speed is 300 units/second and the replay is parsed at 60 frames/second
        // Max should therefore be 300/60 = 5. This generosity may help parse falling data or missing frames in the replay file
        // Actual in game distance is equivalent to 95.25cm https://developer.valvesoftware.com/wiki/Dimensions
        const double maxStepAllowed = 50.0;

        /*
         * Allows the user to select (multiple) player.csv files and parse into separate path files
         */
        public void SelectAndParsePlayerCSVIntoPaths()
        {
            // Displays an OpenFileDialog so the user can select .csv files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select player#.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and a .csv file was selected, open it.
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // If multiple files chosen look at each file in turn
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        // Parse the file
                        ParsePlayerFileIntoPaths(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished parsing player#.csv files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Looks at each position value and compares it against the last to determine if a new path has started
         * Writes a new .csv file for each path
         */
        private void ParsePlayerFileIntoPaths(string fileName)
        {
            using (StreamReader stringReader = new StreamReader(fileName))
            {
                string currentLine;
                string[] lineItems;
                Vector lastPosition, currentPosition = new Vector();
                
                // First line is the column headers
                headerLine = stringReader.ReadLine();

                // Used to create subsequent file path names
                splitFileName = fileName.Split('.');

                currentPath = 0;

                // Create and use a new file stream
                InitNewPathFile();

                // First time reading the file the distance check is not needed. Do a simple write line once to init the currentPosition
                if ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Split the currentLine to look at the individual components
                    lineItems = currentLine.Split(',');

                    // Get the current position and initialise the currentPosition variable
                    currentPosition = new Vector(lineItems[listPositionX], lineItems[listPositionY], lineItems[listPositionZ]);

                    // Write the current line to the current output stream
                    outputStream.WriteLine(currentLine);
                }
                
                // CurrentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Split the currentLine to look at the individual components
                    lineItems = currentLine.Split(',');

                    // Update the last position
                    lastPosition = currentPosition;

                    // Get the current position
                    currentPosition = new Vector(lineItems[listPositionX], lineItems[listPositionY], lineItems[listPositionZ]);

                    // Distance check. Ensures each path is kept seperate despite jumps accounting for death/respawn, end of match etc
                    if ((currentPosition - lastPosition).Length > maxStepAllowed)
                    {
                        // Close the current file
                        outputStream.Close();

                        // Increment the currentPath for use in the new file name
                        currentPath++;

                        // Create and use a new file stream
                        InitNewPathFile();
                    }

                    // Write the current line to the current output stream
                    outputStream.WriteLine(currentLine);
                }

                // Close the last file
                outputStream.Close();
            }
        }

        /*
         * Appropriately names and creates a new path file
         */
        private void InitNewPathFile()
        {
            // Create the fileName. Use the original fileName + the player# + path# as an arbitrary fileName distinguisher
            string outputFileName = splitFileName[0] + "." + splitFileName[1] + ".path" + currentPath + ".csv";

            // Open it
            outputStream = new StreamWriter(outputFileName);

            // Write the column headers
            outputStream.WriteLine(headerLine);
        }
    }
}
