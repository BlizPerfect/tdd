﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Tests
{
    [TestFixture]
    internal class CircularCloudLayouterMainRequirementsTests
    {
        private Point _center;
        private Rectangle[] _rectangles;
        private string _failedTestsDirectory = "FailedTest";

        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(_failedTestsDirectory);
        }

        [SetUp]
        public void SetUp()
        {
            _center = new Point(400, 400);
            var minRectangleWidth = 30;
            var maxRectangleWidth = 70;
            var minRectangleHeight = 20;
            var maxRectangleHeight = 50;
            var rectanglesCount = 1000;

            _rectangles = new Rectangle[rectanglesCount];
            var circularCloudLayouter = new CircularCloudLayouter(_center);
            for (int i = 0; i < rectanglesCount; i++)
            {
                var nextRectangleSize = CircularCloudLayouterWorker.GetNextRectangleSize(
                    minRectangleWidth,
                    maxRectangleWidth,
                    minRectangleHeight,
                    maxRectangleHeight);
                _rectangles[i] = circularCloudLayouter.PutNextRectangle(nextRectangleSize);
            }
        }

        [TestCase(1.1, ExpectedResult = true)]
        [Repeat(10)]
        public bool ShouldPlaceRectanglesInCircle(double goodAspectRatio)
        {
            var boundingBoxSize = GetBoundingBoxSize();
            var actualAspectRatio =
                (double)Math.Max(boundingBoxSize.Width, boundingBoxSize.Height)
                / Math.Min(boundingBoxSize.Width, boundingBoxSize.Height);
            return actualAspectRatio <= goodAspectRatio;
        }

        [TestCase(0.1, ExpectedResult = true)]
        [Repeat(10)]
        public bool ShouldPlaceRectanglesNearCenter(double percentFromBoundingBoxMaxSize)
        {
            var centerX = _rectangles.Average(r => r.Left + r.Width / 2.0);
            var centerY = _rectangles.Average(r => r.Top + r.Height / 2.0);
            var actualCenter = new Point((int)centerX, (int)centerY);

            var distance = Math.Sqrt(Math.Pow(actualCenter.X - _center.X, 2)
                                     + Math.Pow(actualCenter.Y - _center.Y, 2));

            var boundingBoxSize = GetBoundingBoxSize();

            var maxAllowedDistance = Math.Max(boundingBoxSize.Width, boundingBoxSize.Height)
                                     * percentFromBoundingBoxMaxSize;

            return maxAllowedDistance >= distance;
        }

        [Test]
        [Repeat(10)]
        public void ShouldPlaceRectanglesWithoutOverlap()
        {
            for (var i = 0; i < _rectangles.Length; i++)
            {
                for (var j = i + 1; j < _rectangles.Length; j++)
                {
                    Assert.That(
                        _rectangles[i].IntersectsWith(_rectangles[j]) == false,
                        $"Прямоугольники пересекаются:\n" +
                        $"{_rectangles[i].ToFormatedString()}\n" +
                        $"{_rectangles[j].ToFormatedString()}");
                }
            }
        }


        [TestCase(0.55, ExpectedResult = true)]
        [Repeat(10)]
        public bool ShouldPlaceRectanglesTightly(double tight)
        {
            var boundingBoxSize = GetBoundingBoxSize();
            var boundingBoxArea = boundingBoxSize.Width * boundingBoxSize.Height;
            var filledArea = _rectangles.Sum(r => r.Width * r.Height);
            return ((double)filledArea / boundingBoxArea) >= tight;
        }

        [TearDown]
        public void Cleanup()
        {
            if (TestContext.CurrentContext.Result.FailCount == 0)
            {
                return;
            }

            var name = $"{TestContext.CurrentContext.Test.Name}.png";
            var path = Path.Combine(_failedTestsDirectory, name);
            ImageSaver.SaveFile(CircularCloudLayouterPainter.Draw(_rectangles), path);
            Console.WriteLine($"Tag cloud visualization saved to file {path}");
        }

        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            if (Directory.Exists(_failedTestsDirectory)
                && Directory.GetFiles(_failedTestsDirectory).Length == 0)
            {
                Directory.Delete(_failedTestsDirectory);
            }
        }

        private Size GetBoundingBoxSize()
        {
            var minX = _rectangles.Min(r => r.Left);
            var minY = _rectangles.Min(r => r.Top);
            var maxX = _rectangles.Max(r => r.Right);
            var maxY = _rectangles.Max(r => r.Bottom);
            return new Size(maxX - minX, maxY - minY);
        }
    }
}