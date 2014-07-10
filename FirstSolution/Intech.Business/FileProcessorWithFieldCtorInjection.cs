using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class FileProcessorWithFieldCtorInjection
    {
        readonly IConsoleOutput _console;
        bool _stopOnFirstError;

        public FileProcessorWithFieldCtorInjection( IConsoleOutput c )
        {
            if( c == null ) throw new ArgumentNullException( "c" );
            _console = c;
        }

        public bool StopOnFirstError
        {
            get { return _stopOnFirstError; }
            set { _stopOnFirstError = value; }
        }

        public FileProcessorResult Process( string path, Action<bool,FileInfo> onFile = null)
        {
            var r = new FileProcessorResult( path );
            DirectoryInfo d = new DirectoryInfo( path );
            if( d.Exists )
            {
                Process( d, r, false, onFile );
            }
            return r;
        }

        void Process( DirectoryInfo d, FileProcessorResult r, bool isParentHidden, Action<bool, FileInfo> action )
        {
            _console.WriteLine( "Processing directory '{0}'.", d.FullName );
            ++r.TotalDirectoryCount;
            bool thisDirectoryIsHidden = (d.Attributes & FileAttributes.Hidden) != 0;
            if( thisDirectoryIsHidden )
            {
                ++r.HiddenDirectoryCount;
            }
            IEnumerable<FileInfo> files = d.EnumerateFiles();
            IEnumerator<FileInfo> enumerator = files.GetEnumerator();
            try
            {
                while( enumerator.MoveNext() )
                {
                    FileInfo file = enumerator.Current;
                    ++r.TotalFileCount;
                    FileAttributes attrs = file.Attributes;
                    bool isHidden = (attrs & FileAttributes.Hidden) != 0;
                    if( isHidden )
                    {
                        ++r.HiddenFileCount;
                    }
                    if( isHidden || isParentHidden || thisDirectoryIsHidden )
                    {
                        ++r.UnaccessibleFileCount;
                    }
                    #region Side notes on bit flags
                    {
                        bool isArchive = (attrs & FileAttributes.Archive) != 0;

                        bool isArchiveOrHidden = (attrs & (FileAttributes.Archive | FileAttributes.Hidden)) != 0;

                        bool isArchiveAndHidden = (attrs & (FileAttributes.Archive | FileAttributes.Hidden))
                                                            == (FileAttributes.Archive | FileAttributes.Hidden);

                        // An enum, by default, is based on an Int32.
                        int x = (int)attrs;
                        isHidden = (x & 2) != 0;
                        // But we can define 
                        // enum Choucroute : byte { }
                        // to be based on a single byte.
                    }
                    #endregion

                    if( isHidden ) _console.WriteLine( "Processing hidden file '{0}'.", file.FullName );
                    else _console.WriteLine( "Processing file '{0]'.", file.FullName );
                    try
                    {
                        if( action != null ) action( isHidden, file );
                    }
                    catch( Exception ex )
                    {
                        Exception dump = ex;
                        string prefix = "Error: ";
                        do
                        {
                            _console.WriteLine( prefix + ex.Message );
                            prefix += " -> ";
                        }
                        while( (dump = dump.InnerException) != null );

                        if( _stopOnFirstError )
                        {
                            // Solution 1: just rethrow the orginal exception. 
                            // throw;
                            
                            // Solution 2: throws an enhanced exception information WITH the inner exception.
                            // (Better!!)
                            throw new Exception( String.Format( "onFile failed for '{0}'.", file.FullName ), ex );
                        }
                        else
                        {
                            r.AddError( file.FullName, ex );
                        }
                    }
                }
            }
            finally
            {
                enumerator.Dispose();
            }

            // This code is the same as the previous one (try/finally/while...)
            foreach( var subDir in d.EnumerateDirectories() )
            {
                Process( subDir, r, isParentHidden || thisDirectoryIsHidden, action );
            }

        }
    }
}
