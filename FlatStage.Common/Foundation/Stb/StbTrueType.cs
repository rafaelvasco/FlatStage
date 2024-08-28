﻿#pragma warning disable CS0162, CS8618, CA2014, CS8625, CS0649
#nullable disable

using System.Runtime.InteropServices;

namespace Stb;

/* Generated Bitmap */
unsafe partial class StbTrueType
{
    public static void stbtt__fill_active_edges_new(float* scanline, float* scanline_fill, int len,
        stbtt__active_edge* e, float y_top)
    {
        var y_bottom = y_top + 1;
        while (e != null)
        {
            if (e->fdx == 0)
            {
                var x0 = e->fx;
                if (x0 < len)
                {
                    if (x0 >= 0)
                    {
                        stbtt__handle_clipped_edge(scanline, (int)x0, e, x0, y_top, x0, y_bottom);
                        stbtt__handle_clipped_edge(scanline_fill - 1, (int)x0 + 1, e, x0, y_top, x0, y_bottom);
                    }
                    else
                    {
                        stbtt__handle_clipped_edge(scanline_fill - 1, 0, e, x0, y_top, x0, y_bottom);
                    }
                }
            }
            else
            {
                var x0 = e->fx;
                var dx = e->fdx;
                var xb = x0 + dx;
                float x_top = 0;
                float x_bottom = 0;
                float sy0 = 0;
                float sy1 = 0;
                var dy = e->fdy;
                if (e->sy > y_top)
                {
                    x_top = x0 + dx * (e->sy - y_top);
                    sy0 = e->sy;
                }
                else
                {
                    x_top = x0;
                    sy0 = y_top;
                }

                if (e->ey < y_bottom)
                {
                    x_bottom = x0 + dx * (e->ey - y_top);
                    sy1 = e->ey;
                }
                else
                {
                    x_bottom = xb;
                    sy1 = y_bottom;
                }

                if (x_top >= 0 && x_bottom >= 0 && x_top < len && x_bottom < len)
                {
                    if ((int)x_top == (int)x_bottom)
                    {
                        float height = 0;
                        var x = (int)x_top;
                        height = (sy1 - sy0) * e->direction;
                        scanline[x] += stbtt__position_trapezoid_area(height, x_top, x + 1.0f, x_bottom, x + 1.0f);
                        scanline_fill[x] += height;
                    }
                    else
                    {
                        var x = 0;
                        var x1 = 0;
                        var x2 = 0;
                        float y_crossing = 0;
                        float y_final = 0;
                        float step = 0;
                        float sign = 0;
                        float area = 0;
                        if (x_top > x_bottom)
                        {
                            float t = 0;
                            sy0 = y_bottom - (sy0 - y_top);
                            sy1 = y_bottom - (sy1 - y_top);
                            t = sy0;
                            sy0 = sy1;
                            sy1 = t;
                            t = x_bottom;
                            x_bottom = x_top;
                            x_top = t;
                            dx = -dx;
                            dy = -dy;
                            t = x0;
                            x0 = xb;
                            xb = t;
                        }

                        x1 = (int)x_top;
                        x2 = (int)x_bottom;
                        y_crossing = y_top + dy * (x1 + 1 - x0);
                        y_final = y_top + dy * (x2 - x0);
                        if (y_crossing > y_bottom)
                            y_crossing = y_bottom;
                        sign = e->direction;
                        area = sign * (y_crossing - sy0);
                        scanline[x1] += stbtt__sized_triangle_area(area, x1 + 1 - x_top);
                        if (y_final > y_bottom)
                        {
                            y_final = y_bottom;
                            dy = (y_final - y_crossing) / (x2 - (x1 + 1));
                        }

                        step = sign * dy * 1;
                        for (x = x1 + 1; x < x2; ++x)
                        {
                            scanline[x] += area + step / 2;
                            area += step;
                        }

                        scanline[x2] += area + sign *
                            stbtt__position_trapezoid_area(sy1 - y_final, x2, x2 + 1.0f, x_bottom, x2 + 1.0f);
                        scanline_fill[x2] += sign * (sy1 - sy0);
                    }
                }
                else
                {
                    var x = 0;
                    for (x = 0; x < len; ++x)
                    {
                        var y0 = y_top;
                        float x1 = x;
                        float x2 = x + 1;
                        var x3 = xb;
                        var y3 = y_bottom;
                        var y1 = (x - x0) / dx + y_top;
                        var y2 = (x + 1 - x0) / dx + y_top;
                        if (x0 < x1 && x3 > x2)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x1, y1);
                            stbtt__handle_clipped_edge(scanline, x, e, x1, y1, x2, y2);
                            stbtt__handle_clipped_edge(scanline, x, e, x2, y2, x3, y3);
                        }
                        else if (x3 < x1 && x0 > x2)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x2, y2);
                            stbtt__handle_clipped_edge(scanline, x, e, x2, y2, x1, y1);
                            stbtt__handle_clipped_edge(scanline, x, e, x1, y1, x3, y3);
                        }
                        else if (x0 < x1 && x3 > x1)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x1, y1);
                            stbtt__handle_clipped_edge(scanline, x, e, x1, y1, x3, y3);
                        }
                        else if (x3 < x1 && x0 > x1)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x1, y1);
                            stbtt__handle_clipped_edge(scanline, x, e, x1, y1, x3, y3);
                        }
                        else if (x0 < x2 && x3 > x2)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x2, y2);
                            stbtt__handle_clipped_edge(scanline, x, e, x2, y2, x3, y3);
                        }
                        else if (x3 < x2 && x0 > x2)
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x2, y2);
                            stbtt__handle_clipped_edge(scanline, x, e, x2, y2, x3, y3);
                        }
                        else
                        {
                            stbtt__handle_clipped_edge(scanline, x, e, x0, y0, x3, y3);
                        }
                    }
                }
            }

            e = e->next;
        }
    }

    public static void stbtt__handle_clipped_edge(float* scanline, int x, stbtt__active_edge* e, float x0, float y0,
        float x1, float y1)
    {
        if (y0 == y1)
            return;
        if (y0 > e->ey)
            return;
        if (y1 < e->sy)
            return;
        if (y0 < e->sy)
        {
            x0 += (x1 - x0) * (e->sy - y0) / (y1 - y0);
            y0 = e->sy;
        }

        if (y1 > e->ey)
        {
            x1 += (x1 - x0) * (e->ey - y1) / (y1 - y0);
            y1 = e->ey;
        }

        if (x0 <= x && x1 <= x)
        {
            scanline[x] += e->direction * (y1 - y0);
        }
        else if (x0 >= x + 1 && x1 >= x + 1)
        {
        }
        else
        {
            scanline[x] += e->direction * (y1 - y0) * (1 - (x0 - x + (x1 - x)) / 2);
        }
    }

    public static void stbtt__rasterize(stbtt__bitmap* result, stbtt__point* pts, int* wcount, int windings,
        float scale_x, float scale_y, float shift_x, float shift_y, int off_x, int off_y, int invert,
        void* userdata)
    {
        var y_scale_inv = invert != 0 ? -scale_y : scale_y;
        stbtt__edge* e;
        var n = 0;
        var i = 0;
        var j = 0;
        var k = 0;
        var m = 0;
        var vsubsample = 1;
        n = 0;
        for (i = 0; i < windings; ++i) n += wcount[i];

        e = (stbtt__edge*)CRuntime.Malloc((ulong)(sizeof(stbtt__edge) * (n + 1)));
        if (e == null)
            return;
        n = 0;
        m = 0;
        for (i = 0; i < windings; ++i)
        {
            var p = pts + m;
            m += wcount[i];
            j = wcount[i] - 1;
            for (k = 0; k < wcount[i]; j = k++)
            {
                var a = k;
                var b = j;
                if (p[j].y == p[k].y)
                    continue;
                e[n].invert = 0;
                if (invert != 0 && p[j].y > p[k].y ||
                    invert == 0 && p[j].y < p[k].y)
                {
                    e[n].invert = 1;
                    a = j;
                    b = k;
                }

                e[n].x0 = p[a].x * scale_x + shift_x;
                e[n].y0 = (p[a].y * y_scale_inv + shift_y) * vsubsample;
                e[n].x1 = p[b].x * scale_x + shift_x;
                e[n].y1 = (p[b].y * y_scale_inv + shift_y) * vsubsample;
                ++n;
            }
        }

        stbtt__sort_edges(e, n);
        stbtt__rasterize_sorted_edges(result, e, n, vsubsample, off_x, off_y, userdata);
        CRuntime.Free(e);
    }

    public static void stbtt__rasterize_sorted_edges(stbtt__bitmap* result, stbtt__edge* e, int n, int vsubsample,
        int off_x, int off_y, void* userdata)
    {
        var hh = new stbtt__hheap();
        stbtt__active_edge* active = null;
        var y = 0;
        var j = 0;
        var i = 0;
        var scanline_data = stackalloc float[129];
        float* scanline;
        float* scanline2;
        if (result->w > 64)
            scanline = (float*)CRuntime.Malloc((ulong)((result->w * 2 + 1) * sizeof(float)));
        else
            scanline = scanline_data;
        scanline2 = scanline + result->w;
        y = off_y;
        e[n].y0 = (float)(off_y + result->h) + 1;
        while (j < result->h)
        {
            var scan_y_top = y + 0.0f;
            var scan_y_bottom = y + 1.0f;
            var step = &active;
            CRuntime.Memset(scanline, 0, (ulong)(result->w * sizeof(float)));
            CRuntime.Memset(scanline2, 0, (ulong)((result->w + 1) * sizeof(float)));
            while (*step != null)
            {
                var z = *step;
                if (z->ey <= scan_y_top)
                {
                    *step = z->next;
                    z->direction = 0;
                    stbtt__hheap_free(&hh, z);
                }
                else
                {
                    step = &(*step)->next;
                }
            }

            while (e->y0 <= scan_y_bottom)
            {
                if (e->y0 != e->y1)
                {
                    var z = stbtt__new_active(&hh, e, off_x, scan_y_top, userdata);
                    if (z != null)
                    {
                        if (j == 0 && off_y != 0)
                            if (z->ey < scan_y_top)
                                z->ey = scan_y_top;

                        z->next = active;
                        active = z;
                    }
                }

                ++e;
            }

            if (active != null)
                stbtt__fill_active_edges_new(scanline, scanline2 + 1, result->w, active, scan_y_top);
            {
                float sum = 0;
                for (i = 0; i < result->w; ++i)
                {
                    float k = 0;
                    var m = 0;
                    sum += scanline2[i];
                    k = scanline[i] + sum;
                    k = CRuntime.Fabs(k) * 255 + 0.5f;
                    m = (int)k;
                    if (m > 255)
                        m = 255;
                    result->pixels[j * result->stride + i] = (byte)m;
                }
            }

            step = &active;
            while (*step != null)
            {
                var z = *step;
                z->fx += z->fdx;
                step = &(*step)->next;
            }

            ++y;
            ++j;
        }

        stbtt__hheap_cleanup(&hh, userdata);
        if (scanline != scanline_data)
            CRuntime.Free(scanline);
    }

    public static void stbtt__sort_edges(stbtt__edge* p, int n)
    {
        stbtt__sort_edges_quicksort(p, n);
        stbtt__sort_edges_ins_sort(p, n);
    }

    public static void stbtt__sort_edges_ins_sort(stbtt__edge* p, int n)
    {
        var i = 0;
        var j = 0;
        for (i = 1; i < n; ++i)
        {
            var t = p[i];
            var a = &t;
            j = i;
            while (j > 0)
            {
                var b = &p[j - 1];
                var c = a->y0 < b->y0 ? 1 : 0;
                if (c == 0)
                    break;
                p[j] = p[j - 1];
                --j;
            }

            if (i != j)
                p[j] = t;
        }
    }

    public static void stbtt__sort_edges_quicksort(stbtt__edge* p, int n)
    {
        while (n > 12)
        {
            var t = new stbtt__edge();
            var c01 = 0;
            var c12 = 0;
            var c = 0;
            var m = 0;
            var i = 0;
            var j = 0;
            m = n >> 1;
            c01 = (&p[0])->y0 < (&p[m])->y0 ? 1 : 0;
            c12 = (&p[m])->y0 < (&p[n - 1])->y0 ? 1 : 0;
            if (c01 != c12)
            {
                var z = 0;
                c = (&p[0])->y0 < (&p[n - 1])->y0 ? 1 : 0;
                z = c == c12 ? 0 : n - 1;
                t = p[z];
                p[z] = p[m];
                p[m] = t;
            }

            t = p[0];
            p[0] = p[m];
            p[m] = t;
            i = 1;
            j = n - 1;
            for (; ; )
            {
                for (; ; ++i)
                    if (!((&p[i])->y0 < (&p[0])->y0))
                        break;

                for (; ; --j)
                    if (!((&p[0])->y0 < (&p[j])->y0))
                        break;

                if (i >= j)
                    break;
                t = p[i];
                p[i] = p[j];
                p[j] = t;
                ++i;
                --j;
            }

            if (j < n - i)
            {
                stbtt__sort_edges_quicksort(p, j);
                p = p + i;
                n = n - i;
            }
            else
            {
                stbtt__sort_edges_quicksort(p + i, n - i);
                n = j;
            }
        }
    }

    public static void stbtt_Rasterize(stbtt__bitmap* result, float flatness_in_pixels, stbtt_vertex* vertices,
        int num_verts, float scale_x, float scale_y, float shift_x, float shift_y, int x_off, int y_off, int invert,
        void* userdata)
    {
        var scale = scale_x > scale_y ? scale_y : scale_x;
        var winding_count = 0;
        int* winding_lengths = null;
        var windings = stbtt_FlattenCurves(vertices, num_verts, flatness_in_pixels / scale, &winding_lengths,
            &winding_count, userdata);
        if (windings != null)
        {
            stbtt__rasterize(result, windings, winding_lengths, winding_count, scale_x, scale_y, shift_x, shift_y,
                x_off, y_off, (int)invert, userdata);
            CRuntime.Free(winding_lengths);
            CRuntime.Free(windings);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__active_edge
    {
        public stbtt__active_edge* next;
        public float fx;
        public float fdx;
        public float fdy;
        public float direction;
        public float sy;
        public float ey;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__bitmap
    {
        public int w;
        public int h;
        public int stride;
        public byte* pixels;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__edge
    {
        public float x0;
        public float y0;
        public float x1;
        public float y1;
        public int invert;
    }
}

/* Generated Rect Pack */
unsafe partial class StbTrueType
{
    public static void stbrp_init_target(stbrp_context* con, int pw, int ph, stbrp_node* nodes, int num_nodes)
    {
        con->width = pw;
        con->height = ph;
        con->x = 0;
        con->y = 0;
        con->bottom_y = 0;
    }

    public static void stbrp_pack_rects(stbrp_context* con, stbrp_rect* rects, int num_rects)
    {
        var i = 0;
        for (i = 0; i < num_rects; ++i)
        {
            if (con->x + rects[i].w > con->width)
            {
                con->x = 0;
                con->y = con->bottom_y;
            }

            if (con->y + rects[i].h > con->height)
                break;
            rects[i].x = con->x;
            rects[i].y = con->y;
            rects[i].was_packed = 1;
            con->x += rects[i].w;
            if (con->y + rects[i].h > con->bottom_y)
                con->bottom_y = con->y + rects[i].h;
        }

        for (; i < num_rects; ++i) rects[i].was_packed = 0;
    }

    public static int stbtt_PackBegin(stbtt_pack_context spc, byte* pixels, int pw, int ph, int stride_in_bytes,
        int padding, void* alloc_context)
    {
        var context = (stbrp_context*)CRuntime.Malloc((ulong)sizeof(stbrp_context));
        var num_nodes = pw - padding;
        var nodes = (stbrp_node*)CRuntime.Malloc((ulong)(sizeof(stbrp_node) * num_nodes));
        if (context == null || nodes == null)
        {
            if (context != null)
                CRuntime.Free(context);
            if (nodes != null)
                CRuntime.Free(nodes);
            return 0;
        }

        spc.user_allocator_context = alloc_context;
        spc.width = pw;
        spc.height = ph;
        spc.pixels = pixels;
        spc.pack_info = context;
        spc.nodes = nodes;
        spc.padding = padding;
        spc.stride_in_bytes = stride_in_bytes != 0 ? stride_in_bytes : pw;
        spc.h_oversample = 1;
        spc.v_oversample = 1;
        spc.skip_missing = 0;
        stbrp_init_target(context, pw - padding, ph - padding, nodes, num_nodes);
        if (pixels != null)
            CRuntime.Memset(pixels, 0, (ulong)(pw * ph));
        return 1;
    }

    public static void stbtt_PackEnd(stbtt_pack_context spc)
    {
        CRuntime.Free(spc.nodes);
        CRuntime.Free(spc.pack_info);
    }

    public static int stbtt_PackFontRange(stbtt_pack_context spc, byte* fontdata, int font_index, float font_size,
        int first_unicode_codepoint_in_range, int num_chars_in_range, stbtt_packedchar* chardata_for_range)
    {
        var range = new stbtt_pack_range();
        range.first_unicode_codepoint_in_range = first_unicode_codepoint_in_range;
        range.array_of_unicode_codepoints = null;
        range.num_chars = num_chars_in_range;
        range.chardata_for_range = chardata_for_range;
        range.font_size = font_size;
        return stbtt_PackFontRanges(spc, fontdata, font_index, &range, 1);
    }

    public static int stbtt_PackFontRanges(stbtt_pack_context spc, byte* fontdata, int font_index,
        stbtt_pack_range* ranges, int num_ranges)
    {
        var info = new stbtt_fontinfo();
        var i = 0;
        var j = 0;
        var n = 0;
        var return_value = 1;
        stbrp_rect* rects;
        for (i = 0; i < num_ranges; ++i)
            for (j = 0; j < ranges[i].num_chars; ++j)
                ranges[i].chardata_for_range[j].x0 = ranges[i].chardata_for_range[j].y0 =
                    ranges[i].chardata_for_range[j].x1 = ranges[i].chardata_for_range[j].y1 = 0;

        n = 0;
        for (i = 0; i < num_ranges; ++i) n += ranges[i].num_chars;

        rects = (stbrp_rect*)CRuntime.Malloc((ulong)(sizeof(stbrp_rect) * n));
        if (rects == null)
            return 0;
        info.userdata = spc.user_allocator_context;
        stbtt_InitFont(info, fontdata, stbtt_GetFontOffsetForIndex(fontdata, font_index));
        n = stbtt_PackFontRangesGatherRects(spc, info, ranges, num_ranges, rects);
        stbtt_PackFontRangesPackRects(spc, rects, n);
        return_value = stbtt_PackFontRangesRenderIntoRects(spc, info, ranges, num_ranges, rects);
        CRuntime.Free(rects);
        return return_value;
    }

    public static int stbtt_PackFontRangesGatherRects(stbtt_pack_context spc, stbtt_fontinfo info,
        stbtt_pack_range* ranges, int num_ranges, stbrp_rect* rects)
    {
        var i = 0;
        var j = 0;
        var k = 0;
        var missing_glyph_added = 0;
        k = 0;
        for (i = 0; i < num_ranges; ++i)
        {
            var fh = ranges[i].font_size;
            var scale = fh > 0 ? stbtt_ScaleForPixelHeight(info, fh) : stbtt_ScaleForMappingEmToPixels(info, -fh);
            ranges[i].h_oversample = (byte)spc.h_oversample;
            ranges[i].v_oversample = (byte)spc.v_oversample;
            for (j = 0; j < ranges[i].num_chars; ++j)
            {
                var x0 = 0;
                var y0 = 0;
                var x1 = 0;
                var y1 = 0;
                var codepoint = ranges[i].array_of_unicode_codepoints == null
                    ? ranges[i].first_unicode_codepoint_in_range + j
                    : ranges[i].array_of_unicode_codepoints[j];
                var glyph = stbtt_FindGlyphIndex(info, codepoint);
                if (glyph == 0 && (spc.skip_missing != 0 || missing_glyph_added != 0))
                {
                    rects[k].w = rects[k].h = 0;
                }
                else
                {
                    stbtt_GetGlyphBitmapBoxSubpixel(info, glyph, scale * spc.h_oversample, scale * spc.v_oversample,
                        0, 0, &x0, &y0, &x1, &y1);
                    rects[k].w = (int)(x1 - x0 + spc.padding + spc.h_oversample - 1);
                    rects[k].h = (int)(y1 - y0 + spc.padding + spc.v_oversample - 1);
                    if (glyph == 0)
                        missing_glyph_added = 1;
                }

                ++k;
            }
        }

        return k;
    }

    public static void stbtt_PackFontRangesPackRects(stbtt_pack_context spc, stbrp_rect* rects, int num_rects)
    {
        stbrp_pack_rects((stbrp_context*)spc.pack_info, rects, num_rects);
    }

    public static int stbtt_PackFontRangesRenderIntoRects(stbtt_pack_context spc, stbtt_fontinfo info,
        stbtt_pack_range* ranges, int num_ranges, stbrp_rect* rects)
    {
        var i = 0;
        var j = 0;
        var k = 0;
        var missing_glyph = -1;
        var return_value = 1;
        var old_h_over = (int)spc.h_oversample;
        var old_v_over = (int)spc.v_oversample;
        k = 0;
        for (i = 0; i < num_ranges; ++i)
        {
            var fh = ranges[i].font_size;
            var scale = fh > 0 ? stbtt_ScaleForPixelHeight(info, fh) : stbtt_ScaleForMappingEmToPixels(info, -fh);
            float recip_h = 0;
            float recip_v = 0;
            float sub_x = 0;
            float sub_y = 0;
            spc.h_oversample = ranges[i].h_oversample;
            spc.v_oversample = ranges[i].v_oversample;
            recip_h = 1.0f / spc.h_oversample;
            recip_v = 1.0f / spc.v_oversample;
            sub_x = stbtt__oversample_shift((int)spc.h_oversample);
            sub_y = stbtt__oversample_shift((int)spc.v_oversample);
            for (j = 0; j < ranges[i].num_chars; ++j)
            {
                var r = &rects[k];
                if (r->was_packed != 0 && r->w != 0 && r->h != 0)
                {
                    var bc = &ranges[i].chardata_for_range[j];
                    var advance = 0;
                    var lsb = 0;
                    var x0 = 0;
                    var y0 = 0;
                    var x1 = 0;
                    var y1 = 0;
                    var codepoint = ranges[i].array_of_unicode_codepoints == null
                        ? ranges[i].first_unicode_codepoint_in_range + j
                        : ranges[i].array_of_unicode_codepoints[j];
                    var glyph = stbtt_FindGlyphIndex(info, codepoint);
                    var pad = spc.padding;
                    r->x += pad;
                    r->y += pad;
                    r->w -= pad;
                    r->h -= pad;
                    stbtt_GetGlyphHMetrics(info, glyph, &advance, &lsb);
                    stbtt_GetGlyphBitmapBox(info, glyph, scale * spc.h_oversample, scale * spc.v_oversample, &x0,
                        &y0, &x1, &y1);
                    stbtt_MakeGlyphBitmapSubpixel(info, spc.pixels + r->x + r->y * spc.stride_in_bytes,
                        (int)(r->w - spc.h_oversample + 1), (int)(r->h - spc.v_oversample + 1),
                        spc.stride_in_bytes, scale * spc.h_oversample, scale * spc.v_oversample, 0, 0, glyph);
                    if (spc.h_oversample > 1)
                        stbtt__h_prefilter(spc.pixels + r->x + r->y * spc.stride_in_bytes, r->w, r->h,
                            spc.stride_in_bytes, spc.h_oversample);
                    if (spc.v_oversample > 1)
                        stbtt__v_prefilter(spc.pixels + r->x + r->y * spc.stride_in_bytes, r->w, r->h,
                            spc.stride_in_bytes, spc.v_oversample);
                    bc->x0 = (ushort)(short)r->x;
                    bc->y0 = (ushort)(short)r->y;
                    bc->x1 = (ushort)(short)(r->x + r->w);
                    bc->y1 = (ushort)(short)(r->y + r->h);
                    bc->xadvance = scale * advance;
                    bc->xoff = x0 * recip_h + sub_x;
                    bc->yoff = y0 * recip_v + sub_y;
                    bc->xoff2 = (x0 + r->w) * recip_h + sub_x;
                    bc->yoff2 = (y0 + r->h) * recip_v + sub_y;
                    if (glyph == 0)
                        missing_glyph = j;
                }
                else if (spc.skip_missing != 0)
                {
                    return_value = 0;
                }
                else if (r->was_packed != 0 && r->w == 0 && r->h == 0 && missing_glyph >= 0)
                {
                    ranges[i].chardata_for_range[j] = ranges[i].chardata_for_range[missing_glyph];
                }
                else
                {
                    return_value = 0;
                }

                ++k;
            }
        }

        spc.h_oversample = (uint)old_h_over;
        spc.v_oversample = (uint)old_v_over;
        return return_value;
    }

    public static void stbtt_PackSetOversampling(stbtt_pack_context spc, uint h_oversample, uint v_oversample)
    {
        if (h_oversample <= 8)
            spc.h_oversample = h_oversample;
        if (v_oversample <= 8)
            spc.v_oversample = v_oversample;
    }

    public static void stbtt_PackSetSkipMissingCodepoints(stbtt_pack_context spc, int skip)
    {
        spc.skip_missing = skip;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbrp_context
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public int bottom_y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbrp_node
    {
        public byte x;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbrp_rect
    {
        public int x;
        public int y;
        public int id;
        public int w;
        public int h;
        public int was_packed;
    }

    public class stbtt_pack_context
    {
        public uint h_oversample;
        public int height;
        public void* nodes;
        public void* pack_info;
        public int padding;
        public byte* pixels;
        public int skip_missing;
        public int stride_in_bytes;
        public void* user_allocator_context;
        public uint v_oversample;
        public int width;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_pack_range
    {
        public float font_size;
        public int first_unicode_codepoint_in_range;
        public int* array_of_unicode_codepoints;
        public int num_chars;
        public stbtt_packedchar* chardata_for_range;
        public byte h_oversample;
        public byte v_oversample;
    }
}

/* Generated Heap */
unsafe partial class StbTrueType
{
    public static void* stbtt__hheap_alloc(stbtt__hheap* hh, ulong size, void* userdata)
    {
        if (hh->first_free != null)
        {
            var p = hh->first_free;
            hh->first_free = *(void**)p;
            return p;
        }

        if (hh->num_remaining_in_head_chunk == 0)
        {
            var count = size < 32 ? 2000 : size < 128 ? 800 : 100;
            var c = (stbtt__hheap_chunk*)CRuntime.Malloc((ulong)sizeof(stbtt__hheap_chunk) +
                                                          size * (ulong)count);
            if (c == null)
                return null;
            c->next = hh->head;
            hh->head = c;
            hh->num_remaining_in_head_chunk = count;
        }

        --hh->num_remaining_in_head_chunk;
        return (sbyte*)hh->head + sizeof(stbtt__hheap_chunk) + size * (ulong)hh->num_remaining_in_head_chunk;
    }

    public static void stbtt__hheap_cleanup(stbtt__hheap* hh, void* userdata)
    {
        var c = hh->head;
        while (c != null)
        {
            var n = c->next;
            CRuntime.Free(c);
            c = n;
        }
    }

    public static void stbtt__hheap_free(stbtt__hheap* hh, void* p)
    {
        *(void**)p = hh->first_free;
        hh->first_free = p;
    }

    public static stbtt__active_edge* stbtt__new_active(stbtt__hheap* hh, stbtt__edge* e, int off_x,
        float start_point, void* userdata)
    {
        var z = (stbtt__active_edge*)stbtt__hheap_alloc(hh, (ulong)sizeof(stbtt__active_edge), userdata);
        var dxdy = (e->x1 - e->x0) / (e->y1 - e->y0);
        if (z == null)
            return z;
        z->fdx = dxdy;
        z->fdy = dxdy != 0.0f ? 1.0f / dxdy : 0.0f;
        z->fx = e->x0 + dxdy * (start_point - e->y0);
        z->fx -= off_x;
        z->direction = e->invert != 0 ? 1.0f : -1.0f;
        z->sy = e->y0;
        z->ey = e->y1;
        z->next = null;
        return z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__hheap
    {
        public stbtt__hheap_chunk* head;
        public void* first_free;
        public int num_remaining_in_head_chunk;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__hheap_chunk
    {
        public stbtt__hheap_chunk* next;
    }
}

/* Generated Font Info */
unsafe partial class StbTrueType
{
    public static stbtt__buf stbtt__cid_get_glyph_subrs(stbtt_fontinfo info, int glyph_index)
    {
        var fdselect = info.fdselect;
        var nranges = 0;
        var start = 0;
        var end = 0;
        var v = 0;
        var fmt = 0;
        var fdselector = -1;
        var i = 0;
        stbtt__buf_seek(&fdselect, 0);
        fmt = stbtt__buf_get8(&fdselect);
        if (fmt == 0)
        {
            stbtt__buf_skip(&fdselect, glyph_index);
            fdselector = stbtt__buf_get8(&fdselect);
        }
        else if (fmt == 3)
        {
            nranges = (int)stbtt__buf_get(&fdselect, 2);
            start = (int)stbtt__buf_get(&fdselect, 2);
            for (i = 0; i < nranges; i++)
            {
                v = stbtt__buf_get8(&fdselect);
                end = (int)stbtt__buf_get(&fdselect, 2);
                if (glyph_index >= start && glyph_index < end)
                {
                    fdselector = v;
                    break;
                }

                start = end;
            }
        }

        if (fdselector == -1)
            stbtt__new_buf(null, 0);
        return stbtt__get_subrs(info.cff, stbtt__cff_index_get(info.fontdicts, fdselector));
    }

    public static int stbtt__close_shape(stbtt_vertex* vertices, int num_vertices, int was_off, int start_off,
        int sx, int sy, int scx, int scy, int cx, int cy)
    {
        if (start_off != 0)
        {
            if (was_off != 0)
                stbtt_setvertex(&vertices[num_vertices++], STBTT_vcurve, (cx + scx) >> 1, (cy + scy) >> 1, cx, cy);
            stbtt_setvertex(&vertices[num_vertices++], STBTT_vcurve, sx, sy, scx, scy);
        }
        else
        {
            if (was_off != 0)
                stbtt_setvertex(&vertices[num_vertices++], STBTT_vcurve, sx, sy, cx, cy);
            else
                stbtt_setvertex(&vertices[num_vertices++], STBTT_vline, sx, sy, 0, 0);
        }

        return num_vertices;
    }

    public static int stbtt__get_svg(stbtt_fontinfo info)
    {
        uint t = 0;
        if (info.svg < 0)
        {
            t = stbtt__find_table(info.data, (uint)info.fontstart, "SVG ");
            if (t != 0)
            {
                var offset = ttULONG(info.data + t + 2);
                info.svg = (int)(t + offset);
            }
            else
            {
                info.svg = 0;
            }
        }

        return info.svg;
    }

    public static int stbtt__GetCoverageIndex(byte* coverageTable, int glyph)
    {
        var coverageFormat = ttUSHORT(coverageTable);
        switch (coverageFormat)
        {
            case 1:
                {
                    var glyphCount = ttUSHORT(coverageTable + 2);
                    var l = 0;
                    var r = glyphCount - 1;
                    var m = 0;
                    var straw = 0;
                    var needle = glyph;
                    while (l <= r)
                    {
                        var glyphArray = coverageTable + 4;
                        ushort glyphID = 0;
                        m = (l + r) >> 1;
                        glyphID = ttUSHORT(glyphArray + 2 * m);
                        straw = glyphID;
                        if (needle < straw)
                            r = m - 1;
                        else if (needle > straw)
                            l = m + 1;
                        else
                            return m;
                    }

                    break;
                }

            case 2:
                {
                    var rangeCount = ttUSHORT(coverageTable + 2);
                    var rangeArray = coverageTable + 4;
                    var l = 0;
                    var r = rangeCount - 1;
                    var m = 0;
                    var strawStart = 0;
                    var strawEnd = 0;
                    var needle = glyph;
                    while (l <= r)
                    {
                        byte* rangeRecord;
                        m = (l + r) >> 1;
                        rangeRecord = rangeArray + 6 * m;
                        strawStart = ttUSHORT(rangeRecord);
                        strawEnd = ttUSHORT(rangeRecord + 2);
                        if (needle < strawStart)
                        {
                            r = m - 1;
                        }
                        else if (needle > strawEnd)
                        {
                            l = m + 1;
                        }
                        else
                        {
                            var startCoverageIndex = ttUSHORT(rangeRecord + 4);
                            return startCoverageIndex + glyph - strawStart;
                        }
                    }

                    break;
                }

            default:
                return -1;
        }

        return -1;
    }

    public static int stbtt__GetGlyfOffset(stbtt_fontinfo info, int glyph_index)
    {
        var g1 = 0;
        var g2 = 0;
        if (glyph_index >= info.numGlyphs)
            return -1;
        if (info.indexToLocFormat >= 2)
            return -1;
        if (info.indexToLocFormat == 0)
        {
            g1 = info.glyf + ttUSHORT(info.data + info.loca + glyph_index * 2) * 2;
            g2 = info.glyf + ttUSHORT(info.data + info.loca + glyph_index * 2 + 2) * 2;
        }
        else
        {
            g1 = (int)(info.glyf + ttULONG(info.data + info.loca + glyph_index * 4));
            g2 = (int)(info.glyf + ttULONG(info.data + info.loca + glyph_index * 4 + 4));
        }

        return g1 == g2 ? -1 : g1;
    }

    public static int stbtt__GetGlyphClass(byte* classDefTable, int glyph)
    {
        var classDefFormat = ttUSHORT(classDefTable);
        switch (classDefFormat)
        {
            case 1:
                {
                    var startGlyphID = ttUSHORT(classDefTable + 2);
                    var glyphCount = ttUSHORT(classDefTable + 4);
                    var classDef1ValueArray = classDefTable + 6;
                    if (glyph >= startGlyphID && glyph < startGlyphID + glyphCount)
                        return ttUSHORT(classDef1ValueArray + 2 * (glyph - startGlyphID));
                    break;
                }

            case 2:
                {
                    var classRangeCount = ttUSHORT(classDefTable + 2);
                    var classRangeRecords = classDefTable + 4;
                    var l = 0;
                    var r = classRangeCount - 1;
                    var m = 0;
                    var strawStart = 0;
                    var strawEnd = 0;
                    var needle = glyph;
                    while (l <= r)
                    {
                        byte* classRangeRecord;
                        m = (l + r) >> 1;
                        classRangeRecord = classRangeRecords + 6 * m;
                        strawStart = ttUSHORT(classRangeRecord);
                        strawEnd = ttUSHORT(classRangeRecord + 2);
                        if (needle < strawStart)
                            r = m - 1;
                        else if (needle > strawEnd)
                            l = m + 1;
                        else
                            return ttUSHORT(classRangeRecord + 4);
                    }

                    break;
                }

            default:
                return -1;
        }

        return 0;
    }

    public static int stbtt__GetGlyphGPOSInfoAdvance(stbtt_fontinfo info, int glyph1, int glyph2)
    {
        ushort lookupListOffset = 0;
        byte* lookupList;
        ushort lookupCount = 0;
        byte* data;
        var i = 0;
        var sti = 0;
        if (info.gpos == 0)
            return 0;
        data = info.data + info.gpos;
        if (ttUSHORT(data + 0) != 1)
            return 0;
        if (ttUSHORT(data + 2) != 0)
            return 0;
        lookupListOffset = ttUSHORT(data + 8);
        lookupList = data + lookupListOffset;
        lookupCount = ttUSHORT(lookupList);
        for (i = 0; i < lookupCount; ++i)
        {
            var lookupOffset = ttUSHORT(lookupList + 2 + 2 * i);
            var lookupTable = lookupList + lookupOffset;
            var lookupType = ttUSHORT(lookupTable);
            var subTableCount = ttUSHORT(lookupTable + 4);
            var subTableOffsets = lookupTable + 6;
            if (lookupType != 2)
                continue;
            for (sti = 0; sti < subTableCount; sti++)
            {
                var subtableOffset = ttUSHORT(subTableOffsets + 2 * sti);
                var table = lookupTable + subtableOffset;
                var posFormat = ttUSHORT(table);
                var coverageOffset = ttUSHORT(table + 2);
                var coverageIndex = stbtt__GetCoverageIndex(table + coverageOffset, glyph1);
                if (coverageIndex == -1)
                    continue;
                switch (posFormat)
                {
                    case 1:
                        {
                            var l = 0;
                            var r = 0;
                            var m = 0;
                            var straw = 0;
                            var needle = 0;
                            var valueFormat1 = ttUSHORT(table + 4);
                            var valueFormat2 = ttUSHORT(table + 6);
                            if (valueFormat1 == 4 && valueFormat2 == 0)
                            {
                                var valueRecordPairSizeInBytes = 2;
                                var pairSetCount = ttUSHORT(table + 8);
                                var pairPosOffset = ttUSHORT(table + 10 + 2 * coverageIndex);
                                var pairValueTable = table + pairPosOffset;
                                var pairValueCount = ttUSHORT(pairValueTable);
                                var pairValueArray = pairValueTable + 2;
                                if (coverageIndex >= pairSetCount)
                                    return 0;
                                needle = glyph2;
                                r = pairValueCount - 1;
                                l = 0;
                                while (l <= r)
                                {
                                    ushort secondGlyph = 0;
                                    byte* pairValue;
                                    m = (l + r) >> 1;
                                    pairValue = pairValueArray + (2 + valueRecordPairSizeInBytes) * m;
                                    secondGlyph = ttUSHORT(pairValue);
                                    straw = secondGlyph;
                                    if (needle < straw)
                                    {
                                        r = m - 1;
                                    }
                                    else if (needle > straw)
                                    {
                                        l = m + 1;
                                    }
                                    else
                                    {
                                        var xAdvance = ttSHORT(pairValue + 2);
                                        return xAdvance;
                                    }
                                }
                            }
                            else
                            {
                                return 0;
                            }

                            break;
                        }

                    case 2:
                        {
                            var valueFormat1 = ttUSHORT(table + 4);
                            var valueFormat2 = ttUSHORT(table + 6);
                            if (valueFormat1 == 4 && valueFormat2 == 0)
                            {
                                var classDef1Offset = ttUSHORT(table + 8);
                                var classDef2Offset = ttUSHORT(table + 10);
                                var glyph1class = stbtt__GetGlyphClass(table + classDef1Offset, glyph1);
                                var glyph2class = stbtt__GetGlyphClass(table + classDef2Offset, glyph2);
                                var class1Count = ttUSHORT(table + 12);
                                var class2Count = ttUSHORT(table + 14);
                                byte* class1Records;
                                byte* class2Records;
                                short xAdvance = 0;
                                if (glyph1class < 0 || glyph1class >= class1Count)
                                    return 0;
                                if (glyph2class < 0 || glyph2class >= class2Count)
                                    return 0;
                                class1Records = table + 16;
                                class2Records = class1Records + 2 * glyph1class * class2Count;
                                xAdvance = ttSHORT(class2Records + 2 * glyph2class);
                                return xAdvance;
                            }

                            return 0;
                        }

                    default:
                        return 0;
                }
            }
        }

        return 0;
    }

    public static int stbtt__GetGlyphInfoT2(stbtt_fontinfo info, int glyph_index, int* x0, int* y0, int* x1,
        int* y1)
    {
        var c = new stbtt__csctx();
        c.bounds = 1;
        var r = stbtt__run_charstring(info, glyph_index, &c);
        if (x0 != null)
            *x0 = r != 0 ? c.min_x : 0;
        if (y0 != null)
            *y0 = r != 0 ? c.min_y : 0;
        if (x1 != null)
            *x1 = r != 0 ? c.max_x : 0;
        if (y1 != null)
            *y1 = r != 0 ? c.max_y : 0;
        return r != 0 ? c.num_vertices : 0;
    }

    public static int stbtt__GetGlyphKernInfoAdvance(stbtt_fontinfo info, int glyph1, int glyph2)
    {
        var data = info.data + info.kern;
        uint needle = 0;
        uint straw = 0;
        var l = 0;
        var r = 0;
        var m = 0;
        if (info.kern == 0)
            return 0;
        if (ttUSHORT(data + 2) < 1)
            return 0;
        if (ttUSHORT(data + 8) != 1)
            return 0;
        l = 0;
        r = ttUSHORT(data + 10) - 1;
        needle = (uint)((glyph1 << 16) | glyph2);
        while (l <= r)
        {
            m = (l + r) >> 1;
            straw = ttULONG(data + 18 + m * 6);
            if (needle < straw)
                r = m - 1;
            else if (needle > straw)
                l = m + 1;
            else
                return ttSHORT(data + 22 + m * 6);
        }

        return 0;
    }

    public static int stbtt__GetGlyphShapeT2(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
    {
        var count_ctx = new stbtt__csctx();
        count_ctx.bounds = 1;
        var output_ctx = new stbtt__csctx();
        if (stbtt__run_charstring(info, glyph_index, &count_ctx) != 0)
        {
            *pvertices = (stbtt_vertex*)CRuntime.Malloc((ulong)(count_ctx.num_vertices * sizeof(stbtt_vertex)));
            output_ctx.pvertices = *pvertices;
            if (stbtt__run_charstring(info, glyph_index, &output_ctx) != 0) return output_ctx.num_vertices;
        }

        *pvertices = null;
        return 0;
    }

    public static int stbtt__GetGlyphShapeTT(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
    {
        short numberOfContours = 0;
        byte* endPtsOfContours;
        var data = info.data;
        stbtt_vertex* vertices = null;
        var num_vertices = 0;
        var g = stbtt__GetGlyfOffset(info, glyph_index);
        *pvertices = null;
        if (g < 0)
            return 0;
        numberOfContours = ttSHORT(data + g);
        if (numberOfContours > 0)
        {
            byte flags = 0;
            byte flagcount = 0;
            var ins = 0;
            var i = 0;
            var j = 0;
            var m = 0;
            var n = 0;
            var next_move = 0;
            var was_off = 0;
            var off = 0;
            var start_off = 0;
            var x = 0;
            var y = 0;
            var cx = 0;
            var cy = 0;
            var sx = 0;
            var sy = 0;
            var scx = 0;
            var scy = 0;
            byte* points;
            endPtsOfContours = data + g + 10;
            ins = ttUSHORT(data + g + 10 + numberOfContours * 2);
            points = data + g + 10 + numberOfContours * 2 + 2 + ins;
            n = 1 + ttUSHORT(endPtsOfContours + numberOfContours * 2 - 2);
            m = n + 2 * numberOfContours;
            vertices = (stbtt_vertex*)CRuntime.Malloc((ulong)(m * sizeof(stbtt_vertex)));
            if (vertices == null)
                return 0;
            next_move = 0;
            flagcount = 0;
            off = m - n;
            for (i = 0; i < n; ++i)
            {
                if (flagcount == 0)
                {
                    flags = *points++;
                    if ((flags & 8) != 0)
                        flagcount = *points++;
                }
                else
                {
                    --flagcount;
                }

                vertices[off + i].type = flags;
            }

            x = 0;
            for (i = 0; i < n; ++i)
            {
                flags = vertices[off + i].type;
                if ((flags & 2) != 0)
                {
                    short dx = *points++;
                    x += (flags & 16) != 0 ? dx : -dx;
                }
                else
                {
                    if ((flags & 16) == 0)
                    {
                        x = x + (short)(points[0] * 256 + points[1]);
                        points += 2;
                    }
                }

                vertices[off + i].x = (short)x;
            }

            y = 0;
            for (i = 0; i < n; ++i)
            {
                flags = vertices[off + i].type;
                if ((flags & 4) != 0)
                {
                    short dy = *points++;
                    y += (flags & 32) != 0 ? dy : -dy;
                }
                else
                {
                    if ((flags & 32) == 0)
                    {
                        y = y + (short)(points[0] * 256 + points[1]);
                        points += 2;
                    }
                }

                vertices[off + i].y = (short)y;
            }

            num_vertices = 0;
            sx = sy = cx = cy = scx = scy = 0;
            for (i = 0; i < n; ++i)
            {
                flags = vertices[off + i].type;
                x = vertices[off + i].x;
                y = vertices[off + i].y;
                if (next_move == i)
                {
                    if (i != 0)
                        num_vertices = stbtt__close_shape(vertices, num_vertices, was_off, start_off, sx, sy, scx,
                            scy, cx, cy);
                    start_off = (flags & 1) != 0 ? 0 : 1;
                    if (start_off != 0)
                    {
                        scx = x;
                        scy = y;
                        if ((vertices[off + i + 1].type & 1) == 0)
                        {
                            sx = (x + vertices[off + i + 1].x) >> 1;
                            sy = (y + vertices[off + i + 1].y) >> 1;
                        }
                        else
                        {
                            sx = vertices[off + i + 1].x;
                            sy = vertices[off + i + 1].y;
                            ++i;
                        }
                    }
                    else
                    {
                        sx = x;
                        sy = y;
                    }

                    stbtt_setvertex(&vertices[num_vertices++], STBTT_vmove, sx, sy, 0, 0);
                    was_off = 0;
                    next_move = 1 + ttUSHORT(endPtsOfContours + j * 2);
                    ++j;
                }
                else
                {
                    if ((flags & 1) == 0)
                    {
                        if (was_off != 0)
                            stbtt_setvertex(&vertices[num_vertices++], STBTT_vcurve, (cx + x) >> 1, (cy + y) >> 1,
                                cx, cy);
                        cx = x;
                        cy = y;
                        was_off = 1;
                    }
                    else
                    {
                        if (was_off != 0)
                            stbtt_setvertex(&vertices[num_vertices++], STBTT_vcurve, x, y, cx, cy);
                        else
                            stbtt_setvertex(&vertices[num_vertices++], STBTT_vline, x, y, 0, 0);
                        was_off = 0;
                    }
                }
            }

            num_vertices = stbtt__close_shape(vertices, num_vertices, was_off, start_off, sx, sy, scx, scy, cx, cy);
        }
        else if (numberOfContours < 0)
        {
            var more = 1;
            var comp = data + g + 10;
            num_vertices = 0;
            vertices = null;
            while (more != 0)
            {
                ushort flags = 0;
                ushort gidx = 0;
                var comp_num_verts = 0;
                var i = 0;
                stbtt_vertex* comp_verts = null;
                stbtt_vertex* tmp = null;
                var mtx = stackalloc float[] { 1, 0, 0, 1, 0, 0 };
                float m = 0;
                float n = 0;
                flags = (ushort)ttSHORT(comp);
                comp += 2;
                gidx = (ushort)ttSHORT(comp);
                comp += 2;
                if ((flags & 2) != 0)
                {
                    if ((flags & 1) != 0)
                    {
                        mtx[4] = ttSHORT(comp);
                        comp += 2;
                        mtx[5] = ttSHORT(comp);
                        comp += 2;
                    }
                    else
                    {
                        mtx[4] = *(sbyte*)comp;
                        comp += 1;
                        mtx[5] = *(sbyte*)comp;
                        comp += 1;
                    }
                }

                if ((flags & (1 << 3)) != 0)
                {
                    mtx[0] = mtx[3] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                    mtx[1] = mtx[2] = 0;
                }
                else if ((flags & (1 << 6)) != 0)
                {
                    mtx[0] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                    mtx[1] = mtx[2] = 0;
                    mtx[3] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                }
                else if ((flags & (1 << 7)) != 0)
                {
                    mtx[0] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                    mtx[1] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                    mtx[2] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                    mtx[3] = ttSHORT(comp) / 16384.0f;
                    comp += 2;
                }

                m = (float)CRuntime.Sqrt(mtx[0] * mtx[0] + mtx[1] * mtx[1]);
                n = (float)CRuntime.Sqrt(mtx[2] * mtx[2] + mtx[3] * mtx[3]);
                comp_num_verts = stbtt_GetGlyphShape(info, gidx, &comp_verts);
                if (comp_num_verts > 0)
                {
                    for (i = 0; i < comp_num_verts; ++i)
                    {
                        var v = &comp_verts[i];
                        short x = 0;
                        short y = 0;
                        x = v->x;
                        y = v->y;
                        v->x = (short)(m * (mtx[0] * x + mtx[2] * y + mtx[4]));
                        v->y = (short)(n * (mtx[1] * x + mtx[3] * y + mtx[5]));
                        x = v->cx;
                        y = v->cy;
                        v->cx = (short)(m * (mtx[0] * x + mtx[2] * y + mtx[4]));
                        v->cy = (short)(n * (mtx[1] * x + mtx[3] * y + mtx[5]));
                    }

                    tmp = (stbtt_vertex*)CRuntime.Malloc((ulong)((num_vertices + comp_num_verts) *
                                                                   sizeof(stbtt_vertex)));
                    if (tmp == null)
                    {
                        if (vertices != null)
                            CRuntime.Free(vertices);
                        if (comp_verts != null)
                            CRuntime.Free(comp_verts);
                        return 0;
                    }

                    if (num_vertices > 0 && vertices != null)
                        CRuntime.Memcpy(tmp, vertices, (ulong)(num_vertices * sizeof(stbtt_vertex)));
                    CRuntime.Memcpy(tmp + num_vertices, comp_verts,
                        (ulong)(comp_num_verts * sizeof(stbtt_vertex)));
                    if (vertices != null)
                        CRuntime.Free(vertices);
                    vertices = tmp;
                    CRuntime.Free(comp_verts);
                    num_vertices += comp_num_verts;
                }

                more = flags & (1 << 5);
            }
        }

        *pvertices = vertices;
        return num_vertices;
    }

    public static int stbtt__run_charstring(stbtt_fontinfo info, int glyph_index, stbtt__csctx* c)
    {
        var in_header = 1;
        var maskbits = 0;
        var subr_stack_height = 0;
        var sp = 0;
        var v = 0;
        var i = 0;
        var b0 = 0;
        var has_subrs = 0;
        var clear_stack = 0;
        var s = stackalloc float[48];
        var subr_stack = stackalloc stbtt__buf[10];
        var subrs = info.subrs;
        var b = new stbtt__buf();
        float f = 0;
        b = stbtt__cff_index_get(info.charstrings, glyph_index);
        while (b.cursor < b.size)
        {
            i = 0;
            clear_stack = 1;
            b0 = stbtt__buf_get8(&b);
            switch (b0)
            {
                case 0x13:
                case 0x14:
                    if (in_header != 0)
                        maskbits += sp / 2;
                    in_header = 0;
                    stbtt__buf_skip(&b, (maskbits + 7) / 8);
                    break;
                case 0x01:
                case 0x03:
                case 0x12:
                case 0x17:
                    maskbits += sp / 2;
                    break;
                case 0x15:
                    in_header = 0;
                    if (sp < 2)
                        return 0;
                    stbtt__csctx_rmove_to(c, s[sp - 2], s[sp - 1]);
                    break;
                case 0x04:
                    in_header = 0;
                    if (sp < 1)
                        return 0;
                    stbtt__csctx_rmove_to(c, 0, s[sp - 1]);
                    break;
                case 0x16:
                    in_header = 0;
                    if (sp < 1)
                        return 0;
                    stbtt__csctx_rmove_to(c, s[sp - 1], 0);
                    break;
                case 0x05:
                    if (sp < 2)
                        return 0;
                    for (; i + 1 < sp; i += 2) stbtt__csctx_rline_to(c, s[i], s[i + 1]);

                    break;
                case 0x07:
                case 0x06:
                    if (sp < 1)
                        return 0;
                    var goto_vlineto = b0 == 0x07 ? 1 : 0;
                    for (; ; )
                    {
                        if (goto_vlineto == 0)
                        {
                            if (i >= sp)
                                break;
                            stbtt__csctx_rline_to(c, s[i], 0);
                            i++;
                        }

                        goto_vlineto = 0;
                        if (i >= sp)
                            break;
                        stbtt__csctx_rline_to(c, 0, s[i]);
                        i++;
                    }

                    break;
                case 0x1F:
                case 0x1E:
                    if (sp < 4)
                        return 0;
                    var goto_hvcurveto = b0 == 0x1F ? 1 : 0;
                    for (; ; )
                    {
                        if (goto_hvcurveto == 0)
                        {
                            if (i + 3 >= sp)
                                break;
                            stbtt__csctx_rccurve_to(c, 0, s[i], s[i + 1], s[i + 2], s[i + 3],
                                sp - i == 5 ? s[i + 4] : 0.0f);
                            i += 4;
                        }

                        goto_hvcurveto = 0;
                        if (i + 3 >= sp)
                            break;
                        stbtt__csctx_rccurve_to(c, s[i], 0, s[i + 1], s[i + 2], sp - i == 5 ? s[i + 4] : 0.0f,
                            s[i + 3]);
                        i += 4;
                    }

                    break;
                case 0x08:
                    if (sp < 6)
                        return 0;
                    for (; i + 5 < sp; i += 6)
                        stbtt__csctx_rccurve_to(c, s[i], s[i + 1], s[i + 2], s[i + 3], s[i + 4], s[i + 5]);

                    break;
                case 0x18:
                    if (sp < 8)
                        return 0;
                    for (; i + 5 < sp - 2; i += 6)
                        stbtt__csctx_rccurve_to(c, s[i], s[i + 1], s[i + 2], s[i + 3], s[i + 4], s[i + 5]);

                    if (i + 1 >= sp)
                        return 0;
                    stbtt__csctx_rline_to(c, s[i], s[i + 1]);
                    break;
                case 0x19:
                    if (sp < 8)
                        return 0;
                    for (; i + 1 < sp - 6; i += 2) stbtt__csctx_rline_to(c, s[i], s[i + 1]);

                    if (i + 5 >= sp)
                        return 0;
                    stbtt__csctx_rccurve_to(c, s[i], s[i + 1], s[i + 2], s[i + 3], s[i + 4], s[i + 5]);
                    break;
                case 0x1A:
                case 0x1B:
                    if (sp < 4)
                        return 0;
                    f = (float)0.0;
                    if ((sp & 1) != 0)
                    {
                        f = s[i];
                        i++;
                    }

                    for (; i + 3 < sp; i += 4)
                    {
                        if (b0 == 0x1B)
                            stbtt__csctx_rccurve_to(c, s[i], f, s[i + 1], s[i + 2], s[i + 3], (float)0.0);
                        else
                            stbtt__csctx_rccurve_to(c, f, s[i], s[i + 1], s[i + 2], (float)0.0, s[i + 3]);
                        f = (float)0.0;
                    }

                    break;
                case 0x0A:
                case 0x1D:
                    if (b0 == 0x0A && has_subrs == 0)
                    {
                        if (info.fdselect.size != 0)
                            subrs = stbtt__cid_get_glyph_subrs(info, glyph_index);
                        has_subrs = 1;
                    }

                    if (sp < 1)
                        return 0;
                    v = (int)s[--sp];
                    if (subr_stack_height >= 10)
                        return 0;
                    subr_stack[subr_stack_height++] = b;
                    b = stbtt__get_subr(b0 == 0x0A ? subrs : info.gsubrs, v);
                    if (b.size == 0)
                        return 0;
                    b.cursor = 0;
                    clear_stack = 0;
                    break;
                case 0x0B:
                    if (subr_stack_height <= 0)
                        return 0;
                    b = subr_stack[--subr_stack_height];
                    clear_stack = 0;
                    break;
                case 0x0E:
                    stbtt__csctx_close_shape(c);
                    return 1;
                case 0x0C:
                    {
                        float dx1 = 0;
                        float dx2 = 0;
                        float dx3 = 0;
                        float dx4 = 0;
                        float dx5 = 0;
                        float dx6 = 0;
                        float dy1 = 0;
                        float dy2 = 0;
                        float dy3 = 0;
                        float dy4 = 0;
                        float dy5 = 0;
                        float dy6 = 0;
                        float dx = 0;
                        float dy = 0;
                        int b1 = stbtt__buf_get8(&b);
                        switch (b1)
                        {
                            case 0x22:
                                if (sp < 7)
                                    return 0;
                                dx1 = s[0];
                                dx2 = s[1];
                                dy2 = s[2];
                                dx3 = s[3];
                                dx4 = s[4];
                                dx5 = s[5];
                                dx6 = s[6];
                                stbtt__csctx_rccurve_to(c, dx1, 0, dx2, dy2, dx3, 0);
                                stbtt__csctx_rccurve_to(c, dx4, 0, dx5, -dy2, dx6, 0);
                                break;
                            case 0x23:
                                if (sp < 13)
                                    return 0;
                                dx1 = s[0];
                                dy1 = s[1];
                                dx2 = s[2];
                                dy2 = s[3];
                                dx3 = s[4];
                                dy3 = s[5];
                                dx4 = s[6];
                                dy4 = s[7];
                                dx5 = s[8];
                                dy5 = s[9];
                                dx6 = s[10];
                                dy6 = s[11];
                                stbtt__csctx_rccurve_to(c, dx1, dy1, dx2, dy2, dx3, dy3);
                                stbtt__csctx_rccurve_to(c, dx4, dy4, dx5, dy5, dx6, dy6);
                                break;
                            case 0x24:
                                if (sp < 9)
                                    return 0;
                                dx1 = s[0];
                                dy1 = s[1];
                                dx2 = s[2];
                                dy2 = s[3];
                                dx3 = s[4];
                                dx4 = s[5];
                                dx5 = s[6];
                                dy5 = s[7];
                                dx6 = s[8];
                                stbtt__csctx_rccurve_to(c, dx1, dy1, dx2, dy2, dx3, 0);
                                stbtt__csctx_rccurve_to(c, dx4, 0, dx5, dy5, dx6, -(dy1 + dy2 + dy5));
                                break;
                            case 0x25:
                                if (sp < 11)
                                    return 0;
                                dx1 = s[0];
                                dy1 = s[1];
                                dx2 = s[2];
                                dy2 = s[3];
                                dx3 = s[4];
                                dy3 = s[5];
                                dx4 = s[6];
                                dy4 = s[7];
                                dx5 = s[8];
                                dy5 = s[9];
                                dx6 = dy6 = s[10];
                                dx = dx1 + dx2 + dx3 + dx4 + dx5;
                                dy = dy1 + dy2 + dy3 + dy4 + dy5;
                                if (CRuntime.Fabs(dx) > CRuntime.Fabs(dy))
                                    dy6 = -dy;
                                else
                                    dx6 = -dx;
                                stbtt__csctx_rccurve_to(c, dx1, dy1, dx2, dy2, dx3, dy3);
                                stbtt__csctx_rccurve_to(c, dx4, dy4, dx5, dy5, dx6, dy6);
                                break;
                            default:
                                return 0;
                        }
                    }

                    break;
                default:
                    if (b0 != 255 && b0 != 28 && b0 < 32)
                        return 0;
                    if (b0 == 255)
                    {
                        f = (float)(int)stbtt__buf_get(&b, 4) / 0x10000;
                    }
                    else
                    {
                        stbtt__buf_skip(&b, -1);
                        f = (short)stbtt__cff_int(&b);
                    }

                    if (sp >= 48)
                        return 0;
                    s[sp++] = f;
                    clear_stack = 0;
                    break;
            }

            if (clear_stack != 0)
                sp = 0;
        }

        return 0;
    }

    public static int stbtt_FindGlyphIndex(stbtt_fontinfo info, int unicode_codepoint)
    {
        var data = info.data;
        var index_map = (uint)info.index_map;
        var format = ttUSHORT(data + index_map + 0);
        if (format == 0)
        {
            int bytes = ttUSHORT(data + index_map + 2);
            if (unicode_codepoint < bytes - 6)
                return *(data + index_map + 6 + unicode_codepoint);
            return 0;
        }

        if (format == 6)
        {
            uint first = ttUSHORT(data + index_map + 6);
            uint count = ttUSHORT(data + index_map + 8);
            if ((uint)unicode_codepoint >= first && (uint)unicode_codepoint < first + count)
                return ttUSHORT(data + index_map + 10 + (unicode_codepoint - first) * 2);
            return 0;
        }

        if (format == 2) return 0;

        if (format == 4)
        {
            var segcount = (ushort)(ttUSHORT(data + index_map + 6) >> 1);
            var searchRange = (ushort)(ttUSHORT(data + index_map + 8) >> 1);
            var entrySelector = ttUSHORT(data + index_map + 10);
            var rangeShift = (ushort)(ttUSHORT(data + index_map + 12) >> 1);
            var endCount = index_map + 14;
            var search = endCount;
            if (unicode_codepoint > 0xffff)
                return 0;
            if (unicode_codepoint >= ttUSHORT(data + search + rangeShift * 2))
                search += (uint)(rangeShift * 2);
            search -= 2;
            while (entrySelector != 0)
            {
                ushort end = 0;
                searchRange >>= 1;
                end = ttUSHORT(data + search + searchRange * 2);
                if (unicode_codepoint > end)
                    search += (uint)(searchRange * 2);
                --entrySelector;
            }

            search += 2;
            {
                ushort offset = 0;
                ushort start = 0;
                ushort last = 0;
                var item = (ushort)((search - endCount) >> 1);
                start = ttUSHORT(data + index_map + 14 + segcount * 2 + 2 + 2 * item);
                last = ttUSHORT(data + endCount + 2 * item);
                if (unicode_codepoint < start || unicode_codepoint > last)
                    return 0;
                offset = ttUSHORT(data + index_map + 14 + segcount * 6 + 2 + 2 * item);
                if (offset == 0)
                    return (ushort)(unicode_codepoint +
                                     ttSHORT(data + index_map + 14 + segcount * 4 + 2 + 2 * item));
                return ttUSHORT(data + offset + (unicode_codepoint - start) * 2 + index_map + 14 + segcount * 6 +
                                2 + 2 * item);
            }
        }

        if (format == 12 || format == 13)
        {
            var ngroups = ttULONG(data + index_map + 12);
            var low = 0;
            var high = 0;
            low = 0;
            high = (int)ngroups;
            while (low < high)
            {
                var mid = low + ((high - low) >> 1);
                var start_char = ttULONG(data + index_map + 16 + mid * 12);
                var end_char = ttULONG(data + index_map + 16 + mid * 12 + 4);
                if ((uint)unicode_codepoint < start_char)
                {
                    high = mid;
                }
                else if ((uint)unicode_codepoint > end_char)
                {
                    low = mid + 1;
                }
                else
                {
                    var start_glyph = ttULONG(data + index_map + 16 + mid * 12 + 8);
                    if (format == 12)
                        return (int)(start_glyph + unicode_codepoint - start_char);
                    return (int)start_glyph;
                }
            }

            return 0;
        }

        return 0;
    }

    public static byte* stbtt_FindSVGDoc(stbtt_fontinfo info, int gl)
    {
        var i = 0;
        var data = info.data;
        var svg_doc_list = data + stbtt__get_svg(info);
        int numEntries = ttUSHORT(svg_doc_list);
        var svg_docs = svg_doc_list + 2;
        for (i = 0; i < numEntries; i++)
        {
            var svg_doc = svg_docs + 12 * i;
            if (gl >= ttUSHORT(svg_doc) && gl <= ttUSHORT(svg_doc + 2))
                return svg_doc;
        }

        return null;
    }

    public static void stbtt_FreeShape(stbtt_fontinfo info, stbtt_vertex* v)
    {
        CRuntime.Free(v);
    }

    public static byte* stbtt_GetCodepointBitmap(stbtt_fontinfo info, float scale_x, float scale_y, int codepoint,
        int* width, int* height, int* xoff, int* yoff)
    {
        return stbtt_GetCodepointBitmapSubpixel(info, scale_x, scale_y, 0.0f, 0.0f, codepoint, width, height, xoff,
            yoff);
    }

    public static void stbtt_GetCodepointBitmapBox(stbtt_fontinfo font, int codepoint, float scale_x, float scale_y,
        int* ix0, int* iy0, int* ix1, int* iy1)
    {
        stbtt_GetCodepointBitmapBoxSubpixel(font, codepoint, scale_x, scale_y, 0.0f, 0.0f, ix0, iy0, ix1, iy1);
    }

    public static void stbtt_GetCodepointBitmapBoxSubpixel(stbtt_fontinfo font, int codepoint, float scale_x,
        float scale_y, float shift_x, float shift_y, int* ix0, int* iy0, int* ix1, int* iy1)
    {
        stbtt_GetGlyphBitmapBoxSubpixel(font, stbtt_FindGlyphIndex(font, codepoint), scale_x, scale_y, shift_x,
            shift_y, ix0, iy0, ix1, iy1);
    }

    public static byte* stbtt_GetCodepointBitmapSubpixel(stbtt_fontinfo info, float scale_x, float scale_y,
        float shift_x, float shift_y, int codepoint, int* width, int* height, int* xoff, int* yoff)
    {
        return stbtt_GetGlyphBitmapSubpixel(info, scale_x, scale_y, shift_x, shift_y,
            stbtt_FindGlyphIndex(info, codepoint), width, height, xoff, yoff);
    }

    public static int stbtt_GetCodepointBox(stbtt_fontinfo info, int codepoint, int* x0, int* y0, int* x1, int* y1)
    {
        return stbtt_GetGlyphBox(info, stbtt_FindGlyphIndex(info, codepoint), x0, y0, x1, y1);
    }

    public static void stbtt_GetCodepointHMetrics(stbtt_fontinfo info, int codepoint, int* advanceWidth,
        int* leftSideBearing)
    {
        stbtt_GetGlyphHMetrics(info, stbtt_FindGlyphIndex(info, codepoint), advanceWidth, leftSideBearing);
    }

    public static int stbtt_GetCodepointKernAdvance(stbtt_fontinfo info, int ch1, int ch2)
    {
        if (info.kern == 0 && info.gpos == 0)
            return 0;
        return stbtt_GetGlyphKernAdvance(info, stbtt_FindGlyphIndex(info, ch1), stbtt_FindGlyphIndex(info, ch2));
    }

    public static byte* stbtt_GetCodepointSDF(stbtt_fontinfo info, float scale, int codepoint, int padding,
        byte onedge_value, float pixel_dist_scale, int* width, int* height, int* xoff, int* yoff)
    {
        return stbtt_GetGlyphSDF(info, scale, stbtt_FindGlyphIndex(info, codepoint), padding, onedge_value,
            pixel_dist_scale, width, height, xoff, yoff);
    }

    public static int stbtt_GetCodepointShape(stbtt_fontinfo info, int unicode_codepoint, stbtt_vertex** vertices)
    {
        return stbtt_GetGlyphShape(info, stbtt_FindGlyphIndex(info, unicode_codepoint), vertices);
    }

    public static int stbtt_GetCodepointSVG(stbtt_fontinfo info, int unicode_codepoint, sbyte** svg)
    {
        return stbtt_GetGlyphSVG(info, stbtt_FindGlyphIndex(info, unicode_codepoint), svg);
    }

    public static void stbtt_GetFontBoundingBox(stbtt_fontinfo info, int* x0, int* y0, int* x1, int* y1)
    {
        *x0 = ttSHORT(info.data + info.head + 36);
        *y0 = ttSHORT(info.data + info.head + 38);
        *x1 = ttSHORT(info.data + info.head + 40);
        *y1 = ttSHORT(info.data + info.head + 42);
    }

    public static sbyte* stbtt_GetFontNameString(stbtt_fontinfo font, int* length, int platformID, int encodingID,
        int languageID, int nameID)
    {
        var i = 0;
        var count = 0;
        var stringOffset = 0;
        var fc = font.data;
        var offset = (uint)font.fontstart;
        var nm = stbtt__find_table(fc, offset, "name");
        if (nm == 0)
            return null;
        count = ttUSHORT(fc + nm + 2);
        stringOffset = (int)(nm + ttUSHORT(fc + nm + 4));
        for (i = 0; i < count; ++i)
        {
            var loc = (uint)(nm + 6 + 12 * i);
            if (platformID == ttUSHORT(fc + loc + 0) && encodingID == ttUSHORT(fc + loc + 2) &&
                languageID == ttUSHORT(fc + loc + 4) && nameID == ttUSHORT(fc + loc + 6))
            {
                *length = ttUSHORT(fc + loc + 8);
                return (sbyte*)(fc + stringOffset + ttUSHORT(fc + loc + 10));
            }
        }

        return null;
    }

    public static void stbtt_GetFontVMetrics(stbtt_fontinfo info, int* ascent, int* descent, int* lineGap)
    {
        if (ascent != null)
            *ascent = ttSHORT(info.data + info.hhea + 4);
        if (descent != null)
            *descent = ttSHORT(info.data + info.hhea + 6);
        if (lineGap != null)
            *lineGap = ttSHORT(info.data + info.hhea + 8);
    }

    public static int stbtt_GetFontVMetricsOS2(stbtt_fontinfo info, int* typoAscent, int* typoDescent,
        int* typoLineGap)
    {
        var tab = (int)stbtt__find_table(info.data, (uint)info.fontstart, "OS/2");
        if (tab == 0)
            return 0;
        if (typoAscent != null)
            *typoAscent = ttSHORT(info.data + tab + 68);
        if (typoDescent != null)
            *typoDescent = ttSHORT(info.data + tab + 70);
        if (typoLineGap != null)
            *typoLineGap = ttSHORT(info.data + tab + 72);
        return 1;
    }

    public static byte* stbtt_GetGlyphBitmap(stbtt_fontinfo info, float scale_x, float scale_y, int glyph,
        int* width, int* height, int* xoff, int* yoff)
    {
        return stbtt_GetGlyphBitmapSubpixel(info, scale_x, scale_y, 0.0f, 0.0f, glyph, width, height, xoff, yoff);
    }

    public static void stbtt_GetGlyphBitmapBox(stbtt_fontinfo font, int glyph, float scale_x, float scale_y,
        int* ix0, int* iy0, int* ix1, int* iy1)
    {
        stbtt_GetGlyphBitmapBoxSubpixel(font, glyph, scale_x, scale_y, 0.0f, 0.0f, ix0, iy0, ix1, iy1);
    }

    public static void stbtt_GetGlyphBitmapBoxSubpixel(stbtt_fontinfo font, int glyph, float scale_x, float scale_y,
        float shift_x, float shift_y, int* ix0, int* iy0, int* ix1, int* iy1)
    {
        var x0 = 0;
        var y0 = 0;
        var x1 = 0;
        var y1 = 0;
        if (stbtt_GetGlyphBox(font, glyph, &x0, &y0, &x1, &y1) == 0)
        {
            if (ix0 != null)
                *ix0 = 0;
            if (iy0 != null)
                *iy0 = 0;
            if (ix1 != null)
                *ix1 = 0;
            if (iy1 != null)
                *iy1 = 0;
        }
        else
        {
            if (ix0 != null)
                *ix0 = (int)CRuntime.Floor(x0 * scale_x + shift_x);
            if (iy0 != null)
                *iy0 = (int)CRuntime.Floor(-y1 * scale_y + shift_y);
            if (ix1 != null)
                *ix1 = (int)CRuntime.Ceil(x1 * scale_x + shift_x);
            if (iy1 != null)
                *iy1 = (int)CRuntime.Ceil(-y0 * scale_y + shift_y);
        }
    }

    public static byte* stbtt_GetGlyphBitmapSubpixel(stbtt_fontinfo info, float scale_x, float scale_y,
        float shift_x, float shift_y, int glyph, int* width, int* height, int* xoff, int* yoff)
    {
        var ix0 = 0;
        var iy0 = 0;
        var ix1 = 0;
        var iy1 = 0;
        var gbm = new stbtt__bitmap();
        stbtt_vertex* vertices;
        var num_verts = stbtt_GetGlyphShape(info, glyph, &vertices);
        if (scale_x == 0)
            scale_x = scale_y;
        if (scale_y == 0)
        {
            if (scale_x == 0)
            {
                CRuntime.Free(vertices);
                return null;
            }

            scale_y = scale_x;
        }

        stbtt_GetGlyphBitmapBoxSubpixel(info, glyph, scale_x, scale_y, shift_x, shift_y, &ix0, &iy0, &ix1, &iy1);
        gbm.w = ix1 - ix0;
        gbm.h = iy1 - iy0;
        gbm.pixels = null;
        if (width != null)
            *width = gbm.w;
        if (height != null)
            *height = gbm.h;
        if (xoff != null)
            *xoff = ix0;
        if (yoff != null)
            *yoff = iy0;
        if (gbm.w != 0 && gbm.h != 0)
        {
            gbm.pixels = (byte*)CRuntime.Malloc((ulong)(gbm.w * gbm.h));
            if (gbm.pixels != null)
            {
                gbm.stride = gbm.w;
                stbtt_Rasterize(&gbm, 0.35f, vertices, num_verts, scale_x, scale_y, shift_x, shift_y, ix0, iy0, 1,
                    info.userdata);
            }
        }

        CRuntime.Free(vertices);
        return gbm.pixels;
    }

    public static int stbtt_GetGlyphBox(stbtt_fontinfo info, int glyph_index, int* x0, int* y0, int* x1, int* y1)
    {
        if (info.cff.size != 0)
        {
            stbtt__GetGlyphInfoT2(info, glyph_index, x0, y0, x1, y1);
        }
        else
        {
            var g = stbtt__GetGlyfOffset(info, glyph_index);
            if (g < 0)
                return 0;
            if (x0 != null)
                *x0 = ttSHORT(info.data + g + 2);
            if (y0 != null)
                *y0 = ttSHORT(info.data + g + 4);
            if (x1 != null)
                *x1 = ttSHORT(info.data + g + 6);
            if (y1 != null)
                *y1 = ttSHORT(info.data + g + 8);
        }

        return 1;
    }

    public static void stbtt_GetGlyphHMetrics(stbtt_fontinfo info, int glyph_index, int* advanceWidth,
        int* leftSideBearing)
    {
        var numOfLongHorMetrics = ttUSHORT(info.data + info.hhea + 34);
        if (glyph_index < numOfLongHorMetrics)
        {
            if (advanceWidth != null)
                *advanceWidth = ttSHORT(info.data + info.hmtx + 4 * glyph_index);
            if (leftSideBearing != null)
                *leftSideBearing = ttSHORT(info.data + info.hmtx + 4 * glyph_index + 2);
        }
        else
        {
            if (advanceWidth != null)
                *advanceWidth = ttSHORT(info.data + info.hmtx + 4 * (numOfLongHorMetrics - 1));
            if (leftSideBearing != null)
                *leftSideBearing = ttSHORT(info.data + info.hmtx + 4 * numOfLongHorMetrics +
                                           2 * (glyph_index - numOfLongHorMetrics));
        }
    }

    public static int stbtt_GetGlyphKernAdvance(stbtt_fontinfo info, int g1, int g2)
    {
        var xAdvance = 0;
        if (info.gpos != 0)
            xAdvance += stbtt__GetGlyphGPOSInfoAdvance(info, g1, g2);
        else if (info.kern != 0)
            xAdvance += stbtt__GetGlyphKernInfoAdvance(info, g1, g2);
        return xAdvance;
    }

    public static byte* stbtt_GetGlyphSDF(stbtt_fontinfo info, float scale, int glyph, int padding,
        byte onedge_value, float pixel_dist_scale, int* width, int* height, int* xoff, int* yoff)
    {
        var scale_x = scale;
        var scale_y = scale;
        var ix0 = 0;
        var iy0 = 0;
        var ix1 = 0;
        var iy1 = 0;
        var w = 0;
        var h = 0;
        byte* data;
        if (scale == 0)
            return null;
        stbtt_GetGlyphBitmapBoxSubpixel(info, glyph, scale, scale, 0.0f, 0.0f, &ix0, &iy0, &ix1, &iy1);
        if (ix0 == ix1 || iy0 == iy1)
            return null;
        ix0 -= padding;
        iy0 -= padding;
        ix1 += padding;
        iy1 += padding;
        w = ix1 - ix0;
        h = iy1 - iy0;
        if (width != null)
            *width = w;
        if (height != null)
            *height = h;
        if (xoff != null)
            *xoff = ix0;
        if (yoff != null)
            *yoff = iy0;
        scale_y = -scale_y;
        {
            var x = 0;
            var y = 0;
            var i = 0;
            var j = 0;
            float* precompute;
            stbtt_vertex* verts;
            var num_verts = stbtt_GetGlyphShape(info, glyph, &verts);
            data = (byte*)CRuntime.Malloc((ulong)(w * h));
            precompute = (float*)CRuntime.Malloc((ulong)(num_verts * sizeof(float)));
            for (i = 0, j = num_verts - 1; i < num_verts; j = i++)
                if (verts[i].type == STBTT_vline)
                {
                    var x0 = verts[i].x * scale_x;
                    var y0 = verts[i].y * scale_y;
                    var x1 = verts[j].x * scale_x;
                    var y1 = verts[j].y * scale_y;
                    var dist = (float)CRuntime.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
                    precompute[i] = dist == 0 ? 0.0f : 1.0f / dist;
                }
                else if (verts[i].type == STBTT_vcurve)
                {
                    var x2 = verts[j].x * scale_x;
                    var y2 = verts[j].y * scale_y;
                    var x1 = verts[i].cx * scale_x;
                    var y1 = verts[i].cy * scale_y;
                    var x0 = verts[i].x * scale_x;
                    var y0 = verts[i].y * scale_y;
                    var bx = x0 - 2 * x1 + x2;
                    var by = y0 - 2 * y1 + y2;
                    var len2 = bx * bx + by * by;
                    if (len2 != 0.0f)
                        precompute[i] = 1.0f / (bx * bx + by * by);
                    else
                        precompute[i] = 0.0f;
                }
                else
                {
                    precompute[i] = 0.0f;
                }

            for (y = iy0; y < iy1; ++y)
                for (x = ix0; x < ix1; ++x)
                {
                    float val = 0;
                    var min_dist = 999999.0f;
                    var sx = x + 0.5f;
                    var sy = y + 0.5f;
                    var x_gspace = sx / scale_x;
                    var y_gspace = sy / scale_y;
                    var winding = stbtt__compute_crossings_x(x_gspace, y_gspace, num_verts, verts);
                    for (i = 0; i < num_verts; ++i)
                    {
                        var x0 = verts[i].x * scale_x;
                        var y0 = verts[i].y * scale_y;
                        if (verts[i].type == STBTT_vline && precompute[i] != 0.0f)
                        {
                            var x1 = verts[i - 1].x * scale_x;
                            var y1 = verts[i - 1].y * scale_y;
                            float dist = 0;
                            var dist2 = (x0 - sx) * (x0 - sx) + (y0 - sy) * (y0 - sy);
                            if (dist2 < min_dist * min_dist)
                                min_dist = (float)CRuntime.Sqrt(dist2);
                            dist = CRuntime.Fabs((x1 - x0) * (y0 - sy) - (y1 - y0) * (x0 - sx)) * precompute[i];
                            if (dist < min_dist)
                            {
                                var dx = x1 - x0;
                                var dy = y1 - y0;
                                var px = x0 - sx;
                                var py = y0 - sy;
                                var t = -(px * dx + py * dy) / (dx * dx + dy * dy);
                                if (t >= 0.0f && t <= 1.0f)
                                    min_dist = dist;
                            }
                        }
                        else if (verts[i].type == STBTT_vcurve)
                        {
                            var x2 = verts[i - 1].x * scale_x;
                            var y2 = verts[i - 1].y * scale_y;
                            var x1 = verts[i].cx * scale_x;
                            var y1 = verts[i].cy * scale_y;
                            var box_x0 = (x0 < x1 ? x0 : x1) < x2 ? x0 < x1 ? x0 : x1 : x2;
                            var box_y0 = (y0 < y1 ? y0 : y1) < y2 ? y0 < y1 ? y0 : y1 : y2;
                            var box_x1 = (x0 < x1 ? x1 : x0) < x2 ? x2 : x0 < x1 ? x1 : x0;
                            var box_y1 = (y0 < y1 ? y1 : y0) < y2 ? y2 : y0 < y1 ? y1 : y0;
                            if (sx > box_x0 - min_dist && sx < box_x1 + min_dist && sy > box_y0 - min_dist &&
                                sy < box_y1 + min_dist)
                            {
                                var num = 0;
                                var ax = x1 - x0;
                                var ay = y1 - y0;
                                var bx = x0 - 2 * x1 + x2;
                                var by = y0 - 2 * y1 + y2;
                                var mx = x0 - sx;
                                var my = y0 - sy;
                                var res = stackalloc float[] { 0, 0, 0 };
                                float px = 0;
                                float py = 0;
                                float t = 0;
                                float it = 0;
                                float dist2 = 0;
                                var a_inv = precompute[i];
                                if (a_inv == 0.0)
                                {
                                    var a = 3 * (ax * bx + ay * by);
                                    var b = 2 * (ax * ax + ay * ay) + (mx * bx + my * by);
                                    var c = mx * ax + my * ay;
                                    if (a == 0.0)
                                    {
                                        if (b != 0.0) res[num++] = -c / b;
                                    }
                                    else
                                    {
                                        var discriminant = b * b - 4 * a * c;
                                        if (discriminant < 0)
                                        {
                                            num = 0;
                                        }
                                        else
                                        {
                                            var root = (float)CRuntime.Sqrt(discriminant);
                                            res[0] = (-b - root) / (2 * a);
                                            res[1] = (-b + root) / (2 * a);
                                            num = 2;
                                        }
                                    }
                                }
                                else
                                {
                                    var b = 3 * (ax * bx + ay * by) * a_inv;
                                    var c = (2 * (ax * ax + ay * ay) + (mx * bx + my * by)) * a_inv;
                                    var d = (mx * ax + my * ay) * a_inv;
                                    num = stbtt__solve_cubic(b, c, d, res);
                                }

                                dist2 = (x0 - sx) * (x0 - sx) + (y0 - sy) * (y0 - sy);
                                if (dist2 < min_dist * min_dist)
                                    min_dist = (float)CRuntime.Sqrt(dist2);
                                if (num >= 1 && res[0] >= 0.0f && res[0] <= 1.0f)
                                {
                                    t = res[0];
                                    it = 1.0f - t;
                                    px = it * it * x0 + 2 * t * it * x1 + t * t * x2;
                                    py = it * it * y0 + 2 * t * it * y1 + t * t * y2;
                                    dist2 = (px - sx) * (px - sx) + (py - sy) * (py - sy);
                                    if (dist2 < min_dist * min_dist)
                                        min_dist = (float)CRuntime.Sqrt(dist2);
                                }

                                if (num >= 2 && res[1] >= 0.0f && res[1] <= 1.0f)
                                {
                                    t = res[1];
                                    it = 1.0f - t;
                                    px = it * it * x0 + 2 * t * it * x1 + t * t * x2;
                                    py = it * it * y0 + 2 * t * it * y1 + t * t * y2;
                                    dist2 = (px - sx) * (px - sx) + (py - sy) * (py - sy);
                                    if (dist2 < min_dist * min_dist)
                                        min_dist = (float)CRuntime.Sqrt(dist2);
                                }

                                if (num >= 3 && res[2] >= 0.0f && res[2] <= 1.0f)
                                {
                                    t = res[2];
                                    it = 1.0f - t;
                                    px = it * it * x0 + 2 * t * it * x1 + t * t * x2;
                                    py = it * it * y0 + 2 * t * it * y1 + t * t * y2;
                                    dist2 = (px - sx) * (px - sx) + (py - sy) * (py - sy);
                                    if (dist2 < min_dist * min_dist)
                                        min_dist = (float)CRuntime.Sqrt(dist2);
                                }
                            }
                        }
                    }

                    if (winding == 0)
                        min_dist = -min_dist;
                    val = onedge_value + pixel_dist_scale * min_dist;
                    if (val < 0)
                        val = 0;
                    else if (val > 255)
                        val = 255;
                    data[(y - iy0) * w + (x - ix0)] = (byte)val;
                }

            CRuntime.Free(precompute);
            CRuntime.Free(verts);
        }

        return data;
    }

    public static int stbtt_GetGlyphShape(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
    {
        if (info.cff.size == 0)
            return stbtt__GetGlyphShapeTT(info, glyph_index, pvertices);
        return stbtt__GetGlyphShapeT2(info, glyph_index, pvertices);
    }

    public static int stbtt_GetGlyphSVG(stbtt_fontinfo info, int gl, sbyte** svg)
    {
        var data = info.data;
        byte* svg_doc;
        if (info.svg == 0)
            return 0;
        svg_doc = stbtt_FindSVGDoc(info, gl);
        if (svg_doc != null)
        {
            *svg = (sbyte*)data + info.svg + ttULONG(svg_doc + 4);
            return (int)ttULONG(svg_doc + 8);
        }

        return 0;
    }

    public static int stbtt_GetKerningTable(stbtt_fontinfo info, stbtt_kerningentry* table, int table_length)
    {
        var data = info.data + info.kern;
        var k = 0;
        var length = 0;
        if (info.kern == 0)
            return 0;
        if (ttUSHORT(data + 2) < 1)
            return 0;
        if (ttUSHORT(data + 8) != 1)
            return 0;
        length = ttUSHORT(data + 10);
        if (table_length < length)
            length = table_length;
        for (k = 0; k < length; k++)
        {
            table[k].glyph1 = ttUSHORT(data + 18 + k * 6);
            table[k].glyph2 = ttUSHORT(data + 20 + k * 6);
            table[k].advance = ttSHORT(data + 22 + k * 6);
        }

        return length;
    }

    public static int stbtt_GetKerningTableLength(stbtt_fontinfo info)
    {
        var data = info.data + info.kern;
        if (info.kern == 0)
            return 0;
        if (ttUSHORT(data + 2) < 1)
            return 0;
        if (ttUSHORT(data + 8) != 1)
            return 0;
        return ttUSHORT(data + 10);
    }

    public static int stbtt_InitFont(stbtt_fontinfo info, byte* data, int offset)
    {
        return stbtt_InitFont_internal(info, data, offset);
    }

    public static int stbtt_InitFont_internal(stbtt_fontinfo info, byte* data, int fontstart)
    {
        uint cmap = 0;
        uint t = 0;
        var i = 0;
        var numTables = 0;
        info.data = data;
        info.fontstart = fontstart;
        info.cff = stbtt__new_buf(null, 0);
        cmap = stbtt__find_table(data, (uint)fontstart, "cmap");
        info.loca = (int)stbtt__find_table(data, (uint)fontstart, "loca");
        info.head = (int)stbtt__find_table(data, (uint)fontstart, "head");
        info.glyf = (int)stbtt__find_table(data, (uint)fontstart, "glyf");
        info.hhea = (int)stbtt__find_table(data, (uint)fontstart, "hhea");
        info.hmtx = (int)stbtt__find_table(data, (uint)fontstart, "hmtx");
        info.kern = (int)stbtt__find_table(data, (uint)fontstart, "kern");
        info.gpos = (int)stbtt__find_table(data, (uint)fontstart, "GPOS");
        if (cmap == 0 || info.head == 0 || info.hhea == 0 || info.hmtx == 0)
            return 0;
        if (info.glyf != 0)
        {
            if (info.loca == 0)
                return 0;
        }
        else
        {
            var b = new stbtt__buf();
            var topdict = new stbtt__buf();
            var topdictidx = new stbtt__buf();
            uint cstype = 2;
            uint charstrings = 0;
            uint fdarrayoff = 0;
            uint fdselectoff = 0;
            uint cff = 0;
            cff = stbtt__find_table(data, (uint)fontstart, "CFF ");
            if (cff == 0)
                return 0;
            info.fontdicts = stbtt__new_buf(null, 0);
            info.fdselect = stbtt__new_buf(null, 0);
            info.cff = stbtt__new_buf(data + cff, 512 * 1024 * 1024);
            b = info.cff;
            stbtt__buf_skip(&b, 2);
            stbtt__buf_seek(&b, stbtt__buf_get8(&b));
            stbtt__cff_get_index(&b);
            topdictidx = stbtt__cff_get_index(&b);
            topdict = stbtt__cff_index_get(topdictidx, 0);
            stbtt__cff_get_index(&b);
            info.gsubrs = stbtt__cff_get_index(&b);
            stbtt__dict_get_ints(&topdict, 17, 1, &charstrings);
            stbtt__dict_get_ints(&topdict, 0x100 | 6, 1, &cstype);
            stbtt__dict_get_ints(&topdict, 0x100 | 36, 1, &fdarrayoff);
            stbtt__dict_get_ints(&topdict, 0x100 | 37, 1, &fdselectoff);
            info.subrs = stbtt__get_subrs(b, topdict);
            if (cstype != 2)
                return 0;
            if (charstrings == 0)
                return 0;
            if (fdarrayoff != 0)
            {
                if (fdselectoff == 0)
                    return 0;
                stbtt__buf_seek(&b, (int)fdarrayoff);
                info.fontdicts = stbtt__cff_get_index(&b);
                info.fdselect = stbtt__buf_range(&b, (int)fdselectoff, (int)(b.size - fdselectoff));
            }

            stbtt__buf_seek(&b, (int)charstrings);
            info.charstrings = stbtt__cff_get_index(&b);
        }

        t = stbtt__find_table(data, (uint)fontstart, "maxp");
        if (t != 0)
            info.numGlyphs = ttUSHORT(data + t + 4);
        else
            info.numGlyphs = 0xffff;
        info.svg = -1;
        numTables = ttUSHORT(data + cmap + 2);
        info.index_map = 0;
        for (i = 0; i < numTables; ++i)
        {
            var encoding_record = (uint)(cmap + 4 + 8 * i);
            switch (ttUSHORT(data + encoding_record))
            {
                case STBTT_PLATFORM_ID_MICROSOFT:
                    switch (ttUSHORT(data + encoding_record + 2))
                    {
                        case STBTT_MS_EID_UNICODE_BMP:
                        case STBTT_MS_EID_UNICODE_FULL:
                            info.index_map = (int)(cmap + ttULONG(data + encoding_record + 4));
                            break;
                    }

                    break;
                case STBTT_PLATFORM_ID_UNICODE:
                    info.index_map = (int)(cmap + ttULONG(data + encoding_record + 4));
                    break;
            }
        }

        if (info.index_map == 0)
            throw new Exception("The font does not have a table mapping from unicode codepoints to font indices.");
        info.indexToLocFormat = ttUSHORT(data + info.head + 50);
        return 1;
    }

    public static int stbtt_IsGlyphEmpty(stbtt_fontinfo info, int glyph_index)
    {
        short numberOfContours = 0;
        var g = 0;
        if (info.cff.size != 0)
            return stbtt__GetGlyphInfoT2(info, glyph_index, null, null, null, null) == 0 ? 1 : 0;
        g = stbtt__GetGlyfOffset(info, glyph_index);
        if (g < 0)
            return 1;
        numberOfContours = ttSHORT(info.data + g);
        return numberOfContours == 0 ? 1 : 0;
    }

    public static void stbtt_MakeCodepointBitmap(stbtt_fontinfo info, byte* output, int out_w, int out_h,
        int out_stride, float scale_x, float scale_y, int codepoint)
    {
        stbtt_MakeCodepointBitmapSubpixel(info, output, out_w, out_h, out_stride, scale_x, scale_y, 0.0f, 0.0f,
            codepoint);
    }

    public static void stbtt_MakeCodepointBitmapSubpixel(stbtt_fontinfo info, byte* output, int out_w, int out_h,
        int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int codepoint)
    {
        stbtt_MakeGlyphBitmapSubpixel(info, output, out_w, out_h, out_stride, scale_x, scale_y, shift_x, shift_y,
            stbtt_FindGlyphIndex(info, codepoint));
    }

    public static void stbtt_MakeCodepointBitmapSubpixelPrefilter(stbtt_fontinfo info, byte* output, int out_w,
        int out_h, int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int oversample_x,
        int oversample_y, float* sub_x, float* sub_y, int codepoint)
    {
        stbtt_MakeGlyphBitmapSubpixelPrefilter(info, output, out_w, out_h, out_stride, scale_x, scale_y, shift_x,
            shift_y, oversample_x, oversample_y, sub_x, sub_y, stbtt_FindGlyphIndex(info, codepoint));
    }

    public static void stbtt_MakeGlyphBitmap(stbtt_fontinfo info, byte* output, int out_w, int out_h,
        int out_stride, float scale_x, float scale_y, int glyph)
    {
        stbtt_MakeGlyphBitmapSubpixel(info, output, out_w, out_h, out_stride, scale_x, scale_y, 0.0f, 0.0f, glyph);
    }

    public static void stbtt_MakeGlyphBitmapSubpixel(stbtt_fontinfo info, byte* output, int out_w, int out_h,
        int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int glyph)
    {
        var ix0 = 0;
        var iy0 = 0;
        stbtt_vertex* vertices;
        var num_verts = stbtt_GetGlyphShape(info, glyph, &vertices);
        var gbm = new stbtt__bitmap();
        stbtt_GetGlyphBitmapBoxSubpixel(info, glyph, scale_x, scale_y, shift_x, shift_y, &ix0, &iy0, null, null);
        gbm.pixels = output;
        gbm.w = out_w;
        gbm.h = out_h;
        gbm.stride = out_stride;
        if (gbm.w != 0 && gbm.h != 0)
            stbtt_Rasterize(&gbm, 0.35f, vertices, num_verts, scale_x, scale_y, shift_x, shift_y, ix0, iy0, 1,
                info.userdata);
        CRuntime.Free(vertices);
    }

    public static void stbtt_MakeGlyphBitmapSubpixelPrefilter(stbtt_fontinfo info, byte* output, int out_w,
        int out_h, int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int prefilter_x,
        int prefilter_y, float* sub_x, float* sub_y, int glyph)
    {
        stbtt_MakeGlyphBitmapSubpixel(info, output, out_w - (prefilter_x - 1), out_h - (prefilter_y - 1),
            out_stride, scale_x, scale_y, shift_x, shift_y, glyph);
        if (prefilter_x > 1)
            stbtt__h_prefilter(output, out_w, out_h, out_stride, (uint)prefilter_x);
        if (prefilter_y > 1)
            stbtt__v_prefilter(output, out_w, out_h, out_stride, (uint)prefilter_y);
        *sub_x = stbtt__oversample_shift(prefilter_x);
        *sub_y = stbtt__oversample_shift(prefilter_y);
    }

    public static float stbtt_ScaleForMappingEmToPixels(stbtt_fontinfo info, float pixels)
    {
        int unitsPerEm = ttUSHORT(info.data + info.head + 18);
        return pixels / unitsPerEm;
    }

    public static float stbtt_ScaleForPixelHeight(stbtt_fontinfo info, float height)
    {
        var fheight = ttSHORT(info.data + info.hhea + 4) - ttSHORT(info.data + info.hhea + 6);
        return height / fheight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_kerningentry
    {
        public int glyph1;
        public int glyph2;
        public int advance;
    }
}

/* Generated Common */
unsafe partial class StbTrueType
{
    public const int STBTT_MAC_EID_ARABIC = 4;
    public const int STBTT_MAC_EID_CHINESE_TRAD = 2;
    public const int STBTT_MAC_EID_GREEK = 6;
    public const int STBTT_MAC_EID_HEBREW = 5;
    public const int STBTT_MAC_EID_JAPANESE = 1;
    public const int STBTT_MAC_EID_KOREAN = 3;
    public const int STBTT_MAC_EID_ROMAN = 0;
    public const int STBTT_MAC_EID_RUSSIAN = 7;
    public const int STBTT_MAC_LANG_ARABIC = 12;
    public const int STBTT_MAC_LANG_CHINESE_SIMPLIFIED = 33;
    public const int STBTT_MAC_LANG_CHINESE_TRAD = 19;
    public const int STBTT_MAC_LANG_DUTCH = 4;
    public const int STBTT_MAC_LANG_ENGLISH = 0;
    public const int STBTT_MAC_LANG_FRENCH = 1;
    public const int STBTT_MAC_LANG_GERMAN = 2;
    public const int STBTT_MAC_LANG_HEBREW = 10;
    public const int STBTT_MAC_LANG_ITALIAN = 3;
    public const int STBTT_MAC_LANG_JAPANESE = 11;
    public const int STBTT_MAC_LANG_KOREAN = 23;
    public const int STBTT_MAC_LANG_RUSSIAN = 32;
    public const int STBTT_MAC_LANG_SPANISH = 6;
    public const int STBTT_MAC_LANG_SWEDISH = 5;
    public const int STBTT_MS_EID_SHIFTJIS = 2;
    public const int STBTT_MS_EID_SYMBOL = 0;
    public const int STBTT_MS_EID_UNICODE_BMP = 1;
    public const int STBTT_MS_EID_UNICODE_FULL = 10;
    public const int STBTT_MS_LANG_CHINESE = 2052;
    public const int STBTT_MS_LANG_DUTCH = 1043;
    public const int STBTT_MS_LANG_ENGLISH = 1033;
    public const int STBTT_MS_LANG_FRENCH = 1036;
    public const int STBTT_MS_LANG_GERMAN = 1031;
    public const int STBTT_MS_LANG_HEBREW = 1037;
    public const int STBTT_MS_LANG_ITALIAN = 1040;
    public const int STBTT_MS_LANG_JAPANESE = 1041;
    public const int STBTT_MS_LANG_KOREAN = 1042;
    public const int STBTT_MS_LANG_RUSSIAN = 1049;
    public const int STBTT_MS_LANG_SPANISH = 1033;
    public const int STBTT_MS_LANG_SWEDISH = 1053;
    public const int STBTT_PLATFORM_ID_ISO = 2;
    public const int STBTT_PLATFORM_ID_MAC = 1;
    public const int STBTT_PLATFORM_ID_MICROSOFT = 3;
    public const int STBTT_PLATFORM_ID_UNICODE = 0;
    public const int STBTT_UNICODE_EID_ISO_10646 = 2;
    public const int STBTT_UNICODE_EID_UNICODE_1_0 = 0;
    public const int STBTT_UNICODE_EID_UNICODE_1_1 = 1;
    public const int STBTT_UNICODE_EID_UNICODE_2_0_BMP = 3;
    public const int STBTT_UNICODE_EID_UNICODE_2_0_FULL = 4;
    public const int STBTT_vcubic = 4;
    public const int STBTT_vcurve = 3;
    public const int STBTT_vline = 2;
    public const int STBTT_vmove = 1;

    public static int equal(float* a, float* b)
    {
        return a[0] == b[0] && a[1] == b[1] ? 1 : 0;
    }

    public static void stbtt__add_point(stbtt__point* points, int n, float x, float y)
    {
        if (points == null)
            return;
        points[n].x = x;
        points[n].y = y;
    }

    public static int stbtt__CompareUTF8toUTF16_bigendian_prefix(byte* s1, int len1, byte* s2, int len2)
    {
        var i = 0;
        while (len2 != 0)
        {
            var ch = (ushort)(s2[0] * 256 + s2[1]);
            if (ch < 0x80)
            {
                if (i >= len1)
                    return -1;
                if (s1[i++] != ch)
                    return -1;
            }
            else if (ch < 0x800)
            {
                if (i + 1 >= len1)
                    return -1;
                if (s1[i++] != 0xc0 + (ch >> 6))
                    return -1;
                if (s1[i++] != 0x80 + (ch & 0x3f))
                    return -1;
            }
            else if (ch >= 0xd800 && ch < 0xdc00)
            {
                uint c = 0;
                var ch2 = (ushort)(s2[2] * 256 + s2[3]);
                if (i + 3 >= len1)
                    return -1;
                c = (uint)(((ch - 0xd800) << 10) + (ch2 - 0xdc00) + 0x10000);
                if (s1[i++] != 0xf0 + (c >> 18))
                    return -1;
                if (s1[i++] != 0x80 + ((c >> 12) & 0x3f))
                    return -1;
                if (s1[i++] != 0x80 + ((c >> 6) & 0x3f))
                    return -1;
                if (s1[i++] != 0x80 + (c & 0x3f))
                    return -1;
                s2 += 2;
                len2 -= 2;
            }
            else if (ch >= 0xdc00 && ch < 0xe000)
            {
                return -1;
            }
            else
            {
                if (i + 2 >= len1)
                    return -1;
                if (s1[i++] != 0xe0 + (ch >> 12))
                    return -1;
                if (s1[i++] != 0x80 + ((ch >> 6) & 0x3f))
                    return -1;
                if (s1[i++] != 0x80 + (ch & 0x3f))
                    return -1;
            }

            s2 += 2;
            len2 -= 2;
        }

        return i;
    }

    public static int stbtt__compute_crossings_x(float x, float y, int nverts, stbtt_vertex* verts)
    {
        var i = 0;
        var orig = stackalloc float[2];
        var ray = stackalloc float[] { 1, 0 };
        float y_frac = 0;
        var winding = 0;
        y_frac = (float)CRuntime.Fmod(y, 1.0f);
        if (y_frac < 0.01f)
            y += 0.01f;
        else if (y_frac > 0.99f)
            y -= 0.01f;
        orig[0] = x;
        orig[1] = y;
        for (i = 0; i < nverts; ++i)
        {
            if (verts[i].type == STBTT_vline)
            {
                int x0 = verts[i - 1].x;
                int y0 = verts[i - 1].y;
                int x1 = verts[i].x;
                int y1 = verts[i].y;
                if (y > (y0 < y1 ? y0 : y1) && y < (y0 < y1 ? y1 : y0) && x > (x0 < x1 ? x0 : x1))
                {
                    var x_inter = (y - y0) / (y1 - y0) * (x1 - x0) + x0;
                    if (x_inter < x)
                        winding += y0 < y1 ? 1 : -1;
                }
            }

            if (verts[i].type == STBTT_vcurve)
            {
                int x0 = verts[i - 1].x;
                int y0 = verts[i - 1].y;
                int x1 = verts[i].cx;
                int y1 = verts[i].cy;
                int x2 = verts[i].x;
                int y2 = verts[i].y;
                var ax = x0 < (x1 < x2 ? x1 : x2) ? x0 : x1 < x2 ? x1 : x2;
                var ay = y0 < (y1 < y2 ? y1 : y2) ? y0 : y1 < y2 ? y1 : y2;
                var by = y0 < (y1 < y2 ? y2 : y1) ? y1 < y2 ? y2 : y1 : y0;
                if (y > ay && y < by && x > ax)
                {
                    var q0 = stackalloc float[2];
                    var q1 = stackalloc float[2];
                    var q2 = stackalloc float[2];
                    var hits = stackalloc float[4];
                    q0[0] = x0;
                    q0[1] = y0;
                    q1[0] = x1;
                    q1[1] = y1;
                    q2[0] = x2;
                    q2[1] = y2;
                    if (equal(q0, q1) != 0 || equal(q1, q2) != 0)
                    {
                        x0 = verts[i - 1].x;
                        y0 = verts[i - 1].y;
                        x1 = verts[i].x;
                        y1 = verts[i].y;
                        if (y > (y0 < y1 ? y0 : y1) && y < (y0 < y1 ? y1 : y0) && x > (x0 < x1 ? x0 : x1))
                        {
                            var x_inter = (y - y0) / (y1 - y0) * (x1 - x0) + x0;
                            if (x_inter < x)
                                winding += y0 < y1 ? 1 : -1;
                        }
                    }
                    else
                    {
                        var num_hits = stbtt__ray_intersect_bezier(orig, ray, q0, q1, q2, hits);
                        if (num_hits >= 1)
                            if (hits[0] < 0)
                                winding += hits[1] < 0 ? -1 : 1;
                        if (num_hits >= 2)
                            if (hits[2] < 0)
                                winding += hits[3] < 0 ? -1 : 1;
                    }
                }
            }
        }

        return winding;
    }

    public static float stbtt__cuberoot(float x)
    {
        if (x < 0)
            return -(float)CRuntime.Pow(-x, 1.0f / 3.0f);
        return (float)CRuntime.Pow(x, 1.0f / 3.0f);
    }

    public static void stbtt__h_prefilter(byte* pixels, int w, int h, int stride_in_bytes, uint kernel_width)
    {
        var buffer = stackalloc byte[8];
        var safe_w = (int)(w - kernel_width);
        var j = 0;
        CRuntime.Memset(buffer, 0, (ulong)8);
        for (j = 0; j < h; ++j)
        {
            var i = 0;
            uint total = 0;
            CRuntime.Memset(buffer, 0, (ulong)kernel_width);
            total = 0;
            switch (kernel_width)
            {
                case 2:
                    for (i = 0; i <= safe_w; ++i)
                    {
                        total += (uint)(pixels[i] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i];
                        pixels[i] = (byte)(total / 2);
                    }

                    break;
                case 3:
                    for (i = 0; i <= safe_w; ++i)
                    {
                        total += (uint)(pixels[i] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i];
                        pixels[i] = (byte)(total / 3);
                    }

                    break;
                case 4:
                    for (i = 0; i <= safe_w; ++i)
                    {
                        total += (uint)(pixels[i] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i];
                        pixels[i] = (byte)(total / 4);
                    }

                    break;
                case 5:
                    for (i = 0; i <= safe_w; ++i)
                    {
                        total += (uint)(pixels[i] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i];
                        pixels[i] = (byte)(total / 5);
                    }

                    break;
                default:
                    for (i = 0; i <= safe_w; ++i)
                    {
                        total += (uint)(pixels[i] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i];
                        pixels[i] = (byte)(total / kernel_width);
                    }

                    break;
            }

            for (; i < w; ++i)
            {
                total -= buffer[i & (8 - 1)];
                pixels[i] = (byte)(total / kernel_width);
            }

            pixels += stride_in_bytes;
        }
    }

    public static int stbtt__isfont(byte* font)
    {
        if (font[0] == 49 && font[1] == 0 && font[2] == 0 && font[3] == 0)
            return 1;
        if (font[0] == "typ1"[0] && font[1] == "typ1"[1] && font[2] == "typ1"[2] && font[3] == "typ1"[3])
            return 1;
        if (font[0] == "OTTO"[0] && font[1] == "OTTO"[1] && font[2] == "OTTO"[2] && font[3] == "OTTO"[3])
            return 1;
        if (font[0] == 0 && font[1] == 1 && font[2] == 0 && font[3] == 0)
            return 1;
        if (font[0] == "true"[0] && font[1] == "true"[1] && font[2] == "true"[2] && font[3] == "true"[3])
            return 1;
        return 0;
    }

    public static int stbtt__matches(byte* fc, uint offset, byte* name, int flags)
    {
        var nlen = (int)CRuntime.Strlen((sbyte*)name);
        uint nm = 0;
        uint hd = 0;
        if (stbtt__isfont(fc + offset) == 0)
            return 0;
        if (flags != 0)
        {
            hd = stbtt__find_table(fc, offset, "head");
            if ((ttUSHORT(fc + hd + 44) & 7) != (flags & 7))
                return 0;
        }

        nm = stbtt__find_table(fc, offset, "name");
        if (nm == 0)
            return 0;
        if (flags != 0)
        {
            if (stbtt__matchpair(fc, nm, name, nlen, 16, -1) != 0)
                return 1;
            if (stbtt__matchpair(fc, nm, name, nlen, 1, -1) != 0)
                return 1;
            if (stbtt__matchpair(fc, nm, name, nlen, 3, -1) != 0)
                return 1;
        }
        else
        {
            if (stbtt__matchpair(fc, nm, name, nlen, 16, 17) != 0)
                return 1;
            if (stbtt__matchpair(fc, nm, name, nlen, 1, 2) != 0)
                return 1;
            if (stbtt__matchpair(fc, nm, name, nlen, 3, -1) != 0)
                return 1;
        }

        return 0;
    }

    public static int stbtt__matchpair(byte* fc, uint nm, byte* name, int nlen, int target_id, int next_id)
    {
        var i = 0;
        int count = ttUSHORT(fc + nm + 2);
        var stringOffset = (int)(nm + ttUSHORT(fc + nm + 4));
        for (i = 0; i < count; ++i)
        {
            var loc = (uint)(nm + 6 + 12 * i);
            int id = ttUSHORT(fc + loc + 6);
            if (id == target_id)
            {
                int platform = ttUSHORT(fc + loc + 0);
                int encoding = ttUSHORT(fc + loc + 2);
                int language = ttUSHORT(fc + loc + 4);
                if (platform == 0 || platform == 3 && encoding == 1 || platform == 3 && encoding == 10)
                {
                    int slen = ttUSHORT(fc + loc + 8);
                    int off = ttUSHORT(fc + loc + 10);
                    var matchlen =
                        stbtt__CompareUTF8toUTF16_bigendian_prefix(name, nlen, fc + stringOffset + off, slen);
                    if (matchlen >= 0)
                    {
                        if (i + 1 < count && ttUSHORT(fc + loc + 12 + 6) == next_id &&
                            ttUSHORT(fc + loc + 12) == platform && ttUSHORT(fc + loc + 12 + 2) == encoding &&
                            ttUSHORT(fc + loc + 12 + 4) == language)
                        {
                            slen = ttUSHORT(fc + loc + 12 + 8);
                            off = ttUSHORT(fc + loc + 12 + 10);
                            if (slen == 0)
                            {
                                if (matchlen == nlen)
                                    return 1;
                            }
                            else if (matchlen < nlen && name[matchlen] == 32)
                            {
                                ++matchlen;
                                if (stbtt_CompareUTF8toUTF16_bigendian_internal((sbyte*)(name + matchlen),
                                    nlen - matchlen, (sbyte*)(fc + stringOffset + off), slen) != 0)
                                    return 1;
                            }
                        }
                        else
                        {
                            if (matchlen == nlen)
                                return 1;
                        }
                    }
                }
            }
        }

        return 0;
    }

    public static float stbtt__oversample_shift(int oversample)
    {
        if (oversample == 0)
            return 0.0f;
        return -(oversample - 1) / (2.0f * oversample);
    }

    public static float stbtt__position_trapezoid_area(float height, float tx0, float tx1, float bx0, float bx1)
    {
        return stbtt__sized_trapezoid_area(height, tx1 - tx0, bx1 - bx0);
    }

    public static int stbtt__ray_intersect_bezier(float* orig, float* ray, float* q0, float* q1, float* q2,
        float* hits)
    {
        var q0perp = q0[1] * ray[0] - q0[0] * ray[1];
        var q1perp = q1[1] * ray[0] - q1[0] * ray[1];
        var q2perp = q2[1] * ray[0] - q2[0] * ray[1];
        var roperp = orig[1] * ray[0] - orig[0] * ray[1];
        var a = q0perp - 2 * q1perp + q2perp;
        var b = q1perp - q0perp;
        var c = q0perp - roperp;
        float s0 = 0;
        float s1 = 0;
        var num_s = 0;
        if (a != 0.0)
        {
            var discr = b * b - a * c;
            if (discr > 0.0)
            {
                var rcpna = -1 / a;
                var d = (float)CRuntime.Sqrt(discr);
                s0 = (b + d) * rcpna;
                s1 = (b - d) * rcpna;
                if (s0 >= 0.0 && s0 <= 1.0)
                    num_s = 1;
                if (d > 0.0 && s1 >= 0.0 && s1 <= 1.0)
                {
                    if (num_s == 0)
                        s0 = s1;
                    ++num_s;
                }
            }
        }
        else
        {
            s0 = c / (-2 * b);
            if (s0 >= 0.0 && s0 <= 1.0)
                num_s = 1;
        }

        if (num_s == 0) return 0;

        var rcp_len2 = 1 / (ray[0] * ray[0] + ray[1] * ray[1]);
        var rayn_x = ray[0] * rcp_len2;
        var rayn_y = ray[1] * rcp_len2;
        var q0d = q0[0] * rayn_x + q0[1] * rayn_y;
        var q1d = q1[0] * rayn_x + q1[1] * rayn_y;
        var q2d = q2[0] * rayn_x + q2[1] * rayn_y;
        var rod = orig[0] * rayn_x + orig[1] * rayn_y;
        var q10d = q1d - q0d;
        var q20d = q2d - q0d;
        var q0rd = q0d - rod;
        hits[0] = q0rd + s0 * (2.0f - 2.0f * s0) * q10d + s0 * s0 * q20d;
        hits[1] = a * s0 + b;
        if (num_s > 1)
        {
            hits[2] = q0rd + s1 * (2.0f - 2.0f * s1) * q10d + s1 * s1 * q20d;
            hits[3] = a * s1 + b;
            return 2;
        }

        return 1;
    }

    public static float stbtt__sized_trapezoid_area(float height, float top_width, float bottom_width)
    {
        return (top_width + bottom_width) / 2.0f * height;
    }

    public static float stbtt__sized_triangle_area(float height, float width)
    {
        return height * width / 2;
    }

    public static int stbtt__solve_cubic(float a, float b, float c, float* r)
    {
        var s = -a / 3;
        var p = b - a * a / 3;
        var q = a * (2 * a * a - 9 * b) / 27 + c;
        var p3 = p * p * p;
        var d = q * q + 4 * p3 / 27;
        if (d >= 0)
        {
            var z = (float)CRuntime.Sqrt(d);
            var u = (-q + z) / 2;
            var v = (-q - z) / 2;
            u = stbtt__cuberoot(u);
            v = stbtt__cuberoot(v);
            r[0] = s + u + v;
            return 1;
        }
        else
        {
            var u = (float)CRuntime.Sqrt(-p / 3);
            var v = (float)CRuntime.Acos(-CRuntime.Sqrt(-27 / p3) * q / 2) / 3;
            var m = (float)CRuntime.Cos(v);
            var n = (float)CRuntime.Cos(v - 3.141592 / 2) * 1.732050808f;
            r[0] = s + u * 2 * m;
            r[1] = s - u * (m + n);
            r[2] = s - u * (m - n);
            return 3;
        }
    }

    public static void stbtt__tesselate_cubic(stbtt__point* points, int* num_points, float x0, float y0, float x1,
        float y1, float x2, float y2, float x3, float y3, float objspace_flatness_squared, int n)
    {
        var dx0 = x1 - x0;
        var dy0 = y1 - y0;
        var dx1 = x2 - x1;
        var dy1 = y2 - y1;
        var dx2 = x3 - x2;
        var dy2 = y3 - y2;
        var dx = x3 - x0;
        var dy = y3 - y0;
        var longlen = (float)(CRuntime.Sqrt(dx0 * dx0 + dy0 * dy0) + CRuntime.Sqrt(dx1 * dx1 + dy1 * dy1) +
                               CRuntime.Sqrt(dx2 * dx2 + dy2 * dy2));
        var shortlen = (float)CRuntime.Sqrt(dx * dx + dy * dy);
        var flatness_squared = longlen * longlen - shortlen * shortlen;
        if (n > 16)
            return;
        if (flatness_squared > objspace_flatness_squared)
        {
            var x01 = (x0 + x1) / 2;
            var y01 = (y0 + y1) / 2;
            var x12 = (x1 + x2) / 2;
            var y12 = (y1 + y2) / 2;
            var x23 = (x2 + x3) / 2;
            var y23 = (y2 + y3) / 2;
            var xa = (x01 + x12) / 2;
            var ya = (y01 + y12) / 2;
            var xb = (x12 + x23) / 2;
            var yb = (y12 + y23) / 2;
            var mx = (xa + xb) / 2;
            var my = (ya + yb) / 2;
            stbtt__tesselate_cubic(points, num_points, x0, y0, x01, y01, xa, ya, mx, my, objspace_flatness_squared,
                n + 1);
            stbtt__tesselate_cubic(points, num_points, mx, my, xb, yb, x23, y23, x3, y3, objspace_flatness_squared,
                n + 1);
        }
        else
        {
            stbtt__add_point(points, *num_points, x3, y3);
            *num_points = *num_points + 1;
        }
    }

    public static int stbtt__tesselate_curve(stbtt__point* points, int* num_points, float x0, float y0, float x1,
        float y1, float x2, float y2, float objspace_flatness_squared, int n)
    {
        var mx = (x0 + 2 * x1 + x2) / 4;
        var my = (y0 + 2 * y1 + y2) / 4;
        var dx = (x0 + x2) / 2 - mx;
        var dy = (y0 + y2) / 2 - my;
        if (n > 16)
            return 1;
        if (dx * dx + dy * dy > objspace_flatness_squared)
        {
            stbtt__tesselate_curve(points, num_points, x0, y0, (x0 + x1) / 2.0f, (y0 + y1) / 2.0f, mx, my,
                objspace_flatness_squared, n + 1);
            stbtt__tesselate_curve(points, num_points, mx, my, (x1 + x2) / 2.0f, (y1 + y2) / 2.0f, x2, y2,
                objspace_flatness_squared, n + 1);
        }
        else
        {
            stbtt__add_point(points, *num_points, x2, y2);
            *num_points = *num_points + 1;
        }

        return 1;
    }

    public static void stbtt__v_prefilter(byte* pixels, int w, int h, int stride_in_bytes, uint kernel_width)
    {
        var buffer = stackalloc byte[8];
        var safe_h = (int)(h - kernel_width);
        var j = 0;
        CRuntime.Memset(buffer, 0, (ulong)8);
        for (j = 0; j < w; ++j)
        {
            var i = 0;
            uint total = 0;
            CRuntime.Memset(buffer, 0, (ulong)kernel_width);
            total = 0;
            switch (kernel_width)
            {
                case 2:
                    for (i = 0; i <= safe_h; ++i)
                    {
                        total += (uint)(pixels[i * stride_in_bytes] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i * stride_in_bytes];
                        pixels[i * stride_in_bytes] = (byte)(total / 2);
                    }

                    break;
                case 3:
                    for (i = 0; i <= safe_h; ++i)
                    {
                        total += (uint)(pixels[i * stride_in_bytes] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i * stride_in_bytes];
                        pixels[i * stride_in_bytes] = (byte)(total / 3);
                    }

                    break;
                case 4:
                    for (i = 0; i <= safe_h; ++i)
                    {
                        total += (uint)(pixels[i * stride_in_bytes] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i * stride_in_bytes];
                        pixels[i * stride_in_bytes] = (byte)(total / 4);
                    }

                    break;
                case 5:
                    for (i = 0; i <= safe_h; ++i)
                    {
                        total += (uint)(pixels[i * stride_in_bytes] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i * stride_in_bytes];
                        pixels[i * stride_in_bytes] = (byte)(total / 5);
                    }

                    break;
                default:
                    for (i = 0; i <= safe_h; ++i)
                    {
                        total += (uint)(pixels[i * stride_in_bytes] - buffer[i & (8 - 1)]);
                        buffer[(i + kernel_width) & (8 - 1)] = pixels[i * stride_in_bytes];
                        pixels[i * stride_in_bytes] = (byte)(total / kernel_width);
                    }

                    break;
            }

            for (; i < h; ++i)
            {
                total -= buffer[i & (8 - 1)];
                pixels[i * stride_in_bytes] = (byte)(total / kernel_width);
            }

            pixels += 1;
        }
    }

    public static int stbtt_BakeFontBitmap(byte* data, int offset, float pixel_height, byte* pixels, int pw, int ph,
        int first_char, int num_chars, stbtt_bakedchar* chardata)
    {
        return stbtt_BakeFontBitmap_internal(data, offset, pixel_height, pixels, pw, ph, first_char, num_chars,
            chardata);
    }

    public static int stbtt_BakeFontBitmap_internal(byte* data, int offset, float pixel_height, byte* pixels,
        int pw, int ph, int first_char, int num_chars, stbtt_bakedchar* chardata)
    {
        float scale = 0;
        var x = 0;
        var y = 0;
        var bottom_y = 0;
        var i = 0;
        var f = new stbtt_fontinfo();
        f.userdata = null;
        if (stbtt_InitFont(f, data, offset) == 0)
            return -1;
        CRuntime.Memset(pixels, 0, (ulong)(pw * ph));
        x = y = 1;
        bottom_y = 1;
        scale = stbtt_ScaleForPixelHeight(f, pixel_height);
        for (i = 0; i < num_chars; ++i)
        {
            var advance = 0;
            var lsb = 0;
            var x0 = 0;
            var y0 = 0;
            var x1 = 0;
            var y1 = 0;
            var gw = 0;
            var gh = 0;
            var g = stbtt_FindGlyphIndex(f, first_char + i);
            stbtt_GetGlyphHMetrics(f, g, &advance, &lsb);
            stbtt_GetGlyphBitmapBox(f, g, scale, scale, &x0, &y0, &x1, &y1);
            gw = x1 - x0;
            gh = y1 - y0;
            if (x + gw + 1 >= pw)
            {
                y = bottom_y;
                x = 1;
            }

            if (y + gh + 1 >= ph)
                return -i;
            stbtt_MakeGlyphBitmap(f, pixels + x + y * pw, gw, gh, pw, scale, scale, g);
            chardata[i].x0 = (ushort)(short)x;
            chardata[i].y0 = (ushort)(short)y;
            chardata[i].x1 = (ushort)(short)(x + gw);
            chardata[i].y1 = (ushort)(short)(y + gh);
            chardata[i].xadvance = scale * advance;
            chardata[i].xoff = x0;
            chardata[i].yoff = y0;
            x = x + gw + 1;
            if (y + gh + 1 > bottom_y)
                bottom_y = y + gh + 1;
        }

        return bottom_y;
    }

    public static int stbtt_CompareUTF8toUTF16_bigendian(sbyte* s1, int len1, sbyte* s2, int len2)
    {
        return stbtt_CompareUTF8toUTF16_bigendian_internal(s1, len1, s2, len2);
    }

    public static int stbtt_CompareUTF8toUTF16_bigendian_internal(sbyte* s1, int len1, sbyte* s2, int len2)
    {
        return len1 == stbtt__CompareUTF8toUTF16_bigendian_prefix((byte*)s1, len1, (byte*)s2, len2) ? 1 : 0;
    }

    public static int stbtt_FindMatchingFont(byte* fontdata, sbyte* name, int flags)
    {
        return stbtt_FindMatchingFont_internal(fontdata, name, flags);
    }

    public static int stbtt_FindMatchingFont_internal(byte* font_collection, sbyte* name_utf8, int flags)
    {
        var i = 0;
        for (i = 0; ; ++i)
        {
            var off = stbtt_GetFontOffsetForIndex(font_collection, i);
            if (off < 0)
                return off;
            if (stbtt__matches(font_collection, (uint)off, (byte*)name_utf8, flags) != 0)
                return off;
        }
    }

    public static stbtt__point* stbtt_FlattenCurves(stbtt_vertex* vertices, int num_verts, float objspace_flatness,
        int** contour_lengths, int* num_contours, void* userdata)
    {
        stbtt__point* points = null;
        var num_points = 0;
        var objspace_flatness_squared = objspace_flatness * objspace_flatness;
        var i = 0;
        var n = 0;
        var start = 0;
        var pass = 0;
        for (i = 0; i < num_verts; ++i)
            if (vertices[i].type == STBTT_vmove)
                ++n;

        *num_contours = n;
        if (n == 0)
            return null;
        *contour_lengths = (int*)CRuntime.Malloc((ulong)(sizeof(int) * n));
        if (*contour_lengths == null)
        {
            *num_contours = 0;
            return null;
        }

        for (pass = 0; pass < 2; ++pass)
        {
            float x = 0;
            float y = 0;
            if (pass == 1)
            {
                points = (stbtt__point*)CRuntime.Malloc((ulong)(num_points * sizeof(stbtt__point)));
                if (points == null)
                    goto error;
            }

            num_points = 0;
            n = -1;
            for (i = 0; i < num_verts; ++i)
                switch (vertices[i].type)
                {
                    case STBTT_vmove:
                        if (n >= 0)
                            (*contour_lengths)[n] = num_points - start;
                        ++n;
                        start = num_points;
                        x = vertices[i].x;
                        y = vertices[i].y;
                        stbtt__add_point(points, num_points++, x, y);
                        break;
                    case STBTT_vline:
                        x = vertices[i].x;
                        y = vertices[i].y;
                        stbtt__add_point(points, num_points++, x, y);
                        break;
                    case STBTT_vcurve:
                        stbtt__tesselate_curve(points, &num_points, x, y, vertices[i].cx, vertices[i].cy,
                            vertices[i].x, vertices[i].y, objspace_flatness_squared, 0);
                        x = vertices[i].x;
                        y = vertices[i].y;
                        break;
                    case STBTT_vcubic:
                        stbtt__tesselate_cubic(points, &num_points, x, y, vertices[i].cx, vertices[i].cy,
                            vertices[i].cx1, vertices[i].cy1, vertices[i].x, vertices[i].y,
                            objspace_flatness_squared, 0);
                        x = vertices[i].x;
                        y = vertices[i].y;
                        break;
                }

            (*contour_lengths)[n] = num_points - start;
        }

        return points;
    error:;
        CRuntime.Free(points);
        CRuntime.Free(*contour_lengths);
        *contour_lengths = null;
        *num_contours = 0;
        return null;
    }

    public static void stbtt_FreeBitmap(byte* bitmap, void* userdata)
    {
        CRuntime.Free(bitmap);
    }

    public static void stbtt_FreeSDF(byte* bitmap, void* userdata)
    {
        CRuntime.Free(bitmap);
    }

    public static void stbtt_GetBakedQuad(stbtt_bakedchar* chardata, int pw, int ph, int char_index, float* xpos,
        float* ypos, stbtt_aligned_quad* q, int opengl_fillrule)
    {
        var d3d_bias = opengl_fillrule != 0 ? 0 : -0.5f;
        var ipw = 1.0f / pw;
        var iph = 1.0f / ph;
        var b = chardata + char_index;
        var round_x = (int)CRuntime.Floor(*xpos + b->xoff + 0.5f);
        var round_y = (int)CRuntime.Floor(*ypos + b->yoff + 0.5f);
        q->x0 = round_x + d3d_bias;
        q->y0 = round_y + d3d_bias;
        q->x1 = round_x + b->x1 - b->x0 + d3d_bias;
        q->y1 = round_y + b->y1 - b->y0 + d3d_bias;
        q->s0 = b->x0 * ipw;
        q->t0 = b->y0 * iph;
        q->s1 = b->x1 * ipw;
        q->t1 = b->y1 * iph;
        *xpos += b->xadvance;
    }

    public static int stbtt_GetFontOffsetForIndex(byte* data, int index)
    {
        return stbtt_GetFontOffsetForIndex_internal(data, index);
    }

    public static int stbtt_GetFontOffsetForIndex_internal(byte* font_collection, int index)
    {
        if (stbtt__isfont(font_collection) != 0)
            return index == 0 ? 0 : -1;
        if (font_collection[0] == "ttcf"[0] && font_collection[1] == "ttcf"[1] && font_collection[2] == "ttcf"[2] &&
            font_collection[3] == "ttcf"[3])
            if (ttULONG(font_collection + 4) == 0x00010000 || ttULONG(font_collection + 4) == 0x00020000)
            {
                var n = ttLONG(font_collection + 8);
                if (index >= n)
                    return -1;
                return (int)ttULONG(font_collection + 12 + index * 4);
            }

        return -1;
    }

    public static int stbtt_GetNumberOfFonts(byte* data)
    {
        return stbtt_GetNumberOfFonts_internal(data);
    }

    public static int stbtt_GetNumberOfFonts_internal(byte* font_collection)
    {
        if (stbtt__isfont(font_collection) != 0)
            return 1;
        if (font_collection[0] == "ttcf"[0] && font_collection[1] == "ttcf"[1] && font_collection[2] == "ttcf"[2] &&
            font_collection[3] == "ttcf"[3])
            if (ttULONG(font_collection + 4) == 0x00010000 || ttULONG(font_collection + 4) == 0x00020000)
                return ttLONG(font_collection + 8);

        return 0;
    }

    public static void stbtt_GetPackedQuad(stbtt_packedchar* chardata, int pw, int ph, int char_index, float* xpos,
        float* ypos, stbtt_aligned_quad* q, int align_to_integer)
    {
        var ipw = 1.0f / pw;
        var iph = 1.0f / ph;
        var b = chardata + char_index;
        if (align_to_integer != 0)
        {
            float x = (int)CRuntime.Floor(*xpos + b->xoff + 0.5f);
            float y = (int)CRuntime.Floor(*ypos + b->yoff + 0.5f);
            q->x0 = x;
            q->y0 = y;
            q->x1 = x + b->xoff2 - b->xoff;
            q->y1 = y + b->yoff2 - b->yoff;
        }
        else
        {
            q->x0 = *xpos + b->xoff;
            q->y0 = *ypos + b->yoff;
            q->x1 = *xpos + b->xoff2;
            q->y1 = *ypos + b->yoff2;
        }

        q->s0 = b->x0 * ipw;
        q->t0 = b->y0 * iph;
        q->s1 = b->x1 * ipw;
        q->t1 = b->y1 * iph;
        *xpos += b->xadvance;
    }

    public static void stbtt_GetScaledFontVMetrics(byte* fontdata, int index, float size, float* ascent,
        float* descent, float* lineGap)
    {
        var i_ascent = 0;
        var i_descent = 0;
        var i_lineGap = 0;
        float scale = 0;
        var info = new stbtt_fontinfo();
        stbtt_InitFont(info, fontdata, stbtt_GetFontOffsetForIndex(fontdata, index));
        scale = size > 0 ? stbtt_ScaleForPixelHeight(info, size) : stbtt_ScaleForMappingEmToPixels(info, -size);
        stbtt_GetFontVMetrics(info, &i_ascent, &i_descent, &i_lineGap);
        *ascent = i_ascent * scale;
        *descent = i_descent * scale;
        *lineGap = i_lineGap * scale;
    }

    public static void stbtt_setvertex(stbtt_vertex* v, byte type, int x, int y, int cx, int cy)
    {
        v->type = type;
        v->x = (short)x;
        v->y = (short)y;
        v->cx = (short)cx;
        v->cy = (short)cy;
    }

    public static int ttLONG(byte* p)
    {
        return (p[0] << 24) + (p[1] << 16) + (p[2] << 8) + p[3];
    }

    public static short ttSHORT(byte* p)
    {
        return (short)(p[0] * 256 + p[1]);
    }

    public static uint ttULONG(byte* p)
    {
        return (uint)((p[0] << 24) + (p[1] << 16) + (p[2] << 8) + p[3]);
    }

    public static ushort ttUSHORT(byte* p)
    {
        return (ushort)(p[0] * 256 + p[1]);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__point
    {
        public float x;
        public float y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_aligned_quad
    {
        public float x0;
        public float y0;
        public float s0;
        public float t0;
        public float x1;
        public float y1;
        public float s1;
        public float t1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_bakedchar
    {
        public ushort x0;
        public ushort y0;
        public ushort x1;
        public ushort y1;
        public float xoff;
        public float yoff;
        public float xadvance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_packedchar
    {
        public ushort x0;
        public ushort y0;
        public ushort x1;
        public ushort y1;
        public float xoff;
        public float yoff;
        public float xadvance;
        public float xoff2;
        public float yoff2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt_vertex
    {
        public short x;
        public short y;
        public short cx;
        public short cy;
        public short cx1;
        public short cy1;
        public byte type;
        public byte padding;
    }
}

/* Generated CharString */
unsafe partial class StbTrueType
{
    public static void stbtt__csctx_close_shape(stbtt__csctx* ctx)
    {
        if (ctx->first_x != ctx->x || ctx->first_y != ctx->y)
            stbtt__csctx_v(ctx, STBTT_vline, (int)ctx->first_x, (int)ctx->first_y, 0, 0, 0, 0);
    }

    public static void stbtt__csctx_rccurve_to(stbtt__csctx* ctx, float dx1, float dy1, float dx2, float dy2,
        float dx3, float dy3)
    {
        var cx1 = ctx->x + dx1;
        var cy1 = ctx->y + dy1;
        var cx2 = cx1 + dx2;
        var cy2 = cy1 + dy2;
        ctx->x = cx2 + dx3;
        ctx->y = cy2 + dy3;
        stbtt__csctx_v(ctx, STBTT_vcubic, (int)ctx->x, (int)ctx->y, (int)cx1, (int)cy1, (int)cx2, (int)cy2);
    }

    public static void stbtt__csctx_rline_to(stbtt__csctx* ctx, float dx, float dy)
    {
        ctx->x += dx;
        ctx->y += dy;
        stbtt__csctx_v(ctx, STBTT_vline, (int)ctx->x, (int)ctx->y, 0, 0, 0, 0);
    }

    public static void stbtt__csctx_rmove_to(stbtt__csctx* ctx, float dx, float dy)
    {
        stbtt__csctx_close_shape(ctx);
        ctx->first_x = ctx->x = ctx->x + dx;
        ctx->first_y = ctx->y = ctx->y + dy;
        stbtt__csctx_v(ctx, STBTT_vmove, (int)ctx->x, (int)ctx->y, 0, 0, 0, 0);
    }

    public static void stbtt__csctx_v(stbtt__csctx* c, byte type, int x, int y, int cx, int cy, int cx1, int cy1)
    {
        if (c->bounds != 0)
        {
            stbtt__track_vertex(c, x, y);
            if (type == STBTT_vcubic)
            {
                stbtt__track_vertex(c, cx, cy);
                stbtt__track_vertex(c, cx1, cy1);
            }
        }
        else
        {
            stbtt_setvertex(&c->pvertices[c->num_vertices], type, x, y, cx, cy);
            c->pvertices[c->num_vertices].cx1 = (short)cx1;
            c->pvertices[c->num_vertices].cy1 = (short)cy1;
        }

        c->num_vertices++;
    }

    public static void stbtt__track_vertex(stbtt__csctx* c, int x, int y)
    {
        if (x > c->max_x || c->started == 0)
            c->max_x = x;
        if (y > c->max_y || c->started == 0)
            c->max_y = y;
        if (x < c->min_x || c->started == 0)
            c->min_x = x;
        if (y < c->min_y || c->started == 0)
            c->min_y = y;
        c->started = 1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__csctx
    {
        public int bounds;
        public int started;
        public float first_x;
        public float first_y;
        public float x;
        public float y;
        public int min_x;
        public int max_x;
        public int min_y;
        public int max_y;
        public stbtt_vertex* pvertices;
        public int num_vertices;
    }
}

/* Generated Buffer */
unsafe partial class StbTrueType
{
    public static uint stbtt__buf_get(stbtt__buf* b, int n)
    {
        uint v = 0;
        var i = 0;
        for (i = 0; i < n; i++) v = (v << 8) | stbtt__buf_get8(b);

        return v;
    }

    public static byte stbtt__buf_get8(stbtt__buf* b)
    {
        if (b->cursor >= b->size)
            return 0;
        return b->data[b->cursor++];
    }

    public static byte stbtt__buf_peek8(stbtt__buf* b)
    {
        if (b->cursor >= b->size)
            return 0;
        return b->data[b->cursor];
    }

    public static stbtt__buf stbtt__buf_range(stbtt__buf* b, int o, int s)
    {
        var r = stbtt__new_buf(null, 0);
        if (o < 0 || s < 0 || o > b->size || s > b->size - o)
            return r;
        r.data = b->data + o;
        r.size = s;
        return r;
    }

    public static void stbtt__buf_seek(stbtt__buf* b, int o)
    {
        b->cursor = o > b->size || o < 0 ? b->size : o;
    }

    public static void stbtt__buf_skip(stbtt__buf* b, int o)
    {
        stbtt__buf_seek(b, b->cursor + o);
    }

    public static stbtt__buf stbtt__cff_get_index(stbtt__buf* b)
    {
        var count = 0;
        var start = 0;
        var offsize = 0;
        start = b->cursor;
        count = (int)stbtt__buf_get(b, 2);
        if (count != 0)
        {
            offsize = stbtt__buf_get8(b);
            stbtt__buf_skip(b, offsize * count);
            stbtt__buf_skip(b, (int)(stbtt__buf_get(b, offsize) - 1));
        }

        return stbtt__buf_range(b, start, b->cursor - start);
    }

    public static int stbtt__cff_index_count(stbtt__buf* b)
    {
        stbtt__buf_seek(b, 0);
        return (int)stbtt__buf_get(b, 2);
    }

    public static stbtt__buf stbtt__cff_index_get(stbtt__buf b, int i)
    {
        var count = 0;
        var offsize = 0;
        var start = 0;
        var end = 0;
        stbtt__buf_seek(&b, 0);
        count = (int)stbtt__buf_get(&b, 2);
        offsize = stbtt__buf_get8(&b);
        stbtt__buf_skip(&b, i * offsize);
        start = (int)stbtt__buf_get(&b, offsize);
        end = (int)stbtt__buf_get(&b, offsize);
        return stbtt__buf_range(&b, 2 + (count + 1) * offsize + start, end - start);
    }

    public static uint stbtt__cff_int(stbtt__buf* b)
    {
        int b0 = stbtt__buf_get8(b);
        if (b0 >= 32 && b0 <= 246)
            return (uint)(b0 - 139);
        if (b0 >= 247 && b0 <= 250)
            return (uint)((b0 - 247) * 256 + stbtt__buf_get8(b) + 108);
        if (b0 >= 251 && b0 <= 254)
            return (uint)(-(b0 - 251) * 256 - stbtt__buf_get8(b) - 108);
        if (b0 == 28)
            return stbtt__buf_get(b, 2);
        if (b0 == 29)
            return stbtt__buf_get(b, 4);
        return 0;
    }

    public static void stbtt__cff_skip_operand(stbtt__buf* b)
    {
        var v = 0;
        int b0 = stbtt__buf_peek8(b);
        if (b0 == 30)
        {
            stbtt__buf_skip(b, 1);
            while (b->cursor < b->size)
            {
                v = stbtt__buf_get8(b);
                if ((v & 0xF) == 0xF || v >> 4 == 0xF)
                    break;
            }
        }
        else
        {
            stbtt__cff_int(b);
        }
    }

    public static stbtt__buf stbtt__dict_get(stbtt__buf* b, int key)
    {
        stbtt__buf_seek(b, 0);
        while (b->cursor < b->size)
        {
            var start = b->cursor;
            var end = 0;
            var op = 0;
            while (stbtt__buf_peek8(b) >= 28) stbtt__cff_skip_operand(b);

            end = b->cursor;
            op = stbtt__buf_get8(b);
            if (op == 12)
                op = stbtt__buf_get8(b) | 0x100;
            if (op == key)
                return stbtt__buf_range(b, start, end - start);
        }

        return stbtt__buf_range(b, 0, 0);
    }

    public static void stbtt__dict_get_ints(stbtt__buf* b, int key, int outcount, uint* _out_)
    {
        var i = 0;
        var operands = stbtt__dict_get(b, key);
        for (i = 0; i < outcount && operands.cursor < operands.size; i++) _out_[i] = stbtt__cff_int(&operands);
    }

    public static stbtt__buf stbtt__get_subr(stbtt__buf idx, int n)
    {
        var count = stbtt__cff_index_count(&idx);
        var bias = 107;
        if (count >= 33900)
            bias = 32768;
        else if (count >= 1240)
            bias = 1131;
        n += bias;
        if (n < 0 || n >= count)
            return stbtt__new_buf(null, 0);
        return stbtt__cff_index_get(idx, n);
    }

    public static stbtt__buf stbtt__get_subrs(stbtt__buf cff, stbtt__buf fontdict)
    {
        uint subrsoff = 0;
        var private_loc = stackalloc uint[] { 0, 0 };
        var pdict = new stbtt__buf();
        stbtt__dict_get_ints(&fontdict, 18, 2, private_loc);
        if (private_loc[1] == 0 || private_loc[0] == 0)
            return stbtt__new_buf(null, 0);
        pdict = stbtt__buf_range(&cff, (int)private_loc[1], (int)private_loc[0]);
        stbtt__dict_get_ints(&pdict, 19, 1, &subrsoff);
        if (subrsoff == 0)
            return stbtt__new_buf(null, 0);
        stbtt__buf_seek(&cff, (int)(private_loc[1] + subrsoff));
        return stbtt__cff_get_index(&cff);
    }

    public static stbtt__buf stbtt__new_buf(void* p, ulong size)
    {
        var r = new stbtt__buf();
        r.data = (byte*)p;
        r.size = (int)size;
        r.cursor = 0;
        return r;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stbtt__buf
    {
        public byte* data;
        public int cursor;
        public int size;
    }
}

/* Public */
internal static unsafe partial class StbTrueType
{
    public static int NativeAllocations => MemoryStats.Allocations;

    public class stbtt_fontinfo : IDisposable
    {
        public stbtt__buf cff;
        public stbtt__buf charstrings;
        public byte* data = null;
        public stbtt__buf fdselect;
        public stbtt__buf fontdicts;
        public int fontstart;
        public int glyf;
        public int gpos;
        public stbtt__buf gsubrs;
        public int head;
        public int hhea;
        public int hmtx;
        public int index_map;
        public int indexToLocFormat;
        public bool isDataCopy;
        public int kern;
        public int loca;
        public int numGlyphs;
        public stbtt__buf subrs;
        public int svg;
        public void* userdata;

        public void Dispose()
        {
            Dispose(true);
        }

        ~stbtt_fontinfo()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDataCopy && data != null)
            {
                CRuntime.Free(data);
                data = null;
            }
        }
    }

    public static uint stbtt__find_table(byte* data, uint fontstart, string tag)
    {
        int num_tables = ttUSHORT(data + fontstart + 4);
        var tabledir = fontstart + 12;
        int i;
        for (i = 0; i < num_tables; ++i)
        {
            var loc = (uint)(tabledir + 16 * i);
            if ((data + loc + 0)[0] == tag[0] && (data + loc + 0)[1] == tag[1] &&
                (data + loc + 0)[2] == tag[2] && (data + loc + 0)[3] == tag[3])
                return ttULONG(data + loc + 8);
        }

        return 0;
    }

    public static bool stbtt_BakeFontBitmap(byte[] ttf, int offset, float pixel_height, byte[] pixels, int pw,
        int ph,
        int first_char, int num_chars, stbtt_bakedchar[] chardata)
    {
        fixed (byte* ttfPtr = ttf)
        {
            fixed (byte* pixelsPtr = pixels)
            {
                fixed (stbtt_bakedchar* chardataPtr = chardata)
                {
                    var result = stbtt_BakeFontBitmap(ttfPtr, offset, pixel_height, pixelsPtr, pw, ph, first_char,
                        num_chars,
                        chardataPtr);

                    return result != 0;
                }
            }
        }
    }

    /// <summary>
    ///     Creates and initializes a font from ttf/otf/ttc data
    /// </summary>
    /// <param name="data"></param>
    /// <param name="offset"></param>
    /// <returns>null if the data was invalid</returns>
    public static stbtt_fontinfo CreateFont(byte[] data, int offset)
    {
        var dataCopy = (byte*)CRuntime.Malloc((ulong)data.Length);
        Marshal.Copy(data, 0, new IntPtr(dataCopy), data.Length);

        var info = new stbtt_fontinfo
        {
            isDataCopy = true
        };

        if (stbtt_InitFont_internal(info, dataCopy, offset) == 0)
        {
            info.Dispose();
            return null;
        }

        return info;
    }
}

#pragma warning restore CS0162, CS8618, CA2014, CS8625, CS0649
