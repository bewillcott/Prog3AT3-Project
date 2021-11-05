/*
 *  File Name:   NewAccountPage.xaml.cs
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for NewAccountPage.xaml
    /// </summary>
    public partial class NewAccountPage : Page
    {
        #region Private Fields

        private const string Error = @"Error";
        private static readonly Brush ErrorBrush = Brushes.Red;
        private static readonly Brush DefaultBrush = SystemColors.ControlTextBrush;
        private Session session;

        #endregion Private Fields

        #region Public Constructors

        public NewAccountPage(Session session)
        {
            InitializeComponent();
            this.session = session;
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Handles the Click event of the ResetButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            usernameTextBox.Clear();
            firstPasswordBox.Clear();
            secondPasswordBox.Clear();

            if (userNameLabel.Tag != null && ((string)userNameLabel.Tag).Equals(Error))
            {
                userNameLabel.Foreground = DefaultBrush;
                userNameLabel.Tag = null;
            }

            if (firstPasswordLabel.Tag != null && ((string)firstPasswordLabel.Tag).Equals(Error))
            {
                firstPasswordLabel.Foreground = DefaultBrush;
                firstPasswordLabel.Tag = null;
            }

            if (secondPasswordLabel.Tag != null && ((string)secondPasswordLabel.Tag).Equals(Error))
            {
                secondPasswordLabel.Foreground = DefaultBrush;
                secondPasswordLabel.Tag = null;
            }

            session.StatusText = @"";
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">     
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string password = firstPasswordBox.Password;

            if (usernameTextBox.Text.Length == 0)
            {
                session.StatusText = @"You must enter a username.";
                usernameTextBox.Focus();
                userNameLabel.Foreground = ErrorBrush;
                userNameLabel.Tag = Error;
            }
            else
            {
                if (userNameLabel.Tag != null && ((string)userNameLabel.Tag).Equals(Error))
                {
                    userNameLabel.Foreground = DefaultBrush;
                    userNameLabel.Tag = null;
                }

                if (password.Length == 0)
                {
                    session.StatusText = @"You must enter a password.";
                    firstPasswordBox.Focus();
                    firstPasswordBox.SelectAll();
                    firstPasswordLabel.Foreground = ErrorBrush;
                    firstPasswordLabel.Tag = Error;
                }
                else
                {
                    if (firstPasswordLabel.Tag != null && ((string)firstPasswordLabel.Tag).Equals(Error))
                    {
                        firstPasswordLabel.Foreground = DefaultBrush;
                        firstPasswordLabel.Tag = null;
                    }

                    if (!password.Equals(secondPasswordBox.Password))
                    {
                        session.StatusText = @"The second password entry is different to the first.";
                        secondPasswordBox.Focus();
                        secondPasswordBox.SelectAll();
                        secondPasswordLabel.Foreground = ErrorBrush;
                        secondPasswordLabel.Tag = Error;
                    }
                    else
                    {
                        if (secondPasswordLabel.Tag != null && ((string)secondPasswordLabel.Tag).Equals(Error))
                        {
                            secondPasswordLabel.Foreground = DefaultBrush;
                            secondPasswordLabel.Tag = null;
                        }

                        session.StatusText = @"";
                        session.NewAccount(usernameTextBox.Text, firstPasswordBox.Password);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
