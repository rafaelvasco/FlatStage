﻿#pragma warning disable CS0162, CS8618, CA2014, CS8625, CS0649
#nullable disable

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Stb;

/* Generated  */
public static unsafe partial class StbVorbis
{
    public static sbyte[,] channel_position =
    {
            {0, 0, 0, 0, 0, 0},
            {2 | 4 | 1, 0, 0, 0, 0, 0},
            {2 | 1, 4 | 1, 0, 0, 0, 0},
            {2 | 1, 2 | 4 | 1, 4 | 1, 0, 0, 0},
            {2 | 1, 4 | 1, 2 | 1, 4 | 1, 0, 0},
            {2 | 1, 2 | 4 | 1, 4 | 1, 2 | 1, 4 | 1, 0},
            {2 | 1, 2 | 4 | 1, 4 | 1, 2 | 1, 4 | 1, 2 | 4 | 1}
        };

    public static int NativeAllocations => MemoryStats.Allocations;

    public static short[] decode_vorbis_from_memory(byte[] input, out int sampleRate, out int chan)
    {
        short* result = null;
        var length = 0;
        fixed (byte* b = input)
        {
            int c, s;
            length = stb_vorbis_decode_memory(b, input.Length, &c, &s, ref result);
            if (length == -1)
            {
                throw new Exception("Unable to decode");
            }

            chan = c;
            sampleRate = s;
        }

        var output = new short[length * chan];
        Marshal.Copy(new IntPtr(result), output, 0, output.Length);
        CRuntime.Free(result);

        return output;
    }

    public class Residue
    {
        public uint begin;
        public byte classbook;
        public byte** classdata;
        public byte classifications;
        public uint end;
        public uint part_size;
        public short[,] residue_books = null!;
    }

    public class stb_vorbis
    {
        public float*[] A = new float*[2];
        public uint acc;
        public float*[] B = new float*[2];
        public ushort*[] bit_reverse = new ushort*[2];
        public int[] blocksize = new int[2];
        public int blocksize_0;
        public int blocksize_1;
        public byte bytes_in_seg;
        public float*[] C = new float*[2];
        public int channel_buffer_end;
        public int channel_buffer_start;
        public float*[] channel_buffers = new float*[16];
        public int channels;
        public int codebook_count;
        public Codebook* codebooks;
        public string[] comment_list;
        public uint current_loc;
        public int current_loc_valid;
        public int discard_samples_deferred;
        public int end_seg_with_known_loc;
        public int eof;

        public STBVorbisError error;
        public short*[] finalY = new short*[16];
        public uint first_audio_page_offset;
        public byte first_decode;

        internal ArrayBuffer<float> FloatBuffer = new ArrayBuffer<float>(1024);

        public Floor* floor_config;
        public int floor_count;
        public ushort[] floor_types = new ushort[64];
        public uint known_loc_for_packet;
        public int last_page;
        public int last_seg;
        public int last_seg_which;
        public Mapping* mapping;
        public int mapping_count;
        public Mode[] mode_config = new Mode[64];
        public int mode_count;
        public int next_seg;
        public float*[] outputs = new float*[16];
        public ProbedPage p_first;
        public ProbedPage p_last;
        public int packet_bytes;
        public int page_crc_tests;
        public byte page_flag;
        public int previous_length;
        public float*[] previous_window = new float*[16];
        internal ArrayBuffer2D<IntPtr> PtrBuffer2D = new ArrayBuffer2D<IntPtr>(8, 256);
        public byte push_mode;
        public Residue[] residue_config = null!;
        public int residue_count;
        public ushort[] residue_types = new ushort[64];
        public uint sample_rate;
        public uint samples_output;
        public CRCscan[] scan = new CRCscan[4];
        public int segment_count;
        public byte[] segments = new byte[255];
        public uint serial;
        public int setup_offset;
        public byte* stream;
        public byte* stream_end;
        public uint stream_len;
        public byte* stream_start;
        public int temp_offset;
        public uint total_samples;
        public int valid_bits;
        public string vendor;
        public float*[] window = new float*[2];
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Floor1
    {
        public byte partitions;
        public fixed byte partition_class_list[32];
        public fixed byte class_dimensions[16];
        public fixed byte class_subclasses[16];
        public fixed byte class_masterbooks[16];
        public fixed short subclass_books[16 * 8];
        public fixed ushort Xlist[31 * 8 + 2];
        public fixed byte sorted_order[31 * 8 + 2];
        public fixed byte neighbors[(31 * 8 + 2) * 2];
        public byte floor1_multiplier;
        public byte rangebits;
        public int values;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Floor
    {
        [FieldOffset(0)] public Floor0 floor0;

        [FieldOffset(0)] public Floor1 floor1;
    }
}

unsafe partial class StbVorbis
{
    public enum STBVorbisError
    {
        VORBIS__no_error,
        VORBIS_need_more_data = 1,
        VORBIS_invalid_api_mixing,
        VORBIS_outofmem,
        VORBIS_feature_not_supported,
        VORBIS_too_many_channels,
        VORBIS_file_open_failure,
        VORBIS_seek_without_length,
        VORBIS_unexpected_eof = 10,
        VORBIS_seek_invalid,
        VORBIS_invalid_setup = 20,
        VORBIS_invalid_stream,
        VORBIS_missing_capture_pattern = 30,
        VORBIS_invalid_stream_structure_version,
        VORBIS_continued_packet_flag_invalid,
        VORBIS_incorrect_stream_serial_number,
        VORBIS_invalid_first_page,
        VORBIS_bad_packet_type,
        VORBIS_cant_find_last_page,
        VORBIS_seek_failed,
        VORBIS_ogg_skeleton_not_supported
    }

    public struct Codebook
    {
        public int dimensions;
        public int entries;
        public byte* codeword_lengths;
        public float minimum_value;
        public float delta_value;
        public byte value_bits;
        public byte lookup_type;
        public byte sequence_p;
        public byte sparse;
        public uint lookup_values;
        public float* multiplicands;
        public uint* codewords;
        public fixed short fast_huffman[1024];
        public uint* sorted_codewords;
        public int* sorted_values;
        public int sorted_entries;
    }

    public struct CRCscan
    {
        public uint goal_crc;
        public int bytes_left;
        public uint crc_so_far;
        public int bytes_done;
        public uint sample_loc;
    }

    public struct Floor0
    {
        public byte order;
        public ushort rate;
        public ushort bark_map_size;
        public byte amplitude_bits;
        public byte amplitude_offset;
        public byte number_of_books;
        public fixed byte book_list[16];
    }

    public struct Mapping
    {
        public ushort coupling_steps;
        public MappingChannel* chan;
        public byte submaps;
        public fixed byte submap_floor[15];
        public fixed byte submap_residue[15];
    }

    public struct MappingChannel
    {
        public byte magnitude;
        public byte angle;
        public byte mux;
    }

    public struct Mode
    {
        public byte blockflag;
        public byte mapping;
        public ushort windowtype;
        public ushort transformtype;
    }

    public struct ProbedPage
    {
        public uint page_start;
        public uint page_end;
        public uint last_decoded_sample;
    }

    public struct stb_vorbis_info
    {
        public uint sample_rate;
        public int channels;
        public int max_frame_size;
    }

    public struct stbv__floor_ordering
    {
        public ushort x;
        public ushort id;
    }

    public const int VORBIS_packet_comment = 3;
    public const int VORBIS_packet_id = 1;
    public const int VORBIS_packet_setup = 5;
    public static int[,] convert_samples_short_channel_selector = { { 0, 0 }, { 1, 0 }, { 2, 4 } };
    public static uint[] crc_table = new uint[256];
    public static sbyte[] ilog_log2_4 = { 0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4 };

    public static float[] inverse_db_table =
    {
            1.0649863e-07f, 1.1341951e-07f, 1.2079015e-07f, 1.2863978e-07f, 1.3699951e-07f, 1.4590251e-07f,
            1.5538408e-07f, 1.6548181e-07f, 1.7623575e-07f, 1.8768855e-07f, 1.9988561e-07f, 2.1287530e-07f,
            2.2670913e-07f, 2.4144197e-07f, 2.5713223e-07f, 2.7384213e-07f, 2.9163793e-07f, 3.1059021e-07f,
            3.3077411e-07f, 3.5226968e-07f, 3.7516214e-07f, 3.9954229e-07f, 4.2550680e-07f, 4.5315863e-07f,
            4.8260743e-07f, 5.1396998e-07f, 5.4737065e-07f, 5.8294187e-07f, 6.2082472e-07f, 6.6116941e-07f,
            7.0413592e-07f, 7.4989464e-07f, 7.9862701e-07f, 8.5052630e-07f, 9.0579828e-07f, 9.6466216e-07f,
            1.0273513e-06f, 1.0941144e-06f, 1.1652161e-06f, 1.2409384e-06f, 1.3215816e-06f, 1.4074654e-06f,
            1.4989305e-06f, 1.5963394e-06f, 1.7000785e-06f, 1.8105592e-06f, 1.9282195e-06f, 2.0535261e-06f,
            2.1869758e-06f, 2.3290978e-06f, 2.4804557e-06f, 2.6416497e-06f, 2.8133190e-06f, 2.9961443e-06f,
            3.1908506e-06f, 3.3982101e-06f, 3.6190449e-06f, 3.8542308e-06f, 4.1047004e-06f, 4.3714470e-06f,
            4.6555282e-06f, 4.9580707e-06f, 5.2802740e-06f, 5.6234160e-06f, 5.9888572e-06f, 6.3780469e-06f,
            6.7925283e-06f, 7.2339451e-06f, 7.7040476e-06f, 8.2047000e-06f, 8.7378876e-06f, 9.3057248e-06f,
            9.9104632e-06f, 1.0554501e-05f, 1.1240392e-05f, 1.1970856e-05f, 1.2748789e-05f, 1.3577278e-05f,
            1.4459606e-05f, 1.5399272e-05f, 1.6400004e-05f, 1.7465768e-05f, 1.8600792e-05f, 1.9809576e-05f,
            2.1096914e-05f, 2.2467911e-05f, 2.3928002e-05f, 2.5482978e-05f, 2.7139006e-05f, 2.8902651e-05f,
            3.0780908e-05f, 3.2781225e-05f, 3.4911534e-05f, 3.7180282e-05f, 3.9596466e-05f, 4.2169667e-05f,
            4.4910090e-05f, 4.7828601e-05f, 5.0936773e-05f, 5.4246931e-05f, 5.7772202e-05f, 6.1526565e-05f,
            6.5524908e-05f, 6.9783085e-05f, 7.4317983e-05f, 7.9147585e-05f, 8.4291040e-05f, 8.9768747e-05f,
            9.5602426e-05f, 0.00010181521f, 0.00010843174f, 0.00011547824f, 0.00012298267f, 0.00013097477f,
            0.00013948625f, 0.00014855085f, 0.00015820453f, 0.00016848555f, 0.00017943469f, 0.00019109536f,
            0.00020351382f, 0.00021673929f, 0.00023082423f, 0.00024582449f, 0.00026179955f, 0.00027881276f,
            0.00029693158f, 0.00031622787f, 0.00033677814f, 0.00035866388f, 0.00038197188f, 0.00040679456f,
            0.00043323036f, 0.00046138411f, 0.00049136745f, 0.00052329927f, 0.00055730621f, 0.00059352311f,
            0.00063209358f, 0.00067317058f, 0.00071691700f, 0.00076350630f, 0.00081312324f, 0.00086596457f,
            0.00092223983f, 0.00098217216f, 0.0010459992f, 0.0011139742f, 0.0011863665f, 0.0012634633f, 0.0013455702f,
            0.0014330129f, 0.0015261382f, 0.0016253153f, 0.0017309374f, 0.0018434235f, 0.0019632195f, 0.0020908006f,
            0.0022266726f, 0.0023713743f, 0.0025254795f, 0.0026895994f, 0.0028643847f, 0.0030505286f, 0.0032487691f,
            0.0034598925f, 0.0036847358f, 0.0039241906f, 0.0041792066f, 0.0044507950f, 0.0047400328f, 0.0050480668f,
            0.0053761186f, 0.0057254891f, 0.0060975636f, 0.0064938176f, 0.0069158225f, 0.0073652516f, 0.0078438871f,
            0.0083536271f, 0.0088964928f, 0.009474637f, 0.010090352f, 0.010746080f, 0.011444421f, 0.012188144f,
            0.012980198f, 0.013823725f, 0.014722068f, 0.015678791f, 0.016697687f, 0.017782797f, 0.018938423f,
            0.020169149f, 0.021479854f, 0.022875735f, 0.024362330f, 0.025945531f, 0.027631618f, 0.029427276f,
            0.031339626f, 0.033376252f, 0.035545228f, 0.037855157f, 0.040315199f, 0.042935108f, 0.045725273f,
            0.048696758f, 0.051861348f, 0.055231591f, 0.058820850f, 0.062643361f, 0.066714279f, 0.071049749f,
            0.075666962f, 0.080584227f, 0.085821044f, 0.091398179f, 0.097337747f, 0.10366330f, 0.11039993f, 0.11757434f,
            0.12521498f, 0.13335215f, 0.14201813f, 0.15124727f, 0.16107617f, 0.17154380f, 0.18269168f, 0.19456402f,
            0.20720788f, 0.22067342f, 0.23501402f, 0.25028656f, 0.26655159f, 0.28387361f, 0.30232132f, 0.32196786f,
            0.34289114f, 0.36517414f, 0.38890521f, 0.41417847f, 0.44109412f, 0.46975890f, 0.50028648f, 0.53279791f,
            0.56742212f, 0.60429640f, 0.64356699f, 0.68538959f, 0.72993007f, 0.77736504f, 0.82788260f, 0.88168307f,
            0.9389798f, 1.0f
        };

    public static byte[] ogg_page_header = { 0x4f, 0x67, 0x67, 0x53 };
    public static int[] vorbis_decode_packet_rest_range_list = { 256, 128, 86, 64 };
    public static byte[] vorbis_validate_vorbis = { 118, 111, 114, 98, 105, 115 };

    public static void add_entry(Codebook* c, uint huff_code, int symbol, int count, int len, uint* values)
    {
        if (c->sparse == 0)
        {
            c->codewords[symbol] = huff_code;
        }
        else
        {
            c->codewords[count] = huff_code;
            c->codeword_lengths[count] = (byte)len;
            values[count] = (uint)symbol;
        }
    }

    public static uint bit_reverse(uint n)
    {
        n = ((n & 0xAAAAAAAA) >> 1) | ((n & 0x55555555) << 1);
        n = ((n & 0xCCCCCCCC) >> 2) | ((n & 0x33333333) << 2);
        n = ((n & 0xF0F0F0F0) >> 4) | ((n & 0x0F0F0F0F) << 4);
        n = ((n & 0xFF00FF00) >> 8) | ((n & 0x00FF00FF) << 8);
        return (n >> 16) | (n << 16);
    }

    public static int capture_pattern(stb_vorbis f)
    {
        if (0x4f != get8(f))
            return 0;
        if (0x67 != get8(f))
            return 0;
        if (0x67 != get8(f))
            return 0;
        if (0x53 != get8(f))
            return 0;
        return 1;
    }

    public static int codebook_decode(stb_vorbis f, Codebook* c, float* output, int len)
    {
        var i = 0;
        var z = codebook_decode_start(f, c);
        if (z < 0)
            return 0;
        if (len > c->dimensions)
            len = c->dimensions;
        z *= c->dimensions;
        if (c->sequence_p != 0)
        {
            float last = 0;
            for (i = 0; i < len; ++i)
            {
                var val = c->multiplicands[z + i] + last;
                output[i] += val;
                last = val + c->minimum_value;
            }
        }
        else
        {
            float last = 0;
            for (i = 0; i < len; ++i) output[i] += c->multiplicands[z + i] + last;
        }

        return 1;
    }

    public static int codebook_decode_deinterleave_repeat(stb_vorbis f, Codebook* c, float** outputs, int ch,
        int* c_inter_p, int* p_inter_p, int len, int total_decode)
    {
        var c_inter = *c_inter_p;
        var p_inter = *p_inter_p;
        var i = 0;
        var z = 0;
        var effective = c->dimensions;
        if (c->lookup_type == 0)
            return error(f, STBVorbisError.VORBIS_invalid_stream);
        while (total_decode > 0)
        {
            float last = 0;
            z = codebook_decode_scalar(f, c);
            if (z < 0)
            {
                if (f.bytes_in_seg == 0)
                    if (f.last_seg != 0)
                        return 0;
                return error(f, STBVorbisError.VORBIS_invalid_stream);
            }

            if (c_inter + p_inter * ch + effective > len * ch) effective = len * ch - (p_inter * ch - c_inter);

            {
                z *= c->dimensions;
                if (c->sequence_p != 0)
                    for (i = 0; i < effective; ++i)
                    {
                        var val = c->multiplicands[z + i] + last;
                        if (outputs[c_inter] != null)
                            outputs[c_inter][p_inter] += val;
                        if (++c_inter == ch)
                        {
                            c_inter = 0;
                            ++p_inter;
                        }

                        last = val;
                    }
                else
                    for (i = 0; i < effective; ++i)
                    {
                        var val = c->multiplicands[z + i] + last;
                        if (outputs[c_inter] != null)
                            outputs[c_inter][p_inter] += val;
                        if (++c_inter == ch)
                        {
                            c_inter = 0;
                            ++p_inter;
                        }
                    }
            }

            total_decode -= effective;
        }

        *c_inter_p = c_inter;
        *p_inter_p = p_inter;
        return 1;
    }

    public static int codebook_decode_scalar(stb_vorbis f, Codebook* c)
    {
        var i = 0;
        if (f.valid_bits < 10)
            prep_huffman(f);
        i = (int)(f.acc & ((1 << 10) - 1));
        i = c->fast_huffman[i];
        if (i >= 0)
        {
            f.acc >>= c->codeword_lengths[i];
            f.valid_bits -= c->codeword_lengths[i];
            if (f.valid_bits < 0)
            {
                f.valid_bits = 0;
                return -1;
            }

            return i;
        }

        return codebook_decode_scalar_raw(f, c);
    }

    public static int codebook_decode_scalar_raw(stb_vorbis f, Codebook* c)
    {
        var i = 0;
        prep_huffman(f);
        if (c->codewords == null && c->sorted_codewords == null)
            return -1;
        if (c->entries > 8 ? c->sorted_codewords != null : c->codewords == null)
        {
            var code = bit_reverse(f.acc);
            var x = 0;
            var n = c->sorted_entries;
            var len = 0;
            while (n > 1)
            {
                var m = x + (n >> 1);
                if (c->sorted_codewords[m] <= code)
                {
                    x = m;
                    n -= n >> 1;
                }
                else
                {
                    n >>= 1;
                }
            }

            if (c->sparse == 0)
                x = c->sorted_values[x];
            len = c->codeword_lengths[x];
            if (f.valid_bits >= len)
            {
                f.acc >>= len;
                f.valid_bits -= len;
                return x;
            }

            f.valid_bits = 0;
            return -1;
        }

        for (i = 0; i < c->entries; ++i)
        {
            if (c->codeword_lengths[i] == 255)
                continue;
            if (c->codewords[i] == (f.acc & ((1 << c->codeword_lengths[i]) - 1)))
            {
                if (f.valid_bits >= c->codeword_lengths[i])
                {
                    f.acc >>= c->codeword_lengths[i];
                    f.valid_bits -= c->codeword_lengths[i];
                    return i;
                }

                f.valid_bits = 0;
                return -1;
            }
        }

        error(f, STBVorbisError.VORBIS_invalid_stream);
        f.valid_bits = 0;
        return -1;
    }

    public static int codebook_decode_start(stb_vorbis f, Codebook* c)
    {
        var z = -1;
        if (c->lookup_type == 0)
        {
            error(f, STBVorbisError.VORBIS_invalid_stream);
        }
        else
        {
            z = codebook_decode_scalar(f, c);
            if (z < 0)
            {
                if (f.bytes_in_seg == 0)
                    if (f.last_seg != 0)
                        return z;
                error(f, STBVorbisError.VORBIS_invalid_stream);
            }
        }

        return z;
    }

    public static int codebook_decode_step(stb_vorbis f, Codebook* c, float* output, int len, int step)
    {
        var i = 0;
        var z = codebook_decode_start(f, c);
        float last = 0;
        if (z < 0)
            return 0;
        if (len > c->dimensions)
            len = c->dimensions;
        z *= c->dimensions;
        for (i = 0; i < len; ++i)
        {
            var val = c->multiplicands[z + i] + last;
            output[i * step] += val;
            if (c->sequence_p != 0)
                last = val;
        }

        return 1;
    }

    public static void compute_accelerated_huffman(Codebook* c)
    {
        var i = 0;
        var len = 0;
        for (i = 0; i < 1 << 10; ++i) c->fast_huffman[i] = -1;

        len = c->sparse != 0 ? c->sorted_entries : c->entries;
        if (len > 32767)
            len = 32767;
        for (i = 0; i < len; ++i)
            if (c->codeword_lengths[i] <= 10)
            {
                var z = c->sparse != 0 ? bit_reverse(c->sorted_codewords[i]) : c->codewords[i];
                while (z < 1 << 10)
                {
                    c->fast_huffman[z] = (short)i;
                    z += (uint)(1 << c->codeword_lengths[i]);
                }
            }
    }

    public static void compute_bitreverse(int n, ushort* rev)
    {
        var ld = ilog(n) - 1;
        var i = 0;
        var n8 = n >> 3;
        for (i = 0; i < n8; ++i) rev[i] = (ushort)((bit_reverse((uint)i) >> (32 - ld + 3)) << 2);
    }

    public static int compute_codewords(Codebook* c, byte* len, int n, uint* values)
    {
        var i = 0;
        var k = 0;
        var m = 0;
        var available = stackalloc uint[32];
        CRuntime.Memset(available, 0, (ulong)(32 * sizeof(uint)));
        for (k = 0; k < n; ++k)
            if (len[k] < 255)
                break;

        if (k == n) return 1;

        add_entry(c, 0, k, m++, len[k], values);
        for (i = 1; i <= len[k]; ++i) available[i] = 1U << (32 - i);

        for (i = k + 1; i < n; ++i)
        {
            uint res = 0;
            int z = len[i];
            var y = 0;
            if (z == 255)
                continue;
            while (z > 0 && available[z] == 0) --z;

            if (z == 0) return 0;

            res = available[z];
            available[z] = 0;
            add_entry(c, bit_reverse(res), i, m++, len[i], values);
            if (z != len[i])
                for (y = len[i]; y > z; --y)
                    available[y] = (uint)(res + (1 << (32 - y)));
        }

        return 1;
    }

    public static void compute_samples(int mask, short* output, int num_c, float*[] data, int d_offset, int len)
    {
        var buffer = stackalloc float[32];
        var i = 0;
        var j = 0;
        var o = 0;
        var n = 32;
        for (o = 0; o < len; o += 32)
        {
            CRuntime.Memset(buffer, 0, (ulong)(32 * sizeof(float)));
            if (o + n > len)
                n = len - o;
            for (j = 0; j < num_c; ++j)
                if ((channel_position[num_c, j] & mask) != 0)
                    for (i = 0; i < n; ++i)
                        buffer[i] += data[j][d_offset + o + i];

            for (i = 0; i < n; ++i)
            {
                var v = (int)(buffer[i] * (1 << 15));
                if ((uint)(v + 32768) > 65535)
                    v = v < 0 ? -32768 : 32767;
                output[o + i] = (short)v;
            }
        }
    }

    public static void compute_sorted_huffman(Codebook* c, byte* lengths, uint* values)
    {
        var i = 0;
        var len = 0;
        if (c->sparse == 0)
        {
            var k = 0;
            for (i = 0; i < c->entries; ++i)
                if (include_in_sort(c, lengths[i]) != 0)
                    c->sorted_codewords[k++] = bit_reverse(c->codewords[i]);
        }
        else
        {
            for (i = 0; i < c->sorted_entries; ++i) c->sorted_codewords[i] = bit_reverse(c->codewords[i]);
        }

        CRuntime.Qsort(c->sorted_codewords, (ulong)c->sorted_entries, sizeof(uint), uint32_compare);
        c->sorted_codewords[c->sorted_entries] = 0xffffffff;
        len = c->sparse != 0 ? c->sorted_entries : c->entries;
        for (i = 0; i < len; ++i)
        {
            int huff_len = c->sparse != 0 ? lengths[values[i]] : lengths[i];
            if (include_in_sort(c, (byte)huff_len) != 0)
            {
                var code = bit_reverse(c->codewords[i]);
                var x = 0;
                var n = c->sorted_entries;
                while (n > 1)
                {
                    var m = x + (n >> 1);
                    if (c->sorted_codewords[m] <= code)
                    {
                        x = m;
                        n -= n >> 1;
                    }
                    else
                    {
                        n >>= 1;
                    }
                }

                if (c->sparse != 0)
                {
                    c->sorted_values[x] = (int)values[i];
                    c->codeword_lengths[x] = (byte)huff_len;
                }
                else
                {
                    c->sorted_values[x] = i;
                }
            }
        }
    }

    public static void compute_stereo_samples(short* output, int num_c, float*[] data, int d_offset, int len)
    {
        var buffer = stackalloc float[32];
        var i = 0;
        var j = 0;
        var o = 0;
        var n = 32 >> 1;
        for (o = 0; o < len; o += 32 >> 1)
        {
            var o2 = o << 1;
            CRuntime.Memset(buffer, 0, (ulong)(32 * sizeof(float)));
            if (o + n > len)
                n = len - o;
            for (j = 0; j < num_c; ++j)
            {
                var m = channel_position[num_c, j] & (2 | 4);
                if (m == (2 | 4))
                    for (i = 0; i < n; ++i)
                    {
                        buffer[i * 2 + 0] += data[j][d_offset + o + i];
                        buffer[i * 2 + 1] += data[j][d_offset + o + i];
                    }
                else if (m == 2)
                    for (i = 0; i < n; ++i)
                        buffer[i * 2 + 0] += data[j][d_offset + o + i];
                else if (m == 4)
                    for (i = 0; i < n; ++i)
                        buffer[i * 2 + 1] += data[j][d_offset + o + i];
            }

            for (i = 0; i < n << 1; ++i)
            {
                var v = (int)(buffer[i] * (1 << 15));
                if ((uint)(v + 32768) > 65535)
                    v = v < 0 ? -32768 : 32767;
                output[o2 + i] = (short)v;
            }
        }
    }

    public static void compute_twiddle_factors(int n, float* A, float* B, float* C)
    {
        var n4 = n >> 2;
        var n8 = n >> 3;
        var k = 0;
        var k2 = 0;
        for (k = k2 = 0; k < n4; ++k, k2 += 2)
        {
            A[k2] = (float)CRuntime.Cos(4 * k * 3.14159265358979323846264f / n);
            A[k2 + 1] = (float)-CRuntime.Sin(4 * k * 3.14159265358979323846264f / n);
            B[k2] = (float)CRuntime.Cos((k2 + 1) * 3.14159265358979323846264f / n / 2) * 0.5f;
            B[k2 + 1] = (float)CRuntime.Sin((k2 + 1) * 3.14159265358979323846264f / n / 2) * 0.5f;
        }

        for (k = k2 = 0; k < n8; ++k, k2 += 2)
        {
            C[k2] = (float)CRuntime.Cos(2 * (k2 + 1) * 3.14159265358979323846264f / n);
            C[k2 + 1] = (float)-CRuntime.Sin(2 * (k2 + 1) * 3.14159265358979323846264f / n);
        }
    }

    public static void compute_window(int n, float* window)
    {
        var n2 = n >> 1;
        var i = 0;
        for (i = 0; i < n2; ++i)
            window[i] = (float)CRuntime.Sin(0.5 * 3.14159265358979323846264f *
                                             square((float)CRuntime.Sin((i - 0 + 0.5) / n2 * 0.5 *
                                                                         3.14159265358979323846264f)));
    }

    public static void convert_channels_short_interleaved(int buf_c, short* buffer, int data_c, float*[] data,
        int d_offset, int len)
    {
        var i = 0;
        if (buf_c != data_c && buf_c <= 2 && data_c <= 6)
        {
            for (i = 0; i < buf_c; ++i) compute_stereo_samples(buffer, data_c, data, d_offset, len);
        }
        else
        {
            var limit = buf_c < data_c ? buf_c : data_c;
            var j = 0;
            for (j = 0; j < len; ++j)
            {
                for (i = 0; i < limit; ++i)
                {
                    var f = data[i][d_offset + j];
                    var v = (int)(f * (1 << 15));
                    if ((uint)(v + 32768) > 65535)
                        v = v < 0 ? -32768 : 32767;
                    *buffer++ = (short)v;
                }

                for (; i < buf_c; ++i) *buffer++ = 0;
            }
        }
    }

    public static void convert_samples_short(int buf_c, short** buffer, int b_offset, int data_c, float*[] data,
        int d_offset, int samples)
    {
        var i = 0;
        if (buf_c != data_c && buf_c <= 2 && data_c <= 6)
        {
            for (i = 0; i < buf_c; ++i)
                compute_samples(convert_samples_short_channel_selector[buf_c, i], buffer[i] + b_offset, data_c,
                    data, d_offset, samples);
        }
        else
        {
            var limit = buf_c < data_c ? buf_c : data_c;
            for (i = 0; i < limit; ++i) copy_samples(buffer[i] + b_offset, data[i] + d_offset, samples);

            for (; i < buf_c; ++i) CRuntime.Memset(buffer[i] + b_offset, 0, (ulong)(sizeof(short) * samples));
        }
    }

    public static void copy_samples(short* dest, float* src, int len)
    {
        var i = 0;
        for (i = 0; i < len; ++i)
        {
            var v = (int)(src[i] * (1 << 15));
            if ((uint)(v + 32768) > 65535)
                v = v < 0 ? -32768 : 32767;
            dest[i] = (short)v;
        }
    }

    public static void crc32_init()
    {
        var i = 0;
        var j = 0;
        uint s = 0;
        for (i = 0; i < 256; i++)
        {
            for (s = (uint)i << 24, j = 0; j < 8; ++j) s = (uint)((s << 1) ^ (s >= 1U << 31 ? 0x04c11db7 : 0));

            crc_table[i] = s;
        }
    }

    public static uint crc32_update(uint crc, byte b)
    {
        return (crc << 8) ^ crc_table[b ^ (crc >> 24)];
    }

    public static void decode_residue(stb_vorbis f, float** residue_buffers, int ch, int n, int rn,
        byte* do_not_decode)
    {
        var i = 0;
        var j = 0;
        var pass = 0;
        var r = f.residue_config[rn];
        int rtype = f.residue_types[rn];
        int c = r.classbook;
        var classwords = f.codebooks[c].dimensions;
        var actual_size = (uint)(rtype == 2 ? n * 2 : n);
        var limit_r_begin = r.begin < actual_size ? r.begin : actual_size;
        var limit_r_end = r.end < actual_size ? r.end : actual_size;
        var n_read = (int)(limit_r_end - limit_r_begin);
        var part_read = (int)(n_read / r.part_size);
        var temp_alloc_point = f.temp_offset;

        var part_classdata = f.PtrBuffer2D;

        part_classdata.EnsureSize(f.channels, part_read);
        for (i = 0; i < ch; ++i)
            if (do_not_decode[i] == 0)
                CRuntime.Memset(residue_buffers[i], 0, (ulong)(sizeof(float) * n));

        if (rtype == 2 && ch != 1)
        {
            for (j = 0; j < ch; ++j)
                if (do_not_decode[j] == 0)
                    break;

            if (j == ch)
                goto done;
            for (pass = 0; pass < 8; ++pass)
            {
                var pcount = 0;
                var class_set = 0;
                if (ch == 2)
                    while (pcount < part_read)
                    {
                        var z = (int)(r.begin + pcount * r.part_size);
                        var c_inter = z & 1;
                        var p_inter = z >> 1;
                        if (pass == 0)
                        {
                            var codebook = f.codebooks + r.classbook;
                            var q = 0;
                            q = codebook_decode_scalar(f, codebook);
                            if (codebook->sparse != 0)
                                q = codebook->sorted_values[q];
                            if (q == -1)
                                goto done;
                            part_classdata[0, class_set] = (IntPtr)r.classdata[q];
                        }

                        for (i = 0; i < classwords && pcount < part_read; ++i, ++pcount)
                        {
                            var z2 = (int)(r.begin + pcount * r.part_size);
                            int c2 = part_classdata[0, class_set].ToBytePointer()[i];
                            int b = r.residue_books[c2, pass];
                            if (b >= 0)
                            {
                                var book = f.codebooks + b;
                                if (codebook_decode_deinterleave_repeat(f, book, residue_buffers, ch, &c_inter,
                                    &p_inter, n, (int)r.part_size) == 0)
                                    goto done;
                            }
                            else
                            {
                                z2 += (int)r.part_size;
                                c_inter = z2 & 1;
                                p_inter = z2 >> 1;
                            }
                        }

                        ++class_set;
                    }
                else if (ch > 2)
                    while (pcount < part_read)
                    {
                        var z = (int)(r.begin + pcount * r.part_size);
                        var c_inter = z % ch;
                        var p_inter = z / ch;
                        if (pass == 0)
                        {
                            var codebook = f.codebooks + r.classbook;
                            var q = 0;
                            q = codebook_decode_scalar(f, codebook);
                            if (codebook->sparse != 0)
                                q = codebook->sorted_values[q];
                            if (q == -1)
                                goto done;
                            part_classdata[0, class_set] = (IntPtr)r.classdata[q];
                        }

                        for (i = 0; i < classwords && pcount < part_read; ++i, ++pcount)
                        {
                            var z2 = (int)(r.begin + pcount * r.part_size);
                            int c2 = part_classdata[0, class_set].ToBytePointer()[i];
                            int b = r.residue_books[c2, pass];
                            if (b >= 0)
                            {
                                var book = f.codebooks + b;
                                if (codebook_decode_deinterleave_repeat(f, book, residue_buffers, ch, &c_inter,
                                    &p_inter, n, (int)r.part_size) == 0)
                                    goto done;
                            }
                            else
                            {
                                z2 += (int)r.part_size;
                                c_inter = z2 % ch;
                                p_inter = z2 / ch;
                            }
                        }

                        ++class_set;
                    }
            }

            goto done;
        }

        for (pass = 0; pass < 8; ++pass)
        {
            var pcount = 0;
            var class_set = 0;
            while (pcount < part_read)
            {
                if (pass == 0)
                    for (j = 0; j < ch; ++j)
                        if (do_not_decode[j] == 0)
                        {
                            var codebook = f.codebooks + r.classbook;
                            var temp = 0;
                            temp = codebook_decode_scalar(f, codebook);
                            if (codebook->sparse != 0)
                                temp = codebook->sorted_values[temp];
                            if (temp == -1)
                                goto done;
                            part_classdata[j, class_set] = (IntPtr)r.classdata[temp];
                        }

                for (i = 0; i < classwords && pcount < part_read; ++i, ++pcount)
                    for (j = 0; j < ch; ++j)
                        if (do_not_decode[j] == 0)
                        {
                            int c2 = part_classdata[j, class_set].ToBytePointer()[i];
                            int b = r.residue_books[c2, pass];
                            if (b >= 0)
                            {
                                var target = residue_buffers[j];
                                var offset = (int)(r.begin + pcount * r.part_size);
                                var n2 = (int)r.part_size;
                                var book = f.codebooks + b;
                                if (residue_decode(f, book, target, offset, n2, rtype) == 0)
                                    goto done;
                            }
                        }

                ++class_set;
            }
        }

    done:
        f.temp_offset = temp_alloc_point;
    }

    public static int do_floor(stb_vorbis f, Mapping* map, int i, int n, float* target, short* finalY,
        byte* step2_flag)
    {
        var n2 = n >> 1;
        int s = map->chan[i].mux;
        var floor = 0;
        floor = map->submap_floor[s];
        if (f.floor_types[floor] == 0) return error(f, STBVorbisError.VORBIS_invalid_stream);

        var g = &f.floor_config[floor].floor1;
        var j = 0;
        var q = 0;
        var lx = 0;
        var ly = finalY[0] * g->floor1_multiplier;
        for (q = 1; q < g->values; ++q)
        {
            j = g->sorted_order[q];
            if (finalY[j] >= 0)
            {
                var hy = finalY[j] * g->floor1_multiplier;
                int hx = g->Xlist[j];
                if (lx != hx)
                    draw_line(target, lx, ly, hx, hy, n2);
                lx = hx;
                ly = hy;
            }
        }

        if (lx < n2)
            for (j = lx; j < n2; ++j)
                target[j] *= inverse_db_table[ly];

        return 1;
    }

    public static void draw_line(float* output, int x0, int y0, int x1, int y1, int n)
    {
        var dy = y1 - y0;
        var adx = x1 - x0;
        var ady = CRuntime.Abs(dy);
        var _base_ = 0;
        var x = x0;
        var y = y0;
        var err = 0;
        var sy = 0;
        _base_ = dy / adx;
        if (dy < 0)
            sy = _base_ - 1;
        else
            sy = _base_ + 1;
        ady -= CRuntime.Abs(_base_) * adx;
        if (x1 > n)
            x1 = n;
        if (x < x1)
        {
            output[x] *= inverse_db_table[y & 255];
            for (++x; x < x1; ++x)
            {
                err += ady;
                if (err >= adx)
                {
                    err -= adx;
                    y += sy;
                }
                else
                {
                    y += _base_;
                }

                output[x] *= inverse_db_table[y & 255];
            }
        }
    }

    public static int error(stb_vorbis f, STBVorbisError e)
    {
        f.error = e;

        return 0;
    }

    public static float float32_unpack(uint x)
    {
        var mantissa = x & 0x1fffff;
        var sign = x & 0x80000000;
        var exp = (x & 0x7fe00000) >> 21;
        var res = sign != 0 ? -(double)mantissa : mantissa;
        return (float)CRuntime.Ldexp((float)res, (int)exp - 788);
    }

    public static void flush_packet(stb_vorbis f)
    {
        while (get8_packet_raw(f) != -1)
        {
        }
    }

    public static uint get_bits(stb_vorbis f, int n)
    {
        uint z = 0;
        if (f.valid_bits < 0)
            return 0;
        if (f.valid_bits < n)
        {
            if (n > 24)
            {
                z = get_bits(f, 24);
                z += get_bits(f, n - 24) << 24;
                return z;
            }

            if (f.valid_bits == 0)
                f.acc = 0;
            while (f.valid_bits < n)
            {
                var z2 = get8_packet_raw(f);
                if (z2 == -1)
                {
                    f.valid_bits = -1;
                    return 0;
                }

                f.acc += (uint)(z2 << f.valid_bits);
                f.valid_bits += 8;
            }
        }

        z = (uint)(f.acc & ((1 << n) - 1));
        f.acc >>= n;
        f.valid_bits -= n;
        return z;
    }

    public static int get_seek_page_info(stb_vorbis f, ProbedPage* z)
    {
        var header = stackalloc byte[27];
        var lacing = stackalloc byte[255];
        var i = 0;
        var len = 0;
        z->page_start = stb_vorbis_get_file_offset(f);
        getn(f, header, 27);
        if (header[0] != 79 || header[1] != 103 || header[2] != 103 || header[3] != 83)
            return 0;
        getn(f, lacing, header[26]);
        len = 0;
        for (i = 0; i < header[26]; ++i) len += lacing[i];

        z->page_end = (uint)(z->page_start + 27 + header[26] + len);
        z->last_decoded_sample = (uint)(header[6] + (header[7] << 8) + (header[8] << 16) + (header[9] << 24));
        set_file_offset(f, z->page_start);
        return 1;
    }

    public static float* get_window(stb_vorbis f, int len)
    {
        len <<= 1;
        if (len == f.blocksize_0)
            return f.window[0];
        if (len == f.blocksize_1)
            return f.window[1];
        return null;
    }

    public static uint get32(stb_vorbis f)
    {
        uint x = 0;
        x = get8(f);
        x += (uint)(get8(f) << 8);
        x += (uint)(get8(f) << 16);
        x += (uint)get8(f) << 24;
        return x;
    }

    public static int get32_packet(stb_vorbis f)
    {
        uint x = 0;
        x = (uint)get8_packet(f);
        x += (uint)(get8_packet(f) << 8);
        x += (uint)(get8_packet(f) << 16);
        x += (uint)get8_packet(f) << 24;
        return (int)x;
    }

    public static byte get8(stb_vorbis z)
    {
        if (1 != 0)
        {
            if (z.stream >= z.stream_end)
            {
                z.eof = 1;
                return 0;
            }

            return *z.stream++;
        }
    }

    public static int get8_packet(stb_vorbis f)
    {
        var x = get8_packet_raw(f);
        f.valid_bits = 0;
        return x;
    }

    public static int get8_packet_raw(stb_vorbis f)
    {
        if (f.bytes_in_seg == 0)
        {
            if (f.last_seg != 0)
                return -1;
            if (next_segment(f) == 0)
                return -1;
        }

        --f.bytes_in_seg;
        ++f.packet_bytes;
        return get8(f);
    }

    public static int getn(stb_vorbis z, byte* data, int n)
    {
        if (1 != 0)
        {
            if (z.stream + n > z.stream_end)
            {
                z.eof = 1;
                return 0;
            }

            CRuntime.Memcpy(data, z.stream, (ulong)n);
            z.stream += n;
            return 1;
        }
    }

    public static int go_to_page_before(stb_vorbis f, uint limit_offset)
    {
        uint previous_safe = 0;
        uint end = 0;
        if (limit_offset >= 65536 && limit_offset - 65536 >= f.first_audio_page_offset)
            previous_safe = limit_offset - 65536;
        else
            previous_safe = f.first_audio_page_offset;
        set_file_offset(f, previous_safe);
        while (vorbis_find_page(f, &end, null) != 0)
        {
            if (end >= limit_offset && stb_vorbis_get_file_offset(f) < limit_offset)
                return 1;
            set_file_offset(f, end);
        }

        return 0;
    }

    public static int ilog(int n)
    {
        if (n < 0)
            return 0;
        if (n < 1 << 14)
            if (n < 1 << 4)
                return 0 + ilog_log2_4[n];
            else if (n < 1 << 9)
                return 5 + ilog_log2_4[n >> 5];
            else
                return 10 + ilog_log2_4[n >> 10];
        if (n < 1 << 24)
            if (n < 1 << 19)
                return 15 + ilog_log2_4[n >> 15];
            else
                return 20 + ilog_log2_4[n >> 20];
        if (n < 1 << 29)
            return 25 + ilog_log2_4[n >> 25];
        return 30 + ilog_log2_4[n >> 30];
    }

    public static void imdct_step3_inner_r_loop(int lim, float* e, int d0, int k_off, float* A, int k1)
    {
        var i = 0;
        float k00_20 = 0;
        float k01_21 = 0;
        var e0 = e + d0;
        var e2 = e0 + k_off;
        for (i = lim >> 2; i > 0; --i)
        {
            k00_20 = e0[-0] - e2[-0];
            k01_21 = e0[-1] - e2[-1];
            e0[-0] += e2[-0];
            e0[-1] += e2[-1];
            e2[-0] = k00_20 * A[0] - k01_21 * A[1];
            e2[-1] = k01_21 * A[0] + k00_20 * A[1];
            A += k1;
            k00_20 = e0[-2] - e2[-2];
            k01_21 = e0[-3] - e2[-3];
            e0[-2] += e2[-2];
            e0[-3] += e2[-3];
            e2[-2] = k00_20 * A[0] - k01_21 * A[1];
            e2[-3] = k01_21 * A[0] + k00_20 * A[1];
            A += k1;
            k00_20 = e0[-4] - e2[-4];
            k01_21 = e0[-5] - e2[-5];
            e0[-4] += e2[-4];
            e0[-5] += e2[-5];
            e2[-4] = k00_20 * A[0] - k01_21 * A[1];
            e2[-5] = k01_21 * A[0] + k00_20 * A[1];
            A += k1;
            k00_20 = e0[-6] - e2[-6];
            k01_21 = e0[-7] - e2[-7];
            e0[-6] += e2[-6];
            e0[-7] += e2[-7];
            e2[-6] = k00_20 * A[0] - k01_21 * A[1];
            e2[-7] = k01_21 * A[0] + k00_20 * A[1];
            e0 -= 8;
            e2 -= 8;
            A += k1;
        }
    }

    public static void imdct_step3_inner_s_loop(int n, float* e, int i_off, int k_off, float* A, int a_off, int k0)
    {
        var i = 0;
        var A0 = A[0];
        var A1 = A[0 + 1];
        var A2 = A[0 + a_off];
        var A3 = A[0 + a_off + 1];
        var A4 = A[0 + a_off * 2 + 0];
        var A5 = A[0 + a_off * 2 + 1];
        var A6 = A[0 + a_off * 3 + 0];
        var A7 = A[0 + a_off * 3 + 1];
        float k00 = 0;
        float k11 = 0;
        var ee0 = e + i_off;
        var ee2 = ee0 + k_off;
        for (i = n; i > 0; --i)
        {
            k00 = ee0[0] - ee2[0];
            k11 = ee0[-1] - ee2[-1];
            ee0[0] = ee0[0] + ee2[0];
            ee0[-1] = ee0[-1] + ee2[-1];
            ee2[0] = k00 * A0 - k11 * A1;
            ee2[-1] = k11 * A0 + k00 * A1;
            k00 = ee0[-2] - ee2[-2];
            k11 = ee0[-3] - ee2[-3];
            ee0[-2] = ee0[-2] + ee2[-2];
            ee0[-3] = ee0[-3] + ee2[-3];
            ee2[-2] = k00 * A2 - k11 * A3;
            ee2[-3] = k11 * A2 + k00 * A3;
            k00 = ee0[-4] - ee2[-4];
            k11 = ee0[-5] - ee2[-5];
            ee0[-4] = ee0[-4] + ee2[-4];
            ee0[-5] = ee0[-5] + ee2[-5];
            ee2[-4] = k00 * A4 - k11 * A5;
            ee2[-5] = k11 * A4 + k00 * A5;
            k00 = ee0[-6] - ee2[-6];
            k11 = ee0[-7] - ee2[-7];
            ee0[-6] = ee0[-6] + ee2[-6];
            ee0[-7] = ee0[-7] + ee2[-7];
            ee2[-6] = k00 * A6 - k11 * A7;
            ee2[-7] = k11 * A6 + k00 * A7;
            ee0 -= k0;
            ee2 -= k0;
        }
    }

    public static void imdct_step3_inner_s_loop_ld654(int n, float* e, int i_off, float* A, int base_n)
    {
        var a_off = base_n >> 3;
        var A2 = A[0 + a_off];
        var z = e + i_off;
        var _base_ = z - 16 * n;
        while (z > _base_)
        {
            float k00 = 0;
            float k11 = 0;
            float l00 = 0;
            float l11 = 0;
            k00 = z[-0] - z[-8];
            k11 = z[-1] - z[-9];
            l00 = z[-2] - z[-10];
            l11 = z[-3] - z[-11];
            z[-0] = z[-0] + z[-8];
            z[-1] = z[-1] + z[-9];
            z[-2] = z[-2] + z[-10];
            z[-3] = z[-3] + z[-11];
            z[-8] = k00;
            z[-9] = k11;
            z[-10] = (l00 + l11) * A2;
            z[-11] = (l11 - l00) * A2;
            k00 = z[-4] - z[-12];
            k11 = z[-5] - z[-13];
            l00 = z[-6] - z[-14];
            l11 = z[-7] - z[-15];
            z[-4] = z[-4] + z[-12];
            z[-5] = z[-5] + z[-13];
            z[-6] = z[-6] + z[-14];
            z[-7] = z[-7] + z[-15];
            z[-12] = k11;
            z[-13] = -k00;
            z[-14] = (l11 - l00) * A2;
            z[-15] = (l00 + l11) * -A2;
            iter_54(z);
            iter_54(z - 8);
            z -= 16;
        }
    }

    public static void imdct_step3_iter0_loop(int n, float* e, int i_off, int k_off, float* A)
    {
        var ee0 = e + i_off;
        var ee2 = ee0 + k_off;
        var i = 0;
        for (i = n >> 2; i > 0; --i)
        {
            float k00_20 = 0;
            float k01_21 = 0;
            k00_20 = ee0[0] - ee2[0];
            k01_21 = ee0[-1] - ee2[-1];
            ee0[0] += ee2[0];
            ee0[-1] += ee2[-1];
            ee2[0] = k00_20 * A[0] - k01_21 * A[1];
            ee2[-1] = k01_21 * A[0] + k00_20 * A[1];
            A += 8;
            k00_20 = ee0[-2] - ee2[-2];
            k01_21 = ee0[-3] - ee2[-3];
            ee0[-2] += ee2[-2];
            ee0[-3] += ee2[-3];
            ee2[-2] = k00_20 * A[0] - k01_21 * A[1];
            ee2[-3] = k01_21 * A[0] + k00_20 * A[1];
            A += 8;
            k00_20 = ee0[-4] - ee2[-4];
            k01_21 = ee0[-5] - ee2[-5];
            ee0[-4] += ee2[-4];
            ee0[-5] += ee2[-5];
            ee2[-4] = k00_20 * A[0] - k01_21 * A[1];
            ee2[-5] = k01_21 * A[0] + k00_20 * A[1];
            A += 8;
            k00_20 = ee0[-6] - ee2[-6];
            k01_21 = ee0[-7] - ee2[-7];
            ee0[-6] += ee2[-6];
            ee0[-7] += ee2[-7];
            ee2[-6] = k00_20 * A[0] - k01_21 * A[1];
            ee2[-7] = k01_21 * A[0] + k00_20 * A[1];
            A += 8;
            ee0 -= 8;
            ee2 -= 8;
        }
    }

    public static int include_in_sort(Codebook* c, byte len)
    {
        if (c->sparse != 0) return 1;

        if (len == 255)
            return 0;
        if (len > 10)
            return 1;
        return 0;
    }

    public static int init_blocksize(stb_vorbis f, int b, int n)
    {
        var n2 = n >> 1;
        var n4 = n >> 2;
        var n8 = n >> 3;
        f.A[b] = (float*)setup_malloc(sizeof(float) * n2);
        f.B[b] = (float*)setup_malloc(sizeof(float) * n2);
        f.C[b] = (float*)setup_malloc(sizeof(float) * n4);
        if (f.A[b] == null || f.B[b] == null || f.C[b] == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        compute_twiddle_factors(n, f.A[b], f.B[b], f.C[b]);
        f.window[b] = (float*)setup_malloc(sizeof(float) * n2);
        if (f.window[b] == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        compute_window(n, f.window[b]);
        f.bit_reverse[b] = (ushort*)setup_malloc(sizeof(ushort) * n8);
        if (f.bit_reverse[b] == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        compute_bitreverse(n, f.bit_reverse[b]);
        return 1;
    }

    public static void inverse_mdct(float* buffer, int n, stb_vorbis f, int blocktype)
    {
        var n2 = n >> 1;
        var n4 = n >> 2;
        var n8 = n >> 3;
        var l = 0;
        var ld = 0;
        var save_point = f.temp_offset;
        float* u = null;
        float* v = null;
        var A = f.A[blocktype];

        f.FloatBuffer.EnsureSize(n);
        fixed (float* buf2 = f.FloatBuffer.Array)
        {
            {
                float* d;
                float* e;
                float* AA;
                float* e_stop;
                d = &buf2[n2 - 2];
                AA = A;
                e = &buffer[0];
                e_stop = &buffer[n2];
                while (e != e_stop)
                {
                    d[1] = e[0] * AA[0] - e[2] * AA[1];
                    d[0] = e[0] * AA[1] + e[2] * AA[0];
                    d -= 2;
                    AA += 2;
                    e += 4;
                }

                e = &buffer[n2 - 3];
                while (d >= buf2)
                {
                    d[1] = -e[2] * AA[0] - -e[0] * AA[1];
                    d[0] = -e[2] * AA[1] + -e[0] * AA[0];
                    d -= 2;
                    AA += 2;
                    e -= 4;
                }
            }

            u = buffer;
            v = buf2;
            {
                var AA = &A[n2 - 8];
                float* d0;
                float* d1;
                float* e0;
                float* e1;
                e0 = &v[n4];
                e1 = &v[0];
                d0 = &u[n4];
                d1 = &u[0];
                while (AA >= A)
                {
                    float v40_20 = 0;
                    float v41_21 = 0;
                    v41_21 = e0[1] - e1[1];
                    v40_20 = e0[0] - e1[0];
                    d0[1] = e0[1] + e1[1];
                    d0[0] = e0[0] + e1[0];
                    d1[1] = v41_21 * AA[4] - v40_20 * AA[5];
                    d1[0] = v40_20 * AA[4] + v41_21 * AA[5];
                    v41_21 = e0[3] - e1[3];
                    v40_20 = e0[2] - e1[2];
                    d0[3] = e0[3] + e1[3];
                    d0[2] = e0[2] + e1[2];
                    d1[3] = v41_21 * AA[0] - v40_20 * AA[1];
                    d1[2] = v40_20 * AA[0] + v41_21 * AA[1];
                    AA -= 8;
                    d0 += 4;
                    d1 += 4;
                    e0 += 4;
                    e1 += 4;
                }
            }

            ld = ilog(n) - 1;
            imdct_step3_iter0_loop(n >> 4, u, n2 - 1 - n4 * 0, -(n >> 3), A);
            imdct_step3_iter0_loop(n >> 4, u, n2 - 1 - n4 * 1, -(n >> 3), A);
            imdct_step3_inner_r_loop(n >> 5, u, n2 - 1 - n8 * 0, -(n >> 4), A, 16);
            imdct_step3_inner_r_loop(n >> 5, u, n2 - 1 - n8 * 1, -(n >> 4), A, 16);
            imdct_step3_inner_r_loop(n >> 5, u, n2 - 1 - n8 * 2, -(n >> 4), A, 16);
            imdct_step3_inner_r_loop(n >> 5, u, n2 - 1 - n8 * 3, -(n >> 4), A, 16);
            l = 2;
            for (; l < (ld - 3) >> 1; ++l)
            {
                var k0 = n >> (l + 2);
                var k0_2 = k0 >> 1;
                var lim = 1 << (l + 1);
                var i = 0;
                for (i = 0; i < lim; ++i)
                    imdct_step3_inner_r_loop(n >> (l + 4), u, n2 - 1 - k0 * i, -k0_2, A, 1 << (l + 3));
            }

            for (; l < ld - 6; ++l)
            {
                var k0 = n >> (l + 2);
                var k1 = 1 << (l + 3);
                var k0_2 = k0 >> 1;
                var rlim = n >> (l + 6);
                var r = 0;
                var lim = 1 << (l + 1);
                var i_off = 0;
                var A0 = A;
                i_off = n2 - 1;
                for (r = rlim; r > 0; --r)
                {
                    imdct_step3_inner_s_loop(lim, u, i_off, -k0_2, A0, k1, k0);
                    A0 += k1 * 4;
                    i_off -= 8;
                }
            }

            imdct_step3_inner_s_loop_ld654(n >> 5, u, n2 - 1, A, n);
            {
                var bitrev = f.bit_reverse[blocktype];
                var d0 = &v[n4 - 4];
                var d1 = &v[n2 - 4];
                while (d0 >= v)
                {
                    var k4 = 0;
                    k4 = bitrev[0];
                    d1[3] = u[k4 + 0];
                    d1[2] = u[k4 + 1];
                    d0[3] = u[k4 + 2];
                    d0[2] = u[k4 + 3];
                    k4 = bitrev[1];
                    d1[1] = u[k4 + 0];
                    d1[0] = u[k4 + 1];
                    d0[1] = u[k4 + 2];
                    d0[0] = u[k4 + 3];
                    d0 -= 4;
                    d1 -= 4;
                    bitrev += 2;
                }
            }

            {
                var C = f.C[blocktype];
                float* d;
                float* e;
                d = v;
                e = v + n2 - 4;
                while (d < e)
                {
                    float a02 = 0;
                    float a11 = 0;
                    float b0 = 0;
                    float b1 = 0;
                    float b2 = 0;
                    float b3 = 0;
                    a02 = d[0] - e[2];
                    a11 = d[1] + e[3];
                    b0 = C[1] * a02 + C[0] * a11;
                    b1 = C[1] * a11 - C[0] * a02;
                    b2 = d[0] + e[2];
                    b3 = d[1] - e[3];
                    d[0] = b2 + b0;
                    d[1] = b3 + b1;
                    e[2] = b2 - b0;
                    e[3] = b1 - b3;
                    a02 = d[2] - e[0];
                    a11 = d[3] + e[1];
                    b0 = C[3] * a02 + C[2] * a11;
                    b1 = C[3] * a11 - C[2] * a02;
                    b2 = d[2] + e[0];
                    b3 = d[3] - e[1];
                    d[2] = b2 + b0;
                    d[3] = b3 + b1;
                    e[0] = b2 - b0;
                    e[1] = b1 - b3;
                    C += 4;
                    d += 4;
                    e -= 4;
                }
            }

            {
                float* d0;
                float* d1;
                float* d2;
                float* d3;
                var B = f.B[blocktype] + n2 - 8;
                var e = buf2 + n2 - 8;
                d0 = &buffer[0];
                d1 = &buffer[n2 - 4];
                d2 = &buffer[n2];
                d3 = &buffer[n - 4];
                while (e >= v)
                {
                    float p0 = 0;
                    float p1 = 0;
                    float p2 = 0;
                    float p3 = 0;
                    p3 = e[6] * B[7] - e[7] * B[6];
                    p2 = -e[6] * B[6] - e[7] * B[7];
                    d0[0] = p3;
                    d1[3] = -p3;
                    d2[0] = p2;
                    d3[3] = p2;
                    p1 = e[4] * B[5] - e[5] * B[4];
                    p0 = -e[4] * B[4] - e[5] * B[5];
                    d0[1] = p1;
                    d1[2] = -p1;
                    d2[1] = p0;
                    d3[2] = p0;
                    p3 = e[2] * B[3] - e[3] * B[2];
                    p2 = -e[2] * B[2] - e[3] * B[3];
                    d0[2] = p3;
                    d1[1] = -p3;
                    d2[2] = p2;
                    d3[1] = p2;
                    p1 = e[0] * B[1] - e[1] * B[0];
                    p0 = -e[0] * B[0] - e[1] * B[1];
                    d0[3] = p1;
                    d1[0] = -p1;
                    d2[3] = p0;
                    d3[0] = p0;
                    B -= 8;
                    e -= 8;
                    d0 += 4;
                    d2 += 4;
                    d1 -= 4;
                    d3 -= 4;
                }
            }
        }

        f.temp_offset = save_point;
    }

    public static int is_whole_packet_present(stb_vorbis f)
    {
        var s = f.next_seg;
        var first = 1;
        var p = f.stream;
        if (s != -1)
        {
            for (; s < f.segment_count; ++s)
            {
                p += f.segments[s];
                if (f.segments[s] < 255)
                    break;
            }

            if (s == f.segment_count)
                s = -1;
            if (p > f.stream_end)
                return error(f, STBVorbisError.VORBIS_need_more_data);
            first = 0;
        }

        for (; s == -1;)
        {
            byte* q;
            var n = 0;
            if (p + 26 >= f.stream_end)
                return error(f, STBVorbisError.VORBIS_need_more_data);
            if (CRuntime.Memcmp(p, ogg_page_header, 4) != 0)
                return error(f, STBVorbisError.VORBIS_invalid_stream);
            if (p[4] != 0)
                return error(f, STBVorbisError.VORBIS_invalid_stream);
            if (first != 0)
            {
                if (f.previous_length != 0)
                    if ((p[5] & 1) != 0)
                        return error(f, STBVorbisError.VORBIS_invalid_stream);
            }
            else
            {
                if ((p[5] & 1) == 0)
                    return error(f, STBVorbisError.VORBIS_invalid_stream);
            }

            n = p[26];
            q = p + 27;
            p = q + n;
            if (p > f.stream_end)
                return error(f, STBVorbisError.VORBIS_need_more_data);
            for (s = 0; s < n; ++s)
            {
                p += q[s];
                if (q[s] < 255)
                    break;
            }

            if (s == n)
                s = -1;
            if (p > f.stream_end)
                return error(f, STBVorbisError.VORBIS_need_more_data);
            first = 0;
        }

        return 1;
    }

    public static void iter_54(float* z)
    {
        float k00 = 0;
        float k11 = 0;
        float k22 = 0;
        float k33 = 0;
        float y0 = 0;
        float y1 = 0;
        float y2 = 0;
        float y3 = 0;
        k00 = z[0] - z[-4];
        y0 = z[0] + z[-4];
        y2 = z[-2] + z[-6];
        k22 = z[-2] - z[-6];
        z[-0] = y0 + y2;
        z[-2] = y0 - y2;
        k33 = z[-3] - z[-7];
        z[-4] = k00 + k33;
        z[-6] = k00 - k33;
        k11 = z[-1] - z[-5];
        y1 = z[-1] + z[-5];
        y3 = z[-3] + z[-7];
        z[-1] = y1 + y3;
        z[-3] = y1 - y3;
        z[-5] = k11 - k22;
        z[-7] = k11 + k22;
    }

    public static int lookup1_values(int entries, int dim)
    {
        var r = (int)CRuntime.Floor(CRuntime.Exp((float)CRuntime.Log((float)entries) / dim));
        if ((int)CRuntime.Floor(CRuntime.Pow((float)r + 1, dim)) <= entries)
            ++r;
        if (CRuntime.Pow((float)r + 1, dim) <= entries)
            return -1;
        if ((int)CRuntime.Floor(CRuntime.Pow((float)r, dim)) > entries)
            return -1;
        return r;
    }

    public static int maybe_start_packet(stb_vorbis f)
    {
        if (f.next_seg == -1)
        {
            int x = get8(f);
            if (f.eof != 0)
                return 0;
            if (0x4f != x)
                return error(f, STBVorbisError.VORBIS_missing_capture_pattern);
            if (0x67 != get8(f))
                return error(f, STBVorbisError.VORBIS_missing_capture_pattern);
            if (0x67 != get8(f))
                return error(f, STBVorbisError.VORBIS_missing_capture_pattern);
            if (0x53 != get8(f))
                return error(f, STBVorbisError.VORBIS_missing_capture_pattern);
            if (start_page_no_capturepattern(f) == 0)
                return 0;
            if ((f.page_flag & 1) != 0)
            {
                f.last_seg = 0;
                f.bytes_in_seg = 0;
                return error(f, STBVorbisError.VORBIS_continued_packet_flag_invalid);
            }
        }

        return start_packet(f);
    }

    public static void neighbors(ushort* x, int n, int* plow, int* phigh)
    {
        var low = -1;
        var high = 65536;
        var i = 0;
        for (i = 0; i < n; ++i)
        {
            if (x[i] > low && x[i] < x[n])
            {
                *plow = i;
                low = x[i];
            }

            if (x[i] < high && x[i] > x[n])
            {
                *phigh = i;
                high = x[i];
            }
        }
    }

    public static int next_segment(stb_vorbis f)
    {
        var len = 0;
        if (f.last_seg != 0)
            return 0;
        if (f.next_seg == -1)
        {
            f.last_seg_which = f.segment_count - 1;
            if (start_page(f) == 0)
            {
                f.last_seg = 1;
                return 0;
            }

            if ((f.page_flag & 1) == 0)
                return error(f, STBVorbisError.VORBIS_continued_packet_flag_invalid);
        }

        len = f.segments[f.next_seg++];
        if (len < 255)
        {
            f.last_seg = 1;
            f.last_seg_which = f.next_seg - 1;
        }

        if (f.next_seg >= f.segment_count)
            f.next_seg = -1;
        f.bytes_in_seg = (byte)len;
        return len;
    }

    public static int peek_decode_initial(stb_vorbis f, int* p_left_start, int* p_left_end, int* p_right_start,
        int* p_right_end, int* mode)
    {
        var bits_read = 0;
        var bytes_read = 0;
        if (vorbis_decode_initial(f, p_left_start, p_left_end, p_right_start, p_right_end, mode) == 0)
            return 0;
        bits_read = 1 + ilog(f.mode_count - 1);
        if (f.mode_config[*mode].blockflag != 0)
            bits_read += 2;
        bytes_read = (bits_read + 7) / 8;
        f.bytes_in_seg += (byte)bytes_read;
        f.packet_bytes -= bytes_read;
        skip(f, -bytes_read);
        if (f.next_seg == -1)
            f.next_seg = f.segment_count - 1;
        else
            f.next_seg--;
        f.valid_bits = 0;
        return 1;
    }

    public static int point_compare(void* p, void* q)
    {
        var a = (stbv__floor_ordering*)p;
        var b = (stbv__floor_ordering*)q;
        return a->x < b->x ? -1 : a->x > b->x ? 1 : 0;
    }

    public static int predict_point(int x, int x0, int x1, int y0, int y1)
    {
        var dy = y1 - y0;
        var adx = x1 - x0;
        var err = CRuntime.Abs(dy) * (x - x0);
        var off = err / adx;
        return dy < 0 ? y0 - off : y0 + off;
    }

    public static void prep_huffman(stb_vorbis f)
    {
        if (f.valid_bits <= 24)
        {
            if (f.valid_bits == 0)
                f.acc = 0;
            do
            {
                var z = 0;
                if (f.last_seg != 0 && f.bytes_in_seg == 0)
                    return;
                z = get8_packet_raw(f);
                if (z == -1)
                    return;
                f.acc += (uint)z << f.valid_bits;
                f.valid_bits += 8;
            } while (f.valid_bits <= 24);
        }
    }

    public static int residue_decode(stb_vorbis f, Codebook* book, float* target, int offset, int n, int rtype)
    {
        var k = 0;
        if (rtype == 0)
        {
            var step = n / book->dimensions;
            for (k = 0; k < step; ++k)
                if (codebook_decode_step(f, book, target + offset + k, n - offset - k, step) == 0)
                    return 0;
        }
        else
        {
            for (k = 0; k < n;)
            {
                if (codebook_decode(f, book, target + offset, n - k) == 0)
                    return 0;
                k += book->dimensions;
                offset += book->dimensions;
            }
        }

        return 1;
    }

    public static int seek_to_sample_coarse(stb_vorbis f, uint sample_number)
    {
        var left = new ProbedPage();
        var right = new ProbedPage();
        var mid = new ProbedPage();
        var i = 0;
        var start_seg_with_known_loc = 0;
        var end_pos = 0;
        var page_start = 0;
        uint delta = 0;
        uint stream_length = 0;
        uint padding = 0;
        uint last_sample_limit = 0;
        var offset = 0.0;
        var bytes_per_sample = 0.0;
        var probe = 0;
        stream_length = stb_vorbis_stream_length_in_samples(f);
        if (stream_length == 0)
            return error(f, STBVorbisError.VORBIS_seek_without_length);
        if (sample_number > stream_length)
            return error(f, STBVorbisError.VORBIS_seek_invalid);
        padding = (uint)((f.blocksize_1 - f.blocksize_0) >> 2);
        if (sample_number < padding)
            last_sample_limit = 0;
        else
            last_sample_limit = sample_number - padding;
        left = f.p_first;
        while (left.last_decoded_sample == ~0U)
        {
            set_file_offset(f, left.page_end);
            if (get_seek_page_info(f, &left) == 0)
                goto error;
        }

        right = f.p_last;
        if (last_sample_limit <= left.last_decoded_sample)
        {
            if (stb_vorbis_seek_start(f) != 0)
            {
                if (f.current_loc > sample_number)
                    return error(f, STBVorbisError.VORBIS_seek_failed);
                return 1;
            }

            return 0;
        }

        while (left.page_end != right.page_start)
        {
            delta = right.page_start - left.page_end;
            if (delta <= 65536)
            {
                set_file_offset(f, left.page_end);
            }
            else
            {
                if (probe < 2)
                {
                    if (probe == 0)
                    {
                        double data_bytes = right.page_end - left.page_start;
                        bytes_per_sample = data_bytes / right.last_decoded_sample;
                        offset = left.page_start +
                                 bytes_per_sample * (last_sample_limit - left.last_decoded_sample);
                    }
                    else
                    {
                        var error = ((double)last_sample_limit - mid.last_decoded_sample) * bytes_per_sample;
                        if (error >= 0 && error < 8000)
                            error = 8000;
                        if (error < 0 && error > -8000)
                            error = -8000;
                        offset += error * 2;
                    }

                    if (offset < left.page_end)
                        offset = left.page_end;
                    if (offset > right.page_start - 65536)
                        offset = right.page_start - 65536;
                    set_file_offset(f, (uint)offset);
                }
                else
                {
                    set_file_offset(f, left.page_end + delta / 2 - 32768);
                }

                if (vorbis_find_page(f, null, null) == 0)
                    goto error;
            }

            for (; ; )
            {
                if (get_seek_page_info(f, &mid) == 0)
                    goto error;
                if (mid.last_decoded_sample != ~0U)
                    break;
                set_file_offset(f, mid.page_end);
            }

            if (mid.page_start == right.page_start)
            {
                if (probe >= 2 || delta <= 65536)
                    break;
            }
            else
            {
                if (last_sample_limit < mid.last_decoded_sample)
                    right = mid;
                else
                    left = mid;
            }

            ++probe;
        }

        page_start = (int)left.page_start;
        set_file_offset(f, (uint)page_start);
        if (start_page(f) == 0)
            return error(f, STBVorbisError.VORBIS_seek_failed);
        end_pos = f.end_seg_with_known_loc;
        for (; ; )
        {
            for (i = end_pos; i > 0; --i)
                if (f.segments[i - 1] != 255)
                    break;

            start_seg_with_known_loc = i;
            if (start_seg_with_known_loc > 0 || (f.page_flag & 1) == 0)
                break;
            if (go_to_page_before(f, (uint)page_start) == 0)
                goto error;
            page_start = (int)stb_vorbis_get_file_offset(f);
            if (start_page(f) == 0)
                goto error;
            end_pos = f.segment_count - 1;
        }

        f.current_loc_valid = 0;
        f.last_seg = 0;
        f.valid_bits = 0;
        f.packet_bytes = 0;
        f.bytes_in_seg = 0;
        f.previous_length = 0;
        f.next_seg = start_seg_with_known_loc;
        for (i = 0; i < start_seg_with_known_loc; i++) skip(f, f.segments[i]);

        if (vorbis_pump_first_frame(f) == 0)
            return 0;
        if (f.current_loc > sample_number)
            return error(f, STBVorbisError.VORBIS_seek_failed);
        return 1;
    error:;
        stb_vorbis_seek_start(f);
        return error(f, STBVorbisError.VORBIS_seek_failed);
    }

    public static int set_file_offset(stb_vorbis f, uint loc)
    {
        if (f.push_mode != 0)
            return 0;
        f.eof = 0;
        if (1 != 0)
        {
            if (f.stream_start + loc >= f.stream_end || f.stream_start + loc < f.stream_start)
            {
                f.stream = f.stream_end;
                f.eof = 1;
                return 0;
            }

            f.stream = f.stream_start + loc;
            return 1;
        }
    }

    public static void* setup_malloc(int sz)
    {
        sz = (sz + 7) & ~7;

        return sz != 0 ? CRuntime.Malloc((ulong)sz) : null;
    }

    public static void* setup_temp_malloc(int sz)
    {
        sz = (sz + 7) & ~7;
        return CRuntime.Malloc((ulong)sz);
    }

    public static void skip(stb_vorbis z, int n)
    {
        if (1 != 0)
        {
            z.stream += n;
            if (z.stream >= z.stream_end)
                z.eof = 1;
        }
    }

    public static float square(float x)
    {
        return x * x;
    }

    public static int start_decoder(stb_vorbis f)
    {
        var header = stackalloc byte[6];
        byte x = 0;
        byte y = 0;
        var len = 0;
        var i = 0;
        var j = 0;
        var k = 0;
        var max_submaps = 0;
        var longest_floorlist = 0;
        f.first_decode = 1;
        if (start_page(f) == 0)
            return 0;
        if ((f.page_flag & 2) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if ((f.page_flag & 4) != 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if ((f.page_flag & 1) != 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (f.segment_count != 1)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (f.segments[0] != 30)
        {
            if (f.segments[0] == 64 && getn(f, header, 6) != 0 && header[0] == 102 && header[1] == 105 &&
                header[2] == 115 && header[3] == 104 && header[4] == 101 && header[5] == 97 && get8(f) == 100 &&
                get8(f) == 0)
                return error(f, STBVorbisError.VORBIS_ogg_skeleton_not_supported);
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        }

        if (get8(f) != VORBIS_packet_id)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (getn(f, header, 6) == 0)
            return error(f, STBVorbisError.VORBIS_unexpected_eof);
        if (vorbis_validate(header) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (get32(f) != 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        f.channels = get8(f);
        if (f.channels == 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (f.channels > 16)
            return error(f, STBVorbisError.VORBIS_too_many_channels);
        f.sample_rate = get32(f);
        if (f.sample_rate == 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        get32(f);
        get32(f);
        get32(f);
        x = get8(f);
        {
            var log0 = 0;
            var log1 = 0;
            log0 = x & 15;
            log1 = x >> 4;
            f.blocksize_0 = 1 << log0;
            f.blocksize_1 = 1 << log1;
            if (log0 < 6 || log0 > 13)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (log1 < 6 || log1 > 13)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (log0 > log1)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
        }

        x = get8(f);
        if ((x & 1) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_first_page);
        if (start_page(f) == 0)
            return 0;
        if (start_packet(f) == 0)
            return 0;
        if (next_segment(f) == 0)
            return 0;
        if (get8_packet(f) != VORBIS_packet_comment)
            return error(f, STBVorbisError.VORBIS_invalid_setup);
        for (i = 0; i < 6; ++i) header[i] = (byte)get8_packet(f);

        if (vorbis_validate(header) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_setup);
        len = get32_packet(f);

        var sb = new StringBuilder();
        for (i = 0; i < len; ++i) sb.Append((char)get8_packet(f));
        f.vendor = sb.ToString();

        var comment_list_length = get32_packet(f);
        f.comment_list = null;
        if (comment_list_length > 0)
        {
            f.comment_list = new string[comment_list_length];

            for (i = 0; i < comment_list_length; ++i)
            {
                sb.Clear();
                len = get32_packet(f);
                for (j = 0; j < len; ++j) sb.Append((char)get8_packet(f));

                f.comment_list[i] = sb.ToString();
            }
        }

        x = (byte)get8_packet(f);
        if ((x & 1) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_setup);
        skip(f, f.bytes_in_seg);
        f.bytes_in_seg = 0;
        do
        {
            len = next_segment(f);
            skip(f, len);
            f.bytes_in_seg = 0;
        } while (len != 0);

        if (start_packet(f) == 0)
            return 0;
        if (f.push_mode != 0)
            if (is_whole_packet_present(f) == 0)
            {
                if (f.error == STBVorbisError.VORBIS_invalid_stream)
                    f.error = STBVorbisError.VORBIS_invalid_setup;
                return 0;
            }

        crc32_init();
        if (get8_packet(f) != VORBIS_packet_setup)
            return error(f, STBVorbisError.VORBIS_invalid_setup);
        for (i = 0; i < 6; ++i) header[i] = (byte)get8_packet(f);

        if (vorbis_validate(header) == 0)
            return error(f, STBVorbisError.VORBIS_invalid_setup);
        f.codebook_count = (int)(get_bits(f, 8) + 1);
        f.codebooks = (Codebook*)setup_malloc(sizeof(Codebook) * f.codebook_count);
        if (f.codebooks == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        CRuntime.Memset(f.codebooks, 0, (ulong)(sizeof(Codebook) * f.codebook_count));
        for (i = 0; i < f.codebook_count; ++i)
        {
            uint* values;
            var ordered = 0;
            var sorted_count = 0;
            var total = 0;
            byte* lengths;
            var c = f.codebooks + i;
            x = (byte)get_bits(f, 8);
            if (x != 0x42)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            x = (byte)get_bits(f, 8);
            if (x != 0x43)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            x = (byte)get_bits(f, 8);
            if (x != 0x56)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            x = (byte)get_bits(f, 8);
            c->dimensions = (int)((get_bits(f, 8) << 8) + x);
            x = (byte)get_bits(f, 8);
            y = (byte)get_bits(f, 8);
            c->entries = (int)((get_bits(f, 8) << 16) + (y << 8) + x);
            ordered = (int)get_bits(f, 1);
            c->sparse = (byte)(ordered != 0 ? 0 : get_bits(f, 1));
            if (c->dimensions == 0 && c->entries != 0)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (c->sparse != 0)
                lengths = (byte*)setup_temp_malloc(c->entries);
            else
                lengths = c->codeword_lengths = (byte*)setup_malloc(c->entries);
            if (lengths == null)
                return error(f, STBVorbisError.VORBIS_outofmem);
            if (ordered != 0)
            {
                var current_entry = 0;
                var current_length = (int)(get_bits(f, 5) + 1);
                while (current_entry < c->entries)
                {
                    var limit = c->entries - current_entry;
                    var n = (int)get_bits(f, ilog(limit));
                    if (current_length >= 32)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                    if (current_entry + n > c->entries) return error(f, STBVorbisError.VORBIS_invalid_setup);

                    CRuntime.Memset(lengths + current_entry, current_length, (ulong)n);
                    current_entry += n;
                    ++current_length;
                }
            }
            else
            {
                for (j = 0; j < c->entries; ++j)
                {
                    var present = (int)(c->sparse != 0 ? get_bits(f, 1) : 1);
                    if (present != 0)
                    {
                        lengths[j] = (byte)(get_bits(f, 5) + 1);
                        ++total;
                        if (lengths[j] == 32)
                            return error(f, STBVorbisError.VORBIS_invalid_setup);
                    }
                    else
                    {
                        lengths[j] = 255;
                    }
                }
            }

            if (c->sparse != 0 && total >= c->entries >> 2)
            {
                c->codeword_lengths = (byte*)setup_malloc(c->entries);
                if (c->codeword_lengths == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
                CRuntime.Memcpy(c->codeword_lengths, lengths, (ulong)c->entries);
                CRuntime.Free(lengths);
                lengths = c->codeword_lengths;
                c->sparse = 0;
            }

            if (c->sparse != 0)
            {
                sorted_count = total;
            }
            else
            {
                sorted_count = 0;
                for (j = 0; j < c->entries; ++j)
                    if (lengths[j] > 10 && lengths[j] != 255)
                        ++sorted_count;
            }

            c->sorted_entries = sorted_count;
            values = null;
            if (c->sparse == 0)
            {
                c->codewords = (uint*)setup_malloc(sizeof(uint) * c->entries);
                if (c->codewords == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
            }
            else
            {
                uint size = 0;
                if (c->sorted_entries != 0)
                {
                    c->codeword_lengths = (byte*)setup_malloc(c->sorted_entries);
                    if (c->codeword_lengths == null)
                        return error(f, STBVorbisError.VORBIS_outofmem);
                    c->codewords = (uint*)setup_temp_malloc(sizeof(uint) * c->sorted_entries);
                    if (c->codewords == null)
                        return error(f, STBVorbisError.VORBIS_outofmem);
                    values = (uint*)setup_temp_malloc(sizeof(uint) * c->sorted_entries);
                    if (values == null)
                        return error(f, STBVorbisError.VORBIS_outofmem);
                }

                size = (uint)(c->entries + (sizeof(uint) + sizeof(uint)) * c->sorted_entries);
            }

            if (compute_codewords(c, lengths, c->entries, values) == 0)
            {
                if (c->sparse != 0)
                    CRuntime.Free(values);
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            }

            if (c->sorted_entries != 0)
            {
                c->sorted_codewords = (uint*)setup_malloc(sizeof(uint) * (c->sorted_entries + 1));
                if (c->sorted_codewords == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
                c->sorted_values = (int*)setup_malloc(sizeof(int) * (c->sorted_entries + 1));
                if (c->sorted_values == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
                ++c->sorted_values;
                c->sorted_values[-1] = -1;
                compute_sorted_huffman(c, lengths, values);
            }

            if (c->sparse != 0)
            {
                CRuntime.Free(values);
                CRuntime.Free(c->codewords);
                CRuntime.Free(lengths);
                c->codewords = null;
            }

            compute_accelerated_huffman(c);
            c->lookup_type = (byte)get_bits(f, 4);
            if (c->lookup_type > 2)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (c->lookup_type > 0)
            {
                ushort* mults;
                c->minimum_value = float32_unpack(get_bits(f, 32));
                c->delta_value = float32_unpack(get_bits(f, 32));
                c->value_bits = (byte)(get_bits(f, 4) + 1);
                c->sequence_p = (byte)get_bits(f, 1);
                if (c->lookup_type == 1)
                {
                    var values2 = lookup1_values(c->entries, c->dimensions);
                    if (values2 < 0)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                    c->lookup_values = (uint)values2;
                }
                else
                {
                    c->lookup_values = (uint)(c->entries * c->dimensions);
                }

                if (c->lookup_values == 0)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                mults = (ushort*)setup_temp_malloc((int)(sizeof(ushort) * c->lookup_values));
                if (mults == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
                for (j = 0; j < (int)c->lookup_values; ++j)
                {
                    var q = (int)get_bits(f, c->value_bits);
                    if (q == -1)
                    {
                        CRuntime.Free(mults);
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                    }

                    mults[j] = (ushort)q;
                }

                if (c->lookup_type == 1)
                {
                    var len2 = 0;
                    int sparse = c->sparse;
                    float last = 0;
                    if (sparse != 0)
                    {
                        if (c->sorted_entries == 0)
                            goto skip;
                        c->multiplicands = (float*)setup_malloc(sizeof(float) * c->sorted_entries * c->dimensions);
                    }
                    else
                    {
                        c->multiplicands = (float*)setup_malloc(sizeof(float) * c->entries * c->dimensions);
                    }

                    if (c->multiplicands == null)
                    {
                        CRuntime.Free(mults);
                        return error(f, STBVorbisError.VORBIS_outofmem);
                    }

                    len2 = sparse != 0 ? c->sorted_entries : c->entries;
                    for (j = 0; j < len2; ++j)
                    {
                        var z = (uint)(sparse != 0 ? c->sorted_values[j] : j);
                        uint div = 1;
                        for (k = 0; k < c->dimensions; ++k)
                        {
                            var off = (int)(z / div % c->lookup_values);
                            var val = mults[off] * c->delta_value + c->minimum_value + last;
                            c->multiplicands[j * c->dimensions + k] = val;
                            if (c->sequence_p != 0)
                                last = val;
                            if (k + 1 < c->dimensions)
                            {
                                if (div > 0xffffffff / c->lookup_values)
                                {
                                    CRuntime.Free(mults);
                                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                                }

                                div *= c->lookup_values;
                            }
                        }
                    }

                    c->lookup_type = 2;
                }
                else
                {
                    float last = 0;
                    c->multiplicands = (float*)setup_malloc((int)(sizeof(float) * c->lookup_values));
                    if (c->multiplicands == null)
                    {
                        CRuntime.Free(mults);
                        return error(f, STBVorbisError.VORBIS_outofmem);
                    }

                    for (j = 0; j < (int)c->lookup_values; ++j)
                    {
                        var val = mults[j] * c->delta_value + c->minimum_value + last;
                        c->multiplicands[j] = val;
                        if (c->sequence_p != 0)
                            last = val;
                    }
                }

            skip:;
                CRuntime.Free(mults);
            }
        }

        x = (byte)(get_bits(f, 6) + 1);
        for (i = 0; i < x; ++i)
        {
            var z = get_bits(f, 16);
            if (z != 0)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
        }

        f.floor_count = (int)(get_bits(f, 6) + 1);
        f.floor_config = (Floor*)setup_malloc(f.floor_count * sizeof(Floor));
        if (f.floor_config == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        for (i = 0; i < f.floor_count; ++i)
        {
            f.floor_types[i] = (ushort)get_bits(f, 16);
            if (f.floor_types[i] > 1)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (f.floor_types[i] == 0)
            {
                var g = &f.floor_config[i].floor0;
                g->order = (byte)get_bits(f, 8);
                g->rate = (ushort)get_bits(f, 16);
                g->bark_map_size = (ushort)get_bits(f, 16);
                g->amplitude_bits = (byte)get_bits(f, 6);
                g->amplitude_offset = (byte)get_bits(f, 8);
                g->number_of_books = (byte)(get_bits(f, 4) + 1);
                for (j = 0; j < g->number_of_books; ++j) g->book_list[j] = (byte)get_bits(f, 8);

                return error(f, STBVorbisError.VORBIS_feature_not_supported);
            }
            else
            {
                var p = stackalloc stbv__floor_ordering[250];
                var g = &f.floor_config[i].floor1;
                var max_class = -1;
                g->partitions = (byte)get_bits(f, 5);
                for (j = 0; j < g->partitions; ++j)
                {
                    g->partition_class_list[j] = (byte)get_bits(f, 4);
                    if (g->partition_class_list[j] > max_class)
                        max_class = g->partition_class_list[j];
                }

                for (j = 0; j <= max_class; ++j)
                {
                    g->class_dimensions[j] = (byte)(get_bits(f, 3) + 1);
                    g->class_subclasses[j] = (byte)get_bits(f, 2);
                    if (g->class_subclasses[j] != 0)
                    {
                        g->class_masterbooks[j] = (byte)get_bits(f, 8);
                        if (g->class_masterbooks[j] >= f.codebook_count)
                            return error(f, STBVorbisError.VORBIS_invalid_setup);
                    }

                    for (k = 0; k < 1 << g->class_subclasses[j]; ++k)
                    {
                        g->subclass_books[j * 8 + k] = (short)((short)get_bits(f, 8) - 1);
                        if (g->subclass_books[j * 8 + k] >= f.codebook_count)
                            return error(f, STBVorbisError.VORBIS_invalid_setup);
                    }
                }

                g->floor1_multiplier = (byte)(get_bits(f, 2) + 1);
                g->rangebits = (byte)get_bits(f, 4);
                g->Xlist[0] = 0;
                g->Xlist[1] = (ushort)(1 << g->rangebits);
                g->values = 2;
                for (j = 0; j < g->partitions; ++j)
                {
                    int c = g->partition_class_list[j];
                    for (k = 0; k < g->class_dimensions[c]; ++k)
                    {
                        g->Xlist[g->values] = (ushort)get_bits(f, g->rangebits);
                        ++g->values;
                    }
                }

                for (j = 0; j < g->values; ++j)
                {
                    p[j].x = g->Xlist[j];
                    p[j].id = (ushort)j;
                }

                CRuntime.Qsort(p, (ulong)g->values, (ulong)sizeof(stbv__floor_ordering), point_compare);
                for (j = 0; j < g->values - 1; ++j)
                    if (p[j].x == p[j + 1].x)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);

                for (j = 0; j < g->values; ++j) g->sorted_order[j] = (byte)p[j].id;

                for (j = 2; j < g->values; ++j)
                {
                    var low = 0;
                    var hi = 0;
                    neighbors(g->Xlist, j, &low, &hi);
                    g->neighbors[j * 2 + 0] = (byte)low;
                    g->neighbors[j * 2 + 1] = (byte)hi;
                }

                if (g->values > longest_floorlist)
                    longest_floorlist = g->values;
            }
        }

        f.residue_count = (int)(get_bits(f, 6) + 1);
        f.residue_config = new Residue[f.residue_count];
        for (i = 0; i < f.residue_config.Length; ++i) f.residue_config[i] = new Residue();
        ;

        for (i = 0; i < f.residue_count; ++i)
        {
            var residue_cascade = stackalloc byte[64];
            var r = f.residue_config[i];
            f.residue_types[i] = (ushort)get_bits(f, 16);
            if (f.residue_types[i] > 2)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            r.begin = get_bits(f, 24);
            r.end = get_bits(f, 24);
            if (r.end < r.begin)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            r.part_size = get_bits(f, 24) + 1;
            r.classifications = (byte)(get_bits(f, 6) + 1);
            r.classbook = (byte)get_bits(f, 8);
            if (r.classbook >= f.codebook_count)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            for (j = 0; j < r.classifications; ++j)
            {
                byte high_bits = 0;
                var low_bits = (byte)get_bits(f, 3);
                if (get_bits(f, 1) != 0)
                    high_bits = (byte)get_bits(f, 5);
                residue_cascade[j] = (byte)(high_bits * 8 + low_bits);
            }

            r.residue_books = new short[r.classifications, 8];
            for (j = 0; j < r.classifications; ++j)
                for (k = 0; k < 8; ++k)
                    if ((residue_cascade[j] & (1 << k)) != 0)
                    {
                        r.residue_books[j, k] = (short)get_bits(f, 8);
                        if (r.residue_books[j, k] >= f.codebook_count)
                            return error(f, STBVorbisError.VORBIS_invalid_setup);
                    }
                    else
                    {
                        r.residue_books[j, k] = -1;
                    }

            r.classdata = (byte**)setup_malloc(sizeof(byte*) * f.codebooks[r.classbook].entries);
            if (r.classdata == null)
                return error(f, STBVorbisError.VORBIS_outofmem);
            CRuntime.Memset(r.classdata, 0, (ulong)(sizeof(byte*) * f.codebooks[r.classbook].entries));
            for (j = 0; j < f.codebooks[r.classbook].entries; ++j)
            {
                var classwords = f.codebooks[r.classbook].dimensions;
                var temp = j;
                r.classdata[j] = (byte*)setup_malloc(sizeof(byte) * classwords);
                if (r.classdata[j] == null)
                    return error(f, STBVorbisError.VORBIS_outofmem);
                for (k = classwords - 1; k >= 0; --k)
                {
                    r.classdata[j][k] = (byte)(temp % r.classifications);
                    temp /= r.classifications;
                }
            }
        }

        f.mapping_count = (int)(get_bits(f, 6) + 1);
        f.mapping = (Mapping*)setup_malloc(f.mapping_count * sizeof(Mapping));
        if (f.mapping == null)
            return error(f, STBVorbisError.VORBIS_outofmem);
        CRuntime.Memset(f.mapping, 0, (ulong)(f.mapping_count * sizeof(Mapping)));
        for (i = 0; i < f.mapping_count; ++i)
        {
            var m = f.mapping + i;
            var mapping_type = (int)get_bits(f, 16);
            if (mapping_type != 0)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            m->chan = (MappingChannel*)setup_malloc(f.channels * sizeof(MappingChannel));
            if (m->chan == null)
                return error(f, STBVorbisError.VORBIS_outofmem);
            if (get_bits(f, 1) != 0)
                m->submaps = (byte)(get_bits(f, 4) + 1);
            else
                m->submaps = 1;
            if (m->submaps > max_submaps)
                max_submaps = m->submaps;
            if (get_bits(f, 1) != 0)
            {
                m->coupling_steps = (ushort)(get_bits(f, 8) + 1);
                if (m->coupling_steps > f.channels)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                for (k = 0; k < m->coupling_steps; ++k)
                {
                    m->chan[k].magnitude = (byte)get_bits(f, ilog(f.channels - 1));
                    m->chan[k].angle = (byte)get_bits(f, ilog(f.channels - 1));
                    if (m->chan[k].magnitude >= f.channels)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                    if (m->chan[k].angle >= f.channels)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                    if (m->chan[k].magnitude == m->chan[k].angle)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                }
            }
            else
            {
                m->coupling_steps = 0;
            }

            if (get_bits(f, 2) != 0)
                return error(f, STBVorbisError.VORBIS_invalid_setup);
            if (m->submaps > 1)
                for (j = 0; j < f.channels; ++j)
                {
                    m->chan[j].mux = (byte)get_bits(f, 4);
                    if (m->chan[j].mux >= m->submaps)
                        return error(f, STBVorbisError.VORBIS_invalid_setup);
                }
            else
                for (j = 0; j < f.channels; ++j)
                    m->chan[j].mux = 0;

            for (j = 0; j < m->submaps; ++j)
            {
                get_bits(f, 8);
                m->submap_floor[j] = (byte)get_bits(f, 8);
                m->submap_residue[j] = (byte)get_bits(f, 8);
                if (m->submap_floor[j] >= f.floor_count)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                if (m->submap_residue[j] >= f.residue_count)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
            }
        }

        f.mode_count = (int)(get_bits(f, 6) + 1);
        for (i = 0; i < f.mode_count; ++i)
            fixed (Mode* m = &f.mode_config[i])
            {
                m->blockflag = (byte)get_bits(f, 1);
                m->windowtype = (ushort)get_bits(f, 16);
                m->transformtype = (ushort)get_bits(f, 16);
                m->mapping = (byte)get_bits(f, 8);
                if (m->windowtype != 0)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                if (m->transformtype != 0)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
                if (m->mapping >= f.mapping_count)
                    return error(f, STBVorbisError.VORBIS_invalid_setup);
            }

        flush_packet(f);
        f.previous_length = 0;
        for (i = 0; i < f.channels; ++i)
        {
            f.channel_buffers[i] = (float*)setup_malloc(sizeof(float) * f.blocksize_1);
            f.previous_window[i] = (float*)setup_malloc(sizeof(float) * f.blocksize_1 / 2);
            f.finalY[i] = (short*)setup_malloc(sizeof(short) * longest_floorlist);
            if (f.channel_buffers[i] == null || f.previous_window[i] == null || f.finalY[i] == null)
                return error(f, STBVorbisError.VORBIS_outofmem);
            CRuntime.Memset(f.channel_buffers[i], 0, (ulong)(sizeof(float) * f.blocksize_1));
        }

        if (init_blocksize(f, 0, f.blocksize_0) == 0)
            return 0;
        if (init_blocksize(f, 1, f.blocksize_1) == 0)
            return 0;
        f.blocksize[0] = f.blocksize_0;
        f.blocksize[1] = f.blocksize_1;
        {
            var imdct_mem = (uint)((f.blocksize_1 * sizeof(float)) >> 1);
            uint classify_mem = 0;
            var max_part_read = 0;
            for (var i2 = 0; i2 < f.residue_count; ++i2)
            {
                var r = f.residue_config[i2];
                var actual_size = (uint)(f.blocksize_1 / 2);
                var limit_r_begin = r.begin < actual_size ? r.begin : actual_size;
                var limit_r_end = r.end < actual_size ? r.end : actual_size;
                var n_read = (int)(limit_r_end - limit_r_begin);
                var part_read = (int)(n_read / r.part_size);
                if (part_read > max_part_read)
                    max_part_read = part_read;
            }

            classify_mem = (uint)(f.channels * (sizeof(void*) + max_part_read * sizeof(byte)));
        }

        if (f.next_seg == -1)
            f.first_audio_page_offset = stb_vorbis_get_file_offset(f);
        else
            f.first_audio_page_offset = 0;

        return 1;
    }

    public static int start_packet(stb_vorbis f)
    {
        while (f.next_seg == -1)
        {
            if (start_page(f) == 0)
                return 0;
            if ((f.page_flag & 1) != 0)
                return error(f, STBVorbisError.VORBIS_continued_packet_flag_invalid);
        }

        f.last_seg = 0;
        f.valid_bits = 0;
        f.packet_bytes = 0;
        f.bytes_in_seg = 0;
        return 1;
    }

    public static int start_page(stb_vorbis f)
    {
        if (capture_pattern(f) == 0)
            return error(f, STBVorbisError.VORBIS_missing_capture_pattern);
        return start_page_no_capturepattern(f);
    }

    public static int start_page_no_capturepattern(stb_vorbis f)
    {
        uint loc0 = 0;
        uint loc1 = 0;
        uint n = 0;
        if (f.first_decode != 0 && f.push_mode == 0) f.p_first.page_start = stb_vorbis_get_file_offset(f) - 4;

        if (0 != get8(f))
            return error(f, STBVorbisError.VORBIS_invalid_stream_structure_version);
        f.page_flag = get8(f);
        loc0 = get32(f);
        loc1 = get32(f);
        get32(f);
        n = get32(f);
        f.last_page = (int)n;
        get32(f);
        f.segment_count = get8(f);

        fixed (byte* sptr = f.segments)
        {
            if (getn(f, sptr, f.segment_count) == 0)
                return error(f, STBVorbisError.VORBIS_unexpected_eof);
        }

        f.end_seg_with_known_loc = -2;
        if (loc0 != ~0U || loc1 != ~0U)
        {
            var i = 0;
            for (i = f.segment_count - 1; i >= 0; --i)
                if (f.segments[i] < 255)
                    break;

            if (i >= 0)
            {
                f.end_seg_with_known_loc = i;
                f.known_loc_for_packet = loc0;
            }
        }

        if (f.first_decode != 0)
        {
            var i = 0;
            var len = 0;
            len = 0;
            for (i = 0; i < f.segment_count; ++i) len += f.segments[i];

            len += 27 + f.segment_count;
            f.p_first.page_end = (uint)(f.p_first.page_start + len);
            f.p_first.last_decoded_sample = loc0;
        }

        f.next_seg = 0;
        return 1;
    }

    public static void stb_vorbis_close(stb_vorbis p)
    {
        if (p == null)
            return;
        vorbis_deinit(p);
    }

    public static int stb_vorbis_decode_frame_pushdata(stb_vorbis f, byte* data, int data_len, int* channels,
        ref float*[] output, int* samples)
    {
        var i = 0;
        var len = 0;
        var right = 0;
        var left = 0;
        if (f.push_mode == 0)
            return error(f, STBVorbisError.VORBIS_invalid_api_mixing);
        if (f.page_crc_tests >= 0)
        {
            *samples = 0;
            return vorbis_search_for_page_pushdata(f, data, data_len);
        }

        f.stream = data;
        f.stream_end = data + data_len;
        f.error = STBVorbisError.VORBIS__no_error;
        if (is_whole_packet_present(f) == 0)
        {
            *samples = 0;
            return 0;
        }

        if (vorbis_decode_packet(f, &len, &left, &right) == 0)
        {
            var error = f.error;
            if (error == STBVorbisError.VORBIS_bad_packet_type)
            {
                f.error = STBVorbisError.VORBIS__no_error;
                while (get8_packet(f) != -1)
                    if (f.eof != 0)
                        break;

                *samples = 0;
                return (int)(f.stream - data);
            }

            if (error == STBVorbisError.VORBIS_continued_packet_flag_invalid)
                if (f.previous_length == 0)
                {
                    f.error = STBVorbisError.VORBIS__no_error;
                    while (get8_packet(f) != -1)
                        if (f.eof != 0)
                            break;

                    *samples = 0;
                    return (int)(f.stream - data);
                }

            stb_vorbis_flush_pushdata(f);
            f.error = error;
            *samples = 0;
            return 1;
        }

        len = vorbis_finish_frame(f, len, left, right);
        for (i = 0; i < f.channels; ++i) f.outputs[i] = f.channel_buffers[i] + left;

        if (channels != null)
            *channels = f.channels;
        *samples = len;

        output = f.outputs;
        return (int)(f.stream - data);
    }

    public static int stb_vorbis_decode_memory(byte* mem, int len, int* channels, int* sample_rate,
        ref short* output)
    {
        var data_len = 0;
        var offset = 0;
        var total = 0;
        var limit = 0;
        var error = 0;
        short* data;
        var v = stb_vorbis_open_memory(mem, len, &error);
        if (v == null)
            return -1;
        limit = v.channels * 4096;
        *channels = v.channels;
        if (sample_rate != null)
            *sample_rate = (int)v.sample_rate;
        offset = data_len = 0;
        total = limit;
        data = (short*)CRuntime.Malloc((ulong)(total * sizeof(short)));
        if (data == null)
        {
            stb_vorbis_close(v);
            return -2;
        }

        for (; ; )
        {
            var n = stb_vorbis_get_frame_short_interleaved(v, v.channels, data + offset, total - offset);
            if (n == 0)
                break;
            data_len += n;
            offset += n * v.channels;
            if (offset + limit > total)
            {
                short* data2;
                total *= 2;
                data2 = (short*)CRuntime.Realloc(data, (ulong)(total * sizeof(short)));
                if (data2 == null)
                {
                    CRuntime.Free(data);
                    stb_vorbis_close(v);
                    return -2;
                }

                data = data2;
            }
        }

        output = data;
        stb_vorbis_close(v);
        return data_len;
    }

    public static void stb_vorbis_flush_pushdata(stb_vorbis f)
    {
        f.previous_length = 0;
        f.page_crc_tests = 0;
        f.discard_samples_deferred = 0;
        f.current_loc_valid = 0;
        f.first_decode = 0;
        f.samples_output = 0;
        f.channel_buffer_start = 0;
        f.channel_buffer_end = 0;
    }

    public static int stb_vorbis_get_error(stb_vorbis f)
    {
        var e = (int)f.error;
        f.error = STBVorbisError.VORBIS__no_error;
        return e;
    }

    public static uint stb_vorbis_get_file_offset(stb_vorbis f)
    {
        if (f.push_mode != 0)
            return 0;
        if (1 != 0)
            return (uint)(f.stream - f.stream_start);
    }

    public static int stb_vorbis_get_frame_float(stb_vorbis f, int* channels, ref float*[] output)
    {
        var len = 0;
        var right = 0;
        var left = 0;
        var i = 0;
        if (f.push_mode != 0)
            return error(f, STBVorbisError.VORBIS_invalid_api_mixing);
        if (vorbis_decode_packet(f, &len, &left, &right) == 0)
        {
            f.channel_buffer_start = f.channel_buffer_end = 0;
            return 0;
        }

        len = vorbis_finish_frame(f, len, left, right);
        for (i = 0; i < f.channels; ++i) f.outputs[i] = f.channel_buffers[i] + left;

        f.channel_buffer_start = left;
        f.channel_buffer_end = left + len;
        if (channels != null)
            *channels = f.channels;

        output = f.outputs;
        return len;
    }

    public static int stb_vorbis_get_frame_short(stb_vorbis f, int num_c, short** buffer, int num_samples)
    {
        float*[] output = null;
        var len = stb_vorbis_get_frame_float(f, null, ref output);
        if (len > num_samples)
            len = num_samples;
        if (len != 0)
            convert_samples_short(num_c, buffer, 0, f.channels, output, 0, len);
        return len;
    }

    public static int stb_vorbis_get_frame_short_interleaved(stb_vorbis f, int num_c, short* buffer, int num_shorts)
    {
        float*[] output = null;
        var len = 0;
        if (num_c == 1)
            return stb_vorbis_get_frame_short(f, num_c, &buffer, num_shorts);
        len = stb_vorbis_get_frame_float(f, null, ref output);
        if (len != 0)
        {
            if (len * num_c > num_shorts)
                len = num_shorts / num_c;
            convert_channels_short_interleaved(num_c, buffer, f.channels, output, 0, len);
        }

        return len;
    }

    public static stb_vorbis_info stb_vorbis_get_info(stb_vorbis f)
    {
        var d = new stb_vorbis_info();
        d.channels = f.channels;
        d.sample_rate = f.sample_rate;
        d.max_frame_size = f.blocksize_1 >> 1;
        return d;
    }

    public static int stb_vorbis_get_sample_offset(stb_vorbis f)
    {
        if (f.current_loc_valid != 0)
            return (int)f.current_loc;
        return -1;
    }

    public static int stb_vorbis_get_samples_float(stb_vorbis f, int channels, float** buffer, int num_samples)
    {
        float*[] outputs = null;
        var n = 0;
        var z = f.channels;
        if (z > channels)
            z = channels;
        while (n < num_samples)
        {
            var i = 0;
            var k = f.channel_buffer_end - f.channel_buffer_start;
            if (n + k >= num_samples)
                k = num_samples - n;
            if (k != 0)
            {
                for (i = 0; i < z; ++i)
                    CRuntime.Memcpy(buffer[i] + n, f.channel_buffers[i] + f.channel_buffer_start,
                        (ulong)(sizeof(float) * k));

                for (; i < channels; ++i) CRuntime.Memset(buffer[i] + n, 0, (ulong)(sizeof(float) * k));
            }

            n += k;
            f.channel_buffer_start += k;
            if (n == num_samples)
                break;
            if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
                break;
        }

        return n;
    }

    public static int stb_vorbis_get_samples_float_interleaved(stb_vorbis f, int channels, float* buffer,
        int num_floats)
    {
        float*[] outputs = null;
        var len = num_floats / channels;
        var n = 0;
        var z = f.channels;
        if (z > channels)
            z = channels;
        while (n < len)
        {
            var i = 0;
            var j = 0;
            var k = f.channel_buffer_end - f.channel_buffer_start;
            if (n + k >= len)
                k = len - n;
            for (j = 0; j < k; ++j)
            {
                for (i = 0; i < z; ++i) *buffer++ = f.channel_buffers[i][f.channel_buffer_start + j];

                for (; i < channels; ++i) *buffer++ = 0;
            }

            n += k;
            f.channel_buffer_start += k;
            if (n == len)
                break;
            if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
                break;
        }

        return n;
    }

    public static int stb_vorbis_get_samples_short(stb_vorbis f, int channels, short** buffer, int len)
    {
        float*[] outputs = null;
        var n = 0;
        while (n < len)
        {
            var k = f.channel_buffer_end - f.channel_buffer_start;
            if (n + k >= len)
                k = len - n;
            if (k != 0)
                convert_samples_short(channels, buffer, n, f.channels, f.channel_buffers, f.channel_buffer_start,
                    k);
            n += k;
            f.channel_buffer_start += k;
            if (n == len)
                break;
            if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
                break;
        }

        return n;
    }

    public static int stb_vorbis_get_samples_short_interleaved(stb_vorbis f, int channels, short* buffer,
        int num_shorts)
    {
        float*[] outputs = null;
        var len = num_shorts / channels;
        var n = 0;
        while (n < len)
        {
            var k = f.channel_buffer_end - f.channel_buffer_start;
            if (n + k >= len)
                k = len - n;
            if (k != 0)
                convert_channels_short_interleaved(channels, buffer, f.channels, f.channel_buffers,
                    f.channel_buffer_start, k);
            buffer += k * channels;
            n += k;
            f.channel_buffer_start += k;
            if (n == len)
                break;
            if (stb_vorbis_get_frame_float(f, null, ref outputs) == 0)
                break;
        }

        return n;
    }

    public static stb_vorbis stb_vorbis_open_memory(byte* data, int len, int* error)
    {
        stb_vorbis f;
        var p = new stb_vorbis();
        if (data == null)
        {
            if (error != null)
                *error = (int)STBVorbisError.VORBIS_unexpected_eof;
            return null;
        }

        vorbis_init(p);
        p.stream = data;
        p.stream_end = data + len;
        p.stream_start = p.stream;
        p.stream_len = (uint)len;
        p.push_mode = 0;
        if (start_decoder(p) != 0)
        {
            f = vorbis_alloc(p);
            if (f != null)
            {
                f = p;
                vorbis_pump_first_frame(f);
                if (error != null)
                    *error = (int)STBVorbisError.VORBIS__no_error;
                return f;
            }
        }

        if (error != null)
            *error = (int)p.error;
        vorbis_deinit(p);
        return null;
    }

    public static stb_vorbis stb_vorbis_open_pushdata(byte* data, int data_len, int* data_used, int* error)
    {
        stb_vorbis f;
        var p = new stb_vorbis();
        vorbis_init(p);
        p.stream = data;
        p.stream_end = data + data_len;
        p.push_mode = 1;
        if (start_decoder(p) == 0)
        {
            if (p.eof != 0)
                *error = (int)STBVorbisError.VORBIS_need_more_data;
            else
                *error = (int)p.error;
            vorbis_deinit(p);
            return null;
        }

        f = vorbis_alloc(p);
        if (f != null)
        {
            f = p;
            *data_used = (int)(f.stream - data);
            *error = 0;
            return f;
        }

        vorbis_deinit(p);
        return null;
    }

    public static int stb_vorbis_seek(stb_vorbis f, uint sample_number)
    {
        if (stb_vorbis_seek_frame(f, sample_number) == 0)
            return 0;
        if (sample_number != f.current_loc)
        {
            var n = 0;
            var frame_start = f.current_loc;

            float*[] output = null;
            stb_vorbis_get_frame_float(f, &n, ref output);
            f.channel_buffer_start += (int)(sample_number - frame_start);
        }

        return 1;
    }

    public static int stb_vorbis_seek_frame(stb_vorbis f, uint sample_number)
    {
        uint max_frame_samples = 0;
        if (f.push_mode != 0)
            return error(f, STBVorbisError.VORBIS_invalid_api_mixing);
        if (seek_to_sample_coarse(f, sample_number) == 0)
            return 0;
        max_frame_samples = (uint)((f.blocksize_1 * 3 - f.blocksize_0) >> 2);
        while (f.current_loc < sample_number)
        {
            var left_start = 0;
            var left_end = 0;
            var right_start = 0;
            var right_end = 0;
            var mode = 0;
            var frame_samples = 0;
            if (peek_decode_initial(f, &left_start, &left_end, &right_start, &right_end, &mode) == 0)
                return error(f, STBVorbisError.VORBIS_seek_failed);
            frame_samples = right_start - left_start;
            if (f.current_loc + frame_samples > sample_number) return 1;

            if (f.current_loc + frame_samples + max_frame_samples > sample_number)
            {
                vorbis_pump_first_frame(f);
            }
            else
            {
                f.current_loc += (uint)frame_samples;
                f.previous_length = 0;
                maybe_start_packet(f);
                flush_packet(f);
            }
        }

        if (f.current_loc != sample_number)
            return error(f, STBVorbisError.VORBIS_seek_failed);
        return 1;
    }

    public static int stb_vorbis_seek_start(stb_vorbis f)
    {
        if (f.push_mode != 0) return error(f, STBVorbisError.VORBIS_invalid_api_mixing);

        set_file_offset(f, f.first_audio_page_offset);
        f.previous_length = 0;
        f.first_decode = 1;
        f.next_seg = -1;
        return vorbis_pump_first_frame(f);
    }

    public static uint stb_vorbis_stream_length_in_samples(stb_vorbis f)
    {
        uint restore_offset = 0;
        uint previous_safe = 0;
        uint end = 0;
        uint last_page_loc = 0;
        if (f.push_mode != 0)
            return (uint)error(f, STBVorbisError.VORBIS_invalid_api_mixing);
        if (f.total_samples == 0)
        {
            uint last = 0;
            uint lo = 0;
            uint hi = 0;
            var header = stackalloc sbyte[6];
            restore_offset = stb_vorbis_get_file_offset(f);
            if (f.stream_len >= 65536 && f.stream_len - 65536 >= f.first_audio_page_offset)
                previous_safe = f.stream_len - 65536;
            else
                previous_safe = f.first_audio_page_offset;
            set_file_offset(f, previous_safe);
            if (vorbis_find_page(f, &end, &last) == 0)
            {
                f.error = STBVorbisError.VORBIS_cant_find_last_page;
                f.total_samples = 0xffffffff;
                goto done;
            }

            last_page_loc = stb_vorbis_get_file_offset(f);
            while (last == 0)
            {
                set_file_offset(f, end);
                if (vorbis_find_page(f, &end, &last) == 0) break;

                last_page_loc = stb_vorbis_get_file_offset(f);
            }

            set_file_offset(f, last_page_loc);
            getn(f, (byte*)header, 6);
            lo = get32(f);
            hi = get32(f);
            if (lo == 0xffffffff && hi == 0xffffffff)
            {
                f.error = STBVorbisError.VORBIS_cant_find_last_page;
                f.total_samples = 0xffffffff;
                goto done;
            }

            if (hi != 0)
                lo = 0xfffffffe;
            f.total_samples = lo;
            f.p_last.page_start = last_page_loc;
            f.p_last.page_end = end;
            f.p_last.last_decoded_sample = lo;
        done:;
            set_file_offset(f, restore_offset);
        }

        return f.total_samples == 0xffffffff ? 0 : f.total_samples;
    }

    public static float stb_vorbis_stream_length_in_seconds(stb_vorbis f)
    {
        return stb_vorbis_stream_length_in_samples(f) / (float)f.sample_rate;
    }

    public static int uint32_compare(void* p, void* q)
    {
        var x = *(uint*)p;
        var y = *(uint*)q;
        return x < y ? -1 : x > y ? 1 : 0;
    }

    public static stb_vorbis vorbis_alloc(stb_vorbis f)
    {
        return new stb_vorbis();
    }

    public static int vorbis_decode_initial(stb_vorbis f, int* p_left_start, int* p_left_end, int* p_right_start,
        int* p_right_end, int* mode)
    {
        var i = 0;
        var n = 0;
        var prev = 0;
        var next = 0;
        var window_center = 0;
        f.channel_buffer_start = f.channel_buffer_end = 0;
    retry:;
        if (f.eof != 0) return 0;
        if (maybe_start_packet(f) == 0)
            return 0;
        if (get_bits(f, 1) != 0)
        {
            if (f.push_mode != 0)
                return error(f, STBVorbisError.VORBIS_bad_packet_type);
            while (-1 != get8_packet(f))
            {
            }

            goto retry;
        }

        i = (int)get_bits(f, ilog(f.mode_count - 1));
        if (i == -1)
            return 0;
        if (i >= f.mode_count)
            return 0;
        *mode = i;
        fixed (Mode* m = &f.mode_config[i])
        {
            if (m->blockflag != 0)
            {
                n = f.blocksize_1;
                prev = (int)get_bits(f, 1);
                next = (int)get_bits(f, 1);
            }
            else
            {
                prev = next = 0;
                n = f.blocksize_0;
            }

            window_center = n >> 1;
            if (m->blockflag != 0 && prev == 0)
            {
                *p_left_start = (n - f.blocksize_0) >> 2;
                *p_left_end = (n + f.blocksize_0) >> 2;
            }
            else
            {
                *p_left_start = 0;
                *p_left_end = window_center;
            }

            if (m->blockflag != 0 && next == 0)
            {
                *p_right_start = (n * 3 - f.blocksize_0) >> 2;
                *p_right_end = (n * 3 + f.blocksize_0) >> 2;
            }
            else
            {
                *p_right_start = window_center;
                *p_right_end = n;
            }
        }

        return 1;
    }

    public static int vorbis_decode_packet(stb_vorbis f, int* len, int* p_left, int* p_right)
    {
        var mode = 0;
        var left_end = 0;
        var right_end = 0;
        if (vorbis_decode_initial(f, p_left, &left_end, p_right, &right_end, &mode) == 0)
            return 0;
        fixed (Mode* mptr = &f.mode_config[mode])
        {
            return vorbis_decode_packet_rest(f, len, mptr, *p_left, left_end, *p_right, right_end, p_left);
        }
    }

    public static int vorbis_decode_packet_rest(stb_vorbis f, int* len, Mode* mode, int left_start, int left_end,
        int right_start, int right_end, int* p_left)
    {
        Mapping* map;
        var i = 0;
        var j = 0;
        var k = 0;
        var n = 0;
        var n2 = 0;
        var zero_channel = stackalloc int[256];
        var really_zero_channel = stackalloc int[256];
        n = f.blocksize[mode->blockflag];
        map = &f.mapping[mode->mapping];
        n2 = n >> 1;
        for (i = 0; i < f.channels; ++i)
        {
            int s = map->chan[i].mux;
            var floor = 0;
            zero_channel[i] = 0;
            floor = map->submap_floor[s];
            if (f.floor_types[floor] == 0) return error(f, STBVorbisError.VORBIS_invalid_stream);

            var g = &f.floor_config[floor].floor1;
            if (get_bits(f, 1) != 0)
            {
                short* finalY;
                var step2_flag = stackalloc byte[256];
                var range = vorbis_decode_packet_rest_range_list[g->floor1_multiplier - 1];
                var offset = 2;
                finalY = f.finalY[i];
                finalY[0] = (short)get_bits(f, ilog(range) - 1);
                finalY[1] = (short)get_bits(f, ilog(range) - 1);
                for (j = 0; j < g->partitions; ++j)
                {
                    int pclass = g->partition_class_list[j];
                    int cdim = g->class_dimensions[pclass];
                    int cbits = g->class_subclasses[pclass];
                    var csub = (1 << cbits) - 1;
                    var cval = 0;
                    if (cbits != 0)
                    {
                        var c = f.codebooks + g->class_masterbooks[pclass];
                        cval = codebook_decode_scalar(f, c);
                        if (c->sparse != 0)
                            cval = c->sorted_values[cval];
                    }

                    for (k = 0; k < cdim; ++k)
                    {
                        int book = g->subclass_books[pclass * 8 + (cval & csub)];
                        cval = cval >> cbits;
                        if (book >= 0)
                        {
                            var temp = 0;
                            var c = f.codebooks + book;
                            temp = codebook_decode_scalar(f, c);
                            if (c->sparse != 0)
                                temp = c->sorted_values[temp];
                            finalY[offset++] = (short)temp;
                        }
                        else
                        {
                            finalY[offset++] = 0;
                        }
                    }
                }

                if (f.valid_bits == -1)
                {
                    zero_channel[i] = 1;
                    goto error;
                }

                step2_flag[0] = step2_flag[1] = 1;
                for (j = 2; j < g->values; ++j)
                {
                    var low = 0;
                    var high = 0;
                    var pred = 0;
                    var highroom = 0;
                    var lowroom = 0;
                    var room = 0;
                    var val = 0;
                    low = g->neighbors[j * 2 + 0];
                    high = g->neighbors[j * 2 + 1];
                    pred = predict_point(g->Xlist[j], g->Xlist[low], g->Xlist[high], finalY[low], finalY[high]);
                    val = finalY[j];
                    highroom = range - pred;
                    lowroom = pred;
                    if (highroom < lowroom)
                        room = highroom * 2;
                    else
                        room = lowroom * 2;
                    if (val != 0)
                    {
                        step2_flag[low] = step2_flag[high] = 1;
                        step2_flag[j] = 1;
                        if (val >= room)
                            if (highroom > lowroom)
                                finalY[j] = (short)(val - lowroom + pred);
                            else
                                finalY[j] = (short)(pred - val + highroom - 1);
                        else if ((val & 1) != 0)
                            finalY[j] = (short)(pred - ((val + 1) >> 1));
                        else
                            finalY[j] = (short)(pred + (val >> 1));
                    }
                    else
                    {
                        step2_flag[j] = 0;
                        finalY[j] = (short)pred;
                    }
                }

                for (j = 0; j < g->values; ++j)
                    if (step2_flag[j] == 0)
                        finalY[j] = -1;
            }
            else
            {
                zero_channel[i] = 1;
            }

        error:;
        }

        CRuntime.Memcpy(really_zero_channel, zero_channel, (ulong)(sizeof(int) * f.channels));
        for (i = 0; i < map->coupling_steps; ++i)
            if (zero_channel[map->chan[i].magnitude] == 0 || zero_channel[map->chan[i].angle] == 0)
                zero_channel[map->chan[i].magnitude] = zero_channel[map->chan[i].angle] = 0;

        for (i = 0; i < map->submaps; ++i)
        {
            var residue_buffers = stackalloc float*[16];
            var r = 0;
            var do_not_decode = stackalloc byte[256];
            var ch = 0;
            for (j = 0; j < f.channels; ++j)
                if (map->chan[j].mux == i)
                {
                    if (zero_channel[j] != 0)
                    {
                        do_not_decode[ch] = 1;
                        residue_buffers[ch] = null;
                    }
                    else
                    {
                        do_not_decode[ch] = 0;
                        residue_buffers[ch] = f.channel_buffers[j];
                    }

                    ++ch;
                }

            r = map->submap_residue[i];
            decode_residue(f, residue_buffers, ch, n2, r, do_not_decode);
        }

        for (i = map->coupling_steps - 1; i >= 0; --i)
        {
            var n3 = n >> 1;
            var m = f.channel_buffers[map->chan[i].magnitude];
            var a = f.channel_buffers[map->chan[i].angle];
            for (j = 0; j < n3; ++j)
            {
                float a2 = 0;
                float m2 = 0;
                if (m[j] > 0)
                {
                    if (a[j] > 0)
                    {
                        m2 = m[j];
                        a2 = m[j] - a[j];
                    }
                    else
                    {
                        a2 = m[j];
                        m2 = m[j] + a[j];
                    }
                }
                else if (a[j] > 0)
                {
                    m2 = m[j];
                    a2 = m[j] + a[j];
                }
                else
                {
                    a2 = m[j];
                    m2 = m[j] - a[j];
                }

                m[j] = m2;
                a[j] = a2;
            }
        }

        for (i = 0; i < f.channels; ++i)
            if (really_zero_channel[i] != 0)
                CRuntime.Memset(f.channel_buffers[i], 0, (ulong)(sizeof(float) * n2));
            else
                do_floor(f, map, i, n, f.channel_buffers[i], f.finalY[i], null);

        for (i = 0; i < f.channels; ++i) inverse_mdct(f.channel_buffers[i], n, f, mode->blockflag);

        flush_packet(f);
        if (f.first_decode != 0)
        {
            f.current_loc = (uint)(0u - n2);
            f.discard_samples_deferred = n - right_end;
            f.current_loc_valid = 1;
            f.first_decode = 0;
        }
        else if (f.discard_samples_deferred != 0)
        {
            if (f.discard_samples_deferred >= right_start - left_start)
            {
                f.discard_samples_deferred -= right_start - left_start;
                left_start = right_start;
                *p_left = left_start;
            }
            else
            {
                left_start += f.discard_samples_deferred;
                *p_left = left_start;
                f.discard_samples_deferred = 0;
            }
        }
        else if (f.previous_length == 0 && f.current_loc_valid != 0)
        {
        }

        if (f.last_seg_which == f.end_seg_with_known_loc)
        {
            if (f.current_loc_valid != 0 && (f.page_flag & 4) != 0)
            {
                var current_end = f.known_loc_for_packet;
                if (current_end < f.current_loc + (right_end - left_start))
                {
                    if (current_end < f.current_loc)
                        *len = 0;
                    else
                        *len = (int)(current_end - f.current_loc);

                    *len += left_start;
                    if (*len > right_end)
                        *len = right_end;
                    f.current_loc += (uint)*len;
                    return 1;
                }
            }

            f.current_loc = (uint)(f.known_loc_for_packet - (n2 - left_start));
            f.current_loc_valid = 1;
        }

        if (f.current_loc_valid != 0)
            f.current_loc += (uint)(right_start - left_start);
        *len = right_end;
        return 1;
    }

    public static void vorbis_deinit(stb_vorbis p)
    {
        var i = 0;
        var j = 0;

        if (p.residue_config != null)
            for (i = 0; i < p.residue_count; ++i)
            {
                var r = p.residue_config[i];
                if (r.classdata != null)
                {
                    for (j = 0; j < p.codebooks[r.classbook].entries; ++j) CRuntime.Free(r.classdata[j]);

                    CRuntime.Free(r.classdata);
                }
            }

        if (p.codebooks != null)
        {
            for (i = 0; i < p.codebook_count; ++i)
            {
                var c = p.codebooks + i;
                CRuntime.Free(c->codeword_lengths);
                CRuntime.Free(c->multiplicands);
                CRuntime.Free(c->codewords);
                CRuntime.Free(c->sorted_codewords);
                if (c->sorted_values != null) CRuntime.Free(c->sorted_values - 1);
            }

            CRuntime.Free(p.codebooks);
        }

        CRuntime.Free(p.floor_config);
        if (p.mapping != null)
        {
            for (i = 0; i < p.mapping_count; ++i) CRuntime.Free(p.mapping[i].chan);

            CRuntime.Free(p.mapping);
        }

        for (i = 0; i < p.channels && i < 16; ++i)
        {
            CRuntime.Free(p.channel_buffers[i]);
            CRuntime.Free(p.previous_window[i]);
            CRuntime.Free(p.finalY[i]);
        }

        for (i = 0; i < 2; ++i)
        {
            CRuntime.Free(p.A[i]);
            CRuntime.Free(p.B[i]);
            CRuntime.Free(p.C[i]);
            CRuntime.Free(p.window[i]);
            CRuntime.Free(p.bit_reverse[i]);
        }
    }

    public static uint vorbis_find_page(stb_vorbis f, uint* end, uint* last)
    {
        for (; ; )
        {
            var n = 0;
            if (f.eof != 0)
                return 0;
            n = get8(f);
            if (n == 0x4f)
            {
                var retry_loc = stb_vorbis_get_file_offset(f);
                var i = 0;
                if (retry_loc - 25 > f.stream_len)
                    return 0;
                for (i = 1; i < 4; ++i)
                    if (get8(f) != ogg_page_header[i])
                        break;

                if (f.eof != 0)
                    return 0;
                if (i == 4)
                {
                    var header = stackalloc byte[27];
                    uint i2 = 0;
                    uint crc = 0;
                    uint goal = 0;
                    uint len = 0;
                    for (i2 = 0; i2 < 4; ++i2) header[i2] = ogg_page_header[i2];

                    for (; i2 < 27; ++i2) header[i2] = get8(f);

                    if (f.eof != 0)
                        return 0;
                    if (header[4] != 0)
                        goto invalid;
                    goal = (uint)(header[22] + (header[23] << 8) + (header[24] << 16) + ((uint)header[25] << 24));
                    for (i2 = 22; i2 < 26; ++i2) header[i2] = 0;

                    crc = 0;
                    for (i2 = 0; i2 < 27; ++i2) crc = crc32_update(crc, header[i2]);

                    len = 0;
                    for (i2 = 0; i2 < header[26]; ++i2)
                    {
                        int s = get8(f);
                        crc = crc32_update(crc, (byte)s);
                        len += (uint)s;
                    }

                    if (len != 0 && f.eof != 0)
                        return 0;
                    for (i2 = 0; i2 < len; ++i2) crc = crc32_update(crc, get8(f));

                    if (crc == goal)
                    {
                        if (end != null)
                            *end = stb_vorbis_get_file_offset(f);
                        if (last != null)
                        {
                            if ((header[5] & 0x04) != 0)
                                *last = 1;
                            else
                                *last = 0;
                        }

                        set_file_offset(f, retry_loc - 1);
                        return 1;
                    }
                }

            invalid:;
                set_file_offset(f, retry_loc);
            }
        }
    }

    public static int vorbis_finish_frame(stb_vorbis f, int len, int left, int right)
    {
        var prev = 0;
        var i = 0;
        var j = 0;
        if (f.previous_length != 0)
        {
            var i2 = 0;
            var j2 = 0;
            var n = f.previous_length;
            var w = get_window(f, n);
            if (w == null)
                return 0;
            for (i2 = 0; i2 < f.channels; ++i2)
                for (j2 = 0; j2 < n; ++j2)
                    f.channel_buffers[i2][left + j2] = f.channel_buffers[i2][left + j2] * w[j2] +
                                                       f.previous_window[i2][j2] * w[n - 1 - j2];
        }

        prev = f.previous_length;
        f.previous_length = len - right;
        for (i = 0; i < f.channels; ++i)
            for (j = 0; right + j < len; ++j)
                f.previous_window[i][j] = f.channel_buffers[i][right + j];

        if (prev == 0)
            return 0;
        if (len < right)
            right = len;
        f.samples_output += (uint)(right - left);
        return right - left;
    }

    public static void vorbis_init(stb_vorbis p)
    {
        p.eof = 0;
        p.error = STBVorbisError.VORBIS__no_error;
        p.stream = null;
        p.codebooks = null;
        p.page_crc_tests = -1;
    }

    public static int vorbis_pump_first_frame(stb_vorbis f)
    {
        var len = 0;
        var right = 0;
        var left = 0;
        var res = 0;
        res = vorbis_decode_packet(f, &len, &left, &right);
        if (res != 0)
            vorbis_finish_frame(f, len, left, right);
        return res;
    }

    public static int vorbis_search_for_page_pushdata(stb_vorbis f, byte* data, int data_len)
    {
        var i = 0;
        var n = 0;
        for (i = 0; i < f.page_crc_tests; ++i) f.scan[i].bytes_done = 0;

        if (f.page_crc_tests < 4)
        {
            if (data_len < 4)
                return 0;
            data_len -= 3;
            for (i = 0; i < data_len; ++i)
                if (data[i] == 0x4f)
                    if (0 == CRuntime.Memcmp(data + i, ogg_page_header, 4))
                    {
                        var j = 0;
                        var len = 0;
                        uint crc = 0;
                        if (i + 26 >= data_len || i + 27 + data[i + 26] >= data_len)
                        {
                            data_len = i;
                            break;
                        }

                        len = 27 + data[i + 26];
                        for (j = 0; j < data[i + 26]; ++j) len += data[i + 27 + j];

                        crc = 0;
                        for (j = 0; j < 22; ++j) crc = crc32_update(crc, data[i + j]);

                        for (; j < 26; ++j) crc = crc32_update(crc, 0);

                        n = f.page_crc_tests++;
                        f.scan[n].bytes_left = len - j;
                        f.scan[n].crc_so_far = crc;
                        f.scan[n].goal_crc = (uint)(data[i + 22] + (data[i + 23] << 8) + (data[i + 24] << 16) +
                                                     (data[i + 25] << 24));
                        if (data[i + 27 + data[i + 26] - 1] == 255)
                            f.scan[n].sample_loc = uint.MaxValue;
                        else
                            f.scan[n].sample_loc = (uint)(data[i + 6] + (data[i + 7] << 8) + (data[i + 8] << 16) +
                                                           (data[i + 9] << 24));
                        f.scan[n].bytes_done = i + j;
                        if (f.page_crc_tests == 4)
                            break;
                    }
        }

        for (i = 0; i < f.page_crc_tests;)
        {
            uint crc = 0;
            var j = 0;
            var n2 = f.scan[i].bytes_done;
            var m = f.scan[i].bytes_left;
            if (m > data_len - n2)
                m = data_len - n2;
            crc = f.scan[i].crc_so_far;
            for (j = 0; j < m; ++j) crc = crc32_update(crc, data[n2 + j]);

            f.scan[i].bytes_left -= m;
            f.scan[i].crc_so_far = crc;
            if (f.scan[i].bytes_left == 0)
            {
                if (f.scan[i].crc_so_far == f.scan[i].goal_crc)
                {
                    data_len = n2 + m;
                    f.page_crc_tests = -1;
                    f.previous_length = 0;
                    f.next_seg = -1;
                    f.current_loc = f.scan[i].sample_loc;
                    f.current_loc_valid = f.current_loc != ~0U ? 1 : 0;
                    return data_len;
                }

                f.scan[i] = f.scan[--f.page_crc_tests];
            }
            else
            {
                ++i;
            }
        }

        return data_len;
    }

    public static int vorbis_validate(byte* data)
    {
        return CRuntime.Memcmp(data, vorbis_validate_vorbis, 6) == 0 ? 1 : 0;
    }
}

#pragma warning restore CS0162, CS8618, CA2014, CS8625, CS0649