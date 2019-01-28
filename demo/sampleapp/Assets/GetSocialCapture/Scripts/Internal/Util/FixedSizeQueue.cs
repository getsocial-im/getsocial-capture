using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Scripts.Internal.Util
{
    /// <summary>
    /// Fixed size queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedSizedQueue<T>
    {

        #region Private fields

        private readonly Queue<T> _queue = new Queue<T>();
        private readonly int _size;

        #endregion

        #region Public methods

        /// <summary>
        /// Creates new instances with the maximum size specified.
        /// </summary>
        /// <param name="size">Number of maximum elements.</param>
        public FixedSizedQueue(int size)
        {
            _size = size;
        }

        /// <summary>
        /// Adds new element.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        public void Enqueue(T obj)
        {
            _queue.Enqueue(obj);

            while (_queue.Count > _size)
            {
                var o = _queue.Dequeue();
                if (o is Object) Object.Destroy(o as Object);
            }
        }

        /// <summary>
        /// Returns object at specified index.
        /// </summary>
        /// <param name="index">Index of object.</param>
        /// <returns>Object at specified index.</returns>
        public T ElementAt(int index)
        {
            return _queue.ElementAt(index);
        }
        
        /// <summary>
        /// Returns number of elements in the queue.
        /// </summary>
        /// <returns>Number of elements.</returns>
        public int Count()
        {
            return _queue.Count;
        }

        #endregion

    }
}