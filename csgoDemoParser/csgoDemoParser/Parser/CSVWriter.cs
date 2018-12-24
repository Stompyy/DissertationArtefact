using System;
using System.IO;
using DemoInfo;

namespace csgoDemoParser
{
    /*
     * 
     */
    class CSVWriter
    {
        /*
         * 
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
         * 
         */
        public static void PrintPlayerDetails(StreamWriter outputStream, Player player)
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
            catch
            {
                try
                {
                    outputStream.WriteLine(string.Format("{0},{1},{2},{3},{4}",
                    player.Name.ToString(),
                    player.Team.ToString(),
                    player.Position.X,
                    player.Position.Y,
                    player.Position.Z
                    ));
                }
                catch (Exception e)
                {
                    outputStream.WriteLine("Parsing Error: " + e.ToString());
                }
            }
        }
    }
}
