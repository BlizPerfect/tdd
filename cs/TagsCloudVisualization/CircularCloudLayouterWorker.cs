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

        public static Size GetNextRectangleSize(
            int minRectangleWidth,
            int maxRectangleWidth,
            int minRectangleHeight,
            int maxRectangleHeight)
        {
            if (!IsMinAndMaxValuesPositive(
                minRectangleWidth,
                maxRectangleWidth,
                minRectangleHeight,
                maxRectangleHeight))
            {
                throw new ArgumentException("Ширина и высота прямоугольника не может быть отрицательной либо нулём");
            }

            if (!IsMinAndMaxValuesCorrect(
                minRectangleWidth,
                maxRectangleWidth,
                minRectangleHeight,
                maxRectangleHeight))
            {
                throw new ArgumentException("Минимальное значение ширины или высоты не может быть больше максимального");
            }

            var width = _random.Next(minRectangleWidth, maxRectangleWidth);
            var height = _random.Next(minRectangleHeight, maxRectangleHeight);
            return new Size(width, height);
        }

        private static bool IsMinAndMaxValuesCorrect(
            int minRectangleWidth,
            int maxRectangleWidth,
            int minRectangleHeight,
            int maxRectangleHeight)
            => minRectangleWidth <= maxRectangleWidth && minRectangleHeight <= maxRectangleHeight;

        private static bool IsMinAndMaxValuesPositive(
            int minRectangleWidth,
            int maxRectangleWidth,
            int minRectangleHeight,
            int maxRectangleHeight)
            => minRectangleWidth > 0 && maxRectangleWidth > 0
            && minRectangleHeight > 0 && maxRectangleHeight > 0;
    }
}
