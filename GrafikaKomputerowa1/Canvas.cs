using GrafikaKomputerowa1.Shapes;
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
        private int foregroundColor = GetColor(Colors.Black);
        private int backgroundColor = GetColor(Colors.White);
        private int crossColor = GetColor(Colors.Red);
        private int constrainedColor = GetColor(Colors.Orange);
        private int constrainedRadiusColor = GetColor(Colors.DarkGreen);

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

        public void Render(IEnumerable<Shape> shapes, bool antialiasing = false)
        {
            try
            {
                writeableBitmap.Lock();

                Clear();

                foreach (var shape in shapes)
                {
                    if (shape is Line line)
                    {
                        int color = line.ConstrainedLengthLine is not null || line.ConstrainedTangetCircle is not null ? constrainedColor : foregroundColor;
                        if (antialiasing)
                            DrawLineWu(line.Start.X, line.Start.Y, line.End.X, line.End.Y, color);
                        else
                            DrawLine(line.Start.X, line.Start.Y, line.End.X, line.End.Y, color);
                    }
                    if (shape is Polygon polygon)
                    {
                        foreach (var segment in polygon.Segments)
                        {
                            int color = segment.ConstrainedLengthLine is not null || segment.ConstrainedTangetCircle is not null ? constrainedColor : foregroundColor;
                            if (antialiasing)
                                DrawLineWu(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y, color);
                            else
                                DrawLine(segment.Start.X, segment.Start.Y, segment.End.X, segment.End.Y, color);
                        }
                    }
                    else if (shape is Circle circle)
                    {
                        int color;
                        if (circle.ConstrainedRadius)
                            color = constrainedRadiusColor;
                        else if (circle.ConstrainedTangentLine is not null)
                            color = constrainedColor;
                        else
                            color = foregroundColor;

                        if (antialiasing)
                            DrawCircleWu(circle.Center.X, circle.Center.Y, circle.Radius, color);
                        else
                            DrawCircle(circle.Center.X, circle.Center.Y, circle.Radius, color);
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

        private void PutPixelWu(int x, int y, int color, float brightness)
        {
            if (x < 0 || y < 0 || x >= writeableBitmap.Width || y >= writeableBitmap.Height) return;

            unsafe
            {
                IntPtr backBuffer = writeableBitmap.BackBuffer;
                backBuffer += y * writeableBitmap.BackBufferStride;
                backBuffer += x * writeableBitmap.Format.BitsPerPixel / 8;

                int r = (color >> 16) & 0xff;
                int g = (color >> 8) & 0xff;
                int b = (color >> 0) & 0xff;

                r += (int)((255 - r) * (1.0f - brightness));
                g += (int)((255 - g) * (1.0f - brightness));
                b += (int)((255 - b) * (1.0f - brightness));

                *(int*)backBuffer = (r << 16) | (g << 8) | b;
            }
        }

        private void PutPixelWu4(int x, int y, int dx, int dy, int color, float brightness)
        {
            PutPixelWu(x + dx, y + dy, color, brightness);
            PutPixelWu(x - dx, y + dy, color, brightness);
            PutPixelWu(x + dx, y - dy, color, brightness);
            PutPixelWu(x - dx, y - dy, color, brightness);
        }

        private void DrawLineWu(float x1, float y1, float x2, float y2, int color)
        {
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

            if (steep)
            {
                Swap(ref x1, ref y1);
                Swap(ref x2, ref y2);
            }
            if (x1 > x2)
            {
                Swap(ref x1, ref x2);
                Swap(ref y1, ref y2);
            }

            var dx = x2 - x1;
            var dy = y2 - y1;
            var gradient = dy / dx;
            if (dx == 0) gradient = 1.0f;

            var xend = x1;
            var yend = y1;
            var xgap = RFPart(x1 + 0.5f);
            int xpxl1 = (int)xend;
            int ypxl1 = (int)yend;
            if (steep)
            {
                PutPixelWu(ypxl1, xpxl1, color, RFPart(yend) * xgap);
                PutPixelWu(ypxl1 + 1, xpxl1, color, FPart(yend) * xgap);
            }
            else
            {
                PutPixelWu(xpxl1, ypxl1, color, RFPart(yend) * xgap);
                PutPixelWu(xpxl1, ypxl1 + 1, color, FPart(yend) * xgap);
            }

            var intery = yend + gradient;

            xend = x2;
            yend = y2;
            xgap = RFPart(x2 + 0.5f);
            var xpxl2 = (int)xend;
            var ypxl2 = (int)yend;
            if (steep)
            {
                PutPixelWu(ypxl2, xpxl2, color, RFPart(yend) * xgap);
                PutPixelWu(ypxl2 + 1, xpxl2, color, FPart(yend) * xgap);
            }
            else
            {
                PutPixelWu(xpxl2, ypxl2, color, RFPart(yend) * xgap);
                PutPixelWu(xpxl2, ypxl2 + 1, color, FPart(yend) * xgap);
            }

            if (steep)
            {
                for (var x = xpxl1 + 1; x <= xpxl2 - 1; x++)
                {
                    PutPixelWu(IPart(intery), x, color, RFPart(intery));
                    PutPixelWu(IPart(intery) + 1, x, color, FPart(intery));
                    intery += gradient;
                }
            }
            else
            {
                for (var x = xpxl1 + 1; x <= xpxl2 - 1; x++)
                {
                    PutPixelWu(x, IPart(intery), color, RFPart(intery));
                    PutPixelWu(x, IPart(intery) + 1, color, FPart(intery));
                    intery += gradient;
                }
            }
        }

        private void DrawCircleWu(int x0, int y0, float r, int color)
        {
            var radius2 = r * r;
            var quarter = Math.Round(radius2 / Math.Sqrt(radius2 + radius2));
            for(var x = 0; x <= quarter; x++)
            {
                var y = r * (float)Math.Sqrt(1 - x * x / radius2);
                var err = FPart(y);

                PutPixelWu4(x0, y0, x, IPart(y), color, 1 - err);
                PutPixelWu4(x0, y0, x, IPart(y)+1, color, err);
            }

            for(var y = 0; y <= quarter; y++)
            {
                var x = r * (float)Math.Sqrt(1 - y * y / radius2);
                var err = FPart(x);

                PutPixelWu4(x0, y0, IPart(x), y, color, 1 - err);
                PutPixelWu4(x0, y0, IPart(x) + 1, y, color, err);
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

        private static float FPart(float x) => x - (float)Math.Floor(x);
        private static int IPart(float x) => (int)Math.Floor(x);
        private static float RFPart(float x) => 1 - FPart(x);
    }
}
