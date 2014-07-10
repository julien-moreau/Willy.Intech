using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class CharsAndStrings
    {
        [Test]
        public void EncodingBasics()
        {
            string s = "Toto";
            byte[] b32 = Encoding.UTF32.GetBytes( s );
            byte[] b16 = Encoding.Unicode.GetBytes( s );
            byte[] b16BigEndian = Encoding.BigEndianUnicode.GetBytes( s );
            byte[] b8 = Encoding.UTF8.GetBytes( s );
            byte[] b7 = Encoding.UTF7.GetBytes( s );

            s = "Eléonore";
            b32 = Encoding.UTF32.GetBytes( s );
            b16 = Encoding.Unicode.GetBytes( s );
            b16BigEndian = Encoding.BigEndianUnicode.GetBytes( s );
            b8 = Encoding.UTF8.GetBytes( s );
            b7 = Encoding.UTF7.GetBytes( s );

            s = "شخصا";
            b32 = Encoding.UTF32.GetBytes( s );
            b16 = Encoding.Unicode.GetBytes( s );
            b16BigEndian = Encoding.BigEndianUnicode.GetBytes( s );
            b8 = Encoding.UTF8.GetBytes( s );
            b7 = Encoding.UTF7.GetBytes( s );
        }

        [Test]
        public void ComposingCharacters()
        {
            var s = "é";
            byte[] é16 = Encoding.Unicode.GetBytes( s );

            var sD = s.Normalize( NormalizationForm.FormD );
            byte[] éD16 = Encoding.Unicode.GetBytes( sD );

            if( s == sD ) Console.WriteLine( "s and sD are equals." );

            if( StringComparer.InvariantCulture.Equals( s, sD ) ) 
                Console.WriteLine( "s and sD are equals for the InvariantCulture." );
        }

        [Test]
        public void RemovingDiacriticsTests()
        {
            Assert.That( RemoveDiacritics( "" ), Is.EqualTo( "" ) );

            Assert.That( RemoveDiacritics( "Eléonore" ), Is.EqualTo( "Eleonore" ) );

            Assert.That( RemoveDiacritics( "éö¨nèôîâä" ), Is.EqualTo( "eo¨neoiaa" ) );
        }

        string RemoveDiacritics( string s )
        {
            if( s == null ) throw new ArgumentNullException();
            var sD = s.Normalize( NormalizationForm.FormD );
            StringBuilder b = new StringBuilder();
            foreach( var c in sD )
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory( c );
                if( uc != UnicodeCategory.NonSpacingMark ) b.Append( c );
            }
            return b.ToString();
        }



    }
}
