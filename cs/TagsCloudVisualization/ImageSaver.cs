using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    internal static class ImageSaver
    {
        public static void SaveOnFile(Bitmap image, string fileName)
        {
            if (image is null)
            {
                throw new ArgumentNullException("Передаваемое изображение не должно быть null");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Некорректное имя файла для созранения");
            }

            image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
