using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ObserverPattern
    {
        interface IButtonListener
        {
            void OnClick( Button b );
        }

        class Button
        {
            List<IButtonListener> _listeners;

            public Button()
            {
                _listeners = new List<IButtonListener>();
            }

            public void OnClickDetected()
            {
                foreach( var l in _listeners )
                {
                    l.OnClick( this );
                }
            }

            public void Register( IButtonListener listener )
            {
                _listeners.Add( listener );
            }
            
            public void Unregister( IButtonListener listener )
            {
                _listeners.Remove( listener );
            }
        }

        class Panel : IButtonListener
        {
            Button _button1;
            Button _button2;

            public Panel()
            {
                _button1 = new Button();
                _button2 = new Button();
                _button1.Register( this );
                _button2.Register( this );
                // Java: Anonymous class.
                // _button2.Register( new IButtonListener 
                //                        { 
                //                            void OnClick( Button b ) 
                //                            {
                //                                SayHello();
                //                            } 
                //                        } );
            }

            void SayHello()
            {
                Console.WriteLine( "Hello World..." );
            }

            void SayGoodbye()
            {
                Console.WriteLine( "Bye!" );
            }

            void IButtonListener.OnClick( Button b )
            {
                if( b == _button1 ) SayHello();
                else SayGoodbye();
            }
        }

        class ButtonV2
        {
            // Non standard "event" (but works).
            public Action OnClick;

            // Standard event:
            public event EventHandler OnClickStandard;

            public void OnClickDetected()
            {
                var h = OnClick;
                if( h != null ) h();
            }
        }

        class PanelV2
        {
            ButtonV2 _button1;
            ButtonV2 _button2;

            public PanelV2()
            {
                _button1 = new ButtonV2();
                _button2 = new ButtonV2();
                _button1.OnClick += SayHello;
                _button1.OnClick += SayGoodbye;
                // Recall that what is actually done:
                _button1.OnClick += new Action( SayGoodbye );

                // Problem with non standard event: anybody can clear the listener list!
                _button2.OnClick = null;

                _button2.OnClickStandard += SayHelloStandard;
                // Standard event prevents this:
                // _button2.OnClickStandard = null;
            }

            public void TestClickOnButton1()
            {
                _button1.OnClickDetected();
            }

            void SayHelloStandard( object source, EventArgs e )
            {
                Console.WriteLine( "Hello World..." );
            }

            void SayHello()
            {
                Console.WriteLine( "Hello World..." );
            }

            void SayGoodbye()
            {
                Console.WriteLine( "Bye!" );
            }
        }

        [Test]
        public void HistoricalImplementation()
        {

            PanelV2 p = new PanelV2();
            p.TestClickOnButton1();

        }

    }
}
