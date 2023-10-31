﻿using System;

namespace FlatStage.Utils;
/// <summary>
/// Thread safe pool of objects
/// Example usage: new Pool<MyClass>(() => new MyClass(), 35);
/// </summary>
public class Pool<T> where T : class
{
    public delegate T CreationDelegate();

    private readonly CreationDelegate m_create;

    private T[] m_items;
    private int m_maxPooledItems;

    private int m_itemsInPoolCount;
    public int Count { get { return m_itemsInPoolCount; } }

    public Pool(CreationDelegate creator, int maxPoolItems)
    {
        m_items = new T[maxPoolItems < 32 ? maxPoolItems : 32];
        m_maxPooledItems = maxPoolItems;
        m_create = creator;
    }

    /// <summary>
    /// Clears pool and all references to the objects in it
    /// </summary>
    public void Clear()
    {
        lock (m_create)
        {
            m_items.AsSpan().Clear();
            m_itemsInPoolCount = 0;
        }
    }

    /// <summary>
    /// Returns an item from the pool if available, otherwise creates one and returns it
    /// </summary>
    public T Get()
    {
        lock (m_create)
        {
            var n = m_itemsInPoolCount;
            if (n > 0)
            {
                n--;
                var retval = m_items[n];
                m_items[n] = default!;
                m_itemsInPoolCount = n;
                return retval;
            }
        }

        // no item in pool
        return m_create();
    }

    /// <summary>
    /// Returns true if item was returned to pool; false if the pool was full and not added
    /// </summary>
    public bool Return(T item)
    {
        lock (m_create)
        {
            var n = m_itemsInPoolCount;
            if (n == m_maxPooledItems)
                return false; // pool is full

            int curStoreSize = m_items.Length;
            if (n >= curStoreSize)
            {
                // grow
                var newItems = new T[Math.Min(curStoreSize * 2, m_maxPooledItems)];
                m_items.AsSpan().CopyTo(newItems);
                m_items = newItems;
            }
            m_items[n] = item;
            m_itemsInPoolCount = n + 1;
            return true;
        }
    }
}
