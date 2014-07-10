using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class DatabaseAccess
    {
        [Test]
        public void Connection()
        {
           string cn = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=E:\Intech\2014-1\S7-8\S7-8-2014-1\FirstSolution\Intech.Business.Tests\Test.mdf;Integrated Security=True";
            SqlConnection c = new SqlConnection( cn );
            c.InfoMessage += ( o, e ) => Console.WriteLine( e.Message );
            c.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select newid();";
                cmd.CommandType = CommandType.Text;

                object result = cmd.ExecuteScalar();

                Guid g;
                Assert.That( Guid.TryParse( result.ToString(), out g ) );

            }
            finally
            {
                c.Close();
            }
        }

    }
}
