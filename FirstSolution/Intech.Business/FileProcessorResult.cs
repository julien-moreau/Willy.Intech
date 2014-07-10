using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intech.Business
{
    public class FileProcessorResult
    {
        readonly string _rootPath;
        readonly DateTime _date;
        List<Tuple<string,Exception>> _errors;

        static readonly Tuple<string, Exception>[] _noErrors = new Tuple<string, Exception>[0];

        int _totalDirectoryCount;

        internal FileProcessorResult( string rootPath )
        {
            _rootPath = rootPath;
            _date = DateTime.UtcNow;
        }

        internal void AddError( string fName, Exception ex )
        {
            if( _errors == null ) _errors = new List<Tuple<string, Exception>>();
            _errors.Add( Tuple.Create( fName, ex ) );
        }

        public IReadOnlyList<Tuple<string, Exception>> Errors
        {
            get 
            {
                if( _errors != null ) return _errors;
                return _noErrors; 
            }
        }

        public string RootPath
        {
            get { return _rootPath; }
        }

        public DateTime CreationDate
        {
            get { return _date; }
        }

        public bool RootPathExists
        {
            get { return _totalDirectoryCount != 0; }
        }

        public int TotalFileCount { get; internal set; }

        public int TotalDirectoryCount 
        {
            get { return _totalDirectoryCount; }
            internal set { _totalDirectoryCount = value; }
        }

        public int HiddenFileCount { get; internal set; }

        public int HiddenDirectoryCount { get; internal set; }

        public int UnaccessibleFileCount { get; internal set; }
    }
}
