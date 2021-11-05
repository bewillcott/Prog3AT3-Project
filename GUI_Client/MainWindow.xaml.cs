/*
 *  File Name:   MainWindow.xaml.cs
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
    using System.Windows;

    using MyWpfNETCoreLib;

    using static Common.Constants;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        /// <summary>
        /// Defines the blankPage.
        /// </summary>
        private static readonly BlankPage blankPage = new BlankPage();

        /// <summary>
        /// The log console
        /// </summary>
        private readonly LogConsole logConsole;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetTitle();
            CentreFrame.Content = blankPage;
            SetStatusText(null);

            // Setup logging console
            logConsole = new();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the Session.
        /// </summary>
        public Session Session { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Set the Status line text.
        /// </summary>
        /// <param name="message">The message <see cref="string"/>.</param>
        public void SetStatusText(string message)
        {
            statusTextBlock.Text = message != null ? message : string.Empty;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Set the Window Title.
        /// </summary>
        private void SetTitle()
        {
            this.Title = ProductTitle;
        }

        #endregion Private Methods
    }
}
