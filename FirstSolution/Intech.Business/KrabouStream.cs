using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    class KrabouStream
    {
        string _phrase;
        bool _isWriting;

        public KrabouStream(string phrase)
        {
            _phrase = phrase;
            _isWriting = false;
        }

        public bool isWriting()
        {
            return _isWriting;
        }

        public Stream baseStream()
        {
            return null;
        }

    }
}
