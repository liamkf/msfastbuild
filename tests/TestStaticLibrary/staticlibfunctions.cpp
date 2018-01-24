//The static library is build with Visual Studio 2013

int _cdecl StaticLibraryFunction(int arg1)
{
	return arg1 * 10;
}

#define STRING2(x) #x
#define STRING(x) STRING2(x)
#pragma message("Platform toolset is " STRING(_MSC_PLATFORM_TOOLSET))

#if _DEBUG
#if _MSC_PLATFORM_TOOLSET != 120
#error Wrong toolset! Expected 120
#endif
#endif

#if !_DEBUG
#if _MSC_PLATFORM_TOOLSET != 140
#error Wrong toolset! Expected 140
#endif
#endif