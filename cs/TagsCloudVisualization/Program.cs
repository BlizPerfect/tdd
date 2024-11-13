using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(400, 400);
            var minRectangleSize = new Size(30, 20);
            var maxRectangleSize = new Size(70, 50);
            var rectanglesCount = 100;
            var imageFileName = "circularCloudLayouter.png";

            var circularCloudLayouter = new CircularCloudLayouter(center);
            var circularCloudLayouterWorker = new CircularCloudLayouterWorker(minRectangleSize, maxRectangleSize);
            var circularCloudLayouterPainter = new CircularCloudLayouterPainter(imageFileName);

            for (int i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = circularCloudLayouterWorker.GetNextRectangleSize();
                circularCloudLayouter.PutNextRectangle(nextRectangleSize);
            }

            circularCloudLayouterPainter.Save(circularCloudLayouter.Rectangles.ToList());
        }
    }
}
