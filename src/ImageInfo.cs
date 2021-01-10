using System.Collections.Generic;

namespace LicznikObiektow
{
    public class ImageInfo
    {
        public ColorInfo BackgroundColor { get; set; }

        public List<ColorInfo> NonBackgroundColors { get; set; } = new List<ColorInfo>();

        public List<EdgeInfo> Edges { get; set; } = new List<EdgeInfo>();
    }
}