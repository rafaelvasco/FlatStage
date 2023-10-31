using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FlatStage;
/// <summary>
/// List of T alternative with the following properties
/// * Underlying array is accessible (remember to respect m_offset and Count property)
/// * ref return methods
/// * pop/trypop
/// * Very little validation; except in DEBUG
/// * Best iterated over myFastList.ReadOnlySpan
/// </summary>
[DebuggerDisplay("FastList<{typeof(T).Name}> {Count}/{Capacity}")]

public partial class FastList<T>
{
    private T[] m_buffer = System.Array.Empty<T>();
    private int m_offset;
    private int m_count;

    public int Count
    {
        get { return m_count; }
        set
        {
            FlatException.Assert(value <= m_buffer.Length);
            m_count = value;
            if (m_offset + m_count > m_buffer.Length)
                Compact(); // ensure Span can operate
        }
    }

    public int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get { return m_buffer.Length; }
        [MethodImpl(MethodImplOptions.NoInlining)]
        set
        {
            if (value == 0)
            {
                m_buffer = System.Array.Empty<T>();
                m_offset = 0;
                m_count = 0;
                return;
            }

            if (value == m_buffer.Length)
                return;

            var newItems = new T[value];
            m_count = Math.Min(m_count, value);
            if (m_count > 0)
                ReadOnlySpan.Slice(0, m_count).CopyTo(newItems);
            m_buffer = newItems;
            m_offset = 0;
        }
    }

    // cold path, never inline
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Compact()
    {
        FlatException.Assert(m_offset > 0, "Unnecessary compact!");
        var buffer = m_buffer;
        var off = m_offset;
        var cnt = m_count;
        // TODO: compare to aliased Span.CopyTo
        for (int i = 0; i < cnt; i++)
            buffer[i] = buffer[off + i];
        m_offset = 0;
    }

    /// <summary>
    /// May compact buffer if it's offset
    /// </summary>
    public T[] GetBuffer()
    {
        if (m_offset != 0)
            Compact();
        return m_buffer;
    }

    /// <summary>
    /// Replace backing items array and set count
    /// </summary>
    public void ReplaceBuffer(T[] items, int count)
    {
        m_buffer = items;
        m_offset = 0;
        m_count = count;
    }

    /// <summary>
    /// Get readonly span of items in list
    /// </summary>
    public ReadOnlySpan<T> ReadOnlySpan => new ReadOnlySpan<T>(m_buffer, m_offset, m_count);

    /// <summary>
    /// Get span of items in list
    /// </summary>
    public Span<T> Span => new Span<T>(m_buffer, m_offset, m_count);

    /// <summary>
    /// Get memory span of items in list
    /// </summary>
    public Memory<T> Memory => new Memory<T>(m_buffer, m_offset, m_count);

    public FastList(int initialCapacity = 0)
    {
        if (initialCapacity == 0)
            m_buffer = System.Array.Empty<T>();
        else
            m_buffer = new T[initialCapacity];
        m_count = 0;
        m_offset = 0;
    }

    public FastList(ReadOnlySpan<T> copyItems)
    {
        m_buffer = copyItems.ToArray();
        m_count = copyItems.Length;
        m_offset = 0;
    }

    public FastList(IEnumerable<T> copyItems)
    {
        // loop over this twice to avoid unnecessary garbage
        int cnt = 0;
        foreach (var _ in copyItems)
            cnt++;
        m_count = 0;
        m_offset = 0;
        if (cnt == 0)
        {
            m_buffer = System.Array.Empty<T>();
        }
        else
        {
            m_buffer = new T[cnt];
            AddRangeSlow(copyItems);
        }
    }

    private FastList()
    {
    }

    public static FastList<T> CreateFromBackingArray(T[] arr, int initialCount)
    {
        var retval = new FastList<T>();
        retval.m_count = initialCount;
        retval.m_buffer = arr;
        retval.m_offset = 0;
        return retval;
    }

    public static implicit operator ReadOnlySpan<T>(FastList<T> list)
    {
        return list.ReadOnlySpan;
    }
}

// FastList ADD

public partial class FastList<T>
{
    // cold path, never inline
    [MethodImpl(MethodImplOptions.NoInlining)]
    private T[] Grow(int minAddCount)
    {
        // Don't grow by doubling; only by +50% of current
        // Downside: need to trust user to call EnsureCapacity and use initialCapacity wisely for good perf
        // Upside: less memory waste
        int bufLen = m_buffer.Length;
        int newSize = 4 + Math.Max(bufLen + bufLen / 2, m_count + minAddCount);
        var old = this.ReadOnlySpan;
        var newBuffer = new T[newSize];
        old.CopyTo(newBuffer);
        m_offset = 0;
        m_buffer = newBuffer;
        return newBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(int toAddItemsCount)
    {
        if (m_count + toAddItemsCount > m_buffer.Length)
            Grow(toAddItemsCount);
    }

    // cold path, never inline
    [MethodImpl(MethodImplOptions.NoInlining)]
    private T[] Grow()
    {
        var oldLen = m_buffer.Length;
        var newLength = oldLen == 0 ? 4 : oldLen * 2;
        var newBuffer = new T[newLength];

        var old = this.ReadOnlySpan;
        old.CopyTo(newBuffer);

        m_buffer = newBuffer;
        m_offset = 0;
        return newBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(in T item)
    {
        ref var loc = ref AddUninitialized();
        loc = item;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Add()
    {
        ref var loc = ref AddUninitialized();
        loc = default(T);
        return ref loc!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T AddUninitialized()
    {
        int count = m_count;
        var buffer = m_buffer;
        if (count == buffer.Length)
            buffer = Grow();
        int idx = m_offset + count;
        if (idx >= buffer.Length)
        {
            Compact();
            idx = count;
        }
        m_count = count + 1;
        return ref buffer[idx];
    }

    /// <summary>
    /// Reserves items at end of list and returns span
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AddRange(int numItems)
    {
        var span = AddRangeUninitialized(numItems);
        span.Clear();
        return span;
    }

    /// <summary>
    /// Reserves items at end of list and returns span; may contain uninitialized values
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AddRangeUninitialized(int numItems)
    {
        var buffer = m_buffer;
        int cnt = m_count;
        if (cnt + numItems >= buffer.Length)
            buffer = Grow(numItems);
        if (m_offset + cnt + numItems > buffer.Length)
            Compact();
        var retval = new Span<T>(buffer, m_offset + cnt, numItems);
        m_count = cnt + numItems;
        return retval;
    }

    /// <summary>
    /// Adds items to end of list
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddRange(ReadOnlySpan<T> addItems)
    {
        var span = AddRangeUninitialized(addItems.Length);
        addItems.CopyTo(span);
    }

    /// <summary>
    /// Adds items to end of list; using IEnumerable (much slower than adding spans)
    /// </summary>
    public void AddRangeSlow(IEnumerable<T> addItems)
    {
        foreach (var item in addItems)
            Add(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, in T value)
    {
        ref var destination = ref InsertUninitialized(index);
        destination = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T InsertUninitialized(int index)
    {
        int count = m_count;
        FlatException.Assert(index <= count);
        var buffer = m_buffer;
        if (count >= buffer.Length)
            buffer = Grow();
        var offset = m_offset;
        if (offset > 0)
        {
            if (index == 0)
            {
                // yay
                m_count = count + 1;
                offset = offset - 1;
                m_offset = offset;
                return ref buffer[offset];
            }

            if (offset + count == buffer.Length)
            {
                // need to compact
                Compact();
                offset = 0;
            }
        }

        int bufferIndex = offset + index;

        int copyLen = count - index;
        if (copyLen > 0)
        {
            // push forward
            var cache = buffer[bufferIndex];
            for (int i = 0; i < copyLen; i++)
            {
                // swap tmp and destination
                int dstBufIdx = bufferIndex + i + 1;
                var tmp = buffer[dstBufIdx];
                buffer[dstBufIdx] = cache;
                cache = tmp;
            }
        }
        m_count = count + 1;
        return ref m_buffer[bufferIndex];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Insert(int index)
    {
        ref T destination = ref InsertUninitialized(index);
        destination = default!;
        return ref destination!;
    }

    public void InsertRange(int index, ReadOnlySpan<T> items)
    {
        var buf = m_buffer;
        var count = m_count;
        if (count + items.Length > buf.Length)
        {
            // need to allocate memory to fit
            InsertRange_Allocate(index, items);
            return;
        }

        if (m_offset + count + items.Length > buf.Length)
            Compact(); // need to compact to fit

        // ok, we should fit in current size/offset

        // push tail
        var tailLen = count - index;
        if (tailLen > 0)
        {
            var tail = buf.AsSpan(m_offset + index, tailLen);
            var tailDst = buf.AsSpan(m_offset + index + items.Length, tailLen);
            tail.CopyTo(tailDst);
        }

        // insert
        items.CopyTo(buf.AsSpan(m_offset + index, items.Length));
        m_count = count + items.Length;
    }

    // InsertRange() when we need to reallocate entire array
    private void InsertRange_Allocate(int index, ReadOnlySpan<T> items)
    {
        var oldBuffer = m_buffer;
        var count = m_count;
        int oldLen = oldBuffer.Length;
        int newSize = 4 + Math.Max(oldLen + oldLen / 2, count + items.Length);
        var newBuffer = new T[newSize];

        // pre
        if (index > 0)
        {
            var pre = oldBuffer.AsSpan(m_offset, index);
            pre.CopyTo(newBuffer.AsSpan(0, index));
        }

        // insert
        items.CopyTo(newBuffer.AsSpan(index, items.Length));

        // tail
        var tailLen = count - index;
        if (tailLen > 0)
        {
            var tail = oldBuffer.AsSpan(m_offset + index, tailLen);
            tail.CopyTo(newBuffer.AsSpan(index + items.Length, tailLen));
        }

        m_buffer = newBuffer;
        m_offset = 0;
        m_count = count + items.Length;
    }

    /// <summary>
    /// Adds 'numItems' copies of 'item' to list
    /// </summary>
    public void Fill(int numItems, in T item)
    {
        var span = AddRangeUninitialized(numItems);
        span.Fill(item);
    }
}

// FastList REMOVE

public partial class FastList<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            this.Span.Clear();
        m_count = 0;
        m_offset = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(int minimumCapacity)
    {
        if (m_buffer.Length < minimumCapacity)
            m_buffer = new T[minimumCapacity];
        else if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            this.Span.Clear();
        m_count = 0;
        m_offset = 0;
    }

    /// <summary>
    /// Removes first instance of item in list; returns true if found and removed
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(in T item)
    {
        var idx = IndexOf(item);
        if (idx == -1)
            return false;
        RemoveAt(idx);
        return true;
    }

    /// <summary>
    /// Removes first instance of item in list; returns true if found and removed; does NOT maintain order of list after removal
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool RemoveUnordered(in T item)
    {
        var idx = IndexOf(item);
        if (idx == -1)
            return false;
        RemoveAtUnordered(idx);
        return true;
    }

    /// <summary>
    /// Removes item at index; maintaining list order
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveAt(int index)
    {
        FlatException.Assert(index >= 0 && index < m_count);

        int offset = m_offset;
        int countAfterRemove = m_count - 1;
        m_count = countAfterRemove;

        if (index == 0)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                m_buffer[offset] = default(T)!;
            if (countAfterRemove == 0)
                m_offset = 0; // effectively a clear
            else
                m_offset = offset + 1;
            return;
        }

        int copyLen = countAfterRemove - index;
        if (copyLen > 0)
        {
            var buffer = m_buffer;
            for (int i = 0; i < copyLen; i++)
            {
                int c = offset + index + i;
                buffer[c] = buffer[c + 1];
            }
        }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            m_buffer[offset + countAfterRemove] = default(T)!;
    }

    /// <summary>
    /// Removes item at index; not maintaining list order
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveAtUnordered(int index)
    {
        FlatException.Assert(index >= 0 && index < m_count);

        int offset = m_offset;
        int countAfterRemove = m_count--;

        if (index == 0)
        {
            if (countAfterRemove == 0)
                m_offset = 0; // effectively a clear
            else
                m_offset = offset + 1;
            return;
        }

        // simply swap last item into place of removed item
        m_buffer[offset + index] = m_buffer[offset + countAfterRemove];

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            m_buffer[offset + countAfterRemove] = default!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveRange(int index, int count)
    {
        int localSpanCount = m_count;

        if (index + count > localSpanCount)
            FlatException.Throw("Bad range");

        if (count == 0)
            return;

        // clear out the data if necessary
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            Span.Slice(index, count).Clear();

        m_count -= count;

        if (count == localSpanCount)
        {
            // removing entire span
            m_offset = 0;
            return;
        }

        if (index + count == localSpanCount)
        {
            // removing a tailing part of span
            return;
        }

        if (index == 0)
        {
            // removing first part of span
            m_offset += count;
            return;
        }

        // removing a middle section of span
        // contract latter part
        int copyLen = localSpanCount - (index + count);
        if (copyLen > 0)
        {
            var buffer = m_buffer;
            var offset = m_offset + index;
            for (int i = 0; i < copyLen; i++)
            {
                int c = offset + i;
                buffer[c] = buffer[c + count];
            }
        }
    }

    /// <summary>
    /// Returns all items matching predicate; returns number of items removed
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int RemoveAll(Predicate<T> match)
    {
        int offset = m_offset;
        int count = m_count;

        int idx = 0;
        while (idx < count && !match(m_buffer[offset + idx]))
            idx++;

        if (idx >= count)
            return 0;

        int i = idx + 1;
        while (i < count)
        {
            while (i < count && match(m_buffer[offset + i]))
                i++;
            if (i < count)
                m_buffer[offset + idx++] = m_buffer[offset + i++];
        }
        Array.Clear(m_buffer, offset + idx, count - idx);
        int result = count - idx;
        m_count = idx;
        return result;
    }

    /// <summary>
    /// Creates a new FastList with all matching items; or NULL if nothing matches
    /// </summary>
    public FastList<T>? Find(Predicate<T> match)
    {
        FastList<T>? retval = null;

        int offset = m_offset;
        int count = m_count;

        int idx = 0;
        while (idx < count)
        {
            if (match(m_buffer[offset + idx]) == true)
            {
                if (retval is null)
                    retval = new FastList<T>(count - idx);
                retval.Add(m_buffer[offset + idx]);
            }
            idx++;
        }
        return retval;
    }
}

// FastList JSON

public class JsonConverterFactoryForFastListOfT : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsGenericType
        && typeToConvert.GetGenericTypeDefinition() == typeof(FastList<>);

    public override JsonConverter CreateConverter(
        Type typeToConvert, JsonSerializerOptions options)
    {
        FlatException.Assert(typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(FastList<>));

        Type elementType = typeToConvert.GetGenericArguments()[0];

        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
            typeof(JsonConverterForFastListOfT<>)
                .MakeGenericType(new Type[] { elementType }),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: null,
            culture: null)!;

        return converter;
    }
}

public class JsonConverterForFastListOfT<T> : JsonConverter<FastList<T>>
{
    public override FastList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }
        reader.Read();

        var elements = new FastList<T>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            elements.Add(JsonSerializer.Deserialize<T>(ref reader, options)!);
            reader.Read();
        }

        return elements;
    }

    public override void Write(Utf8JsonWriter writer, FastList<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        var span = value.ReadOnlySpan;
        foreach (ref readonly var item in span)
            JsonSerializer.Serialize(writer, item, options);

        writer.WriteEndArray();
    }
}

// FastList ACCESS

public partial class FastList<T>
{
    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref m_buffer[m_offset + index];
    }

    public ref T this[uint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref m_buffer[m_offset + index];
    }

    /// <summary>
    /// Creates a new array; use this.Items for in-place access
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray()
    {
        if (m_count == 0)
            return System.Array.Empty<T>();
        return ReadOnlySpan.ToArray();
    }

    /// <summary>
    /// Returns and removes the last item in the list; returns false if list contains no items
    /// </summary>
    public bool TryPop(out T result)
    {
        var cnt = m_count;
        if (cnt == 0)
        {
            result = default!;
            return false;
        }

        var index = m_offset + cnt - 1;
        result = m_buffer[index];
        m_buffer[index] = default!;
        cnt--;
        if (cnt == 0)
            m_offset = 0;
        m_count = cnt;
        return true;
    }

    /// <summary>
    /// Returns and removes the last item in the list; throws if empty
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Pop()
    {
        var index = m_offset + m_count - 1;
        var result = m_buffer[index];
        m_buffer[index] = default!;
        m_count--;
        return result;
    }

    /// <summary>
    /// If count > 0; removes first element in list, putting it in 'item' and returns true
    /// </summary>
    public bool TryDequeue(out T item)
    {
        if (m_count == 0)
        {
            item = default!;
            return false;
        }
        item = m_buffer[m_offset];
        m_offset++;
        m_count--;
        if (m_count == 0)
            m_offset = 0;
        return true;
    }

    /// <summary>
    /// Copy items of list to destination; throws if destination too small
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<T> destination)
    {
        ReadOnlySpan.CopyTo(destination);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item)
    {
        return Array.IndexOf(m_buffer, item, m_offset, m_count) != -1;
        //return ReadOnlySpan.Contains(item);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(in T item)
    {
        //if (item is IEquatable<T>)
        //	return ReadOnlySpan.IndexOf(item);
        //else
        return Array.IndexOf(m_buffer, item, m_offset, m_count) - m_offset;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int LastIndexOf(in T item)
    {
        int off = m_offset;
        int cnt = m_count;

        return Array.LastIndexOf(m_buffer, item, m_offset + m_count - 1, m_count) - m_offset;
        //return ReadOnlySpan.LastIndexOf(item);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty() { return m_count == 0; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reverse()
    {
        if (m_count != 0)
            Array.Reverse<T>(m_buffer, m_offset, m_count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Sort()
    {
        this.Span.Sort();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Sort(Comparison<T> comparison)
    {
        this.Span.Sort(comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Sort(IComparer<T> comparer)
    {
        this.Span.Sort(comparer);
    }
}

