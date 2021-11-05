/*
 *  File Name:   ObservableAvlTree.cs
 *
 *  Copyright (c) 2021 Bradley Willcott
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * ****************************************************************
 * Name: Bradley Willcott
 * ID:   M198449
 * Date: 17/08/2021
 * ****************************************************************
 */

namespace MyNETCoreLib
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Defines the <see cref="ObservableAvlTree{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ObservableAvlTree<T> : AvlTree<T>, INotifyCollectionChanged where T : IComparable<T>
    {
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Add new data to tree node and re-balance
        /// uses the Balance_Tree and Rotate methods.
        /// </summary>
        /// <param name="item">The data<see cref="E:System.String" />.</param>
        /// <returns>
        ///   <c>true</c> unless <paramref name="item" /> is <c>null</c> or a duplicate.
        /// </returns>
        /// <remarks>
        /// <b>Note:</b>
        /// <br />
        /// Neither <see langword="null" />s nor duplicates are allowed.
        /// </remarks>
        public override bool Add(T item)
        {
            Console.WriteLine($"Add({item}) : Count={Count}");
            var rtn = base.Add(item);
            Console.WriteLine($"Add({item}) : Count={Count}");

            if (rtn)
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            }

            return rtn;
        }

        /// <summary>
        /// Removes all items from the <see cref="E:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        /// Delete the <paramref name="target" /> from the tree.
        /// </summary>
        /// <param name="target">The target<see cref="E:System.String" />.</param>
        /// <returns>
        /// <c>true</c> unless <paramref name="target" /> is <c>null</c>, or <paramref name="target" />
        /// is not found.
        /// </returns>
        public override bool Delete(T target)
        {
            Console.WriteLine($"override Delete({target})");

            var index = IndexOf(target);
            var rtn = base.Delete(target);

            if (rtn)
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, target, index);
            }

            return rtn;
        }

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action));

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="changedItem">The changed item.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object changedItem) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem));

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="changedItem">The changed item.</param>
        /// <param name="index">The index.</param>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object changedItem, int index) =>
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem, index));

        /// <summary>
        /// Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="args">The args <see cref="NotifyCollectionChangedEventArgs"/>.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args) =>
            CollectionChanged?.Invoke(this, args);
    }
}