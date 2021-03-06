/*
 *  File Name:   ReindexFailedException.cs
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
 * Date: 19/08/2021
 * ****************************************************************
 */

namespace MyNETCoreLib
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="ReindexFailedException" />.
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.Guid("06EF1B0D-C1CE-4A3F-A4CA-00DB0E5C28BA")]
    public class ReindexFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexFailedException"/> class.
        /// </summary>
        public ReindexFailedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexFailedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ReindexFailedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexFailedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or
        /// a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is
        /// specified.</param>
        public ReindexFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexFailedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
        /// that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" />
        /// that contains contextual information about the source or destination.</param>
        protected ReindexFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// When overridden in a derived class, sets the
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the
        /// exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that
        /// holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that
        /// contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}