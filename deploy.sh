#!/bin/bash
rm -rf bin/Release/net6.0/*.zip 
dotnet publish -c Release -r linux-x64
cp -r bin/Release/net6.0/linux-x64/ /var/www/html