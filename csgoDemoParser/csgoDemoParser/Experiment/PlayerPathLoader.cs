using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class PlayerPathLoader :  IDisposable
    {
        // The positions of these values in a comma seperated split list from a playerPath.csv file ReadLine()
        const int listPositionX = 2;
        const int listPositionY = 3;
        const int listPositionZ = 4;

        // The name of the path being looked at
        public string pathName;

        // The name of the team this player belongs to
        public string teamName;

        // The data structure used to hold a player Path. Where the int key is the frame number, and the string[] value is the parsed player information at that frame
        public Dictionary<int, string[]> pathDictionary;

        /*
         * Constructor
         */
        public PlayerPathLoader()
        {
            // Store the frame against the csv line
            pathDictionary = new Dictionary<int, string[]>();
        }

        /*
         * Allows a user to select a number of playerPath.csv files, randomly selects and returns the first non zero
         * length path found in the list, else returns false
         */
        public bool ChooseValidPath()
        {
            // Initialise to false
            bool validPathChosen = false;
            
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select playerPath#.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Shuffle fileNames list
                string[] potentialPaths = openFileDialog.FileNames;
                new Random().Shuffle(potentialPaths);
                
                // Go through the shuffled list, LoadCSVPath until returns a non-zero length path
                while (!validPathChosen)
                {
                    foreach (string fileName in potentialPaths)
                    {
                        // Returns false if zero length path
                        if (LoadCSVPath(fileName))
                        {
                            // If valid path found, break
                            validPathChosen = true;
                            pathName = fileName;
                            break;
                        }
                    }

                    // If no valid paths selected, fail
                    if (!validPathChosen)
                    {
                        // Fail message
                        MessageBox.Show("No non-zero length paths selected.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return false;
                    }
                }

                // Else show success message
                MessageBox.Show("Potential path successfully chosen.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return true;
            }
            return false;
        }

        /*
         * Necessary for IDisposable element
         */
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /*
         * Load the csv path file into memory
         */
        public bool LoadCSVPath(string fileName)
        {
            pathName = fileName;

            using (StreamReader stringReader = new StreamReader(fileName))
            {
                // Load the pathfile.csv into memory
                int frame = 0;

                // First line is the column headers - ignore
                string currentLine = stringReader.ReadLine();

                // To get the team name we do one pass, setting the team name variable
                if ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Split the .csv line
                    string[] splitCurrentLine = currentLine.Split(',');

                    // Set the team name from the frame data
                    teamName = splitCurrentLine[1];

                    // Populate the dictionary
                    pathDictionary.Add(frame, splitCurrentLine);

                    // Increment the frame number
                    frame++;
                }

                // ReadLine() returns null at the end of the file
                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Split the .csv line
                    string[] splitCurrentLine = currentLine.Split(',');

                    // Populate the dictionary
                    pathDictionary.Add(frame, splitCurrentLine);

                    // Increment the frame number
                    frame++;
                }

                // Now check if it is a zero distance path. Do each value at a time to allow an early exit
                // Check first dictionary value against last value
                if (pathDictionary[0][listPositionX] == pathDictionary[pathDictionary.Count - 1][listPositionX])
                {
                    if (pathDictionary[0][listPositionY] == pathDictionary[pathDictionary.Count - 1][listPositionY])
                    {
                        if (pathDictionary[0][listPositionZ] == pathDictionary[pathDictionary.Count - 1][listPositionZ])
                        {
                            // If last position is identical to start position then we presume that the path has no length
                            // Show path irrelevant message
                            MessageBox.Show("Path: " + fileName + " travels no distance. Discard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return false;
                        }
                    }
                }

                // Success message
                MessageBox.Show("Finished importing path data csv file. \n Random path: " + fileName + " chosen.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return true;
            }
        }
    }

    /*
     * Randomly shuffles an array
     * Found at https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
     */
    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
