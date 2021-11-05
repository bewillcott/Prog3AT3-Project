/*
 *  File Name:   AvlTree.Node.cs
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

    public partial class AvlTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// Represents a node in a <see cref="AvlTree{T}"/>.  This class cannot be inherited.
        /// </summary>
        public sealed class Node<E> where E : IComparable<E>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="value">The value<see cref="string"/>.</param>
            public Node(E value)
            {
                this.Value = value;
            }

            /// <summary>
            /// Gets or sets the Value.
            /// </summary>
            public E Value { get; set; }

            /// <summary>
            /// Gets or sets the index.
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// Gets or sets the Left.
            /// </summary>
            public Node<E> Left { get; set; }

            /// <summary>
            /// Gets or sets the Right.
            /// </summary>
            public Node<E> Right { get; set; }
        }
    }
}