/*
 *  File Name:   TextBlockOutputter.cs
 *
 *  The copyright of the original code is undetermined. Therefore, to protect
 *  all of the code I am covering it under the GNU General Public License v3
 *  or later.
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
 * Date: 18/08/2021
 * ****************************************************************
 */

namespace MyWpfNETCoreLib
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Controls;

    /// <summary>
    /// Defines the <see cref="TextBlockOutputter" />.
    /// </summary>
    /// <remarks>
    /// The original code was copied from:
    /// https://social.technet.microsoft.com/wiki/contents/articles/12347.wpf-howto-add-a-debugoutput-console-to-your-application.aspx
    /// <code>
    /// // MainWindow.xaml
    ///<Window
    ///        xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation Jump "
    ///        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml Jump "
    ///        x:Class="WpfApplication68.MainWindow"
    ///        Title="MainWindow" Height="350" Width="525"
    ///        xmlns:local="clr-namespace:WpfApplication68">
    ///
    ///    <Grid>
    ///        <TextBox Height = "200" Width="400" x:Name="TestBox"/>
    ///    </Grid>
    ///
    ///</Window>
    ///
    /// // MainWindow.xaml.cs
    ///
    ///using System;
    ///using System.Windows;
    ///using System.Threading;
    ///
    ///namespace WpfApplication68
    ///{
    ///    public partial class MainWindow : Window
    ///    {
    ///        TextBoxOutputter outputter;
    ///
    ///        public MainWindow()
    ///        {
    ///            InitializeComponent();
    ///
    ///            outputter = new TextBoxOutputter(TestBox);
    ///            Console.SetOut(outputter);
    ///            Console.WriteLine("Started");
    ///
    ///            var timer1 = new Timer(TimerTick, "Timer1", 0, 1000);
    ///            var timer2 = new Timer(TimerTick, "Timer2", 0, 500);
    ///        }
    ///
    ///        void TimerTick(object state)
    ///        {
    ///            var who = state as string;
    ///            Console.WriteLine(who);
    ///        }
    ///
    ///    }
    ///}
    ///
    /// </code>
    /// </remarks>

    public class TextBlockOutputter : TextWriter
    {
        /// <summary>
        /// Defines the textBox.
        /// </summary>
        private readonly TextBlock textBlock;

        /// <summary>
        /// The counter
        /// </summary>
        private int counter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlockOutputter"/> class.
        /// </summary>
        /// <param name="output">The output<see cref="TextBox"/>.</param>
        public TextBlockOutputter(TextBlock output)
        {
            textBlock = output;
        }

        /// <summary>
        /// Gets the Encoding.
        /// </summary>
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        /// <summary>
        /// Writes a string to the text stream. If the given string is null, nothing
        /// is written to the text stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void WriteLine(string value)
        {
            if (value != null)
            {
                value = new StringBuilder($"{counter++} - ").AppendLine(value).ToString();
                Write(value.ToCharArray());
            }
        }

        /// <summary>
        /// Writes a character to the text stream.
        /// </summary>
        /// <param name="value">The character to write to the text stream.</param>
        public override void Write(char value)
        {
            base.Write(value);
            textBlock.Dispatcher.BeginInvoke(new Action(() =>
            {
                textBlock.Text = new StringBuilder(textBlock.Text).Append(value).ToString();
            }));
        }
    }
}