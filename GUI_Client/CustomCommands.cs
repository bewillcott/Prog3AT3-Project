/*
 *  File Name:   CustomCommands.cs
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
 * Date: 27/10/2021
 * ****************************************************************
 */

namespace GUIClient
{
    using System.Windows.Input;

    /// <summary>
    /// Defines the <see cref="CustomCommands" />.
    /// </summary>
    public static class CustomCommands
    {
        /// <summary>
        /// Defines the About.
        /// </summary>
        public static readonly RoutedUICommand About =
            new(@"_About", nameof(About), typeof(CustomCommands));

        /// <summary>
        /// Defines the Exit.
        /// </summary>
        public static readonly RoutedUICommand Exit =
            new(
                        @"E_xit",
                        nameof(Exit),
                        typeof(CustomCommands),
                        new InputGestureCollection {
                        new KeyGesture(Key.F4, ModifierKeys.Alt)});

        /// <summary>
        /// Defines the Login.
        /// </summary>
        public static readonly RoutedUICommand Login =
            new(
                        @"_Login",
                        nameof(Login),
                        typeof(CustomCommands),
                        new InputGestureCollection {
                        new KeyGesture(Key.L, ModifierKeys.Control)});

        /// <summary>
        /// Defines the Logout.
        /// </summary>
        public static readonly RoutedUICommand Logout =
            new(
                        @"Log_out",
                        nameof(Logout),
                        typeof(CustomCommands),
                        new InputGestureCollection {
                        new KeyGesture(Key.O, ModifierKeys.Control)});

        /// <summary>
        /// Defines the Logout.
        /// </summary>
        public static readonly RoutedUICommand NewAccount =
            new(
                        @"_New Account",
                        nameof(NewAccount),
                        typeof(CustomCommands),
                        new InputGestureCollection {
                        new KeyGesture(Key.N, ModifierKeys.Control)});

        /// <summary>
        /// Defines the Send.
        /// </summary>
        public static readonly RoutedUICommand Send =
            new(@"_Send", nameof(Send), typeof(CustomCommands));
    }
}