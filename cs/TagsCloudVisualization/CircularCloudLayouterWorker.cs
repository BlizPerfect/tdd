using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouterWorker
    {
        private Size _minRectangleSize;
        private Size _maxRectangleSize;
        private Random _random = new Random();

        public CircularCloudLayouterWorker(Size minRectangleSize, Size maxRectangleSize)
        {
            _minRectangleSize = minRectangleSize;
            _maxRectangleSize = maxRectangleSize;
        }

        public Size GetNextRectangleSize()
        {
            var width = _random.Next(_minRectangleSize.Width, _maxRectangleSize.Width);
            var height = _random.Next(_minRectangleSize.Height, _maxRectangleSize.Height);
            return new Size(width, height);
        }
    }
}
