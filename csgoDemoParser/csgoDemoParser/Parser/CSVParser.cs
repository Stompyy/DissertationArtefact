using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class CSVParser
    {
        /*
         * 
         */
        public void ParseCSVFileIntoPlayers(string fileName)
        {
            using (StreamReader stringReader = new StreamReader(fileName))
            {
                Dictionary<string, StreamWriter> playerStreamMap = new Dictionary<string, StreamWriter>();

                // First line is the column headers
                string headerLine = stringReader.ReadLine();

                string currentLine, playerName;
                string[] lineItems;
                
                // CurrentLine will be null when the StreamReader reaches the end of file
                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    lineItems = currentLine.Split(',');
                    playerName = lineItems[0];

                    if (playerStreamMap.ContainsKey(playerName))
                    {
                        playerStreamMap[playerName].WriteLine(currentLine);
                    }
                    else
                    {
                        // Create the fileName. Use the origonal fileName without extension + position in the dictionary as an arbitrary fileName distinguisher
                        string outputFileName = fileName.Split('.')[0] + ".player" + playerStreamMap.Count + ".csv";
                        // and open it. 
                        StreamWriter outputStream = new StreamWriter(outputFileName);

                        // Write the column headers
                        outputStream.WriteLine(headerLine);

                        // Add to dictionary
                        playerStreamMap.Add(playerName, outputStream);
                    }
                }

                foreach (StreamWriter stream in playerStreamMap.Values)
                {
                    stream.Close();
                }

                MessageBox.Show("Finished parsing .csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
