using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    class TestHelper
    {
        static readonly DirectoryInfo _solutionFolder;
        static DirectoryInfo _testSupportFolder;


        static TestHelper()
        {
            _solutionFolder = new DirectoryInfo(
                    Path.GetDirectoryName( 
                        Path.GetDirectoryName( 
                            Path.GetDirectoryName(
                                Path.GetDirectoryName( 
                                    new Uri( Assembly.GetExecutingAssembly().CodeBase ).LocalPath ) ) ) ) );
        }
        
        static public DirectoryInfo TestSupportFolder
        {
            get
            {
                return _testSupportFolder 
                            ?? (_testSupportFolder = new DirectoryInfo(
                                                               Path.Combine(
                                                                   SolutionFolder.FullName,
                                                                   "Intech.Business.Tests",
                                                                   "TestSupport" ) ))
                            // Coalescence operator can be chained (thanks Maxime!)
                            ?? new DirectoryInfo( "" );

                /* Avoid Unreachable code detected warning...
                
                 return _testSupportFolder != null 
                            ? _testSupportFolder
                            : (_testSupportFolder = new DirectoryInfo(
                                   Path.Combine(
                                       SolutionFolder.FullName,
                                       "Intech.Business.Tests",
                                       "TestSupport" ) ) );


                if( _testSupportFolder == null )
                {

                    _testSupportFolder = new DirectoryInfo(
                                   Path.Combine(
                                       SolutionFolder.FullName,
                                       "Intech.Business.Tests",
                                       "TestSupport" ) );
                }
                return _testSupportFolder;
                */
            }
        }

        static public DirectoryInfo SolutionFolder
        {
            get { return _solutionFolder; }
        }

        static public string TestSupportPath
        {
            get { return TestSupportFolder.FullName; }
        }

    }
}
