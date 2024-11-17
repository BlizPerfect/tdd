using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Tests
{
    internal static class RectangleExtensions
    {
        public static string ToFormatedString(this Rectangle rectangle)
        {
            return $"X={rectangle.X}, Y={rectangle.Y}, Width={rectangle.Width}, Height={rectangle.Height}";
        }
    }
}