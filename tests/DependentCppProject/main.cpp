#include "stdio.h"
#include "../TestStaticLibrary/staticlibfunctions.h"
#include "../TestDynamicLibrary/dynamiclibfunctions.h"

int main(int argc, char * argv[])
{
	StaticLibraryFunction(1);
	DynamicLibraryFunction(1);
	printf("DependentCpp project says hello!");
	return 0;
}

#if _DEBUG && !defined(_WIN64)
#if !defined(MSFBPROPSHEET)
#error Property sheet failure.
#endif
#endif