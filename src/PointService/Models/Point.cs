namespace PointService.Models
{
    internal class Point(double x, double y, Quadrant quadrant)
    {
        // TODO: Ask abount the point variable type and what is the max point number
        public double X { get; init; } = x;

        public double Y { get; init; } = y;

        public Quadrant Quadrant { get; init; } = quadrant;
    }
}
