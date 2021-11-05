/*
 *  File Name:   AvlTree.cs
 *
 * Code supplied as part of the Learning Content.
 * Comments added by VS plug-in.
 *
 * Modifications to the code by me are annotated with: (BW)
 * There are many added methods and even an added inner class.
 * These are not necessarily annotated as they are not modified code
 * but added code. Therefore, they should be easily distinguishable
 * from the original code base.
 *
 * ****************************************************************
 * Name: Bradley Willcott
 * ID:   M198449
 * Date: 16/08/2021
 * ****************************************************************
 */

namespace MyNETCoreLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class AvlTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Defines the <see cref="DataEnum" />.
        /// </summary>
        /// <remarks>
        /// This code is based on the code at:
        /// https://docs.microsoft.com/en-au/dotnet/api/system.collections.ienumerator?view=net-5.0
        /// </remarks>
        public class DataEnum<E> : IEnumerator<E> where E : IComparable<E>
        {
            /// <summary>
            /// The disposed value
            /// </summary>
            private bool disposedValue;

            /// <summary>
            /// The list
            /// </summary>
            private E[] list;

            /// <summary>
            /// The current position within the list array.
            /// </summary>
            private int position = -1;

            /// <summary>
            /// Initializes a new instance of the <see cref="DataEnum"/> class.
            /// </summary>
            /// <param name="list">The list.</param>
            public DataEnum(E[] list)
            {
                this.list = list;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="DataEnum`1"/> class.
            /// </summary>
            // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            ~DataEnum()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: false);
            }

            /// <summary>
            /// Gets the Current object.
            /// </summary>
            public E Current
            {
                get
                {
                    try
                    {
                        return list[position];
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        throw new InvalidOperationException("Current is invalid!", ex);
                    }
                }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current { get => Current; }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// MoveNext to the next element in the list.
            /// </summary>
            /// <returns><c>true</c> if <see cref="Current"/> now points to a valid entry in the list.</returns>
            public bool MoveNext()
            {
                position++;
                return position < list.Length;
            }

            /// <summary>
            /// Reset the position to before the beginning of the list.
            /// </summary>
            public void Reset()
            {
                position = -1;
            }

            /// <summary>
            /// Releases unmanaged and - optionally - managed resources.
            /// </summary>
            /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // dispose managed state (managed objects)
                    }

                    // free unmanaged resources (unmanaged objects) and override finalizer
                    // set large fields to null
                    list = null;
                    disposedValue = true;
                }
            }
        }
    }
}