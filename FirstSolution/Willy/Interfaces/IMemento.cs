using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy.Interfaces
{

    public interface IMemento
    {
        /// <summary>
        /// Saves the current object to a Memento
        /// </summary>
        /// <returns></returns>
        object SaveToMemento();

        /// <summary>
        /// Restores the object to the Memento obj
        /// </summary>
        /// <param name="obj"></param>
        void RestoreFromMemento(object obj);
    }

    public interface IMementoObject
    {
        /// <summary>
        /// All answers must implement the Clone() method for the CareTaker
        /// </summary>
        /// <returns></returns>
        object Clone();
    }

}
