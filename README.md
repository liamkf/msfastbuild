![msfastbuild example image](http://liam.flookes.com/msfastbuild.png "msfastbuild example")

msfastbuild is a utility to make it easier to use [FASTBuild](https://github.com/fastbuild/fastbuild) with Visual Studio vcxproj and solution files. The goal is to be able to take advantage of FASTBuild's nifty caching and distribution without needing to hack out a bff file (FASTBuild's build file format) as it can be confusing at first and annoying if it's not your project or you already have everything in your project files.

The bff file it generates can also be a good starting point for a custom made bff file! It hashes the Visual Studio projects so it will only regenerate the bff file if things change and can take advantage of FASTBuild's speedy 'nothing to build' checks.

To use it you need Visual Studio 2015 R3 installed. You also need the [FBuild.exe](http://fastbuild.org/docs/download.html) either somewhere in your path, or to specify where it is using the -b argument on the commandline, or in VSIX msfastbuild options if you are using the VSIX. By default it passes -dist to FASTBuild, and relies on using the FASTBuild caching environment variables to control caching behaviour (i.e. it doesn't pass anything by default).

It's a work in progress, but it has been used successfully with most of the Visual Studio 2015 projects attempted. If you're not using the VSIX (plugin for Visual Studio 2015) which adds "FASTBuild Project" and "FASTBuild solution" to the Build menu, you can also use it from the commandline. If you were building say... SDL with the following command line:

$>msbuild SDL.sln /p:Configuration=Debug;Platform=Win32

with msfastbuild you can build with:

$>msfastbuild -s ./SDL.sln -c Debug -f Win32

Or to select only one project in the solution and with different FASTBuild arguments like so:

$>msfastbuild -s ./SDL.sln -p ./SDLMain/SDLMain.vcxproj -c Debug -f Win32 -a"-dist -ide"

msfastbuild should (in theory) parse the correct dependencies and build using FASTBuild, putting the output in the same place and using the same arguments as msbuild or Visual Studio would.

In practice there are quirks, some of which may be addressed as time goes by:

- You need Visual Studio 2015 R3 installed, even if you're building against the 2013 tools. Things requiring 2012 tools do not build correctly. They could build without a huge amount of work, it just hasn't been done.
- Pre-link build events are not supported: I don't see a good way to inject it into the BFF without modifying FASTBuild.
- The "Use Library Dependency Inputs" option for project dependencies is not supported. For the most part everything will still build, but /INCREMENTAL linking will not be as effective if you are changing the libraries frequently.
- The AssemblerListingLocation option for compiling is not supported, remote builders complain.
- The DesigntimePreprocessorDefinitions option for resource compiling is not supported.
- ProfileGuidedDatabase linking option is not supported as it was parsing strangely, and I think may require checking the global solution settings.
- Completely ignores MIDL.
- There's a lot of weird vcxproj/solutions doing weird things I'm not handling correctly.

Projects which seem to happily FASTBuild (some of which I updated to use VS2015):

- SDL2
- Zlib
- LZ4
- Doom3
- Beegame (which is mine... :))
- Handmade Quake
- Gross middleware
- cocos2d-x, required moving the prelink event to prebuild... which seemed fine, and adjusting some "int/\*bool\*/param" type constructs which preprocess oddly

There's a lot of room for improvment, but if there's a VS2015 project that doesn't work... let me know and I'll see if I can hack out why! Pull requests of course are very welcome! :)
