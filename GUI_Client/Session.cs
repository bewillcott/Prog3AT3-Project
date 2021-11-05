/*
 *  File Name:   Session.cs
 *
 *  Project:     GUI_Client
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
 * Date: 29/10/2021
 * ****************************************************************
 */

namespace GUIClient
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    using Common;

    using static Common.Constants;
    using static Common.SessionConstants;

    public class Session : IDisposable, INotifyPropertyChanged
    {
        private readonly string serverName;
        private readonly int serverPort;
        private bool chatSessionOpen;
        private TcpClient clientSocket;
        private bool disposed;
        private StreamReader inStream;
        private bool loggedIn;
        private StreamWriter outStream;
        private string statusText;

        /// <summary>
        /// Initialize a new <see cref="Session"/> object.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="serverPort"></param>
        public Session(string serverName, int serverPort)
        {
            clientSocket = new();
            this.serverName = serverName;
            this.serverPort = serverPort;
        }

        ~Session()
        {
            Dispose(false);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether [chat session open].
        /// </summary>
        /// <value><c>true</c> if [chat session open]; otherwise, <c>false</c>.</value>
        public bool ChatSessionOpen
        {
            get => chatSessionOpen;
            private set
            {
                if (chatSessionOpen != value)
                {
                    chatSessionOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value><c>true</c> if this instance is connected; otherwise, <c>false</c>.</value>
        public bool IsConnected => clientSocket.Connected;

        /// <summary>
        /// Gets a value indicating whether [logged in].
        /// </summary>
        /// <value><c>true</c> if [logged in]; otherwise, <c>false</c>.</value>
        public bool LoggedIn
        {
            get => loggedIn;
            private set
            {
                if (loggedIn != value)
                {
                    loggedIn = value;
                    Log($"LoggedIn: {LoggedIn}");
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Any status text from the running session.
        /// </summary>
        public string StatusText
        {
            get => statusText;
            set
            {
                statusText = value;
                Log($"StatusText: {StatusText}");
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Session"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        private bool Connected
        {
            get
            {
                try
                {
                    Log(@"Check if connected ...");

                    if (clientSocket.Connected)
                    {
                        clientSocket.Close();
                        clientSocket = new();
                    }

                    clientSocket.Connect(serverName, serverPort);

                    // input stream from client
                    inStream = new(clientSocket.GetStream());

                    // output stream to client
                    outStream = new(clientSocket.GetStream());
                    outStream.AutoFlush = true;

                    StatusText = $"Connected to server: {clientSocket.Client.RemoteEndPoint.ToString()}";
                }
                catch (SocketException ex)
                {
                    Log(ex.Message);
                    StatusText = @"Connection to server failed!  Server is not running.";
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    StatusText = @"Connection to server failed!";
                }

                Log($"Connected: {clientSocket.Connected}");
                return clientSocket.Connected;
            }
        }

        /// <summary>
        /// Closes the chat session.
        /// </summary>
        public void CloseChat()
        {
            if (ChatSessionOpen)
            {
                Log(@"Close Chat Session");
                WriteLine(ChatClose);
                ChatSessionOpen = false;
            }
        }

        public void Dispose()
        {
            Log(@"Session.Dispose()");
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Logs in the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void Login(string username, string password)
        {
            Log($"Login({username}, {password})");

            if (Connected)
            {
                WriteLine($"{LoginRequest}:{username}:{password}");

                string incoming = ReadLine();

                Log(incoming);
                string[] parts = incoming.Split(':');

                switch (parts[0])
                {
                    case LoginOK:
                        {
                            StatusText = @"Login successful";
                            LoggedIn = true;
                            break;
                        }

                    case LoginFailed:
                        {
                            StatusText = @"The login attempt failed!";
                            LoggedIn = false;
                            break;
                        }

                    case BadRequest:
                        {
                            StatusText = @"Either the Username and/or Password missing";
                            LoggedIn = false;
                            break;
                        }

                    case InvalidRequest:
                        {
                            StatusText = @"Already logged in!";
                            LoggedIn = false;
                            break;
                        }

                    default:
                        {
                            StatusText = $"Unexpected response from the server! {incoming}";
                            LoggedIn = false;
                            break;
                        }
                }
            }

            Log($"{StatusText}");
            Log($"LoggedIn: {LoggedIn}");
        }

        /// <summary>
        /// Creates a new user account, and logs in the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void NewAccount(string username, string password)
        {
            Log($"NewAccount({username}, {password})");

            if (Connected)
            {
                WriteLine($"{NewUserRequest}:{username}:{password}");

                string incoming = ReadLine();

                Log(incoming);
                string[] parts = incoming.Split(':');

                switch (parts[0])
                {
                    case NewUserOK:
                        {
                            StatusText = @"New Account created";
                            LoggedIn = true;
                            break;
                        }

                    case NewUserFailed:
                        {
                            StatusText = @"Failed to create the new account!";
                            LoggedIn = false;
                            break;
                        }

                    case BadRequest:
                        {
                            StatusText = @"Either the Username and/or Password missing";
                            LoggedIn = false;
                            break;
                        }

                    case InvalidRequest:
                        {
                            StatusText = @"Already logged in!";
                            LoggedIn = false;
                            break;
                        }

                    default:
                        {
                            StatusText = $"Unexpected response from the server! {incoming}";
                            LoggedIn = false;
                            break;
                        }
                }
            }

            Log($"{StatusText}");
            Log($"LoggedIn: {LoggedIn}");
        }

        /// <summary>
        /// Opens the chat session.
        /// </summary>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        public bool OpenChat()
        {
            Log(@"Open Chat Session");

            if (LoggedIn)
            {
                // Open Chat Session
                WriteLine(ChatRequest);

                string incoming = ReadLine();

                Log(incoming);
                string[] parts = incoming.Split(':');

                switch (parts[0])
                {
                    case ChatOK:
                        {
                            StatusText = parts[1];
                            ChatSessionOpen = true;
                            break;
                        }

                    case ChatDenied:
                        {
                            StatusText = @"Last login attempt failed - Chat denied!";
                            ChatSessionOpen = false;
                            break;
                        }

                    case BadRequest:
                        {
                            StatusText = @"There was an error connecting to the server!";
                            ChatSessionOpen = false;
                            break;
                        }

                    case InvalidRequest:
                        {
                            StatusText = @"Sorry, you haven't logged in yet!";
                            ChatSessionOpen = false;
                            break;
                        }

                    default:
                        {
                            StatusText = $"Unexpected response from the server! {incoming}";
                            ChatSessionOpen = false;
                            break;
                        }
                }
            }

            Log($"{StatusText}");
            Log($"ChatSessionOpen: {ChatSessionOpen}");

            return ChatSessionOpen;
        }

        /// <summary>
        /// Reads a line of characters from the server and returns the data as a string.
        /// </summary>
        /// <returns>
        /// The next line of input from the server, or <c>null</c> if the end of the input is reached.
        /// </returns>
        public string ReadLine() => inStream.ReadLine();

        /// <summary>
        /// Write the <see cref="string"/> of <paramref name="text"/>, followed by a line
        /// terminator, to the server.
        /// </summary>
        /// <param name="text">String to send to the server</param>
        public void WriteLine(string text)
        {
            outStream.WriteLine(text);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (clientSocket.Connected)
                {
                    // Disconnect from Server
                    LogOut();
                    outStream.Flush();
                    outStream.Close();
                    outStream.Dispose();

                    inStream.Close();
                    inStream.Dispose();
                }

                // Cleanup
                clientSocket.Close();
                clientSocket.Dispose();
                disposed = true;
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Logs out of the server session.
        /// </summary>
        private void LogOut()
        {
            CloseChat();

            if (LoggedIn)
            {
                Log(@"Logout of the Session");
                WriteLine(SessionConstants.LogOut);
                LoggedIn = false;
            }
        }
    }
}
