using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    public static class MyLinqExtension
    {
        /// <summary>
        /// Applies an action to items of the enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the enumerated element.</typeparam>
        /// <param name="this">This enumerable.</param>
        /// <param name="a">Action to apply to each and every items.</param>
        public static void ForEach<T>( this IEnumerable<T> @this, Action<T> a )
        {
            foreach( var i in @this ) a( i );
        }

        /// <summary>
        /// Specialized for each implementation for <see cref="IGrouping{TKey,TItem}"/>.
        /// </summary>
        /// <typeparam name="TKey">Type of the grouping key.</typeparam>
        /// <typeparam name="TItem">Type of the items.</typeparam>
        /// <param name="this">This enumerable of grouping.</param>
        /// <param name="groupAction">Action that will be applied on each group key.</param>
        /// <param name="itemAction">Action to apply to each and every items.</param>
        public static void ForEach<TKey, TItem>( this IEnumerable<IGrouping<TKey, TItem>> @this, 
                                                Action<TKey> groupAction,
                                                Action<TItem> itemAction )
        {
            foreach( var i in @this )
            {
                groupAction( i.Key );
                i.ForEach( itemAction );
            }
        }
    }

    [TestFixture]
    public class LinqAndReflection
    {
        [Test]
        public void AllTheMethodsWeHaveImplemented()
        {
            Assembly me = Assembly.GetExecutingAssembly();

            // Getting all the methods for all the Types 
            // => SelectMany "concatenates" the intermediate set of Methods.
            // Version with an anonymous function:
            IEnumerable<MethodInfo> allmethods = me.GetTypes()
                                                    .SelectMany( delegate( Type t ) { return t.GetMethods(); } );
            // Cleaner with a lambda function.
            allmethods = me.GetTypes()
                           .SelectMany( t => t.GetMethods() );

            // Where clause.
            var oneAndOnlyOneParameter = allmethods.Where( m => m.GetParameters().Length == 1);

            // Concretisation with a projection (the name is extracted)
            foreach( var name in oneAndOnlyOneParameter.Select( m => m.Name ) )
            {
                Console.WriteLine( name );
            }
            // Same as: (concretisation without the projection)
            foreach( var m in oneAndOnlyOneParameter )
            {
                Console.WriteLine( m.Name );
            }
        }

        [Test]
        public void AllTheMethodsWeHaveImplementedInOneLine()
        {
            Assembly me = Assembly.GetExecutingAssembly();
            
            // How to make a "one-liner"?
            me.GetTypes()
                .SelectMany( t => t.GetMethods() )
                .Where( m => m.GetParameters().Length == 1 )
                .Select( m => m.ReflectedType.FullName + '.' + m.Name )
                // This is how to order the items.
                .OrderBy( name => name.Length ).ThenBy( name => name )
                .ForEach( Console.WriteLine );

            // Grouping!
            me.GetTypes()
                .SelectMany( t => t.GetMethods() )
                .GroupBy( m => m.GetParameters().Length )
                .ForEach( paramCount => Console.WriteLine( "{0} parameter(s)", paramCount ),
                          m => Console.WriteLine( m.ReflectedType.FullName + '.' + m.Name ) );

        }

    }
}
