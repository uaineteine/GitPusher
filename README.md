# GitPusher

A source control productivity booster. This program stores your current development branch and remote, both committing and pushing all changes with a single argument input. This is ideal for rapid commits and pushing when the development becomes intense.

I developed this app during my honours year to minimise the risk to RSI.

![screen](https://raw.githubusercontent.com/uaineteine/GitPusher/release/screenshots/b2.0.3.png)

**Note:** this program is in beta bugs are to be expected until full release, however please report any findings to me and I will attempt fix them ASAP :)

### Requires:

Git- but if you're getting this here, then you should have this already.

This project is target for .NET core 2.1. It should be compliant with earlier versions of course, however when compiling with VS, one will have to change the project settings.

Currently it is only developed for Windows but linux support is eyed for the future. See the [roadmap](#Roadmap) section below.

### Built With

Since **Beta 2.0.3** the project has been built with a very simple INI parser library of my own design.

* [INIParser](https://bitbucket.org/uaineteinestudio/iniparser) by [UaineTeine](https://bitbucket.org/uaineteinestudio/) - The INI file parser library which is required to compile the software.

**Beta 1.0.3** and earlier versions required an Ini-Parser package:

* [Ini-Parser](https://github.com/rickyah/ini-parser) by [rickyah](https://github.com/rickyah) - The INI file parser library which is required to compile the software. [GitHub](https://github.com/rickyah/ini-parser) |  [NuGet](https://www.nuget.org/packages/ini-parser/)

### Installing

##### Building with Windows:

Use Visual Studio to build the project file into an executable, deploy to the working directory of any future project you are making.

##### Building with Linux:

Open terminal and use .NET core to build with:

```
dotnet publish -c release -r ubuntu.16.04-x64 --self-contained
```

For either platform, place the built app into your working directory and run. Git creditional storing is required in that project folder. Set with:
```
git config credential.helper store
```

## Authors

* **Daniel Stamer-Squair** - *UaineTeine*

Copyright © 2019-2021 Daniel Stamer-Squair
    
## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Donate

If you like my work and are feeling generous, you can leave me tip on ko-fi. Even the smallest donation is more than welcome and will make my day :)

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/C0C43PQ0I)

Alternatively you can become a patron :D

[![patroen](https://i.imgur.com/SWniXXj.png)](https://www.patreon.com/bePatron?u=51145413)

## Roadmap

**Beta 2.1**

* Complete linux use
* GUI app
