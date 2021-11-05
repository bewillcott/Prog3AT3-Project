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
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="AvlTree" />.<br/>
    /// This tree does not support <c>null</c> entries.
    /// </summary>
    // (BW) Converted to be a Generic class, and added implementation of:
    // - ICollection<E>
    // - ICollection
    public partial class AvlTree<T> : IList<T>, IList where T : IComparable<T>
    {
        /// <summary>
        /// The version of data last indexed.
        /// </summary>
        private int lastIndexVersion;

        /// <summary>
        /// The version of the data.
        /// </summary>
        private int version;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree"/> class as a
        /// Balanced Binary Search Tree.
        /// </summary>
        public AvlTree()
        {
            Balanced = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{T}"/> class with the contents
        /// of the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list of existing items to start with.</param>
        /// <param name="balanced">Set tree as a {Balance} Binary Search Tree.</param>
        public AvlTree(IList<T> list, bool balanced = true)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count > 0)
            {
                foreach (T item in list)
                {
                    _ = InternalAdd(item);
                }
            }

            Balanced = balanced;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="AvlTree{T}"/> is balanced.
        /// </summary>
        /// <value>
        ///   <c>true</c> if balanced; otherwise, <c>false</c>.
        /// </value>
        public bool Balanced { get; }

        /// <summary>
        /// Gets the Count.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="System.Collections.IList" /> has a fixed size.
        /// </summary>
        bool IList.IsFixedSize => false;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is
        /// read-only.
        /// </summary>
        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is
        /// read-only.
        /// </summary>
        bool IList.IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" />
        /// is synchronized (thread safe).
        /// </summary>
        bool ICollection.IsSynchronized => false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the
        /// <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        object ICollection.SyncRoot { get; }

        /// <summary>
        /// Gets a value indicating whether [index is dirty].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [index is dirty]; otherwise, <c>false</c>.
        /// </value>
        protected bool IndexIsDirty => lastIndexVersion != version;

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        protected Node<T> Root { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <remarks>
        /// Due to the implementation requirements of this method, it is strongly advised
        /// that it NOT be used within a loop of any kind.<br/>
        /// If you need to iterate through the list, then use <see cref="AvlTree{T}.GetEnumerator"/>
        /// </remarks>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">if attempt to <see langword="set"/> a new value.</exception>
        public T this[int index]
        {
            get
            {
                //Console.WriteLine($"this[{index}]");
                return GetNodeAt(index).Value;
            }

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <remarks>
        /// Due to the implementation requirements of this method, it is strongly advised
        /// that it NOT be used within a loop of any kind.<br/>
        /// If you need to iterate through the list, then use <see cref="AvlTree{T}.GetEnumerator"/>
        /// </remarks>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        object IList.this[int index]
        {
            get
            {
                //Console.WriteLine($"IList.this[{index}]");
                return GetNodeAt(index).Value;
            }

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Add new data to tree node and re-balance
        /// uses the Balance_Tree and Rotate methods.
        /// </summary>
        /// <remarks>
        /// <b>Note:</b><br/>
        /// Neither <see langword="null"/>s nor duplicates are allowed.
        /// </remarks>
        /// <param name="item">The data<see cref="string"/>.</param>
        /// <returns><c>true</c> unless <paramref name="item"/> is <c>null</c> or a duplicate.</returns>
        public virtual bool Add(T item)
        {
            return InternalAdd(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <seealso cref="Add(T)"/>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        void ICollection<T>.Add(T item)
        {
            var rtn = Add(item);
            //Console.WriteLine($"ICollection<T>.Add({item}) : {rtn}");
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to add to the <see cref="T:System.Collections.IList" />.
        /// </param>
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not
        /// inserted into the collection.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IList.Add(object value)
        {
            var rtn = (value is T item) && Add(item) ? IndexOf(item) : -1;
            //Console.WriteLine($"IList.Add({value}) : {rtn}");
            return rtn;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public virtual void Clear()
        {
            Root = null;
            Count = 0;
            version++;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The object to locate in the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found in the
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="value">
        /// The object to locate in the <see cref="T:System.Collections.IList" />.
        /// </param>
        /// <returns>
        ///   <see langword="true" /> if the <see cref="T:System.Object" /> is found in the
        ///   <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        bool IList.Contains(object value)
        {
            return (value is T item) && Find(item) != null;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        /// <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination
        /// of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying
        /// begins.</param>
        //   E:System.ArgumentNullException:
        //     array is null.
        //
        //   E:System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   E:System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.ICollection`1
        //     is greater than the available space from arrayIndex to the end of the destination
        //     array.
        public void CopyTo(T[] array, int arrayIndex)
        {
            //Console.WriteLine($"CopyTo({array}, {arrayIndex})");

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException($"{nameof(array)} size is too small!");
            }

            var index = arrayIndex;

            if (Count > 0)
            {
                FillArray(Root, array, ref index);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an
        /// <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array" /> that is the destination of
        /// the elements copied from <see cref="T:System.Collections.ICollection" />.
        /// The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="index">
        /// The zero-based index in <paramref name="array" /> at which copying begins.
        /// </param>
        void ICollection.CopyTo(Array array, int index)
        {
            //Console.WriteLine($"ICollection.CopyTo({array}, {index})");

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException($"{nameof(array)} size is too small!");
            }

            var idx = index;

            // This code was copied from:
            // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/LinkedList.cs
            // with modifications to work in a BBST.

            if (array is T[] tArray)
            {
                CopyTo(tArray, idx);
            }
            else
            {
                // No need to use reflection to verify that the types are compatible because it isn't
                // 100% correct and we can rely on the runtime validation during the cast that happens
                // below (i.e. we will get an ArrayTypeMismatchException).
                if (array is object[] objects)
                {
                    try
                    {
                        var list = new T[Count];
                        CopyTo(list, 0);

                        for (int i = 0; i < list.Length; i++)
                        {
                            objects[i] = list[i];
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException($"Invalid array type: {nameof(array)}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid argument type: {nameof(array)}");
                }
            }
        }

        /// <summary>
        /// Delete the <paramref name="target"/> from the tree.
        /// </summary>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <returns>
        /// <c>true</c> unless <paramref name="target"/> is <c>null</c>, or <paramref name="target"/>
        /// is not found.
        /// </returns>
        public virtual bool Delete(T target)
        {
            var rtn = false;

            if (target != null)
            {
                Root = DeleteNode(Root, target, ref rtn, Balanced);

                if (rtn)
                {
                    Count--;
                    version++;
                }
            }
            //Console.WriteLine($"virtual Delete({target}) : {rtn}");

            return rtn;
        }

        /// <summary>
        /// Display the data items in order.
        /// </summary>
        public void Display()
        {
            if (Root == null)
            {
                Console.WriteLine("Tree is empty");
                return;
            }

            Console.WriteLine(DisplayInOrder(Root));
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            var list = new T[Count];
            CopyTo(list, 0);
            //Console.WriteLine($"GetEnumerator() : list => {list}");

            return new DataEnum<T>(list);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            //Console.WriteLine($"IEnumerable<T>.GetEnumerator()");
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through
        /// the collection.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            //Console.WriteLine($"IEnumerable.GetEnumerator()");
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the
        /// <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the
        /// <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int IndexOf(T item)
        {
            ReIndex();
            var node = Find(item);
            var rtn = node != null ? node.Index : -1;
            //Console.WriteLine($"IndexOf({item}) : {rtn}");

            return rtn;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to locate in the <see cref="T:System.Collections.IList" />.
        /// </param>
        /// <returns>
        /// The index of <paramref name="value" /> if found in the list; otherwise, -1.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IList.IndexOf(object value)
        {
            var rtn = value is T item ? IndexOf(item) : -1;
            //Console.WriteLine($"IList.IndexOf({value}) : {rtn}");
            return rtn;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified
        /// index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item" /> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="value" /> should be inserted.
        /// </param>
        /// <param name="value">
        /// The object to insert into the <see cref="T:System.Collections.IList" />.
        /// </param>
        /// <exception cref="System.NotImplementedException"></exception>
        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.
        ///   This method also returns <see langword="false" /> if <paramref name="item" /> is not found
        ///   in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(T item)
        {
            //Console.WriteLine($"Remove({item})");
            return Delete(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// <see cref="T:System.Collections.IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to remove from the <see cref="T:System.Collections.IList" />.
        /// </param>
        /// <exception cref="System.NotImplementedException"></exception>
        void IList.Remove(object value)
        {
            if (value is T target)
            {
                //Console.WriteLine($"IList.Remove({value})");
                Delete(target);
            }
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RemoveAt(int index)
        {
            var node = GetNodeAt(index);
            var temp = false;
            //Console.WriteLine($"RemoveAt({index}) : {node.Value}");

            DeleteNode(node, node.Value, ref temp, Balanced);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void IList.RemoveAt(int index)
        {
            var node = GetNodeAt(index);
            var temp = false;
            //Console.WriteLine($"IList.RemoveAt({index}) : {node.Value}");

            DeleteNode(node, node.Value, ref temp, Balanced);
        }

        /// <summary>
        /// Finds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="Node{T}"/> if found, or <see langword="null"/> otherwise.</returns>
        // (BW) Changed signature and converted to use generics.
        protected Node<T> Find(T key)
        {
            Node<T> rtn = null;

            if (key != null)
            {
                var node = FindRecursive(key, Root);
                rtn = node != null && node.Value.Equals(key) ? node : null;
            }

            return rtn;
        }

        /// <summary>
        /// Re-index the tree.
        /// </summary>
        // (BW) Added new method
        protected void ReIndex()
        {
            var index = -1;

            if (Count > 0 && IndexIsDirty)
            {
                //Console.WriteLine($"ReIndexing ...");
                IndexRecursively(Root, ref index);
                lastIndexVersion = version;

                if (index != Count - 1)
                {
                    throw new ReindexFailedException($"Re-indexing has failed: Count({Count}), index({index})");
                }
            }
        }

        /// <summary>
        /// The AddRecursive.
        /// </summary>
        /// <param name="current">The current<see cref="Node"/>.</param>
        /// <param name="node">The n<see cref="Node"/>.</param>
        /// <param name="added">
        /// <see langword="true" /> if successful, <see langword="false" /> if already exists.
        /// </param>
        /// <param name="balanced"><see langword="true"/> if tree is to be kept balanced.</param>
        /// <returns>The <see cref="Node"/>.</returns>
        private static Node<T> AddRecursive(Node<T> current, Node<T> node, ref bool added, bool balanced)
        {
            if (current == null)
            {
                current = node;
                added = true;
                // (BW) Cut out: "return current;"
            }
            else if (node.Value.CompareTo(current.Value) < 0)
            {
                current.Left = AddRecursive(current.Left, node, ref added, balanced);
                // (BW) Moved Balance_Tree()
            }
            else if (node.Value.CompareTo(current.Value) > 0)
            {
                current.Right = AddRecursive(current.Right, node, ref added, balanced);
                // (BW) Moved Balance_Tree()
            }

            // (BW) Moved Balance_Tree() and added if()
            if (balanced && added)
            {
                current = Balance_Tree(current);
            }

            return current;
        }

        /// <summary>
        /// The Balance_Factor.
        /// </summary>
        /// <param name="current">The current<see cref="Node"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        private static int Balance_Factor(Node<T> current)
        {
            // (BW) Changed to 'var'
            var l = GetHeight(current.Left);
            var r = GetHeight(current.Right);
            var b_factor = l - r;

            return b_factor;
        }

        /// <summary>
        /// Method to balance tree after insert and delete.
        /// </summary>
        /// <param name="current">The current<see cref="Node"/>.</param>
        /// <returns>The <see cref="Node"/>.</returns>
        private static Node<T> Balance_Tree(Node<T> current)
        {
            // (BW) Changed to 'var'
            var b_factor = Balance_Factor(current);

            if (b_factor > 1)
            {
                // (BW) Changed to using a ternary operator
                current = Balance_Factor(current.Left) > 0 ? RotateLL(current) : RotateLR(current);
            }
            else if (b_factor < -1)
            {
                // (BW) Changed to using a ternary operator
                current = Balance_Factor(current.Right) > 0 ? RotateRL(current) : RotateRR(current);
            }

            return current;
        }

        /// <summary>
        /// The DeleteNode.
        /// </summary>
        /// <param name="current">The current<see cref="Node"/>.</param>
        /// <param name="target">The target<see cref="string"/>.</param>
        /// <param name="found"><c>true</c> if found.</param>
        /// <param name="balanced"><see langword="true"/> if tree is to be kept balanced.</param>
        /// <returns>The <see cref="Node"/>.</returns>
        private static Node<T> DeleteNode(Node<T> current, T target, ref bool found, bool balanced)
        {
            Node<T> parent;

            // (BW) Changed from "if (current == null){ return null;}"
            if (current != null)
            {
                //left subtree
                if (target.CompareTo(current.Value) < 0)
                {
                    current.Left = DeleteNode(current.Left, target, ref found, balanced);

                    if (balanced && Balance_Factor(current) == -2)//here
                    {
                        // (BW) Changed to using a ternary operator
                        current = Balance_Factor(current.Right) <= 0 ? RotateRR(current) : RotateRL(current);
                    }
                }
                //right subtree
                else if (target.CompareTo(current.Value) > 0)
                {
                    current.Right = DeleteNode(current.Right, target, ref found, balanced);

                    if (balanced && Balance_Factor(current) == 2)
                    {
                        // (BW) Changed to using a ternary operator
                        current = Balance_Factor(current.Left) >= 0 ? RotateLL(current) : RotateLR(current);
                    }
                }
                //if target is found
                else
                {
                    if (current.Right != null)
                    {
                        //delete its in-order successor
                        parent = current.Right;

                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }

                        current.Value = parent.Value;
                        current.Right = DeleteNode(current.Right, parent.Value, ref found, balanced);

                        if (balanced && Balance_Factor(current) == 2)//re-balancing
                        {
                            // (BW) Changed to using a ternary operator
                            current = Balance_Factor(current.Left) >= 0 ? RotateLL(current) : RotateLR(current);
                        }
                    }
                    else
                    {   //if current.left != null
                        // (BW) Changed from "return current.Left;"
                        current = current.Left;
                    }

                    found = true;
                }
            }

            return current;
        }

        /// <summary>
        /// The DisplayInOrder.
        /// </summary>
        /// <param name="current">The current<see cref="Node"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string DisplayInOrder(Node<T> current)
        {
            // (BW) Changed from 'string s'
            var rtn = new StringBuilder();

            if (current != null)
            {
                rtn.Append($"{DisplayInOrder(current.Left)}");
                rtn.Append(current.Value).Append(", ");
                rtn.Append($"{DisplayInOrder(current.Right)}");
            }

            return rtn.Length > 0 ? rtn.ToString() : null;
        }

        /// <summary>
        /// Fills the array.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        private static void FillArray(Node<T> current, T[] array, ref int index)
        {
            if (current != null)
            {
                FillArray(current.Left, array, ref index);
                array[index++] = current.Value;
                FillArray(current.Right, array, ref index);
            }
        }

        /// <summary>
        /// Recursively search for the
        /// </summary>
        /// <param name="index">The index<see cref="int"/>.</param>
        /// <param name="current">The current<see cref="avl_tree.AvlTree{T}.Node{T}"/>.</param>
        /// <returns>
        /// The <see cref="avl_tree.AvlTree{T}.Node{T}"/> if <paramref name="index"/> is found,
        /// otherwise <see langword="null"/>.
        /// </returns>
        private static Node<T> FindIndexRecursive(int index, Node<T> current)
        {
            // (BW) Added rtn var to replace multiple "return"s
            Node<T> rtn = null;

            if (current != null)
            {
                if (index == current.Index)
                {
                    rtn = current;
                }
                else rtn = FindIndexRecursive(index, index < current.Index ? current.Left : current.Right);
            }

            return rtn;
        }

        /// <summary>
        /// Recursively find the <paramref name="target"/> within the
        /// <paramref name="current"/> <see cref="Node{T}"/>'s subtree.
        /// </summary>
        /// <param name="target">The target object being sought.</param>
        /// <param name="current">The current <see cref="Node{T}"/>.</param>
        /// <returns>The <see cref="Node{T}"/> if found, otherwise <see langword="null"/>.</returns>
        private static Node<T> FindRecursive(T target, Node<T> current)
        {
            // (BW) Added rtn var to replace multiple "return"s
            Node<T> rtn = null;

            if (current != null)
            {
                // (BW) Moved Equals to if()
                if (target.Equals(current.Value))
                {
                    rtn = current;
                } // (BW) Changed to using a ternary operator
                else rtn = FindRecursive(target, target.CompareTo(current.Value) < 0 ? current.Left : current.Right);
            }

            return rtn;
        }

        /// <summary>
        /// Get the height of the <paramref name="current"/> <see cref="Node{T}"/>.
        /// </summary>
        /// <param name="current">The current<see cref="Node{T}"/>.</param>
        /// <returns>The height.</returns>
        private static int GetHeight(Node<T> current)
        {
            // (BW) Changed to 'var'
            var height = 0;

            if (current != null)
            {
                // (BW) Changed to 'var'
                var leftHeight = GetHeight(current.Left);
                var rightHeight = GetHeight(current.Right);
                var maxHeight = Max(leftHeight, rightHeight);
                height = maxHeight + 1;
            }

            return height;
        }

        /// <summary>
        /// Recursively index the nodes.
        /// </summary>
        /// <param name="current">The current <see cref="Node{T}"/>.</param>
        /// <param name="index">The index <see langword="int"/>.</param>
        // (BW) Added new method
        private static void IndexRecursively(Node<T> current, ref int index)
        {
            if (current != null)
            {
                IndexRecursively(current.Left, ref index);
                current.Index = ++index;
                IndexRecursively(current.Right, ref index);
            }
        }

        /// <summary>
        /// The maximum height between <paramref name="leftHeight"/> and <paramref name="rightHeight"/>.
        /// </summary>
        /// <param name="leftHeight">The left height.</param>
        /// <param name="rightHeight">The right height.</param>
        /// <returns>The height.</returns>
        private static int Max(int leftHeight, int rightHeight)
        {
            return leftHeight > rightHeight ? leftHeight : rightHeight;
        }

        /// <summary>
        /// Rotate subtree Left-Left
        /// </summary>
        /// <param name="parent">The parent <see cref="Node{T}"/>.</param>
        /// <returns>The pivot <see cref="Node{T}"/>.</returns>
        private static Node<T> RotateLL(Node<T> parent)
        {
            // (BW) Changed to 'var'
            var pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;

            return pivot;
        }

        /// <summary>
        /// Rotate subtree Left-Right
        /// </summary>
        /// <param name="parent">The parent <see cref="Node{T}"/>.</param>
        /// <returns>The pivot <see cref="Node{T}"/>.</returns>
        private static Node<T> RotateLR(Node<T> parent)
        {
            // (BW) Changed to 'var'
            var pivot = parent.Left;
            parent.Left = RotateRR(pivot);

            return RotateLL(parent);
        }

        /// <summary>
        /// Rotate subtree Right-Left
        /// </summary>
        /// <param name="parent">The parent <see cref="Node{T}"/>.</param>
        /// <returns>The pivot <see cref="Node{T}"/>.</returns>
        private static Node<T> RotateRL(Node<T> parent)
        {
            // (BW) Changed to 'var'
            var pivot = parent.Right;
            parent.Right = RotateLL(pivot);

            return RotateRR(parent);
        }

        /// <summary>
        /// Rotate subtree Right-Right
        /// </summary>
        /// <param name="parent">The parent <see cref="Node{T}"/>.</param>
        /// <returns>The pivot <see cref="Node{T}"/>.</returns>
        private static Node<T> RotateRR(Node<T> parent)
        {
            // (BW) Changed to 'var'
            var pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;

            return pivot;
        }

        /// <summary>
        /// Gets the <see cref="avl_tree.AvlTree{T}.Node{T}"/> at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index to search for.</param>
        /// <returns>
        /// The <see cref="avl_tree.AvlTree{T}.Node{T}"/> if found, <see langword="null"/> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
        private Node<T> GetNodeAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            ReIndex();
            var rtn = FindIndexRecursive(index, Root);
            return rtn;
        }

        /// <summary>
        /// This is used to add an <paramref name="item"/> to the tree.
        /// </summary>
        /// <remarks>
        /// The reason it has been pulled out of the public method, is to allow constructor access
        /// to it, as the public method is a virtual one.
        /// </remarks>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private bool InternalAdd(T item)
        {
            var rtn = false;

            if (item != null)
            {
                var newItem = new Node<T>(item);

                if (Root == null)
                {
                    Root = newItem;
                    rtn = true;
                }
                else
                {
                    Root = AddRecursive(Root, newItem, ref rtn, Balanced);
                }

                if (rtn)
                {
                    Count++;
                    version++;
                }
            }

            //Console.WriteLine($"Add({item}) : {rtn}");
            return rtn;
        }
    }
}