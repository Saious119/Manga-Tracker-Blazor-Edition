# Manga Tracker (Blazor Edition)

## Description
While we wait for .NET MAUI to become stable here is a Blazor verison of my App, you can use this to input the manga you own and keep track of progress!
It uses Cockroach DB and PostgreSQL in the background to store your Manga, just create an account (right now Discord OAUTH is all thats there, but more will be added in the future).

you will need dotnet 8 to use but it should work on all platforms.

just cd into the folder and use ```dotnet run``` and it should build and run, first time will be longer than subsequent times.

or if you grab a release just open the folder and run the executable file (for windows its a .exe for linux/mac its a shell script)

## Future work
- Currently trying to find a way to package this nicer so you won't have to download every update or have a terminal appear.
- Add more ways to log in (like gmail and twitter)
- Make the form for submissions easier
- Make it so the user doesn't have to provide their own connect strings and tokens
