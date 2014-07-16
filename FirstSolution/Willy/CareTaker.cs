using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willy
{
    public class CareTaker
    {
        private List<object> _savedObjects;

        /// <summary>
        /// ctor
        /// </summary>
        public CareTaker()
        {
            _savedObjects = new List<object>();
        }

        /// <summary>
        /// Adds a memento to the list
        /// </summary>
        /// <param name="m"></param>
        public void AddMemento(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object cannot be null when adding to the Memento's list");

            _savedObjects.Add(obj);
        }
        
        /// <summary>
        /// Retunrs the Memento at index "index"
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetMemento(int index)
        {
            if (index < 0 || index > _savedObjects.Count)
                throw new IndexOutOfRangeException("Cannot acceed to the specified memento (index out of range)");

            return _savedObjects.ElementAt(index);
        }

        /// <summary>
        /// Returns the Memento (object) objects count
        /// </summary>
        public int ObjectsCount
        {
            get { return _savedObjects.Count; }
        }
    }
}
