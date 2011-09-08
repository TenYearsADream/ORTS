using System;
using System.Drawing;

namespace ORTS.Core.Extensions
{
    public static class BitmapExtensionMethods
    {
        public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point, Bitmap> action)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    action(point, bitmap);
                }
            }
        }

        public static void ExecuteForEachPixel(this Bitmap bitmap, Action<Point> action)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    action(point);
                }
            }
        }

        public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color> colourFunc)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    bitmap.SetPixel(x, y, colourFunc(point));
                }
            }
        }

        public static void SetEachPixelColour(this Bitmap bitmap, Func<Point, Color, Color> colourFunc)
        {
            Point point = new Point(0, 0);
            for (int x = 0; x < bitmap.Width; x++)
            {
                point.X = x;
                for (int y = 0; y < bitmap.Height; y++)
                {
                    point.Y = y;
                    bitmap.SetPixel(x, y, colourFunc(point, bitmap.GetPixel(x, y)));
                }
            }
        }
    }
}