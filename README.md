# ConsoleTeleBot

The application launches console applications, intercepts their output, and compares it using Regix rules with a database of rules. After finding matches, data is extracted from the string using Regix and inserted into the response text. The resulting response is sent to Telegram using the Telegram Bot API.

Додаток запускає консольні програми, перехоплює їх вивід і порівнює його, використовуючи Regix правила, з базою даних правил. Після знаходження збігів, дані витягуються з рядка, використовуючи Regix і вставляються у текст відповіді. Результуюча відповідь відправляється в Telegram, використовуючи Telegram Bot API.

To run the Electron.Net version of the "Setup Wizard", you need to run the following command in the ConsoleTeleBotWizard project folder:
```
dotnet electronize start
```
In order to build the Electron.Net version of the "Configuration Wizard" for different operating systems, you need to run the following commands:
```
dotnet electronize build /target win
dotnet electronize build /target osx
dotnet electronize build /target linux
```
