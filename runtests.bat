set ShouldPause=1
set FBPath="E:\fastbuild-dev\fastbuild\tmp\x64-Release\Tools\FBuild\FBuildApp\FBuild.exe"
set FBArgs="-dist -ide -clean"
cd %~dp0\tests\bees\solution
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p bees.vcxproj -c Debug -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
cd %~dp0\tests\SDL2-2.0.4\VisualC\SDL\
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p SDL.vcxproj -c Debug -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p SDL.vcxproj -c Release -f x64 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
cd %~dp0\tests\lz4-r131\visual\2012\lz4\
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p lz4.vcxproj -c Debug -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
cd %~dp0\tests\notepad-plus-plus\PowerEditor\visual.net\
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p notepadPlus.vs2015.vcxproj -c "Unicode Debug" -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
cd %~dp0\tests\zlib-1.2.8\contrib\vstudio\vc11\
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p zlibstat.vcxproj -c Debug -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p zlibstat.vcxproj -c Release -f x64 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p zlibvc.vcxproj -c Debug -f Win32 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause
"%~dp0/msfastbuild/bin/Debug/msfastbuild.exe" -p zlibvc.vcxproj -c Release -f x64 -a%FBArgs% -b%FBPath%
IF DEFINED ShouldPause pause