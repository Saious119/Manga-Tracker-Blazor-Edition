# Manga Tracker (Blazor Edition)

## Description
While we wait for .NET MAUI to become stable here is a Blazor verison of my App, you can use this to input the manga you own and keep track of progress!
It uses Mongo DB in the background to store your Manga, just create an account (right now local accounts and google OAuth are avaiable).

you will need dotnet 6 to use but it should work on all platforms.

just cd into the folder and use ```dotnet run``` and it should build and run, first time will be longer than subsequent times.

## Future work
- Currently trying to find a way to package this nicer so you won't have to download and run my code from a terminal.
- Add more ways to log in (like discord and twitter)
- make the form for submissions easier

## Warning for Add manga forms
There are currently no labels (sorry about that)
the order for what goes in what box is <Name of Manga> <Author> <Volumes read> <total volumes owned or read> 