using System.Diagnostics;
using MemoryPack;
using MemoryPack.Compression;

namespace FlatStage;
public static class GameSaveIO
{
    public static string? RootPath { get; private set; }

    private static string? _currentSaveId;
    private static readonly GameSaveData? _currentWriteData;
    private static GameSaveData? _currentReadData;
    private static bool _writeReady;

    static GameSaveIO()
    {
        _currentWriteData = new GameSaveData()
        {
            Id = string.Empty,
            FloatValues = new Dictionary<string, float>(),
            IntValues = new Dictionary<string, int>(),
            StringValues = new Dictionary<string, string>(),
        };
    }

    internal static void SetRootPath(string rootPath)
    {
        RootPath = rootPath;
    }

    public static void BeginWrite(string id)
    {
        _currentSaveId = id;
        Debug.Assert(_currentWriteData != null, nameof(_currentWriteData) + " != null");

        _currentWriteData.Id = id;
        _currentWriteData.StringValues.Clear();
        _currentWriteData.IntValues.Clear();
        _currentWriteData.FloatValues.Clear();

        _writeReady = true;

    }

    public static void Set(string key, string value)
    {
        CheckWrite();

        Debug.Assert(_currentWriteData != null, nameof(_currentWriteData) + " != null");
        _currentWriteData.StringValues[key] = value;
    }

    public static void Set(string key, int value)
    {
        CheckWrite();

        Debug.Assert(_currentWriteData != null, nameof(_currentWriteData) + " != null");
        _currentWriteData.IntValues[key] = value;
    }

    public static void Set(string key, float value)
    {
        CheckWrite();

        Debug.Assert(_currentWriteData != null, nameof(_currentWriteData) + " != null");
        _currentWriteData.FloatValues[key] = value;
    }

    public static void EndWrite()
    {
        _writeReady = false;

        if (!Path.Exists(RootPath))
        {
            Debug.Assert(RootPath != null, nameof(RootPath) + " != null");
            Directory.CreateDirectory(RootPath);
        }

        var saveFilePath = Path.Combine(RootPath, _currentSaveId + ContentProperties.BinaryExt);

        var copyPath = string.Empty;

        if (File.Exists(saveFilePath))
        {
            copyPath = Path.Combine(Path.ChangeExtension(saveFilePath, ".bak"));

            File.Copy(saveFilePath, copyPath);

            File.Delete(saveFilePath);
        }

        try
        {
            using var stream = File.Open(saveFilePath, FileMode.Create);

            using var compressor = new BrotliCompressor();

            MemoryPackSerializer.Serialize(in compressor, in _currentWriteData);

            using var writer = new BinaryWriter(stream);

            writer.Write(compressor.ToArray());

            writer.Close();
        }
        catch (Exception)
        {
            if (!string.IsNullOrEmpty(copyPath) && File.Exists(copyPath))
            {
                File.Copy(copyPath, Path.ChangeExtension(copyPath, ContentProperties.BinaryExt));
            }

            throw;
        }
        finally
        {
            if (File.Exists(copyPath))
            {
                File.Delete(copyPath);
            }
        }
    }

    public static bool LoadSave(string id)
    {
        Debug.Assert(RootPath != null, nameof(RootPath) + " != null");
        var savePath = Path.Combine(RootPath, id + ContentProperties.BinaryExt);

        if (!File.Exists(savePath))
        {
            return false;
        }

        using var stream = File.OpenRead(savePath);

        using var reader = new BinaryReader(stream);

        var buffer = reader.ReadBytes((int)stream.Length);

        using var decompressor = new BrotliDecompressor();

        var decompressedBuffer = decompressor.Decompress(buffer);

        _currentReadData = MemoryPackSerializer.Deserialize<GameSaveData>(decompressedBuffer) ?? throw new Exception("Failed to read GameSave");

        return true;
    }

    public static string GetString(string key)
    {
        if (_currentReadData == null)
        {
            throw new InvalidOperationException("No Save State Loaded");
        }

        if (_currentReadData.StringValues.TryGetValue(key, out var value))
        {
            return value;
        }

        throw new Exception($"Could not find value with key: {key}");
    }

    public static int GetInteger(string key)
    {
        if (_currentReadData == null)
        {
            throw new InvalidOperationException("No Save State Loaded");
        }

        if (_currentReadData.IntValues.TryGetValue(key, out var value))
        {
            return value;
        }

        throw new Exception($"Could not find value with key: {key}");
    }

    public static float GetFloat(string key)
    {
        if (_currentReadData == null)
        {
            throw new InvalidOperationException("No Save State Loaded");
        }

        if (_currentReadData.FloatValues.TryGetValue(key, out var value))
        {
            return value;
        }

        throw new Exception($"Could not find value with key: {key}");
    }

    private static void CheckWrite()
    {
        if (!_writeReady)
        {
            throw new InvalidOperationException("Can't write data into save state without calling BeginWrite first");
        }
    }

}
