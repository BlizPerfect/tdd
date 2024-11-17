using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouterPainter
    {
        private string _fileName;
        public CircularCloudLayouterPainter(string fileName)
        {
            _fileName = fileName;
        }

        public void Save(IList<Rectangle> rectangles, int? paddingPerSide = null)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            var correctPaddingPerSide = paddingPerSide ?? 10;

            var minimums = new Point(rectangles.Min(r => r.Left), rectangles.Min(r => r.Top));
            var maximums = new Point(rectangles.Max(r => r.Right), rectangles.Max(r => r.Bottom));

            var imageSize = GetImageSize(minimums, maximums, correctPaddingPerSide);
            using (var bitmap = new Bitmap(imageSize.Width, imageSize.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    using (var pen = new Pen(Color.Black, 1))
                    {
                        for (int i = 0; i < rectangles.Count; i++)
                        {
                            var currentRectangle = rectangles[i];
                            var positionOnCanvas = GetPositionOnCanvas(currentRectangle, minimums, correctPaddingPerSide);
                            graphics.DrawRectangle(pen, positionOnCanvas.X, positionOnCanvas.Y, currentRectangle.Width, currentRectangle.Height);
                        }
                    }
                    bitmap.Save(_fileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            Console.WriteLine($"Изображение сохранено как {_fileName}");
        }

        private Point GetPositionOnCanvas(Rectangle rectangle, Point minimums, int padding)
            => new Point(rectangle.X - minimums.X + padding, rectangle.Y - minimums.Y + padding);

        private Size GetImageSize(Point minimums, Point maximums, int paddingPerSide)
            => new Size(maximums.X - minimums.X + 2 * paddingPerSide, maximums.Y - minimums.Y + 2 * paddingPerSide);
    }
}
