
# Metatrader Autosaver
## _A bot that automatically saves on Metatrader when I save on Visual Code._


![Made with C#enter code here](https://github.com/joelachankeng/Metatrader-Autosaver/blob/main/csharp.png)


This bot was made via console C#.

## Problem

I hate Metatrader 4 IDE. 
It doesn't come with an intellisense, linter, autoformatter, or any of the beneficial plugins that Visual Code has.

So I develop Metatrader scripts and expert advisors with Visual Code.

However, it is a pain and waste of productive time to switch back to Metatrader 4 IDE and click the Compile button every time I made a code change.

## Solution
- Bot detects when Visual Code is focused.
- Bot listens for the Control + S key.
- Bot triggers F7 key (compile button shortcut) on Metatrader Application

## Tech


- [Microsoft Visual Studio Community 2019](https://visualstudio.microsoft.com/downloads/)
- UIAutomationClient.dll located in `~\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\UIAutomationClient.dll`

## Installation

Metatrader Autosaver requires [.NET Core 3.1+](https://dotnet.microsoft.com/en-us/download/dotnet/3.1) to run.

Open the solution in Visual Studio and Start Debugging.
Or
Open exe file located in `\bin\Debug\netcoreapp3.1`


## Development

Want to contribute? Great!

Send me a message


## License

MIT

**Free Software, Hell Yeah!**


