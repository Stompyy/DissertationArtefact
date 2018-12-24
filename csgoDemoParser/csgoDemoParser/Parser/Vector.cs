using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoDemoParser
{
    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector()
        {

        }

        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector(double x, double y, double z)
        {
            this.X = (float)x;
            this.Y = (float)y;
            this.Z = (float)z;
        }

        public Vector(string x, string y, string z)
        {
            this.X = float.Parse(x);
            this.Y = float.Parse(y);
            this.Z = float.Parse(z);
        }

        public Vector(float a)
        {
            this.X = a;
            this.Y = a;
            this.Z = a;
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
            }
        }

        public Vector Normalised
        {
            get
            {
                float length = (float)this.Length;
                return new Vector() { X = this.X / length, Y = this.Y / length, Z = this.Z / length };
            }
        }

        public static double Dot(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public double Angle2D
        {
            get
            {
                return Math.Atan2(this.Y, this.X);
            }
        }

        public double Absolute
        {
            get
            {
                return Math.Sqrt(AbsoluteSquared);
            }
        }

        public double AbsoluteSquared
        {
            get
            {
                return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            }
        }

        /// <summary>
        /// Copy this instance. So if you want to permanently store the position of a player at a point in time, 
        /// COPY it. 
        /// </summary>
        public Vector Copy()
        {
            return new Vector(X, Y, Z);
        }

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

        public override string ToString()
        {
            return "{X: " + X + ", Y: " + Y + ", Z: " + Z + " }";
        }

        public string ToMyString()
        {
            return "@value" + X + "@value" + Y + "@value" + Z;
        }

        public string ToMyEasierSplitString()
        {
            return "@" + X + "@" + Y + "@" + Z;
        }
    }
}
