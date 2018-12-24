using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * 
     */
    class PlayerPathParser
    {
        string[] splitFileName;
        string headerLine;
        StreamWriter outputStream;
        int currentPath;

        const double maxStepAllowed = 50.0;

        /*
         * 
         */
        public void ParsePlayerFileIntoPaths(string fileName)
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
                    currentPosition = new Vector(lineItems[2], lineItems[3], lineItems[4]);

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
                    currentPosition = new Vector(lineItems[2], lineItems[3], lineItems[4]);

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
         * 
         */
        void InitNewPathFile()
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
