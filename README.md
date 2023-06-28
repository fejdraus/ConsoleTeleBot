# ConsoleTeleBot
To run the Electron.Net version of the "Setup Wizard", you need to run the following command in the ConsoleTeleBotMaster project folder:
```
dotnet electronize start
```
In order to build the Electron.Net version of the "Configuration Wizard" for different operating systems, you need to run the following commands:
```
dotnet electronize build /target win
dotnet electronize build /target osx
dotnet electronize build /target linux
```
