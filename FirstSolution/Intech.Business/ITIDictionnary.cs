using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    class ITIDictionnary<TKey, TValue>
    {
        int _count;
        Bucket[] _buckets;

        class Bucket
        {
            public readonly TKey Key;
            public TValue Value;
            public Bucket Next;
            public Bucket(TKey k, TValue v, Bucket next)
            {
                Key = k;
                Value = v;
                Next = next;
            }
        }

        public ITIDictionnary()
        {
            _buckets = new Bucket[11];
            for (int i = 0; i < 11; i++)
                _buckets[i] = null;
            _count = 0;
        }

        public int Count
        {
            get { return _count; }
        }

        public bool Remove(TKey key)
        {
            int slot = Math.Abs(key.getHashCode() % _buckets.Length);
            Bucket b = _buckets[slot];

            if (b != null)
            {
                Bucket oldB = b;
                do
                {
                    if (b.Key == key)
                    {
                        oldB.Next = b.Next;
                        --_count;
                        return true;
                    }
                    oldB = b;
                    b = b.Next;
                } while (b.Next != null);
            }

            return false;
        }

        public void Add(TKey key, TValue value)
        {
            AddOrReplace(key, value, true);
        }

        public void AddOrReplace(TKey key, TValue value, bool add)
        {
            int h = value.GetHashCode();
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];
            if (b == null)
            {
                _buckets[slot] = new Bucket(key, value, null);
            }
            else
            {
                int cbCount = 0;
                do
                {
                    if (EqualityComparer<TKey>.Default.Equals(b.Key, key))
                    {
                        if (add)
                            throw new InvalidOperationException("exist déjà connard");
                        
                        b.Value = value;
                        return;
                    }
                    b = b.Next;
                    cbCount++;
                } while (b != null);
                b = new Bucket(key, value, _buckets[slot]);

                if (cbCount == 5)
                {
                    // Reallocate
                    Bucket[] newBuckets = new Bucket[_buckets.Length+5];
                    Array.Copy(_buckets, newBuckets, _buckets.Length);
                    _buckets = newBuckets;
                    slot = Math.Abs(h % _buckets.Length);
                }

                _buckets[slot] = b;
            }
            ++_count;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int h = key.GetHashCode();
            int slot = Math.Abs(h % _buckets.Length);
            Bucket b = _buckets[slot];

            while (b != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(b.Key, key))
                {
                    value = b.Value;
                    return true;
                }
                b = b.Next;
            }

            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue v;
                if (!TryGetValue(key, out v))
                {
                    throw new KeyNotFoundException();
                }
                return v;
            }

            set
            {
                AddOrReplace(key, value, false);
            }
        }
    }
}
