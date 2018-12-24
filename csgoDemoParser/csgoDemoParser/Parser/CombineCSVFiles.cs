using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace csgoDemoParser
{
    class CombineCSVFiles
    {
        /*
         * 
         */
        public void AddNewCSVFile(string fileName, StreamWriter streamWriter)
        {
            using (StreamReader stringReader = new StreamReader(fileName))
            {
                // First line is the column headers
                string headerLine = stringReader.ReadLine();

                string currentLine;

                // CurrentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    streamWriter.WriteLine(currentLine);
                }
            }
        }

        /*
         * 
         */
        public StreamWriter CreateNewCSVFile(string fileName)
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
