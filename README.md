
# Prog3 AT3 Project
This is a TAFE assignment for the Diploma in Software Development, at the South Metropolitan TAFE,
Rockingham, Western Australia.

The project is to implement a Client/Server application that provides a Login facility that
utilizes standard hashing techniques.

## Implementation

To show a possible use-case, I have implemented an Echo Chat facility, where the server
essentially echoes back what is sent to it.  To use this facility, the user must first login.

This project consists of five sub-projects and uses the following 3rd party libraries:

Projects:
- [Common][c]
- [GUI Client][g]
- [MyNETCoreLib][n]
- [MyWpfNETCoreLib][w]
- [SocketServer][s]

Third Party Libraries (as Nuget Packages):
- [CsvHelper][ch] v27.1.1
- [Microsoft.AspNetCore.Cryptography.KeyDerivation][kd] v5.0.11


### Common Library

This is a non-executable jar, containing a collection of classes that are in 
'common' use across multiple projects.

### GUI Client

This application allows the user to perform the following functions:

- Create a new account
- Login to an existing account
- Chat to the echo server (Socket Server)

### MyNETCoreLib

This library project was copied from another of my projects: [Prog3AT2-Two][p2].

This is a Class Library that contains the BBST: `AvlTree<T>` and the
`ObservableAvlTree<T>`.

### MyWpfNETCoreLib
This library project was copied from another of my projects: [Prog3AT2-Two][p2].

This is a WPF Class Library that contains a helper console window:
`LogConsole` and a supporting class: `TextBlockOutputter`.
Together they provide a means to redirect the `Console.Write{Line}()`
output to a window for easier debugging.

### Socket Server

A lot of the initial code for this project was ported from another of my projects:
[Java3AT2-Four][j4].

The functionality for the `Server` is a merging of that in the original two servers: 
`RMIServer` and `SocketServer`.  To achieve this, it was necessary to develop a
communications protocol, to handle the sequencing of commands and data.

This server application provides the following functionality:

- Create new accounts
- Validate login requests
- Store user account details in a CSV file
    - Username
    - Password: hashed
- Echo chat functionality.





[c]:https://github.com/bewillcott/Prog3AT3-Project/tree/master/Common
[g]:https://github.com/bewillcott/Prog3AT3-Project/tree/master/GUI_Client
[n]:https://github.com/bewillcott/Prog3AT3-Project/tree/master/MyNETCoreLib
[w]:https://github.com/bewillcott/Prog3AT3-Project/tree/master/MyWpfNETCoreLib
[s]:https://github.com/bewillcott/Prog3AT3-Project/tree/master/SocketServer

[ch]:https://joshclose.github.io/CsvHelper/
[kd]:https://www.nuget.org/packages/Microsoft.AspNetCore.Cryptography.KeyDerivation/

[p2]:https://github.com/bewillcott/Prog3AT2-Two
[j4]:https://github.com/bewillcott/Java3AT2-Four

