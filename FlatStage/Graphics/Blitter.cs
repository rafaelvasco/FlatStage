using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlatStage.Graphics;

public static unsafe class Blitter
{
    private const int AuxBufferSize = 1024 * 1024 * 4;
    private const int AuxBufferStackSize = 2;

    private static IntPtr _pixels;
    private static int _pixelsSizeInBytes;
    private static bool _ready;
    private static Rect _clipRect;
    private static Color _drawColor = Color.White;
    private static int _surfaceW;
    private static int _surfaceH;
    private static readonly byte[]?[] AuxBuffers = new byte[AuxBufferStackSize][];
    private static int _auxBuffersIdx;

    public static void Begin(Memory<byte> pixels, int surfaceWidth, int surfaceHeight)
    {
        if (_ready)
        {
            throw new InvalidOperationException("Cannot nest Begin call.");
        }

        _pixelsSizeInBytes = pixels.Length;

        SetDrawState((IntPtr)pixels.Pin().Pointer, surfaceWidth, surfaceHeight);
    }

    private static void SetDrawState(IntPtr pixels, int surfaceWidth, int surfaceHeight)
    {
        _pixels = pixels;
        _surfaceW = surfaceWidth;
        _surfaceH = surfaceHeight;
        _drawColor = Color.White;
        _clipRect = new Rect(0, 0, _surfaceW, _surfaceH);
        _ready = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CheckReady()
    {
        if (!_ready)
        {
            throw new Exception("Blitter: Not Ready. All calls must happen inside Begin/End block");
        }
    }

    public static void End()
    {
        _pixels = IntPtr.Zero;
        _surfaceW = _surfaceH = 0;
        _ready = false;
        _clipRect = Rect.Empty;
    }

    public static void SetColor(Color color)
    {
        _drawColor = color;
    }

    public static void Clip(Rect rect)
    {
        _clipRect = rect;

        if (_clipRect.IsEmpty)
        {
            _clipRect = new Rect(0, 0, _surfaceW, _surfaceH);
        }
        else if (!new Rect(0, 0, _surfaceW, _surfaceH).Contains(_clipRect))
        {
            _clipRect = new Rect(0, 0, _surfaceW, _surfaceH);
        }
    }

    public static void Clip(int x = 0, int y = 0, int w = 0, int h = 0)
    {
        Clip(new Rect(x, y, w, h));
    }

    public static void PixelSet(int x, int y)
    {
        CheckReady();

        if (!_clipRect.Contains(x, y))
        {
            return;
        }

        ref var col = ref _drawColor;

        byte r = col.R;
        byte g = col.G;
        byte b = col.B;
        byte a = col.A;

        byte* ptr = (byte*)_pixels;

        byte* ptrIdx = ptr + (x + y * _surfaceW) * 4;

        *(ptrIdx + 0) = b;
        *(ptrIdx + 1) = g;
        *(ptrIdx + 2) = r;
        *(ptrIdx + 3) = a;
    }

    public static Color PixelGet(int x, int y)
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        byte* ptrIdx = ptr + (x + y * _surfaceW) * 4;

        return new Color(
            *(ptrIdx + 2),
            *(ptrIdx + 1),
            *(ptrIdx + 0),
            *(ptrIdx + 3)
        );
    }

    public static void Clear()
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        var len = _pixelsSizeInBytes - 4;

        for (int i = 0; i <= len; i += 4)
        {
            *(ptr + i + 0) = 0;
            *(ptr + i + 1) = 0;
            *(ptr + i + 2) = 0;
            *(ptr + i + 3) = 0;
        }
    }

    public static void Fill()
    {
        CheckReady();

        ref var col = ref _drawColor;

        byte* ptr = (byte*)_pixels;

        var len = _pixelsSizeInBytes - 4;

        for (int i = 0; i <= len; i += 4)
        {
            *(ptr + i + 0) = col.B;
            *(ptr + i + 1) = col.G;
            *(ptr + i + 2) = col.R;
            *(ptr + i + 3) = col.A;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void HLine(int sx, int ex, int y)
    {
        CheckReady();

        var minX = _clipRect.Left;
        var maxX = _clipRect.Right;

        if (y < _clipRect.Top || y > _clipRect.Bottom)
        {
            return;
        }

        if (sx < minX && ex < minX)
        {
            return;
        }

        if (sx > maxX && ex > maxX)
        {
            return;
        }

        if (ex < sx)
        {
            Calc.Swap(ref ex, ref sx);
        }

        ref var col = ref _drawColor;

        byte* ptr = (byte*)_pixels;

        int sw = _surfaceW;

        for (int x = sx; x < ex; ++x)
        {
            byte* ptrIdx = ptr + (x + y * sw) * 4;

            *ptrIdx = col.B;
            *(ptrIdx + 1) = col.G;
            *(ptrIdx + 2) = col.R;
            *(ptrIdx + 3) = col.A;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void VLine(int sy, int ey, int x)
    {
        CheckReady();

        if (x < _clipRect.Left || x > _clipRect.Right)
        {
            return;
        }

        var minY = _clipRect.Top;
        var maxY = _clipRect.Bottom;
        if (sy < minY && ey < minY) return;
        if (sy > maxY && ey > maxY) return;

        if (ey < sy)
        {
            (sy, ey) = (ey, sy);
        }

        ref var col = ref _drawColor;

        byte r = col.R;
        byte g = col.G;
        byte b = col.B;
        byte a = col.A;

        int sw = _surfaceW;

        byte* ptr = (byte*)_pixels;

        for (int y = sy; y < ey; ++y)
        {
            if (y < _clipRect.Top || y > _clipRect.Bottom)
            {
                continue;
            }

            byte* ptrIdx = ptr + (x + y * sw) * 4;

            *ptrIdx = b;
            *(ptrIdx + 1) = g;
            *(ptrIdx + 2) = r;
            *(ptrIdx + 3) = a;
        }
    }

    public static void FillRect(int x, int y, int w, int h)
    {
        CheckReady();

        ref var col = ref _drawColor;

        int left = Math.Max(x, _clipRect.Left);
        int right = Math.Min(x + w, _clipRect.Right);
        int top = Math.Max(y, _clipRect.Top);
        int bottom = Math.Min(y + h, _clipRect.Bottom);

        int sw = _surfaceW;

        byte* ptr = (byte*)_pixels;

        for (int px = left; px < right; ++px)
        {
            for (int py = top; py < bottom; ++py)
            {
                byte* ptrIdx = ptr + (px + py * sw) * 4;

                *ptrIdx = col.B;
                *(ptrIdx + 1) = col.G;
                *(ptrIdx + 2) = col.R;
                *(ptrIdx + 3) = col.A;
            }
        }
    }

    public static void DrawRect(int x, int y, int w, int h, int lineSize = 1)
    {
        CheckReady();

        lineSize = Calc.Max(lineSize, 1);

        if (lineSize == 1)
        {
            HLine(x - 1, x + w, y - 1); // Top
            HLine(x - 1, x + w, y + h); // Down
            VLine(y, y + h, x - 1); // Left
            VLine(y - 1, y + h + 1, x + w); // Right
        }
        else
        {
            FillRect(x - lineSize, y - lineSize, w + lineSize, lineSize); // Top
            FillRect(x, y + h, w + lineSize, lineSize); // Down
            FillRect(x - lineSize, y, lineSize, h + lineSize); // Left
            FillRect(x + w, y - lineSize, lineSize, h + lineSize); // Right
        }
    }

    public static void DrawLine(int x0, int y0, int x1, int y1, int lineSize = 1)
    {
        CheckReady();

        void OnePxLine()
        {
            int dx = Calc.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Calc.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                PixelSet(x0, y0);
                if (x0 == x1 && y0 == y1) break;
                var e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        void ThickLine()
        {
            int dx = Calc.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Calc.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            float ed = dx + dy == 0 ? 1 : Calc.Sqrt((float)dx * dx + (float)dy * dy);

            for (lineSize = (lineSize + 1) / 2; ;)
            {
                PixelSet(x0, y0);
                var e2 = err;
                var x2 = x0;
                if (2 * e2 >= -dx)
                {
                    int y2;
                    for (e2 += dy, y2 = y0; e2 < ed * lineSize && (y1 != y2 || dx > dy); e2 += dx)
                        PixelSet(x0, y2);
                    if (x0 == x1) break;
                    e2 = err;
                    err -= dy;
                    x0 += sx;
                }

                if (2 * e2 <= dy)
                {
                    for (e2 = dx - e2; e2 < ed * lineSize && (x1 != x2 || dx < dy); e2 += dy)
                        PixelSet(x2 += sx, y0);
                    if (y0 == y1) break;
                    err += dx;
                    y0 += sy;
                }
            }
        }

        if (lineSize == 1)
        {
            OnePxLine();
            return;
        }

        ThickLine();
    }

    public static void DrawCircle(int centerX, int centerY, int radius)
    {
        CheckReady();

        if (radius > 0)
        {
            int x = -radius, y = 0, err = 2 - 2 * radius;
            do
            {
                PixelSet(centerX - x, centerY + y);
                PixelSet(centerX - y, centerY - x);
                PixelSet(centerX + x, centerY - y);
                PixelSet(centerX + y, centerY + x);

                radius = err;
                if (radius <= y) err += ++y * 2 + 1;
                if (radius > x || err > y) err += ++x * 2 + 1;
            } while (x < 0);
        }
        else
        {
            PixelSet(centerX, centerY);
        }
    }

    public static void FillCircle(int centerX, int centerY, int radius)
    {
        CheckReady();

        if (radius < 0 || centerX < -radius || centerY < -radius || centerX - _clipRect.Width > radius ||
            centerY - _clipRect.Height > radius)
        {
            return;
        }

        if (radius > 0)
        {
            int x0 = 0;
            int y0 = radius;
            int d = 3 - 2 * radius;

            while (y0 >= x0)
            {
                HLine(centerX - y0, centerX + y0, centerY - x0);

                if (x0 > 0)
                {
                    HLine(centerX - y0, centerX + y0, centerY + x0);
                }

                if (d < 0)
                {
                    d += 4 * x0++ + 6;
                }
                else
                {
                    if (x0 != y0)
                    {
                        HLine(centerX - x0, centerX + x0, centerY - y0);
                        HLine(centerX - x0, centerX + x0, centerY + y0);
                    }

                    d += 4 * (x0++ - y0--) + 10;
                }
            }
        }
        else
        {
            PixelSet(centerX, centerY);
        }
    }

    public static void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, int lineSize = 1)
    {
        CheckReady();

        DrawLine(x1, y1, x2, y2, lineSize);
        DrawLine(x2, y2, x3, y3, lineSize);
        DrawLine(x3, y3, x1, y1, lineSize);
    }

    public static void ColorAdd(byte r, byte g, byte b, byte a)
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        for (int i = 0; i < _pixelsSizeInBytes / 4; ++i)
        {
            byte* ptrIdx = ptr + i * 4;

            var sb = *ptrIdx + b;
            var sg = *(ptrIdx + 1) + g;
            var sr = *(ptrIdx + 2) + r;
            var sa = *(ptrIdx + 3) + a;

            *ptrIdx = (byte)Calc.Clamp(sb, 0, 255);
            *(ptrIdx + 1) = (byte)Calc.Clamp(sg, 0, 255);
            *(ptrIdx + 2) = (byte)Calc.Clamp(sr, 0, 255);
            *(ptrIdx + 3) = (byte)Calc.Clamp(sa, 0, 255);
        }
    }

    public static void ColorMult(float r, float g, float b, float a)
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        for (int i = 0; i < _pixelsSizeInBytes / 4; ++i)
        {
            byte* ptrIdx = ptr + i * 4;

            if (*(ptrIdx + 3) == 0)
            {
                continue;
            }

            var sb = *ptrIdx * b;
            var sg = *(ptrIdx + 1) * g;
            var sr = *(ptrIdx + 2) * r;
            var sa = *(ptrIdx + 3) * a;

            *ptrIdx = (byte)Calc.Clamp(sb, 0, 255);
            *(ptrIdx + 1) = (byte)Calc.Clamp(sg, 0, 255);
            *(ptrIdx + 2) = (byte)Calc.Clamp(sr, 0, 255);
            *(ptrIdx + 3) = (byte)Calc.Clamp(sa, 0, 255);
        }
    }

    public static void PixelShift(int shiftX, int shiftY)
    {
        CheckReady();

        Span<byte> copy = GetCopy(_pixelsSizeInBytes);

        int sw = _surfaceW;
        int sh = _surfaceH;

        byte* ptr = (byte*)_pixels;

        fixed (byte* copyPtr = &MemoryMarshal.GetReference(copy))
        {
            for (int x = 0; x < sw; ++x)
            {
                var px = x - shiftX + sw;
                while (px < 0)
                {
                    px += sw;
                }

                px %= sw;
                for (int y = 0; y < sh; ++y)
                {
                    var py = y - shiftY + sh;
                    while (py < 0)
                    {
                        py += sh;
                    }

                    py %= sh;

                    int oldIdx = (px + py * sw) * 4;
                    int newIdx = (x + y * sw) * 4;

                    byte* ptrIdx = ptr + newIdx;
                    byte* copyIdx = copyPtr + oldIdx;

                    *ptrIdx = *copyIdx;
                    *(ptrIdx + 1) = *(copyIdx + 1);
                    *(ptrIdx + 2) = *(copyIdx + 2);
                    *(ptrIdx + 3) = *(copyIdx + 3);
                }
            }
        }
    }

    public static void Blit(
        Span<byte> pastePixels,
        int pastePixelsW,
        int pastePixelsH,
        int x,
        int y,
        Rect region = default,
        int w = 0,
        int h = 0,
        bool flip = false
    )
    {


        fixed (byte* paste = &MemoryMarshal.GetReference(pastePixels))
        {
            Blit(
                paste,
                pastePixelsW,
                pastePixelsH,
                x, y,
                region,
                w,
                h,
                flip
            );
        }
    }

    public static unsafe void Blit(
        byte* pastePixels,
        int pastePixelsW,
        int pastePixelsH,
        int x,
        int y,
        Rect region = default,
        int w = 0,
        int h = 0,
        bool flip = false
    )
    {
        CheckReady();

        if (region.IsEmpty)
        {
            region = new Rect(0, 0, pastePixelsW, pastePixelsH);
        }

        if (w == 0)
        {
            w = region.Width;
        }

        if (h == 0)
        {
            h = region.Height;
        }

        float factorW = (float)w / region.Width;
        float factorH = (float)h / region.Height;

        var minX = Math.Max(x, _clipRect.Left);
        var minY = Math.Max(y, _clipRect.Top);
        var maxX = Math.Min(x + w, _clipRect.Right);
        var maxY = Math.Min(y + h, _clipRect.Bottom);

        int sw = _surfaceW;

        ref var col = ref _drawColor;

        byte* ptr = (byte*)_pixels;

        if (!flip)
        {
            for (int px = minX; px < maxX; ++px)
            {
                for (int py = minY; py < maxY; ++py)
                {
                    byte* srcIdx = pastePixels + (region.X + (int)((px - x) / factorW) +
                                            (region.Y + (int)((py - y) / factorH)) * pastePixelsW) * 4;

                    if (*(srcIdx + 3) == 0)
                    {
                        continue;
                    }

                    byte* ptrIdx = ptr + (px + py * sw) * 4;

                    if (col.Abgr == 1)
                    {
                        *ptrIdx = *srcIdx;
                        *(ptrIdx + 1) = *(srcIdx + 1);
                        *(ptrIdx + 2) = *(srcIdx + 2);
                        *(ptrIdx + 3) = *(srcIdx + 3);
                    }
                    else
                    {
                        var sbf = *srcIdx / 255.0f;
                        var sgf = *(srcIdx + 1) / 255.0f;
                        var srf = *(srcIdx + 2) / 255.0f;
                        var saf = *(srcIdx + 3) / 255.0f;

                        var bf = col.B / 255.0f;
                        var gf = col.G / 255.0f;
                        var rf = col.R / 255.0f;
                        var af = col.A / 255.0f;

                        *ptrIdx = (byte)(sbf * bf * 255.0f);
                        *(ptrIdx + 1) = (byte)(sgf * gf * 255.0f);
                        *(ptrIdx + 2) = (byte)(srf * rf * 255.0f);
                        *(ptrIdx + 3) = (byte)(saf * af * 255.0f);
                    }
                }
            }
        }
        else
        {
            var startPixX = region.Right - 1;

            for (int px = minX; px < maxX; ++px)
            {
                for (int py = minY; py < maxY; ++py)
                {
                    byte* srcIdx = pastePixels + (startPixX + (int)((px - x) / factorW) +
                                            (region.Y + (int)((py - y) / factorH)) * pastePixelsW) * 4;

                    if (*(srcIdx + 3) == 0)
                    {
                        continue;
                    }

                    byte* ptrIdx = ptr + (px + py * sw) * 4;

                    if (col.Abgr == 1)
                    {
                        *ptrIdx = *srcIdx;
                        *(ptrIdx + 1) = *(srcIdx + 1);
                        *(ptrIdx + 2) = *(srcIdx + 2);
                        *(ptrIdx + 3) = *(srcIdx + 3);
                    }
                    else
                    {
                        var sbf = *srcIdx / 255.0f;
                        var sgf = *(srcIdx + 1) / 255.0f;
                        var srf = *(srcIdx + 2) / 255.0f;
                        var saf = *(srcIdx + 3) / 255.0f;

                        var bf = col.B / 255.0f;
                        var gf = col.G / 255.0f;
                        var rf = col.R / 255.0f;
                        var af = col.A / 255.0f;

                        *ptrIdx = (byte)(sbf * bf * 255.0f);
                        *(ptrIdx + 1) = (byte)(sgf * gf * 255.0f);
                        *(ptrIdx + 2) = (byte)(srf * rf * 255.0f);
                        *(ptrIdx + 3) = (byte)(saf * af * 255.0f);
                    }
                }
            }
        }
    }

    // ==========================================================
    // FILTERS
    // ==========================================================

    public static void ConvertRgbaToBgra(bool premultiplyAlpha = true)
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        var len = _pixelsSizeInBytes - 4;
        for (int i = 0; i <= len; i += 4)
        {
            byte r = *(ptr + i);
            byte g = *(ptr + i + 1);
            byte b = *(ptr + i + 2);
            byte a = *(ptr + i + 3);

            if (!premultiplyAlpha)
            {
                *(ptr + i) = b;
                *(ptr + i + 1) = g;
                *(ptr + i + 2) = r;
            }
            else
            {
                *(ptr + i) = (byte)(b * a / 255);
                *(ptr + i + 1) = (byte)(g * a / 255);
                *(ptr + i + 2) = (byte)(r * a / 255);
            }

            *(ptr + i + 3) = a;
        }
    }

    public static void ConvertBgraToRgba()
    {
        CheckReady();

        byte* ptr = (byte*)_pixels;

        var len = _pixelsSizeInBytes - 4;
        for (int i = 0; i <= len; i += 4)
        {
            byte b = *(ptr + i);
            byte g = *(ptr + i + 1);
            byte r = *(ptr + i + 2);
            byte a = *(ptr + i + 3);

            *(ptr + i) = r;
            *(ptr + i + 1) = g;
            *(ptr + i + 2) = b;
            *(ptr + i + 3) = a;
        }
    }

    public static void DropShadow(int offsetX, int offsetY, Color color)
    {
        CheckReady();

        byte r = color.R;
        byte g = color.G;
        byte b = color.B;
        byte a = color.A;

        Span<byte> copy = GetCopy(_pixelsSizeInBytes);

        PixelShift(offsetX, offsetY);
        ColorAdd(255, 255, 255, 0);

        ColorMult(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);

        Blit(copy, _surfaceW, _surfaceH, 0, 0);
    }

    private static Span<byte> GetCopy(int length)
    {
        if (length > AuxBufferSize)
        {
            throw new InvalidOperationException(
                $"Blitter GetCopy: Overflow. Length {length} is larger than max {AuxBufferSize}");
        }

        AuxBuffers[_auxBuffersIdx] ??= new byte[AuxBufferSize];

        Unsafe.CopyBlockUnaligned(Unsafe.AsPointer(ref AuxBuffers[_auxBuffersIdx]), (void*)_pixels, (uint)length);

        var result = new Span<byte>(AuxBuffers[_auxBuffersIdx], 0, length);

        _auxBuffersIdx++;

        if (_auxBuffersIdx > AuxBufferStackSize - 1)
        {
            _auxBuffersIdx = 0;
        }

        return result;
    }
}