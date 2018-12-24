using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DemoInfo;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * 
     */
    class RawDataParser
    {
        /*
         * 
         */
        public void ParseDemFile(string fileName)
        {
            // By using "using" we make sure that the fileStream is properly disposed else will cause memory leak
            using (Stream fileStream = File.OpenRead(fileName))
            {
                using (DemoParser parser = new DemoParser(fileStream))
                {
                    parser.ParseHeader();

                    string outputFileName = fileName.Split('.')[0] + ".rawdata.csv";
                    // and open it. 
                    var outputStream = new StreamWriter(outputFileName);

                    outputStream.WriteLine(CSVWriter.GenerateCSVHeader(parser.Map));

                    do
                    {
                        foreach (var player in parser.PlayingParticipants)
                        {
                            if (player.IsAlive)
                                CSVWriter.PrintPlayerDetails(outputStream, player);
                        }

                    } while (parser.ParseNextTick());

                    outputStream.Close();

                    MessageBox.Show("Finished parsing .dem file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        /*
         * 
         */
        public string GetMapName(string fileName)
        {
            string mapName = "";

            // By using "using" we make sure that the fileStream is properly disposed else will cause memory leak
            using (Stream fileStream = File.OpenRead(fileName))
            {
                using (DemoParser parser = new DemoParser(fileStream))
                {
                    parser.ParseHeader();

                    mapName = parser.Map;
                }

                fileStream.Close();
            }

            return mapName;
        }
    }
}
