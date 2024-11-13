using System;
using NUnit.Framework;
using FluentAssertions;
using System.Drawing;

namespace TagsCloudVisualization
{
    [TestFixture]
    internal class CircularCloudLayouterTests
    {
        private CircularCloudLayouter _circularCloudLayouter;
        private CircularCloudLayouterWorker _circularCloudLayouterWorker;
        private CircularCloudLayouterPainter _circularCloudLayouterPainter;

        public void SetUp(int x = 400,
            int y = 400,
            int minRectangleWidth = 30,
            int minRectangleHeight = 20,
            int maxRectangleWidth = 70,
            int maxRectangleHeight = 50,
            string imageFileName = "circularCloudLayouter.png")
        {
            var center = new Point(x, y);
            var minRectangleSize = new Size(minRectangleWidth, minRectangleHeight);
            var maxRectangleSize = new Size(maxRectangleWidth, maxRectangleHeight);

            _circularCloudLayouter = new CircularCloudLayouter(center);
            _circularCloudLayouterWorker = new CircularCloudLayouterWorker(minRectangleSize, maxRectangleSize);
            _circularCloudLayouterPainter = new CircularCloudLayouterPainter(imageFileName);
        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        public void CircularCloudLayouter_ThrowsArgumentException_OnNegativeCenterPoint(int x, int y)
        {
            var center = new Point(x, y);
            Assert.Throws<ArgumentException>(() => new CircularCloudLayouter(center));
        }


        [TestCase(0, 100, 1)]
        [TestCase(-1, 100, 1)]
        [TestCase(100, 0, 1)]
        [TestCase(100, -1, 1)]

        public void PutNextRectangle_ThrowsArgumentException_OnAnyNegativeOrZeroSize(int width, int height, int rectanglesCount)
        {
            SetUp();
            for (int i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = _circularCloudLayouterWorker.GetNextRectangleSize();
                Assert.Throws<ArgumentException>(() => _circularCloudLayouter.PutNextRectangle(new Size(width, height)));
            }
        }

        [TestCase(1)]
        public void PutNextRectangle_ReturnsRectangleType(int rectanglesCount)
        {
            SetUp();
            for (int i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = _circularCloudLayouterWorker.GetNextRectangleSize();
                Assert.That(_circularCloudLayouter.PutNextRectangle(nextRectangleSize), Is.TypeOf(typeof(Rectangle)));
            }
        }

        [TestCase(1, "TestFile.png", ExpectedResult = true)]
        [TestCase(0, "TestFile.png", ExpectedResult = false)]
        public bool CircularCloudLayouterPainter_CreatesFile(int rectanglesCount, string fileName)
        {
            SetUp(imageFileName: fileName);
            File.Delete(fileName);

            for (int i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = _circularCloudLayouterWorker.GetNextRectangleSize();
                _circularCloudLayouter.PutNextRectangle(nextRectangleSize);
            }

            _circularCloudLayouterPainter.Save(_circularCloudLayouter.Rectangles.ToList());
            return File.Exists(fileName);
        }
    }
}
