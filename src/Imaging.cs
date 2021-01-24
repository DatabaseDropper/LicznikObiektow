using System;
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

        public static Color[,] ConvertFileTo2DArrayWithGrayScale(string fileName)
        {
            var img = new Bitmap(fileName);
            var arr = new Color[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    var pixel = img.GetPixel(i, j);

                    var pixelValue = (int)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

                    arr[i, j] = pixelValue < 90 ? Color.Black : Color.White;
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

        public static ImageInfo GetImageDetails2(Color [,] image)
        {
            var info = new ImageInfo();

            var grouped = _2DArrayGroup(image);

            info.BackgroundColor = grouped.OrderByDescending(x => x.Value.Occured).First().Value;

            info.NonBackgroundColors = grouped
                                       .Where(x => x.Key != info.BackgroundColor.Color)
                                       .Select(x => x.Value)
                                       .ToList();

            info.Groups = GenerateGroups2(info.NonBackgroundColors);

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

        private static Dictionary<int, List<Point>> GenerateGroups2(List<ColorInfo> nonBackgroundColors)
        {
            var dictionary = new Dictionary<int, List<Point>>();

            var points = nonBackgroundColors.SelectMany(x => x.Points).ToList();

            var checkLater = new List<(int Id, Point P)>();

            foreach (var point in points)
            {
                var group = FindGroup(point, dictionary);

                if (group.HasValue)
                {
                    dictionary[group.Value].Add(point);
                }
                else
                {
                    var id = dictionary.Count;
                    dictionary.Add(id, new List<Point> { point });
                    checkLater.Add((id, point));
                }
            }

            TryNormalize(checkLater, dictionary);

            return dictionary.Where(x => x.Value.Count > 0).ToDictionary(x => x.Key, x => x.Value);
        }

        private static void TryNormalize(List<(int Id, Point P)> checkLater, Dictionary<int, List<Point>> dictionary)
        {
            foreach (var item in checkLater)
            {
                while (true)
                {
                    var anyPointWasMoved = false;
                    var pointsToNormalize = dictionary[item.Id].ToList();

                    foreach (var point in pointsToNormalize)
                    {
                        if (TryNormalizePoint(point, item.Id, dictionary))
                        {
                            anyPointWasMoved = true;
                        }
                    }

                    if (!anyPointWasMoved)
                        break;
                }
            }
        }

        private static bool TryNormalizePoint(Point point, int original_group_id, Dictionary<int, List<Point>> dictionary)
        {
            var groups = FindGroups(point, dictionary);

            var other_groups = groups.Where(x => x != original_group_id).ToList();

            if (other_groups.Any())
            {
                var original_group = dictionary[original_group_id];
                original_group.Remove(point);
                var other_grp = dictionary[other_groups.First()];
                other_grp.Add(point);
                return true;
            }

            return false;
        }

        private static int? FindGroup(Point point, Dictionary<int, List<Point>> dictionary)
        {
            foreach (var entry in dictionary)
            {
                foreach (var p in entry.Value)
                {
                    if (PointIsNextToOtherPoint(point, p))
                    {
                        return entry.Key;
                    }
                }
            }
            return null;
        }

        private static List<int> FindGroups(Point point, Dictionary<int, List<Point>> dictionary)
        {
            var list = new List<int>();

            foreach (var entry in dictionary)
            {
                foreach (var p in entry.Value)
                {
                    if (PointIsNextToOtherPoint(point, p))
                    {
                        list.Add(entry.Key);
                    }
                }
            }

            return list.Distinct().ToList();
        }

        public static bool PointIsNextToOtherPoint(Point point, Point newPoint)
        {
            var moveBy = new List<(int MoveX, int MoveY)>
            {
                (-1, 1),
                (0, 1),
                (1, 1),

                (-1, 0),
                (0, 0),
                (1, 0),

                (-1, -1),
                (0, -1),
                (1, -1)
            };

            foreach (var move in moveBy)
            {
                if (newPoint.X + move.MoveX == point.X && newPoint.Y + move.MoveY == point.Y)
                    return true;
            }

            return false;
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

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = image[x, y];
                    var point = new Point(x, y);

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

        public static Color[,] DrawEdges2(Color[,] image, Dictionary<int, List<Point>> Groups)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);

            var result = new Color[width, height];

            var rnd = new Random();

            var dictionaryColors = Groups.ToDictionary(x => x.Key, x => Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var found = Groups.Where(x => x.Value.Any(e => e.X == i && e.Y == j)).ToList();

                    if (found.Any())
                    {
                        result[i, j] = dictionaryColors[found.First().Key];
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
