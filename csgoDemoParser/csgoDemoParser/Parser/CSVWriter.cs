using System;
using System.IO;
using DemoInfo;

namespace csgoDemoParser
{
    class CSVWriter
    {
        /*
         * Returns the standard csv header used for all game, player, and path .csv files
         */
        public static string GenerateCSVHeader(string mapName)
        {
            return string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                "Name",
                "Team",
                "Position.X",
                "Position.Y",
                "Position.Z",
                "ActiveWeapon",
                "HP",
                "Armour",
                "Velocity.X",
                "Velocity.Y",
                "Velocity.Z",
                "IsDucking",
                "HasHelmet",
                "HasDefuseKit",
                "ViewDirectionX",
                "ViewDirectionY",
                "Map = " + mapName
            );
        }

        /*
         * Writes the player details to the output stream, comma seperated, according to the order of the header names
         */
        public static void WritePlayerDetails(StreamWriter outputStream, Player player)
        {
            try
            {
                outputStream.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                    player.Name.ToString(),
                    player.Team.ToString(),
                    player.Position.X,
                    player.Position.Y,
                    player.Position.Z,
                    player.ActiveWeapon.Weapon.ToString(),
                    player.HP.ToString(),
                    player.Armor.ToString(),
                    player.Velocity.X,
                    player.Velocity.Y,
                    player.Velocity.Z,
                    player.IsDucking.ToString(),
                    player.HasHelmet.ToString(),
                    player.HasDefuseKit.ToString(),
                    player.ViewDirectionX.ToString(),
                    player.ViewDirectionY.ToString()
                    ));
            }
            catch (Exception e)
            {
                // If some elements of the player have not been initialised yet, catch will allow the error to pass and continue
                outputStream.WriteLine("Parsing Error: " + e.ToString());
            }
        }
    }
}
