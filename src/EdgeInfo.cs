using System.Collections.Generic;
using System.Drawing;

namespace LicznikObiektow
{
    public class EdgeInfo
    {
        public EdgeInfo(ColorInfo color)
        {
            Color = color;
        }

        public ColorInfo Color { get; set; }

        public List<Point> EdgePixels { get; set; } = new List<Point>();
    }
}
