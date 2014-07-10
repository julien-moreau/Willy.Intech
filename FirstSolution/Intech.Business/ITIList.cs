using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{ 
    public class ITIList<T> : IEnumerable<T>
    {
        T[] _array;
        int _count;

        public ITIList()
        {
            _array = new T[8];
        }

        public int Count
        {
            get { return _count; }
        }

        public void Add( T value )
        {
            if( _array.Length == _count )
            {
                var newOne = new T[ _array.Length * 2 ];
                Array.Copy( _array, newOne, _array.Length );
                _array = newOne;
            }
            _array[_count++] = value;
        }

        public T this[int i]
        {
            get 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                return _array[i]; 
            }
            set 
            {
                if( i >= _count ) throw new IndexOutOfRangeException();
                _array[i] = value;
            }
        }

        public bool Remove( T value )
        {
            int idx = IndexOf( value );
            if( idx >= 0 )
            {
                RemoveAt( idx );
                return true;
            }
            return false;
        }

        public int IndexOf( T value )
        {
            var comparer = EqualityComparer<T>.Default;
            for( int i = 0; i < _count; ++i )
            {
                if( comparer.Equals( value, _array[i] ) ) return i;
            }
            return -1;
        }

        public void RemoveAt( int i )
        {
            if( i < 0 || i >= _count ) throw new IndexOutOfRangeException();
            Array.Copy( _array, i + 1, _array, i, _count - (i+1) );
            _array[--_count] = default( T );
        }

        // This is a nested Type.
        // It can access private fields and methods
        // of any instance of its enclosing Type.
        class E : IEnumerator<T>
        {
            readonly ITIList<T> _list;
            int _currentIndex;

            public E( ITIList<T> l )
            {
                _currentIndex = -1;
                _list = l;
                // In java: _array ==> _list._array; 
            }

            public T Current
            {
                get 
                {
                    if( _currentIndex < 0 ) 
                        throw new InvalidOperationException( "MoveNext() must be called first." );
                    if( _currentIndex >= _list._count ) 
                        throw new InvalidOperationException( "Current must not be used if MoveNext() returned false." );
                    return _list._array[_currentIndex];
                }
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return ++_currentIndex < _list._count;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new E( this );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
