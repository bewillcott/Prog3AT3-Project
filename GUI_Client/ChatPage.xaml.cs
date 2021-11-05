/*
 *  File Name:   ChatPage.xaml.cs
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
 * Date: 28/10/2021
 * ****************************************************************
 */

namespace GUIClient
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    using static Common.SessionConstants;

    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private Session session;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatPage"/> class.
        /// </summary>
        public ChatPage(Session session)
        {
            this.session = session;
            session.OpenChat();
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the sendButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Send message to chat server
            session.WriteLine(outgoingMsgTextBlock.Text);

            // Display message in local outgoing message list
            outgoingDoc.Blocks.Add(new Paragraph(new Run(outgoingMsgTextBlock.Text)));
            outgoingMsgTextBlock.Clear();

            string incoming = session.ReadLine();

            // Display replay from the chat server
            incomingDoc.Blocks.Add(new Paragraph(new Run(incoming)));
        }

        private void Send_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = session.ChatSessionOpen;
        }

        private void Send_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // Send message to chat server
            session.WriteLine(outgoingMsgTextBlock.Text);

            // Display message in local outgoing message list
            outgoingDoc.Blocks.Add(new Paragraph(new Run(outgoingMsgTextBlock.Text)));
            outgoingMsgTextBlock.Clear();

            string incoming = session.ReadLine();

            // Display replay from the chat server
            incomingDoc.Blocks.Add(new Paragraph(new Run(incoming)));
        }
    }
}