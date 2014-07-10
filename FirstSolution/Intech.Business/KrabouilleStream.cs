using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class KrabouilleStream : Stream
    {
        readonly Stream _base;
        readonly bool _isWriting;
        readonly byte[] _secretKey;
        readonly Random _random;
        long _position;

        public KrabouilleStream( Stream s, bool isWriting, string passPhrase )
        {
            if( s == null ) throw new ArgumentNullException();
            if( String.IsNullOrEmpty( passPhrase ) ) throw new ArgumentNullException();
            if( (isWriting && !s.CanWrite) || (!isWriting && !s.CanRead) )
            {
                throw new ArgumentException();
            }
            _base = s;
            _isWriting = isWriting;
            _secretKey = Encoding.UTF7.GetBytes( passPhrase );
            int start = 1;
            for( int i = 0; i < _secretKey.Length; ++i )
            {
                start <<= 1;
                start += _secretKey[i];
            }
            _random = new Random( start );
        }

        public override bool CanRead
        {
            get { return !_isWriting; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _isWriting; }
        }

        public override void Flush()
        {
            _base.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { return _position; }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            throw new NotSupportedException();
        }

        public override void SetLength( long value )
        {
            throw new NotSupportedException();
        }


        public override int Read( byte[] buffer, int offset, int count )
        {
            if( _isWriting ) throw new InvalidOperationException();
            int len = _base.Read( buffer, offset, count );
            if( len > 0 )
            {
                int idxSecret = (int)(_position % _secretKey.Length);
                for( int i = offset; i < offset + len; ++i )
                {
                    byte secret = _secretKey[ ++idxSecret % _secretKey.Length ];
                    secret ^= (byte)_random.Next();
                    buffer[i] = (byte)(buffer[i] ^ secret);
                }
                _position += len;
            }
            return len;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            if( !_isWriting ) throw new InvalidOperationException();

            int idxSecret = (int)(_position % _secretKey.Length);
            for( int i = offset; i < offset + count; ++i )
            {
                byte secret = _secretKey[++idxSecret % _secretKey.Length];
                secret ^= (byte)_random.Next();
                buffer[i] = (byte)(buffer[i] ^ secret);
            }

            _base.Write( buffer, offset, count );
            _position += count;
        }
    }
}
