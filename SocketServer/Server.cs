/*
 *  File Name:   Server.cs
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
 * Date: 27/10/2021
 * ****************************************************************
 */

namespace SocketServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using static Common.Constants;
    using static Common.SessionConstants;

    /// <summary>
    /// Defines the <see cref="Server" />.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        private static void Main(string[] args)
        {
            IPAddress ipAddress = Dns.GetHostEntry(ServerName).AddressList[0];
            IPEndPoint endPoint = new(ipAddress, ServerPort);
            TcpListener serverSocket = null;
            UserAuthenticator userAuthenticator = new();

            try
            {
                // Create a socket that will use TCP protocol
                serverSocket = new(endPoint);

                serverSocket.Start();

                Log($"\n{DoubleLine}");
                Log($"{TitleIndent}Java3 AT2 Four - Socket Server ({Common.Constants.Version})");
                Log($"{DoubleLine}\n");
                Log($"Server is listening on port {((IPEndPoint)serverSocket.Server.LocalEndPoint).Port.ToString()}");
                Log($"{Line}\n");

                bool keepServerAlive = true;

                // -----------------
                // Connection Loop
                // -----------------
                while (keepServerAlive)
                {
                    // wait, listen and accept connection
                    Log("Waiting for a connection...");

                    using (TcpClient clientSocket = serverSocket.AcceptTcpClient())
                    {
                        SessionState sessionState = new();
                        sessionState.ClientHostName = ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address.ToString(); // client name
                        sessionState.ClientPortNumber = ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Port; // port used

                        Log($"[{sessionState.SessionId}] Connected from {sessionState.ClientHostName} on {sessionState.ClientPortNumber}");

                        try
                        {
                            // input stream from client
                            using (StreamReader inStream = new(clientSocket.GetStream()))
                            // output stream to client
                            using (StreamWriter outStream = new(clientSocket.GetStream()))
                            {
                                outStream.AutoFlush = true;

                                // --------------
                                // Session Loop
                                // --------------
                                do
                                {
                                    // Get Client request string
                                    string incoming = inStream.ReadLine();

                                    Log(incoming);
                                    string[] request = incoming.Split(':');

                                    switch (request[0])
                                    {
                                        case LoginRequest:
                                            {
                                                // Process Login request
                                                if (sessionState.Username == null)
                                                {
                                                    if (request.Length == 3)
                                                    {
                                                        if (userAuthenticator.ValidatePassword(sessionState, request[1], request[2]))
                                                        {
                                                            // Logged in!
                                                            sessionState.Message = LoginOK;
                                                        }
                                                        else
                                                        {
                                                            // Password is wrong
                                                            sessionState.Message = LoginFailed;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Either username &/or password missing
                                                        sessionState.Message = BadRequest;
                                                    }
                                                }
                                                else
                                                {
                                                    // Already logged in
                                                    sessionState.Message = InvalidRequest;
                                                }

                                                break;
                                            }

                                        case NewUserRequest:
                                            {
                                                // Process new account request
                                                if (sessionState.Username == null)
                                                {
                                                    if (request.Length == 3)
                                                    {
                                                        if (userAuthenticator.CreateNewAccount(sessionState, request[1], request[2]))
                                                        {
                                                            // Logged in!
                                                            sessionState.Message = NewUserOK;
                                                        }
                                                        else
                                                        {
                                                            // Password is wrong
                                                            sessionState.Message = NewUserFailed;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Either username &/or password missing
                                                        sessionState.Message = BadRequest;
                                                    }
                                                }
                                                else
                                                {
                                                    // Already logged in
                                                    sessionState.Message = InvalidRequest;
                                                }

                                                break;
                                            }

                                        case ChatRequest:
                                            {
                                                // Setup for Chat session
                                                if (sessionState.CanOpenChat())
                                                {
                                                    // Chat session is Open!
                                                    sessionState.ChatOpen = true;

                                                    // Notify client of connection success.
                                                    String msg = $":[{sessionState.SessionId}] "
                                                        + $"You have connected to Chat server {Common.Constants.Version}";
                                                    sessionState.Message = ChatOK + msg;
                                                }
                                                else if (sessionState.Username != null && !sessionState.LoggedIn)
                                                {
                                                    // Sorry, can't let you in
                                                    sessionState.Message = ChatDenied;
                                                }
                                                else
                                                {
                                                    // Sorry, can't let you in
                                                    sessionState.Message = InvalidRequest;
                                                }

                                                break;
                                            }

                                        case LogOut:
                                            {
                                                sessionState.Message = @"Client logging out";
                                                sessionState.SetPasswordValid(false);
                                                break;
                                            }

                                        default:
                                            {
                                                // Unknown request
                                                sessionState.Message = InvalidRequest;
                                                break;
                                            }
                                    }

                                    if (clientSocket.Client.Connected)
                                    {
                                        // Send message to Client
                                        outStream.WriteLine(sessionState.Message);
                                    }

                                    Log(sessionState.Message);

                                    // Chat Session?
                                    if (sessionState.ChatOpen)
                                    {
                                        // -------------------
                                        // Chat Session Loop
                                        // -------------------
                                        while (sessionState.ChatOpen)
                                        {
                                            String inLine = inStream.ReadLine(); // read a line from client
                                            Log($"[{sessionState.SessionId}] Received from client: {inLine}");

                                            if (inLine == null)
                                            {
                                                Log($"[{sessionState.SessionId}] Client disconnected uncleanly!");
                                                Log($"{Line}\n");
                                                sessionState.ChatOpen = false;
                                            }
                                            else
                                            {
                                                switch (inLine)
                                                {
                                                    case ChatClose:
                                                        {
                                                            Log($"[{sessionState.SessionId}] Client closed session.");
                                                            Log($"{Line}\n");
                                                            sessionState.ChatOpen = false;
                                                            break;
                                                        }

                                                    case LogOut:
                                                        {
                                                            Log($"[{sessionState.SessionId}] Client logging out.");
                                                            Log($"{Line}\n");
                                                            sessionState.ChatOpen = false;
                                                            break;
                                                        }

                                                    default:
                                                        {
                                                            // Reply to client
                                                            String outLine = $"[{sessionState.SessionId}] You said '{inLine}'";
                                                            outStream.WriteLine(outLine); // send a message to client
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                } while (sessionState.LoggedIn);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log(ex.ToString());
                        }

                        clientSocket.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            finally
            {
                if (serverSocket != null)
                    serverSocket.Stop();
            }
        }
    }
}