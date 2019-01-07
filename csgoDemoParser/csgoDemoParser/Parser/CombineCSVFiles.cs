using System;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class CombineCSVFiles
    {
        /*
         * Used to combine all .csv files into one .csv file
         */
        public static void SelectAndCombineMultipleCSVFilesIntoMasterCSV()
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select path.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Create the file and write the headers
                StreamWriter streamWriter = CreateNewCSVFile("master");

                // For each file write the contents to the newly created file
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        // Add the contents of the fileName to the streamWriter
                        AddNewCSVFile(fileName, streamWriter);
                    }
                    catch (Exception ex)
                    {
                        // Else failed message
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                
                // Close the stream
                streamWriter.Close();

                // Success message
                MessageBox.Show("Finished parsing path#.csv files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Igores the header line and copies all other data to the streamWriter
         */
        private static void AddNewCSVFile(string fileName, StreamWriter streamWriter)
        {
            using (StreamReader stringReader = new StreamReader(fileName))
            {
                // First line is the column headers - ignore
                string headerLine = stringReader.ReadLine();

                string currentLine;

                // CurrentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    // Write the line
                    streamWriter.WriteLine(currentLine);
                }
            }
        }

        /*
         * Creates, opens, and returns a new .csv file with name fileName
         */
        private static StreamWriter CreateNewCSVFile(string fileName)
        {
            // Create the fileName. Use the origonal fileName without extension + position in the dictionary as an arbitrary fileName distinguisher
            string outputFileName = fileName + ".csv";
            // and open it. 
            StreamWriter outputStream = new StreamWriter(outputFileName);

            // Write the column headers
            outputStream.WriteLine(CSVWriter.GenerateCSVHeader("de_Inferno"));

            return outputStream;
        }
    }
}
