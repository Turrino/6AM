using System;
using System.ComponentModel;

namespace BayeuxBundle.Models
{
    [TypeConverter(typeof(PixelTypeConverter))]
    public class PixelInfo
    {
        public bool IsTransparent;
        public byte R;
        public byte G;
        public byte B;
        public byte A = 255; // Not really in use right now.

        public override bool Equals(object obj)
        {
            if (obj is PixelInfo pxl)
            {
                if (pxl.R == R && pxl.G == G && pxl.B == B)
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1218123847;
            hashCode = hashCode * -1521134295 + IsTransparent.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        public static PixelInfo Red => new PixelInfo() { R = 255 };
        public static PixelInfo Green => new PixelInfo() { G = 255 };
        public static PixelInfo Blue => new PixelInfo() { B = 255 };
        public static PixelInfo White => new PixelInfo() { R = 255, G = 255, B = 255 };
        public static PixelInfo Black => new PixelInfo();

        public PixelInfo(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
            IsTransparent = A == 0;
        }

        private PixelInfo()
        {

        }

        public PixelInfo(string fromString)
        {
            A = Convert.ToByte(fromString.Substring(1, 2), 16);
            R = Convert.ToByte(fromString.Substring(3, 2), 16);
            G = Convert.ToByte(fromString.Substring(5, 2), 16);
            B = Convert.ToByte(fromString.Substring(7, 2), 16);
        }

        public override string ToString()
        {
            return $"#{BitConverter.ToString(new byte[] { A, R, G, B  })}".Replace("-", ""); 
        }
    }
}
