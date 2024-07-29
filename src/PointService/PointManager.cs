namespace PointService
{
    using PointService.Models;

    internal class PointManager
    {
        // TODO: Key here need to be base on the max distance number
        private readonly Dictionary<double, ICollection<Point>> _pointMap;

        // TODO: Need to check how much the max distance can be?
        private double _maxDistance;

        public PointManager()
        {
            _maxDistance = 0;
            _pointMap = [];
        }

        public void AddPoint(double x, double y)
        {
            var distance = Math.Sqrt(x * x + y * y);
            _maxDistance = Math.Max(distance, _maxDistance);
            if (!_pointMap.ContainsKey(distance))
            {
                _pointMap.Add(distance, []);
            }

            var quedratan = GetQuadrant(x, y);
            var point = new Point(x, y, quedratan);
            _pointMap[distance].Add(point);
        }

        public IEnumerable<Point> GetFurthestPointsOrDefault()
        {
            _pointMap.TryGetValue(_maxDistance, out var points);
            return points;
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
