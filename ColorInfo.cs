using System.Collections.Generic;
using System.Drawing;

namespace LicznikObiektow
{
    public class ColorInfo
    {
        public ColorInfo(Color color)
        {
            Color = color;
        }

        public int Occured => Points.Count;

        public List<Point> Points { get; set; } = new List<Point>();

        public Color Color { get; }

        public ColorInfo AddPoint(Point p)
        {
            Points.Add(p);
            return this;
        }

        public override string ToString()
        {
            return $"Color '{Color.Name}' occured {Occured} times";
        }
    }
}
