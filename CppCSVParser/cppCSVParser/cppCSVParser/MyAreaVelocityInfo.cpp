#include "pch.h"
#include "MyAreaVelocityInfo.h"


MyAreaVelocityInfo::MyAreaVelocityInfo()
{
	this->cumulativeVelocity = new MyVector();
	this->count = 0;
}

MyAreaVelocityInfo::~MyAreaVelocityInfo()
{
	if (this->cumulativeVelocity != NULL)
	{
		delete this->cumulativeVelocity;
		this->cumulativeVelocity = NULL;
	}
}

void MyAreaVelocityInfo::Add(float _x, float _y, float _z)
{
	this->cumulativeVelocity->Add(_x, _y, _z);
	this->count++;
}

MyVector* MyAreaVelocityInfo::Average()
{
	this->cumulativeVelocity->Divide(this->count);
	return this->cumulativeVelocity;
}
