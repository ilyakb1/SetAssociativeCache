using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NWaySetAssociativeCache.Core.ReplacementAlgorithm
{
    public class MostRecentlyUsedReplacementAlgorithm<TKey, TValue> : ICache<TKey, TValue>
    {
        readonly Dictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>> uniqueList = new Dictionary<TKey, LinkedListNode<CacheItem<TKey, TValue>>>();

        readonly LinkedList<CacheItem<TKey, TValue>> cacheLinkedList = new LinkedList<CacheItem<TKey, TValue>>();

        readonly object stateGuard = new object();

        public MostRecentlyUsedReplacementAlgorithm(int capacity)
        {
            Contract.Requires(capacity > 1);
            Capacity = capacity;
        }

        public int Capacity { get; }

        public bool TryGet(TKey key, out TValue value)
        {
            value = default(TValue);
            if (key == null) return false;

            lock (stateGuard)
            {
                LinkedListNode<CacheItem<TKey, TValue>> node;
                if (!uniqueList.TryGetValue(key, out node)) return false;

                value = node.Value.Value;
                cacheLinkedList.Remove(node);
                cacheLinkedList.AddLast(node);
                return true;
            }
        }

        public void Clear()
        {
            lock (stateGuard)
            {
                uniqueList.Clear();
                cacheLinkedList.Clear();
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) return;

            lock (stateGuard)
            {
                if (uniqueList.ContainsKey(key)) return;
                if (uniqueList.Count >= Capacity) RemoveLast();

                var cacheItem = new CacheItem<TKey, TValue>(key, value);
                var node = new LinkedListNode<CacheItem<TKey, TValue>>(cacheItem);
                cacheLinkedList.AddLast(node);
                uniqueList.Add(key, node);
            }
        }

        void RemoveLast()
        {
            var node = cacheLinkedList.Last;
            cacheLinkedList.RemoveLast();
            if (node.Value.Key != null) uniqueList.Remove(node.Value.Key);
        }
    }
}
