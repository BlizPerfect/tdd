using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal static class CircularCloudLayouterWorker
    {
        private static Random _random = new Random();

        public static Size GetNextRectangleSize(Size minRectangleSize, Size maxRectangleSize)
        {
            var width = _random.Next(minRectangleSize.Width, maxRectangleSize.Width);
            var height = _random.Next(minRectangleSize.Height, maxRectangleSize.Height);
            return new Size(width, height);
        }
    }
}
