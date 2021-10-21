﻿using GrafikaKomputerowa1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa1
{
    public sealed class Canvas
    {
        private int foregroundColor = GetColor(0, 0, 0);
        private int backgroundColor = GetColor(255, 255, 255);
        private int crossColor = GetColor(255, 0, 0);

        private WriteableBitmap writeableBitmap;

        public WriteableBitmap RecreateBitmap(double width, double height)
        {
            writeableBitmap = new WriteableBitmap(
                (int)width, (int)height,
                96, 96, PixelFormats.Bgr32, null);

            Clear();

            return writeableBitmap;
        }

        public void Clear()
        {
            try
            {
                writeableBitmap.Lock();

                unsafe
                {
                    IntPtr backBuffer = writeableBitmap.BackBuffer;
                    int w = (int)writeableBitmap.Width;
                    int h = (int)writeableBitmap.Height;
                    var bytesPerPixel = writeableBitmap.Format.BitsPerPixel / 8;

                    for (int i = 0; i < w * h; i++)
                    {
                        *(int*)backBuffer = backgroundColor;
                        backBuffer += bytesPerPixel;
                    }
                }
            }
            finally
            {
                writeableBitmap.Unlock();
            }
        }

        public void Render(IEnumerable<Shape> shapes)
        {
            try
            {
                writeableBitmap.Lock();

                Clear();

                foreach (var shape in shapes)
                {
                    if (shape is Line line)
                    {
                        DrawLine(line.Start.X, line.Start.Y, line.End.X, line.End.Y, foregroundColor);
                    }
                    if (shape is Polygon polygon)
                    {
                        foreach (var segment in polygon.Segments)
                            DrawLine(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y, foregroundColor);
                    }
                    else if (shape is Circle circle)
                    {
                        DrawCircle(circle.Center.X, circle.Center.Y, circle.Radius, foregroundColor);
                    }
                }

                foreach (var vertex in shapes.SelectMany(x => x.GetVertices()))
                {
                    DrawCross(vertex.X, vertex.Y, crossColor);
                }

                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)writeableBitmap.Width, (int)writeableBitmap.Height));
            }
            finally
            {
                writeableBitmap.Unlock();
            }
        }

        private void PutPixel(int x, int y, int color)
        {
            if (x < 0 || y < 0 || x >= writeableBitmap.Width || y >= writeableBitmap.Height) return;

            unsafe
            {
                IntPtr backBuffer = writeableBitmap.BackBuffer;
                backBuffer += y * writeableBitmap.BackBufferStride;
                backBuffer += x * writeableBitmap.Format.BitsPerPixel / 8;

                *(int*)backBuffer = color;
            }
        }

        private void FillPixels(int x, int y, int w, int h, int color)
        {
            x = Math.Clamp(x, 0, (int)writeableBitmap.Width);
            y = Math.Clamp(y, 0, (int)writeableBitmap.Height);
            w = Math.Clamp(w, 0, (int)writeableBitmap.Width - x);
            h = Math.Clamp(h, 0, (int)writeableBitmap.Height - y);

            var stride = writeableBitmap.BackBufferStride;
            var bytesPerPixel = writeableBitmap.Format.BitsPerPixel / 8;
            unsafe
            {
                IntPtr backBuffer = writeableBitmap.BackBuffer;
                backBuffer += y * stride;
                backBuffer += x * bytesPerPixel;

                for (int j = y; j < h + y; j++)
                {
                    for (int i = x; i < w + x; i++)
                    {
                        *(int*)backBuffer = color;
                        backBuffer += bytesPerPixel;
                    }
                    backBuffer += stride - w * bytesPerPixel;
                }
            }
        }

        private void DrawCross(int x, int y, int color)
        {
            for (int j = y - 2; j <= y + 2; j++)
                PutPixel(x, j, color);
            for (int i = x - 2; i <= x + 2; i++)
                PutPixel(i, y, color);
        }

        private void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            var dx = Math.Abs(x2 - x1);
            var sx = x1 < x2 ? 1 : -1;
            var dy = -Math.Abs(y2 - y1);
            var sy = y1 < y2 ? 1 : -1;
            var err = dx + dy;
            while (true)
            {
                PutPixel(x1, y1, color);
                if (x1 == x2 && y1 == y2) break;

                var e2 = err * 2;
                if (e2 >= dy)
                {
                    err += dy;
                    x1 += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        private void DrawCircle(int x0, int y0, int r, int color)
        {
            PutPixel(x0, y0, color);

            var x = 0;
            var y = r;
            var P = 3 - 2 * r;
            while (y >= x)
            {
                PutPixel(x + x0, y + y0, color);
                PutPixel(-x + x0, y + y0, color);
                PutPixel(x + x0, -y + y0, color);
                PutPixel(-x + x0, -y + y0, color);
                PutPixel(y + x0, x + y0, color);
                PutPixel(-y + x0, x + y0, color);
                PutPixel(y + x0, -x + y0, color);
                PutPixel(-y + x0, -x + y0, color);

                if (P < 0)
                {
                    x += 1;
                    P += 4 * x + 6;
                }
                else
                {
                    x += 1;
                    y -= 1;
                    P += 4 * (x - y) + 10;
                }
            }
        }

        private static int GetColor(Color color)
        {
            return GetColor(color.R, color.G, color.B);
        }

        private static int GetColor(byte r, byte g, byte b)
        {
            return (r << 16) | (g << 8) | b;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
