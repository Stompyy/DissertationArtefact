using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class Debug
    {
        /*
         * Debug tool - Following a long winded initial data collection period, 3/164 .dem files were discovered to be misnamed
         * as de_Inferno map, and actually contained other map data. This function is required to find the map names from the .dem files
         */
        public static void WriteAllMapNamesToCSVFile()
        {
            // Data structure holds the game name and it's map name
            Dictionary<string, string> mapNameDictionary = new Dictionary<string, string>();

            // Displays an OpenFileDialog so the user can select .dem files.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Demo files|*.dem";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select .dem files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .dem file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Look at each file selected
                foreach (string fileName in openFileDialog.FileNames)
                {
                    // Static function returns the map name for the .dem file
                    string mapName = RawDataParser.GetMapName(fileName);

                    // Add to the dictionary
                    mapNameDictionary.Add(fileName, mapName);
                }
            }

            // Write the dictionary out to a .csv file

            // Generic output file name
            string outputFileName = "mapNameDict.csv";
            // and open it. 
            StreamWriter outputStream = new StreamWriter(outputFileName);

            // This could be written to an output stream in the above foreach loop, but have chosen to keep the data collection sparate to the
            // data writing. The .csv output is just for a user to see the output and could be done in a message box or similar for equal effect.
            foreach (string game in mapNameDictionary.Keys)
            {
                // Write the contents of the dictionary to the output stream
                outputStream.WriteLine(game + "," + mapNameDictionary[game]);
            }

            // Finally close the stream
            outputStream.Close();

            // Success message
            MessageBox.Show("Finished mapNameDictFile.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
