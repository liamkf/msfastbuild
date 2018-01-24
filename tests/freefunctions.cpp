#include "stdafx.h"
//This file doesn't use the PCH (although it's included...)

int NumSquared(int num)
{
	return num * num;
}

template <int i>
struct TemplateAbuserClass
{
	TemplateAbuserClass() :val(1) {}
	int val;

	TemplateAbuserClass<i-1> x;
	TemplateAbuserClass<i-2> y;
};
template <> struct TemplateAbuserClass<0>
{
	char a;
};
template <> struct TemplateAbuserClass<1>
{
	char a;
};

int TemplateAbuserClassFunction()
{
	TemplateAbuserClass<40> b;
	return b.val;
}