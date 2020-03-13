# GitPusher

A source control productivity booster. This program stores your current development branch and remote, both committing and pushing all changes with a single argument input. This is ideal for rapid commits and pushing when the development becomes intense.

## Getting Started

Compling yourself: Use Visual Studio and the project file to create either a new project to be modified or add to a solution for use as a DLL.

### Requires:

Git

This project is target for .NET 4.5. It should be compliant with earlier versions of course, however when compiling with VS, one will have to change the project settings.

Currently it is only developed for windows but linux development is eyeed for the future. See the [roadmap](## Roadmap) section below.

### Built With

* [Ini-Parser](https://github.com/rickyah/ini-parser) by [rickyah](https://github.com/rickyah) - The INI file parser library which is required to compile the software. [GitHub](https://github.com/rickyah/ini-parser) |  [NuGet](https://www.nuget.org/packages/ini-parser/)

### Installing

Copy the built app into your working directory. Git creditional storing is required in that project folder. Use with git "git config credential.helper store".

## Authors

* **Daniel Stamer-Squair** - *UaineTeine*

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Roadmap

**Alpha 1.4**

>error checking for change branch and remote names with feedback directly from the command call

>ability to merge branches