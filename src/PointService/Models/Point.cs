namespace PointService.Models
{
    internal class Point(double x, double y, Quadrant quadrant)
    {
        public double X { get; init; } = x;

        public double Y { get; init; } = y;

        public Quadrant Quadrant { get; init; } = quadrant;
    }
}
