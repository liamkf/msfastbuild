#include "stdafx.h"
#include "freefunctions.h"
#include "dependentheader.h"

int main(int argc, char * argv[])
{
	printf("Hi mom!");
	TemplateAbuserClassFunction();
	return 0;
}

#if !defined(SPACEDEFINE)
#error SPACEDEFINE failed
#endif
#if !defined(DEPENDENT_HEADER)
#error DEPENDENT_HEADER failed
#endif