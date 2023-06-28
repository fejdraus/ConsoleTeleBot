# ConsoleTeleBot
Для запуска Electron.Net версию "Мастера настройки", необходимо выпонить в папке проекта ConsoleTeleBotMaster команду:
dotnet electronize start

Для того что бы собрать Electron.Net версию "Мастера настройки" для разных ОС, необходимо выполнить команды:
dotnet electronize build /target win
dotnet electronize build /target osx
dotnet electronize build /target linux
