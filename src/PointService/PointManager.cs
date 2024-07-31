using PointService.Models;

namespace PointService
{
    internal class PointManager
    {
        private double _maxDistance;
        private readonly Dictionary<double, ICollection<Point>> _pointMap;

        public PointManager()
        {
            _maxDistance = 0;
            _pointMap = [];
        }

        public void AddPoint(double x, double y)
        {
            // Calculate the distance with some precision to avoid floating-point issues
            var distance = Math.Round(Hypot(x, y), 5);

            _maxDistance = Math.Max(distance, _maxDistance);
            if (!_pointMap.ContainsKey(distance))
            {
                _pointMap.Add(distance, []);
            }

            var quadrant = GetQuadrant(x, y);
            var point = new Point(x, y, quadrant);
            _pointMap[distance].Add(point);
        }

        public IEnumerable<Point> GetFurthestPointsOrDefault()
        {
            _pointMap.TryGetValue(_maxDistance, out var points);
            return points;
        }

        private double Hypot(double x, double y)
        {
            if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                return double.PositiveInfinity;
            }

            double absX = Math.Abs(x);
            double absY = Math.Abs(y);

            if (absX > absY)
            {
                double ratio = absY / absX;
                return absX * Math.Sqrt(1 + ratio * ratio);
            }
            else if (absY != 0)
            {
                double ratio = absX / absY;
                return absY * Math.Sqrt(1 + ratio * ratio);
            }

            return absX;
        }

        private Quadrant GetQuadrant(double x, double y)
        {
            return (x, y) switch
            {
                _ when x > 0 && y > 0 => Quadrant.I,
                _ when x < 0 && y > 0 => Quadrant.II,
                _ when x < 0 && y < 0 => Quadrant.III,
                _ when x > 0 && y < 0 => Quadrant.IV,
                _ when x == 0 && y == 0 => Quadrant.Origin,
                _ when x == 0 => Quadrant.YAxis,
                _ => Quadrant.XAxis,
            };
        }
    }
}
