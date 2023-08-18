using System.Runtime.InteropServices;

namespace FlatStage.Foundation.FreeType;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_BBox
{
    public int xMin;
    public int yMin;
    public int xMax;
    public int yMax;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Bitmap
{
    public uint rows;
    public uint width;
    public int pitch;
    public byte* buffer;
    public ushort num_grays;
    public FT_Pixel_Mode pixel_mode;
    public byte palette_mode;
    public void* palette;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Bitmap_Size
{
    public short height;
    public short width;
    public int size;
    public int x_ppem;
    public int y_ppem;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_CharMapRec
{
    public FT_FaceRec* face;
    public FT_Encoding encoding;
    public ushort platform_id;
    public ushort encoding_id;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_DriverRec
{
}

public enum FT_Encoding : int
{
    FT_ENCODING_NONE = 0,
    FT_ENCODING_MS_SYMBOL = 1937337698,
    FT_ENCODING_UNICODE = 1970170211,
    FT_ENCODING_SJIS = 1936353651,
    FT_ENCODING_PRC = 1734484000,
    FT_ENCODING_BIG5 = 1651074869,
    FT_ENCODING_WANSUNG = 2002873971,
    FT_ENCODING_JOHAB = 1785686113,
    FT_ENCODING_GB2312 = 1734484000,
    FT_ENCODING_MS_SJIS = 1936353651,
    FT_ENCODING_MS_GB2312 = 1734484000,
    FT_ENCODING_MS_BIG5 = 1651074869,
    FT_ENCODING_MS_WANSUNG = 2002873971,
    FT_ENCODING_MS_JOHAB = 1785686113,
    FT_ENCODING_ADOBE_STANDARD = 1094995778,
    FT_ENCODING_ADOBE_EXPERT = 1094992453,
    FT_ENCODING_ADOBE_CUSTOM = 1094992451,
    FT_ENCODING_ADOBE_LATIN_1 = 1818326065,
    FT_ENCODING_OLD_LATIN_2 = 1818326066,
    FT_ENCODING_APPLE_ROMAN = 1634889070
}

public enum FT_Error : int
{
    FT_Err_Ok = 0,
    FT_Err_Cannot_Open_Resource = 1,
    FT_Err_Unknown_File_Format = 2,
    FT_Err_Invalid_File_Format = 3,
    FT_Err_Invalid_Version = 4,
    FT_Err_Lower_Module_Version = 5,
    FT_Err_Invalid_Argument = 6,
    FT_Err_Unimplemented_Feature = 7,
    FT_Err_Invalid_Table = 8,
    FT_Err_Invalid_Offset = 9,
    FT_Err_Array_Too_Large = 10,
    FT_Err_Missing_Module = 11,
    FT_Err_Missing_Property = 12,
    FT_Err_Invalid_Glyph_Index = 16,
    FT_Err_Invalid_Character_Code = 17,
    FT_Err_Invalid_Glyph_Format = 18,
    FT_Err_Cannot_Render_Glyph = 19,
    FT_Err_Invalid_Outline = 20,
    FT_Err_Invalid_Composite = 21,
    FT_Err_Too_Many_Hints = 22,
    FT_Err_Invalid_Pixel_Size = 23,
    FT_Err_Invalid_SVG_Document = 24,
    FT_Err_Invalid_Handle = 32,
    FT_Err_Invalid_Library_Handle = 33,
    FT_Err_Invalid_Driver_Handle = 34,
    FT_Err_Invalid_Face_Handle = 35,
    FT_Err_Invalid_Size_Handle = 36,
    FT_Err_Invalid_Slot_Handle = 37,
    FT_Err_Invalid_CharMap_Handle = 38,
    FT_Err_Invalid_Cache_Handle = 39,
    FT_Err_Invalid_Stream_Handle = 40,
    FT_Err_Too_Many_Drivers = 48,
    FT_Err_Too_Many_Extensions = 49,
    FT_Err_Out_Of_Memory = 64,
    FT_Err_Unlisted_Object = 65,
    FT_Err_Cannot_Open_Stream = 81,
    FT_Err_Invalid_Stream_Seek = 82,
    FT_Err_Invalid_Stream_Skip = 83,
    FT_Err_Invalid_Stream_Read = 84,
    FT_Err_Invalid_Stream_Operation = 85,
    FT_Err_Invalid_Frame_Operation = 86,
    FT_Err_Nested_Frame_Access = 87,
    FT_Err_Invalid_Frame_Read = 88,
    FT_Err_Raster_Uninitialized = 96,
    FT_Err_Raster_Corrupted = 97,
    FT_Err_Raster_Overflow = 98,
    FT_Err_Raster_Negative_Height = 99,
    FT_Err_Too_Many_Caches = 112,
    FT_Err_Invalid_Opcode = 128,
    FT_Err_Too_Few_Arguments = 129,
    FT_Err_Stack_Overflow = 130,
    FT_Err_Code_Overflow = 131,
    FT_Err_Bad_Argument = 132,
    FT_Err_Divide_By_Zero = 133,
    FT_Err_Invalid_Reference = 134,
    FT_Err_Debug_OpCode = 135,
    FT_Err_ENDF_In_Exec_Stream = 136,
    FT_Err_Nested_DEFS = 137,
    FT_Err_Invalid_CodeRange = 138,
    FT_Err_Execution_Too_Long = 139,
    FT_Err_Too_Many_Function_Defs = 140,
    FT_Err_Too_Many_Instruction_Defs = 141,
    FT_Err_Table_Missing = 142,
    FT_Err_Horiz_Header_Missing = 143,
    FT_Err_Locations_Missing = 144,
    FT_Err_Name_Table_Missing = 145,
    FT_Err_CMap_Table_Missing = 146,
    FT_Err_Hmtx_Table_Missing = 147,
    FT_Err_Post_Table_Missing = 148,
    FT_Err_Invalid_Horiz_Metrics = 149,
    FT_Err_Invalid_CharMap_Format = 150,
    FT_Err_Invalid_PPem = 151,
    FT_Err_Invalid_Vert_Metrics = 152,
    FT_Err_Could_Not_Find_Context = 153,
    FT_Err_Invalid_Post_Table_Format = 154,
    FT_Err_Invalid_Post_Table = 155,
    FT_Err_DEF_In_Glyf_Bytecode = 156,
    FT_Err_Missing_Bitmap = 157,
    FT_Err_Missing_SVG_Hooks = 158,
    FT_Err_Syntax_Error = 160,
    FT_Err_Stack_Underflow = 161,
    FT_Err_Ignore = 162,
    FT_Err_No_Unicode_Glyph_Name = 163,
    FT_Err_Glyph_Too_Big = 164,
    FT_Err_Missing_Startfont_Field = 176,
    FT_Err_Missing_Font_Field = 177,
    FT_Err_Missing_Size_Field = 178,
    FT_Err_Missing_Fontboundingbox_Field = 179,
    FT_Err_Missing_Chars_Field = 180,
    FT_Err_Missing_Startchar_Field = 181,
    FT_Err_Missing_Encoding_Field = 182,
    FT_Err_Missing_Bbx_Field = 183,
    FT_Err_Bbx_Too_Big = 184,
    FT_Err_Corrupted_Font_Header = 185,
    FT_Err_Corrupted_Font_Glyphs = 186,
    FT_Err_Max = 187
}

public enum FT_FaceFlag : int
{
    FT_FACE_FLAG_SCALABLE = (1 << 0),
    FT_FACE_FLAG_FIXED_SIZES = (1 << 1),
    FT_FACE_FLAG_FIXED_WIDTH = (1 << 2),
    FT_FACE_FLAG_SFNT = (1 << 3),
    FT_FACE_FLAG_HORIZONTAL = (1 << 4),
    FT_FACE_FLAG_VERTICAL = (1 << 5),
    FT_FACE_FLAG_KERNING = (1 << 6),
    FT_FACE_FLAG_FAST_GLYPHS = (1 << 7),
    FT_FACE_FLAG_MULTIPLE_MASTERS = (1 << 8),
    FT_FACE_FLAG_GLYPH_NAMES = (1 << 9),
    FT_FACE_FLAG_EXTERNAL_STREAM = (1 << 10),
    FT_FACE_FLAG_HINTER = (1 << 11),
    FT_FACE_FLAG_CID_KEYED = (1 << 12),
    FT_FACE_FLAG_TRICKY = (1 << 13),
    FT_FACE_FLAG_COLOR = (1 << 14),
    FT_FACE_FLAG_VARIATION = (1 << 15),
    FT_FACE_FLAG_SVG = (1 << 16),
    FT_FACE_FLAG_SBIX = (1 << 17),
    FT_FACE_FLAG_SBIX_OVERLAY = (1 << 18)
}

public enum FT_Glyph_Format : int
{
    FT_GLYPH_FORMAT_NONE = 0,
    FT_GLYPH_FORMAT_COMPOSITE = 1668246896,
    FT_GLYPH_FORMAT_BITMAP = 1651078259,
    FT_GLYPH_FORMAT_OUTLINE = 1869968492,
    FT_GLYPH_FORMAT_PLOTTER = 1886154612,
    FT_GLYPH_FORMAT_SVG = 1398163232
}

public enum FT_Load : int
{
    FT_LOAD_DEFAULT = 0x0,
    FT_LOAD_NO_SCALE = (1 << 0),
    FT_LOAD_NO_HINTING = (1 << 1),
    FT_LOAD_RENDER = (1 << 2),
    FT_LOAD_NO_BITMAP = (1 << 3),
    FT_LOAD_VERTICAL_LAYOUT = (1 << 4),
    FT_LOAD_FORCE_AUTOHINT = (1 << 5),
    FT_LOAD_CROP_BITMAP = (1 << 6),
    FT_LOAD_PEDANTIC = (1 << 7),
    FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = (1 << 9),
    FT_LOAD_NO_RECURSE = (1 << 10),
    FT_LOAD_IGNORE_TRANSFORM = (1 << 11),
    FT_LOAD_MONOCHROME = (1 << 12),
    FT_LOAD_LINEAR_DESIGN = (1 << 13),
    FT_LOAD_SBITS_ONLY = (1 << 14),
    FT_LOAD_NO_AUTOHINT = (1 << 15),
    FT_LOAD_COLOR = (1 << 20),
    FT_LOAD_COMPUTE_METRICS = (1 << 21),
    FT_LOAD_BITMAP_METRICS_ONLY = (1 << 22),
    FT_LOAD_NO_SVG = (1 << 24),
    FT_LOAD_ADVANCE_ONLY = (1 << 8),
    FT_LOAD_SVG_ONLY = (1 << 23)
}

public enum FT_Pixel_Mode : byte
{
    FT_PIXEL_MODE_NONE = 0,
    FT_PIXEL_MODE_MONO = 1,
    FT_PIXEL_MODE_GRAY = 2,
    FT_PIXEL_MODE_GRAY2 = 3,
    FT_PIXEL_MODE_GRAY4 = 4,
    FT_PIXEL_MODE_LCD = 5,
    FT_PIXEL_MODE_LCD_V = 6,
    FT_PIXEL_MODE_BGRA = 7,
    FT_PIXEL_MODE_MAX = 8
}

public enum FT_Render_Mode : int
{
    FT_RENDER_MODE_NORMAL = 0,
    FT_RENDER_MODE_LIGHT = 1,
    FT_RENDER_MODE_MONO = 2,
    FT_RENDER_MODE_LCD = 3,
    FT_RENDER_MODE_LCD_V = 4,
    FT_RENDER_MODE_SDF = 5,
    FT_RENDER_MODE_MAX = 6
}

public enum FT_Size_Request_Type : int
{
    FT_SIZE_REQUEST_TYPE_NOMINAL = 0,
    FT_SIZE_REQUEST_TYPE_REAL_DIM = 1,
    FT_SIZE_REQUEST_TYPE_BBOX = 2,
    FT_SIZE_REQUEST_TYPE_CELL = 3,
    FT_SIZE_REQUEST_TYPE_SCALES = 4,
    FT_SIZE_REQUEST_TYPE_MAX = 5
}

public enum FT_StyleFlag : int
{
    FT_STYLE_FLAG_ITALIC = (1 << 0),
    FT_STYLE_FLAG_BOLD = (1 << 1)
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_FaceRec
{
    public int num_faces;
    public int face_index;
    public FT_FaceFlag face_flags;
    public FT_StyleFlag style_flags;
    public int num_glyphs;
    public byte* family_name;
    public byte* style_name;
    public int num_fixed_sizes;
    public FT_Bitmap_Size* available_sizes;
    public int num_charmaps;
    public FT_CharMapRec** charmaps;
    public FT_Generic generic;
    public FT_BBox bbox;
    public ushort units_per_EM;
    public short ascender;
    public short descender;
    public short height;
    public short max_advance_width;
    public short max_advance_height;
    public short underline_position;
    public short underline_thickness;
    public FT_GlyphSlotRec* glyph;
    public FT_SizeRec* size;
    public FT_CharMapRec* charmap;
    public FT_DriverRec* driver;
    public FT_MemoryRec* memory;
    public FT_StreamRec* stream;
    public FT_ListRec sizes_list;
    public FT_Generic autohint;
    public void* extensions;
    public FT_Face_InternalRec* _internal;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Face_InternalRec
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Generic
{
    public void* data;
    public delegate* unmanaged<void*>* finalizer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_GlyphSlotRec
{
    public FT_LibraryRec* library;
    public FT_FaceRec* face;
    public FT_GlyphSlotRec* next;
    public uint glyph_index;
    public FT_Generic generic;
    public FT_Glyph_Metrics metrics;
    public int linearHoriAdvance;
    public int linearVertAdvance;
    public FT_Vector advance;
    public FT_Glyph_Format format;
    public FT_Bitmap bitmap;
    public int bitmap_left;
    public int bitmap_top;
    public FT_Outline_ outline;
    public uint num_subglyphs;
    public FT_SubGlyphRec_* subglyphs;
    public void* control_data;
    public int control_len;
    public int lsb_delta;
    public int rsb_delta;
    public void* other;
    public FT_Slot_InternalRec_* _internal;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Glyph_Metrics
{
    public int width;
    public int height;
    public int horiBearingX;
    public int horiBearingY;
    public int horiAdvance;
    public int vertBearingX;
    public int vertBearingY;
    public int vertAdvance;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_LibraryRec
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_ListNodeRec
{
    public FT_ListNodeRec* prev;
    public FT_ListNodeRec* next;
    public void* data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_ListRec
{
    public FT_ListNodeRec* head;
    public FT_ListNodeRec* tail;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Matrix
{
    public int xx;
    public int xy;
    public int yx;
    public int yy;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_MemoryRec
{
    public void* user;
    public delegate* unmanaged<FT_MemoryRec*, int>* alloc;
    public delegate* unmanaged<FT_MemoryRec*, void*>* free;
    public delegate* unmanaged<FT_MemoryRec*, int, int, void*>* realloc;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_ModuleRec_
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Open_Args
{
    public uint flags;
    public byte* memory_base;
    public int memory_size;
    public byte* pathname;
    public FT_StreamRec* stream;
    public FT_ModuleRec_* driver;
    public int num_params;
    public FT_Parameter* _params;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Outline_
{
    public short n_contours;
    public short n_points;
    public FT_Vector* points;
    public byte* tags;
    public short* contours;
    public int flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Parameter
{
    public uint tag;
    public void* data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_SizeRec
{
    public FT_FaceRec* face;
    public FT_Generic generic;
    public FT_Size_Metrics metrics;
    public FT_Size_InternalRec* _internal;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Size_InternalRec
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Size_Metrics
{
    public ushort x_ppem;
    public ushort y_ppem;
    public int x_scale;
    public int y_scale;
    public int ascender;
    public int descender;
    public int height;
    public int max_advance;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Size_RequestRec
{
    public FT_Size_Request_Type type;
    public int width;
    public int height;
    public uint horiResolution;
    public uint vertResolution;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Slot_InternalRec_
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_StreamDesc
{
    public int value;
    public void* pointer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_StreamRec
{
    public byte* _base;
    public uint size;
    public uint pos;
    public FT_StreamDesc descriptor;
    public FT_StreamDesc pathname;
    public delegate* unmanaged<FT_StreamRec*, uint, byte*, uint>* read;
    public delegate* unmanaged<FT_StreamRec*>* close;
    public FT_MemoryRec* memory;
    public byte* cursor;
    public byte* limit;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_SubGlyphRec_
{
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct FT_Vector
{
    public int x;
    public int y;
}

internal static unsafe class FT
{
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Init_FreeType(FT_LibraryRec** alibrary);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Done_FreeType(FT_LibraryRec* library);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_New_Face(FT_LibraryRec* library, byte* filepathname, int face_index, FT_FaceRec** aface);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_New_Memory_Face(FT_LibraryRec* library, byte* file_base, int file_size, int face_index, FT_FaceRec** aface);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Open_Face(FT_LibraryRec* library, FT_Open_Args* args, int face_index, FT_FaceRec** aface);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Attach_File(FT_FaceRec* face, byte* filepathname);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Attach_Stream(FT_FaceRec* face, FT_Open_Args* parameters);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Reference_Face(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Done_Face(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Select_Size(FT_FaceRec* face, int strike_index);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Request_Size(FT_FaceRec* face, FT_Size_RequestRec* req);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Set_Char_Size(FT_FaceRec* face, int char_width, int char_height, uint horz_resolution, uint vert_resolution);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Set_Pixel_Sizes(FT_FaceRec* face, uint pixel_width, uint pixel_height);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Load_Glyph(FT_FaceRec* face, uint glyph_index, FT_Load load_flags);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Load_Char(FT_FaceRec* face, uint char_code, FT_Load load_flags);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FT_Set_Transform(FT_FaceRec* face, FT_Matrix* matrix, FT_Vector* delta);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FT_Get_Transform(FT_FaceRec* face, FT_Matrix* matrix, FT_Vector* delta);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Render_Glyph(FT_GlyphSlotRec* slot, FT_Render_Mode render_mode);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Get_Kerning(FT_FaceRec* face, uint left_glyph, uint right_glyph, uint kern_mode, FT_Vector* akerning);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Get_Track_Kerning(FT_FaceRec* face, int point_size, int degree, int* akerning);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Select_Charmap(FT_FaceRec* face, FT_Encoding encoding);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Set_Charmap(FT_FaceRec* face, FT_CharMapRec* charmap);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_Get_Charmap_Index(FT_CharMapRec* charmap);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint FT_Get_Char_Index(FT_FaceRec* face, uint charcode);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint FT_Get_First_Char(FT_FaceRec* face, uint* agindex);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint FT_Get_Next_Char(FT_FaceRec* face, uint char_code, uint* agindex);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Face_Properties(FT_FaceRec* face, uint num_properties, FT_Parameter* properties);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint FT_Get_Name_Index(FT_FaceRec* face, byte* glyph_name);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Get_Glyph_Name(FT_FaceRec* face, uint glyph_index, void* buffer, uint buffer_max);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* FT_Get_Postscript_Name(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern FT_Error FT_Get_SubGlyph_Info(FT_GlyphSlotRec* glyph, uint sub_index, int* p_index, uint* p_flags, int* p_arg1, int* p_arg2, FT_Matrix* p_transform);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern ushort FT_Get_FSType_Flags(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint FT_Face_GetCharVariantIndex(FT_FaceRec* face, uint charcode, uint variantSelector);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_Face_GetCharVariantIsDefault(FT_FaceRec* face, uint charcode, uint variantSelector);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* FT_Face_GetVariantSelectors(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* FT_Face_GetVariantsOfChar(FT_FaceRec* face, uint charcode);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint* FT_Face_GetCharsOfVariant(FT_FaceRec* face, uint variantSelector);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_MulDiv(int a, int b, int c);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_MulFix(int a, int b);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_DivFix(int a, int b);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_RoundFix(int a);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_CeilFix(int a);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FT_FloorFix(int a);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FT_Vector_Transform(FT_Vector* vector, FT_Matrix* matrix);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FT_Library_Version(FT_LibraryRec* library, int* amajor, int* aminor, int* apatch);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte FT_Face_CheckTrueTypePatents(FT_FaceRec* face);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte FT_Face_SetUnpatentedHinting(FT_FaceRec* face, byte value);
    [DllImport("freetype.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte* FT_Error_String(FT_Error error_code);
}
