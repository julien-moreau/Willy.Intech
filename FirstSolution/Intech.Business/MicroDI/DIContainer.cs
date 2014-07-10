using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.MicroDI
{
    public class DIContainer : IDependencyResolver
    {
        class Entry
        {
            public Entry( Type mappedType )
            {
                MappedType = mappedType;
            }

            public readonly Type MappedType;
            public object Singleton;
        }

        readonly Dictionary<Type,Entry> _mappedTypes;

        public DIContainer()
        {
            _mappedTypes = new Dictionary<Type, Entry>();
        }

        public object Resolve( Type t )
        {
            Entry e = MapType( t );
            Debug.Assert( e != null && !e.MappedType.IsAbstract && !e.MappedType.IsInterface );
            return ObtainInstance( e );
        }

        private object ObtainInstance( Entry e )
        {
            if( e.Singleton != null ) return e.Singleton;
            return CreateInstance( e.MappedType );
        }

        private object CreateInstance( Type mapped )
        {
            var availableCtors = mapped.GetConstructors()
                .Select( c => new { Ctor = c, Parameters = c.GetParameters() } )
                .OrderByDescending( c => c.Parameters.Length )
                .Take( 1 );
            var theBest = availableCtors.Single();

            object[] parameters = new object[theBest.Parameters.Length];
            for( int i = 0; i < parameters.Length; ++i )
            {
                parameters[i] = Resolve( theBest.Parameters[i].ParameterType );
            }
            return Activator.CreateInstance( mapped, parameters );
        }

        public void Register( Type abstraction, Type implementation )
        {
            Entry e;
            if( !_mappedTypes.TryGetValue( implementation, out e ))
            {
                e = new Entry( implementation );
                _mappedTypes.Add( implementation, e );
            }
            if( abstraction != implementation ) _mappedTypes.Add( abstraction, e );
        }

        public void Register( object singleton )
        {
            Type t = singleton.GetType();
            Register( t, t );
            _mappedTypes[t].Singleton = singleton;
        }

        private Entry MapType( Type t )
        {
            Entry e;
            if( _mappedTypes.TryGetValue( t, out e ) ) return e;
            
            if( t.IsInterface || t.IsAbstract ) throw new InvalidOperationException( "Unregistered abstraction." );
            
            e = new Entry( t );
            _mappedTypes.Add( t, e );
            return e;
        }
    }
}
