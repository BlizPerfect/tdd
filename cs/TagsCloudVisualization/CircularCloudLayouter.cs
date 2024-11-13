using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public readonly Point Center;
        private float _radius = 2.0f;
        private Random _random = new Random();
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
        public IReadOnlyList<Rectangle> Rectangles => _rectangles.AsReadOnly();

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
            {
                throw new ArgumentException("Координаты центра не могут быть меньше нуля.");
            }
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Размеры прямоугольника не могут быть меньше либо равны нуля.");
            }

            var result = new Rectangle();
            _radius -= 1.0f;
            var isPlaced = false;
            while (!isPlaced)
            {
                var points = GetCoordinatesOnEllipse(Center, _radius);
                var pointsStartIndex = _random.Next(points.Length);

                for (int j = 0; j < points.Length; j++)
                {
                    var index = (pointsStartIndex + j) % points.Length;

                    int x = points[index].X - rectangleSize.Width / 2;
                    int y = points[index].Y - rectangleSize.Height / 2;
                    var location = new Point(x, y);

                    var nextRectangle = new Rectangle(location, rectangleSize);

                    var isIntersects = false;
                    foreach (var rect in _rectangles)
                    {
                        if (rect.IntersectsWith(nextRectangle))
                        {
                            isIntersects = true;
                            break;
                        }
                    }

                    if (!isIntersects)
                    {
                        _rectangles.Add(nextRectangle);
                        isPlaced = true;
                        result = nextRectangle;
                        break;
                    }
                }
                _radius += 1.0f;
            }
            return result;
        }

        private Point[] GetCoordinatesOnEllipse(Point rectangleCenter, float radius)
        {
            var result = new Point[360];
            var step = 1;

            for (int angle = 0; angle < 360; angle += step)
            {
                double angleInRadians = angle * Math.PI / 180;

                var x = (int)(rectangleCenter.X + radius * Math.Cos(angleInRadians));
                var y = (int)(rectangleCenter.Y + radius * Math.Sin(angleInRadians));

                result[angle] = new Point(x, y);
            }
            return result;
        }
    }
}
