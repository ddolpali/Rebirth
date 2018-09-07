﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Common.Game
{
    public abstract class CObjectPool<TKey, TValue> : IEnumerable<TValue>
    {
        private int m_uidBase = 10000;
        private readonly Dictionary<TKey, TValue> m_cache;

        public int Count => m_cache.Count;

        protected CObjectPool()
        {
            m_cache = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            m_cache.Add(key,value);
        }
        public bool Remove(TKey key)
        {
            return m_cache.Remove(key);
        }
        public void Clear()
        {
            m_cache.Clear();
        }
        public TValue Get(TKey key)
        {
            return m_cache[key];
        }

        protected int GetUniqueId()
        {
            return Interlocked.Increment(ref m_uidBase);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return m_cache.Values.ToList().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
