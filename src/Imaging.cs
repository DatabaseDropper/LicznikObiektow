using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LicznikObiektow
{
    public static class Imaging
    {
        /// <summary>
        /// Przekształcenie pliku na tablicę 2 wymiarową Colorów
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Color[,] ConvertFileTo2DArray(string fileName)
        {
            var img = new Bitmap(fileName);
            var arr = new Color[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    var pixel = img.GetPixel(i, j);

                    arr[i, j] = pixel;
                }
            }

            return arr;
        }

        /// <summary>
        /// Wyciąganie informacji z obrazkach
        /// Oszacowanie co jest tłem => najczęstszy kolor
        /// Pogrupowanie grup kolorów w obiekty oraz zapisanie ich współrzędnych w celu obrysowania.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageInfo GetImageDetails(Color [,] image)
        {
            var info = new ImageInfo();

            var grouped = _2DArrayGroup(image);

            info.BackgroundColor = grouped.OrderByDescending(x => x.Value.Occured).First().Value;

            info.NonBackgroundColors = grouped
                                       .Where(x => x.Key != info.BackgroundColor.Color)
                                       .Select(x => x.Value)
                                       .ToList();

            info.Edges = GenerateEdges(info.NonBackgroundColors);

            return info;
        }

        private static List<EdgeInfo> GenerateEdges(List<ColorInfo> nonBackgroundColors)
        {
            var edges = new List<EdgeInfo>();

            foreach (var info in nonBackgroundColors)
            {
                var edgePoints = FindEdgePoints(info);
                edges.Add(edgePoints);
            }

            return edges;
        }


        /// <summary>
        /// Wyciągamy wszystkie kolumny oraz wszystkie wiersze, a następnie
        /// dla każdego wiersza oraz dla każdej kolumny odszukujemy te punkty, którę się na nich znajdują,
        /// a następnie zapisujemy tylko skrajne punkty
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static EdgeInfo FindEdgePoints(ColorInfo info)
        {
            var edgeInfo = new EdgeInfo(info);

            var points = info.Points;
            var rows = points.Select(x => x.Y).Distinct().ToList();
            var cols = points.Select(x => x.X).Distinct().ToList();

            foreach (var row in rows)
            {
                var pointsInThisRow = points.Where(x => x.Y == row).ToList();
                var ordered = pointsInThisRow.OrderByDescending(x => x.X).ToList();

                Point? highestX = ordered.FirstOrDefault();
                Point? lowestX = ordered.LastOrDefault();

                if (highestX != null && !edgeInfo.EdgePixels.Contains(highestX.Value))
                    edgeInfo.EdgePixels.Add(highestX.Value);

                if (lowestX != null && !edgeInfo.EdgePixels.Contains(lowestX.Value))
                    edgeInfo.EdgePixels.Add(lowestX.Value);
            }

            foreach (var col in cols)
            {
                var pointsInThisColumn = points.Where(x => x.X == col).ToList();
                var ordered = pointsInThisColumn.OrderByDescending(x => x.Y).ToList();

                Point? highestY = ordered.FirstOrDefault();
                Point? lowestY = ordered.LastOrDefault();

                if (highestY != null && !edgeInfo.EdgePixels.Contains(highestY.Value))
                    edgeInfo.EdgePixels.Add(highestY.Value);

                if (lowestY != null && !edgeInfo.EdgePixels.Contains(lowestY.Value))
                    edgeInfo.EdgePixels.Add(lowestY.Value);
            }

            return edgeInfo;
        }

        /// <summary>
        /// Grupowanie punktów (x, y) po kolorach
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static Dictionary<Color, ColorInfo> _2DArrayGroup(Color[,] image)
        {
            var dictionary = new Dictionary<Color, ColorInfo>();

            var width = image.GetLength(0);
            var height = image.GetLength(1);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var color = image[i, j];
                    var point = new Point(i, j);

                    if (dictionary.ContainsKey(color))
                    {
                        dictionary[color].AddPoint(point);
                    }
                    else
                    {
                        dictionary[color] = new ColorInfo(color).AddPoint(point);
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Nakładanie czerwonych ramek wokół obiektów na obrazie
        /// </summary>
        /// <param name="image"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static Color[,] DrawEdges(Color[,] image, List<EdgeInfo> edges)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);

            var result = new Color[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var found = edges.FirstOrDefault(x => x.EdgePixels.Any(e => e.X == i && e.Y == j));

                    if (found != null)
                    {
                        result[i, j] = Color.Red;
                    }
                    else
                    {
                        result[i, j] = image[i, j];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Zapisywanie tablicy 2D jako obraz do pliku
        /// </summary>
        /// <param name="path"></param>
        /// <param name="image"></param>
        public static void DrawImage(string path, Color[,] image)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);
            var myBitmap = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var color = image[i, j];
                    myBitmap.SetPixel(i, j, color);
                }
            }

            myBitmap.Save(path);
        }
    }
}
