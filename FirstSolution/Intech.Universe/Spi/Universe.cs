using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Intech.Space.Spi
{
    public interface IUniverseTrait
    {
        XElement ToXml();
        void Write( BinaryWriter w );
    }

        interface IUniverseTraitFactory
        {
            IUniverseTrait From( XElement e );
            IUniverseTrait From( BinaryReader r );

        }


    [Serializable]
    public class Star : ISerializable
    {
        readonly Galaxy _galaxy;
        string _name;

        [NonSerialized]
        readonly List<IUniverseTrait> _traits;

        internal Star( Galaxy g, string name )
        {
            Debug.Assert( name != null );
            _galaxy = g;
            _name = name;
            _traits = new List<IUniverseTrait>();
        }


        public IList<IUniverseTrait> Traits { get { return _traits; } }

        public string Name
        {
            get { return _name; }
            set
            {
                if( _name != value )
                {
                    if( String.IsNullOrWhiteSpace( value ) ) throw new ArgumentException();
                    _name = value;
                }
            }
        }

        public bool IsDestroyed { get { return _name == null; } }

        public void Destroy()
        {
            if( _name != null )
            {
                _name = null;
                _galaxy.OnDestroy( this );
            }
        }

        public Galaxy Galaxy { get { return _name != null ? _galaxy : null; } }


        protected Star( SerializationInfo info, StreamingContext context )
        {
            int v = info.GetInt32( "version" );
            if( v == 0 )
            {
                _name = info.GetString( "n" );
                _galaxy = (Galaxy)info.GetValue( "g", typeof( Galaxy ) );
            }
            else 
            { 
                //... 
            }
        }

        public Star( Galaxy galaxy, BinaryReader r )
        {
            _galaxy = galaxy;
            _name = r.ReadString();
        }

        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue( "version", 0 );
            info.AddValue( "n", _name );
            info.AddValue( "g", _galaxy );
        }

        internal void Serialize( BinaryWriter w )
        {
            Debug.Assert( _name != null );
            w.Write( _name );
        }
    }

    [Serializable]
    public class Galaxy
    {
        readonly Universe _universe;
        readonly List<Star> _stars;
        string _name;

        internal Galaxy( Universe universe, string name )
        {
            _universe = universe;
            _name = name;
            _stars = new List<Star>();
        }

        public Universe Universe { get { return _universe; } }
        
        public string Name 
        {
            get { return _name; }
            set
            {
                if( _name != value )
                {
                    if( String.IsNullOrWhiteSpace( value ) ) throw new ArgumentException();
                    _universe.OnRename( this, value );
                    _name = value;
                }
            }
        }

        public IReadOnlyList<Star> Stars { get { return _stars; } }

        public Star CreateStar( string name )
        {
            if( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException();
            Star s = new Star( this, name );
            _stars.Add( s );
            return s;
        }

        internal void OnDestroy( Star star )
        {
            Debug.Assert( _stars.Contains( star ) );
            _stars.Remove( star );
        }

        #region Serialization

        internal Galaxy( Universe u, BinaryReader r )
        {
            _universe = u;
            _name = r.ReadString();
            int nbStar = r.ReadInt32();
            _stars = new List<Star>( nbStar );
            while( --nbStar >= 0 )
            {
                _stars.Add( new Star( this, r ) );
            }

        }

        internal void Serialize( BinaryWriter w )
        {
            w.Write( _name );
            w.Write( _stars.Count );
            foreach( var s in _stars ) s.Serialize( w );
        }

        #endregion

        internal Galaxy( Universe u, XElement e, IUniverseTraitFactory traitFactory = null )
        {
            _universe = u;
            _name = e.Attribute( "Name" ).Value;
            _stars = e.Elements("Stars")
                        .Elements("Star")
                        .Select( es => new Star( this, es.Attribute("Name").Value ))
                        .ToList();
        }

        internal XElement ToXml()
        {
            return new XElement( "Galaxy",
                        new XAttribute( "Name", _name ),
                        new XAttribute( "StarCount", _stars.Count ),
                        new XElement( "Stars",
                            _stars.Select( s => new XElement( "Star",
                                                    new XAttribute( "Name", s.Name ) ) )
                        ));
        }
    }

    [Serializable]
    public class Universe
    {
        readonly Dictionary<string,Galaxy> _galaxies;

        public Universe()
        {
            _galaxies = new Dictionary<string, Galaxy>();
        }

        #region Xml 

        public Universe( XElement e )
            : this()
        {
            int v = Int32.Parse( e.Attribute( "Version" ).Value );
            _galaxies = e.Elements( "Galaxies" )
                            .Elements( "Galaxy" )
                            .Select( eg => new Galaxy( this, eg ) )
                            .ToDictionary( g => g.Name );
        }

        public XElement ToXml()
        {
            return new XElement( "Universe",
                                 new XAttribute( "Version", 0 ),
                                 new XAttribute( "CreationDate", DateTime.UtcNow ),
                                 new XElement( "Galaxies",
                                        _galaxies.Values.Select( g => g.ToXml() )
                                     ) );
        }


        #endregion 

        #region Serialization

        public Universe( BinaryReader r )
            : this()
        {
            if( r.ReadString() != "Intech-Spi-Universe" )
            {
                throw new InvalidDataException( "This is not a valid stream for a Universe" );
            }
            int v = r.ReadInt32();
            while( r.ReadBoolean() )
            {
                var g = new Galaxy( this, r );
                _galaxies.Add( g.Name, g );
            }
        }

        public void HomeMadeSerialize( Stream s )
        {
            HomeMadeSerialize( new BinaryWriter( s ) );
        }

        public void HomeMadeSerialize( BinaryWriter w )
        {
            w.Write( "Intech-Spi-Universe" );
            // Version!
            w.Write( 0 );
            foreach( var g in _galaxies.Values )
            {
                w.Write( true );
                g.Serialize( w );
            }
            w.Write( false );
        }

        #endregion


        class DictionaryValuesWrapper<TValue> : IReadOnlyCollection<TValue>
        {
            readonly ICollection<TValue> _values;

            public DictionaryValuesWrapper(ICollection<TValue> c)
            {
                _values = c;
            }

            public int Count
            {
                get { return _values.Count; }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return _values.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _values.GetEnumerator();
            }
        }

        public IReadOnlyCollection<Galaxy> Galaxies
        {
            get { return new DictionaryValuesWrapper<Galaxy>( _galaxies.Values ); }
        }

        public Galaxy FindOrCreateGalaxy( string name )
        {
            if( String.IsNullOrWhiteSpace( name ) ) throw new ArgumentException();
            Galaxy g;
            if( !_galaxies.TryGetValue( name, out g ) )
            {
                g = new Galaxy( this, name );
                _galaxies.Add( name, g );
            }
            return g;
        }
        
        public Galaxy FindGalaxy( string name )
        {
            Galaxy g;
            _galaxies.TryGetValue( name, out g );
            return g;
        }

        internal void OnRename( Galaxy galaxy, string newName )
        {
            if( FindGalaxy( newName ) != null )
            {
                throw new ArgumentException( "Galaxy exists with this name." );
            }
            _galaxies.Remove( galaxy.Name );
            _galaxies.Add( newName, galaxy );
        }
    }
}
