/*
 *  File Name:   Pbkdf2Tests.cs
 *
 *  Project:     SocketServerTests
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
 * Date: 1/11/2021
 * ****************************************************************
 */

namespace SocketServer.Tests
{
    using Xunit;

    using static SocketServer.Pbkdf2;

    public class Pbkdf2Tests
    {
        #region Public Methods

        [Theory]
        [InlineData("Sdgr5#6778fFttW#r")]
        [InlineData("/ dlsf shfsle;jfdi")]
        [InlineData("8fn4w92md zekjd9a093d")]
        public void CreateHash_SamePassword_ReturnsDifferentHash(string password)
        {
            // Act
            string hash = CreateHash(password);
            string secondHash = CreateHash(password);
            bool result = hash.Equals(secondHash);

            // Assert
            Assert.False(result, "Two hashes are equal.");
        }

        [Theory]
        [InlineData("Sdgr5#6778fFttW#r")]
        [InlineData("/ dlsf shfsle;jfdi")]
        [InlineData("8fn4w92md zekjd9a093d")]
        public void VerifyPassword_SamePassword_ReturnsTrue(string password)
        {
            // Act
            string hash = CreateHash(password);
            bool result = VerifyPassword(password, hash);

            // Assert
            Assert.True(result, "Good password not accepted.");
        }

        [Theory]
        [InlineData("Sdgr5#6778fFttW#r")]
        [InlineData("/ dlsf shfsle;jfdi")]
        [InlineData("8fn4w92md zekjd9a093d")]
        public void VerifyPassword_WrongPassword_ReturnsFalse(string password)
        {
            // Arrange
            const string testPassword = @"I am wrong!";

            // Act
            string hash = CreateHash(password);
            bool result = VerifyPassword(testPassword, hash);

            // Assert
            Assert.False(result, "Wrong password accepted.");
        }

        #endregion Public Methods
    }
}
