/*
 *  File Name:   SessionState.cs
 *
 *  Project:     SocketServer
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
 * Date: 28/10/2021
 * ****************************************************************
 */

namespace SocketServer
{
    /// <summary>
    /// Holds state information about a single current session.
    /// </summary>
    public class SessionState
    {
        /// <summary>
        /// Defines the lastSessionId.
        /// </summary>
        private static int lastSessionId = 0;

        private bool passwordvalid;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionState"/> class.
        /// </summary>
        public SessionState()
        {
            SessionId = ++lastSessionId;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a Chat session is Open.
        /// </summary>
        public bool ChatOpen { get; set; }

        /// <summary>
        /// Gets or sets the ClientHostName.
        /// </summary>
        public string ClientHostName { get; set; }

        /// <summary>
        /// Gets or sets the ClientPortNumber.
        /// </summary>
        public int ClientPortNumber { get; set; }

        /// <summary>
        /// Gets a value indicating whether [logged in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [logged in]; otherwise, <c>false</c>.
        /// </value>
        public bool LoggedIn => passwordvalid;

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public int SessionId { get; private set; }

        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Is a Chat session allowed?.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CanOpenChat()
        {
            return LoggedIn && !ChatOpen;
        }

        public void SetPasswordValid(bool value)
        {
            passwordvalid = value;
        }
    }
}