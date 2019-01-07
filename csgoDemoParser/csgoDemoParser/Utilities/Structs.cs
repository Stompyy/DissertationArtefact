
namespace csgoDemoParser
{
    /*
     * Useful structs for holding specific player information
     */
    public class PlayerNamePosition
    {
        public string Name { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        /*
         * Constructor
         */
        public PlayerNamePosition(string name, double x, double y, double z)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    /*
     * Useful structs for holding specific player information
     */
    public class PlayerNamePositionVelocity
    {
        public string Name { get; set; }

        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }

        public double VelX { get; set; }
        public double VelY { get; set; }
        public double VelZ { get; set; }

        /*
         * Constructor
         */
        public PlayerNamePositionVelocity(string name, double posX, double posY, double posZ, double velX = 0.0, double velY = 0.0, double velZ = 0.0)
        {
            this.Name = name;
            this.PosX = posX;
            this.PosY = posY;
            this.PosZ = posZ;
            this.VelX = velX;
            this.VelY = velY;
            this.VelZ = velZ;
        }
    }
}
