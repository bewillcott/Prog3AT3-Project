/*
 *  File Name:   SessionConstants.cs
 *
 *  Project:     Common
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

namespace Common
{
    /// <summary>
    /// Defines the <see cref="SessionConstants" />.
    /// </summary>
    public static class SessionConstants
    {
        /// <summary>
        /// Sent to Client when Client's request was corrupted or not correctly formatted.
        /// </summary>
        public const string BadRequest = @"BAD_REQUEST";

        /// <summary>
        /// Defines the ChatClose.
        /// </summary>
        public const string ChatClose = @"CHAT_CLOSE";

        /// <summary>
        /// Defines the ChatDenied.
        /// </summary>
        public const string ChatDenied = @"CHAT_DENIED";

        /// <summary>
        /// Defines the ChatOK.
        /// </summary>
        public const string ChatOK = @"CHAT_OK";

        /// <summary>
        /// Defines the ChatRequest.
        /// </summary>
        public const string ChatRequest = @"CHAT_REQUEST";

        /// <summary>
        /// Sent to Client when Client has made a request out of sequence.
        /// </summary>
        public const string InvalidRequest = @"INVALID_REQUEST";

        /// <summary>
        /// Defines the LoginFailed.
        /// </summary>
        public const string LoginFailed = @"LOGIN_FAILED";

        /// <summary>
        /// Defines the LoginOK.
        /// </summary>
        public const string LoginOK = @"LOGIN_OK";

        /// <summary>
        /// Defines the LoginRequest.
        /// </summary>
        public const string LoginRequest = @"LOGIN_REQUEST";

        /// <summary>
        /// Defines the LogOut.
        /// </summary>
        public const string LogOut = @"LOG_OUT";

        /// <summary>
        /// Defines the NewUserFailed.
        /// </summary>
        public const string NewUserFailed = @"NEW_USER_FAILED";

        /// <summary>
        /// Defines the NewUserOK.
        /// </summary>
        public const string NewUserOK = @"NEW_USER_OK";

        /// <summary>
        /// Defines the NewUserRequest.
        /// </summary>
        public const string NewUserRequest = @"NEW_USER_REQUEST";

        /// <summary>
        /// Defines the Password.
        /// </summary>
        public const string Password = @"PASS_WORD";

        /// <summary>
        /// Defines the UserName.
        /// </summary>
        public const string UserName = @"USER_NAME";
    }
}