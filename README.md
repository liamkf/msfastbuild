![msfastbuild example image](http://liam.flookes.com/msfastbuild.png "msfastbuild example")

*** Archived ***

msfastbuild is a utility to make it easier to use [FASTBuild](https://github.com/fastbuild/fastbuild) with Visual Studio 2015 vcxproj and solution files. The goal is to be able to take advantage of FASTBuild's nifty caching and distribution without needing to hack out a bff file (FASTBuild's build file format) as it can be confusing at first and annoying if it's not your project or you already have everything in your project files.

The bff file it generates can also be a good starting point for a custom made bff file. msfastbuild hashes the Visual Studio projects so it will only regenerate the bff file if things change and can take advantage of FASTBuild's speedy 'nothing to build' checks.

It's a work in progress, but it has been used successfully for many Visual Studio 2015 projects. The VSIX plugin for Visual Studio 2015 adds "FASTBuild Project" and "FASTBuild solution" options to the Build menu, and it should also be usable from the commandline.

## Setup

To use msfastbuild you need Visual Studio 2015 R3 installed. It relies on being able to find msbuild.

You need the [FBuild.exe](http://fastbuild.org/docs/download.html) either somewhere in your path, or to specify where it is using the -b argument on the commandline, or in VSIX msfastbuild options if you are using the VSIX. 

By default msfastbuild passes -dist to FASTBuild. This can be overriden in the VSIX options or with the -a option on the command line.

## VSIX Usage

To use msfastbuild from within Visual Studio, simply install the VSIX and either set the FBuild.exe path in the VSIX options, or have it in your path.

## Command line usage

If you were building say... SDL with the following command line:

`$>msbuild SDL.sln /p:Configuration=Debug;Platform=Win32`

with msfastbuild you would build with:

`$>msfastbuild -s ./SDL.sln -c Debug -f Win32`

To select only one project in the solution and provide different FASTBuild arguments:

`$>msfastbuild -s ./SDL.sln -p ./SDLMain/SDLMain.vcxproj -c Debug -f Win32 -a"-dist -ide"`

## Quirks

Some of these may be addressed as time goes by:

- You need Visual Studio 2015 R3 installed, even if you're building against the 2013 tools. Things requiring 2012 tools do not build correctly. They could build without a huge amount of work, it just hasn't been done.
- Pre-link build events are not supported: I don't see a good way to inject it into the BFF without modifying FASTBuild.
- The "Use Library Dependency Inputs" option for project dependencies is not supported. For the most part everything will still build, but /INCREMENTAL linking will not be as effective if you are changing the libraries frequently.
- The AssemblerListingLocation option for compiling is not supported, remote builders complain.
- The DesigntimePreprocessorDefinitions option for resource compiling is not supported.
- ProfileGuidedDatabase linking option is not supported as it was parsing strangely, and I think may require checking the global solution settings.
- Completely ignores MIDL.
- There's a lot of weird vcxproj/solutions doing weird things I'm not handling correctly.

## Buildable projects

Some of these were modified to use VS2015:

- SDL2
- Zlib
- LZ4
- Doom3
- Beegame (which is mine... :))
- Handmade Quake
- Gross middleware
- cocos2d-x, required moving the prelink event to prebuild... which seemed fine, and adjusting some "int/\*bool\*/param" type constructs which preprocess oddly

## No longer being updated or supported.

There remains a lot of room for improvement, however I currently have no current plans (as of November 2022) to update this to support later versions of Visual Studio. There have been many promising forks looking to add support for later versions that interested parties may want to peruse!
