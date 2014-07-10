using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class ITIDictionary<TKey,TValue> : IEnumerable<KeyValuePair<TKey,TValue>>
    {
        int _count;
        Bucket[] _buckets;
        IEqualityComparer<TKey> _strategy;

        static int[] _primeNumbers = new int[] { 11, 23, 47, 97, 199, 397, 809 };

        class Bucket
        {
            public readonly TKey Key;
            public TValue Value;
            public Bucket Next;

            public Bucket( TKey key, TValue v, Bucket next )
            {
                Key = key;
                Value = v;
                Next = next;
            }
        }

        public ITIDictionary()
        {
            _strategy = EqualityComparer<TKey>.Default;
            _buckets = new Bucket[11];
        }

        public ITIDictionary( IEqualityComparer<TKey> strategy )
        {
            if( strategy == null ) throw new ArgumentNullException( "strategy" );
            _strategy = strategy;
            _buckets = new Bucket[11];
        }

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TValue> Values
        {
            get { return new EBase<TValue>( this, b => b.Value ); }
        }

        public IEnumerable<TKey> Keys
        {
            get { return new EBase<TKey>( this, delegate( Bucket b ) { return b.Key; } ); }
        }

        public void Add( TKey key, TValue value )
        {
            AddOrReplace( key, value, true );
        }

        bool AddOrReplace( TKey key, TValue value, bool add )
        {
            int h = _strategy.GetHashCode( key );
            int slot = Math.Abs( h % _buckets.Length );
            Bucket b = _buckets[slot];
            if( b == null )
            {
                AddNewBucket( slot, key, value );
            }
            else
            {
                do
                {
                    if( _strategy.Equals( key, b.Key ) )
                    {
                        if( add ) throw new InvalidOperationException( "Dans ta face." );
                        b.Value = value;
                        // It could be useful to know if the value has been
                        // added or updated...
                        return false;
                    }
                    b = b.Next;
                }
                while( b != null );
                AddNewBucket( slot, key, value );
            }
            return true;
        }

        Bucket AddNewBucket( int slot, TKey key, TValue value )
        {
            ++_count;
            Bucket b = new Bucket( key, value, _buckets[slot] );
            _buckets[slot] = b;
            // Should I grow ?
            int avgCount = _count / _buckets.Length;
            if( avgCount > 3 )
            {
                Grow();
            }
            return b;
        }

        private void Grow()
        {
            int nextLength = _primeNumbers[Array.IndexOf( _primeNumbers, _buckets.Length ) + 1];
            Bucket[] newBuckets = new Bucket[nextLength];
            for( int i = 0; i < _buckets.Length; ++i )
            {
                Bucket b = _buckets[i];
                while( b != null )
                {
                    int newSlot = Math.Abs( _strategy.GetHashCode( b.Key ) % newBuckets.Length );
                    var oldNext = b.Next;
                    b.Next = newBuckets[newSlot];
                    newBuckets[newSlot] = b;
                    b = oldNext;
                }
            }
            _buckets = newBuckets;
        }

        public bool Remove( TKey key )
        {
            int slot = Math.Abs( _strategy.GetHashCode( key ) % _buckets.Length );
            Bucket b = _buckets[slot];
            Bucket previous = null;
            while( b != null )
            {
                if( _strategy.Equals( b.Key, key ) )
                {
                    if( previous != null ) previous.Next = b.Next;
                    else _buckets[slot] = b.Next;
                    --_count;
                    return true;
                }
                previous = b;
                b = b.Next;
            }
            return false;
        }

        public bool TryGetValue( TKey key, out TValue value )
        {
            int h = _strategy.GetHashCode( key );
            int slot = Math.Abs( h % _buckets.Length );
            Bucket b = _buckets[slot];
            while( b != null )
            {
                if( _strategy.Equals( b.Key, key ) )
                {
                    value = b.Value;
                    return true;
                }
                b = b.Next;
            }
            value = default( TValue );
            return false;
        }

        /// <summary>
        /// Gets the value based on a key.
        /// If the key does not exist, an exception is thrown!!!!!!
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get 
            {
                TValue v;
                if( !TryGetValue( key, out v ) )
                {
                    throw new KeyNotFoundException();
                }
                return v; 
            }
            set
            {
                // Add or replace the value for the given key.
                AddOrReplace( key, value, false );
            }
        }

        class EBase<T> : IEnumerable<T>, IEnumerator<T>
        {
            protected readonly ITIDictionary<TKey, TValue> _d;
            int _iSlot;
            Bucket _current;
            Func<Bucket,T> _getT;

            public EBase( ITIDictionary<TKey, TValue> d, Func<Bucket,T> getter )
            {
                _d = d;
                _iSlot = -1;
                _getT = getter;
                // _current = null;
            }

            public T Current
            {
                get
                {
                    if( _current == null )
                    {
                        throw new InvalidOperationException( "MoveNext() must have been called and returned true." );
                    }
                    return _getT( _current );
                }
            }

            public bool MoveNext()
            {
                if( _current != null ) _current = _current.Next;
                while( _current == null )
                {
                    ++_iSlot;
                    if( _iSlot >= _d._buckets.Length ) return false;
                    _current = _d._buckets[_iSlot];
                }
                Debug.Assert( _current != null );
                return true;
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new EBase<T>( _d, _getT );
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return  GetEnumerator();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new EBase<KeyValuePair<TKey, TValue>>( this, 
                b => new KeyValuePair<TKey, TValue>( b.Key, b.Value ) );
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Traverse( Action<TKey, TValue> a )
        {
            for( int i = 0; i < _buckets.Length; ++i )
            {
                Bucket b = _buckets[i];
                while( b != null )
                {
                    a( b.Key, b.Value );
                    b = b.Next;
                }
            }
        }

        public IEnumerable<KeyValuePair<TKey,TValue>> EnumerateContent()
        {
            for( int i = 0; i < _buckets.Length; ++i )
            {
                Bucket b = _buckets[i];
                while( b != null )
                {
                    yield return new KeyValuePair<TKey,TValue>( b.Key, b.Value );
                    b = b.Next;
                }
            }
        }

    }

}
