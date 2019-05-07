using System;

namespace csgoDemoParser
{
    /*
     * The Vector class used in this program to describe position, acceleration, and velocity direction and magnitude
     */
    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        // Default constructor
        public Vector()
        {
            this.X = 0.0f;
            this.Y = 0.0f;
            this.Z = 0.0f;
        }

        // Float constructor
        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        // Double constructor
        public Vector(double x, double y, double z)
        {
            this.X = (float)x;
            this.Y = (float)y;
            this.Z = (float)z;
        }

        // String constructor
        public Vector(string x, string y, string z)
        {
            this.X = float.Parse(x);
            this.Y = float.Parse(y);
            this.Z = float.Parse(z);
        }

        // Single float constructor
        public Vector(float a)
        {
            this.X = a;
            this.Y = a;
            this.Z = a;
        }

        // Returns the Euclidian length of the vector
        public double Length
        {
            get
            {
                return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
            }
        }

        // Normalises the Vector to length 1
        public Vector Normalised
        {
            get
            {
                float length = (float)this.Length;
                return new Vector() { X = this.X / length, Y = this.Y / length, Z = this.Z / length };
            }
        }

        // Returns the dot product of the Vector
        public static double Dot(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // Mathematical operators for the Vector
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector() { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector() { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        public static Vector operator *(Vector a, float b)
        {
            return new Vector() { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector operator *(Vector a, double b)
        {
            return a * (float)b;
        }

        public static Vector operator /(Vector a, float b)
        {
            return new Vector() { X = a.X / b, Y = a.Y / b, Z = a.Z / b };
        }

        public static Vector operator /(Vector a, int b)
        {
            return new Vector() { X = a.X / (float)b, Y = a.Y / (float)b, Z = a.Z / (float)b };
        }

        // Explicit string representation of the Vector
        public override string ToString()
        {
            return "{X: " + X + ", Y: " + Y + ", Z: " + Z + " }";
        }

        // Non comma seperated ToString() functions, for CSV writing
        public string ToMyString()
        {
            return "@value" + X + "@value" + Y + "@value" + Z;
        }

        // A string representation that is easier to split. '@' is chosen as an arbitary non-numerical char that String.Split() can safely parse by
        public string ToMyEasierSplitString()
        {
            return "@" + X + "@" + Y + "@" + Z;
        }
    }
}
