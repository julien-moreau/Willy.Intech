using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class TeeStream : Stream
    {
        readonly Stream _out1;
        readonly Stream _out2;
        long _position;
        long _originP1;
        long _originP2;

        public TeeStream( Stream o1, Stream o2 )
        {
            if( o1 == null || o2 == null ) throw new ArgumentNullException();
            if( !o1.CanWrite || !o2.CanWrite ) throw new ArgumentException( "Stop drinking!" );
            _out1 = o1;
            _out2 = o2;
            if( _out1.CanSeek && _out2.CanSeek )
            {
                _originP1 = _out1.Position;
                _originP2 = _out2.Position;
            }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return _out1.CanSeek && _out2.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _out1.Flush();
            _out2.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                if( !CanSeek ) throw new NotSupportedException();
                _position = value;
                _out1.Position = _originP1 + value;
                _out2.Position = _originP2 + value; 
            }
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            throw new NotSupportedException();
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            switch( origin )
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.End:
                    throw new NotSupportedException();
                case SeekOrigin.Current:
                    Position = _position + offset;
                    break;
            }
            return _position;
        }

        public override void SetLength( long value )
        {
            throw new NotSupportedException();
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            _position += count;
            _out1.Write( buffer, offset, count );
            _out2.Write( buffer, offset, count );
        }
    }
}
