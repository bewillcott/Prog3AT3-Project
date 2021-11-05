/*
 *  File Name:   LogConsole.xaml.cs
 *
 *  The copyright of the original code is undetermined. Therefore, to protect
 *  all of the code I am covering it under the GNU General Public License v3
 *  or later.
 *
 *  Copyright (c) 2016 Matt McManis, 2021 Bradley Willcott
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
 * Date: 18/08/2021
 * ****************************************************************
 */

namespace MyWpfNETCoreLib
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    /// <summary>
    /// Interaction logic for LogConsole.xaml.
    /// </summary>
    /// <remarks>
    /// Original code copied from:
    /// https://stackoverflow.com/questions/41277425/how-to-make-simple-read-only-output-log-console-for-wpf
    /// </remarks>
    public partial class LogConsole : Window
    {
        /// <summary>
        /// The outputter
        /// </summary>
        private readonly TextBlockOutputter outputter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogConsole"/> class.
        /// </summary>
        public LogConsole()
        {
            InitializeComponent();

            outputter = new(OutputBlock);
            Console.SetOut(outputter);
            WriteLine(nameof(LogConsole));
            WriteLine("==========\n");
            Console.WriteLine("Started");
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(string text)
        {
            OutputBlock.Inlines.Add(text);
        }

        /// <summary>
        /// Writes the specified text line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(string text)
        {
            OutputBlock.Inlines.Add($"{text}\n");
        }
    }
}