using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class PlayerPathLoader
    {
        const int listPositionX = 2;
        const int listPositionY = 3;
        const int listPositionZ = 4;

        public Dictionary<int, string[]> pathDictionary;

        public PlayerPathLoader(string fileName)
        {
            pathDictionary = new Dictionary<int, string[]>();

            LoadCSVPath(fileName);
        }

        private void LoadCSVPath(string fileName)
        {
            using (StreamReader stringReader = new StreamReader(fileName))
            {
                // Load the pathfile.csv into memory
                int frame = 0;

                // First line is the column headers - ignore
                string currentLine = stringReader.ReadLine();

                while ((currentLine = stringReader.ReadLine()) != null)
                {
                    string[] splitCurrentLine = currentLine.Split(',');

                    pathDictionary.Add(frame, splitCurrentLine);

                    frame++;
                }

                // Check if it is a zero distance path
                if (pathDictionary[0][listPositionX] == pathDictionary[pathDictionary.Count - 1][listPositionX])
                {
                    if (pathDictionary[0][listPositionY] == pathDictionary[pathDictionary.Count - 1][listPositionY])
                    {
                        if (pathDictionary[0][listPositionZ] == pathDictionary[pathDictionary.Count - 1][listPositionZ])
                        {
                            // Path irrelevant message
                            MessageBox.Show("Path: " + fileName + " travels no distance. Discard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }

                // Success message
                MessageBox.Show("Finished importing path data csv file..", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
