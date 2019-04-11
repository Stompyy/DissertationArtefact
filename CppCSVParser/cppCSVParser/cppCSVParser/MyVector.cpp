#include "pch.h"
#include "MyVector.h"


MyVector::MyVector()
{
	this->x = 0.0f;
	this->y = 0.0f;
	this->z = 0.0f;
}

MyVector::MyVector(float _x, float _y, float _z)
{
	this->x = _x;
	this->y = _y;
	this->z = _z;
}

MyVector::~MyVector()
{
}

void MyVector::operator+=(const MyVector &_myVector)
{
	this->x += _myVector.x;
	this->y += _myVector.y;
	this->z += _myVector.z;
}

void MyVector::Divide(const float &_div)
{
	this->x /= _div;
	this->y /= _div;
	this->z /= _div;
}

void MyVector::Add(float _x, float _y, float _z)
{
	this->x += _x;
	this->y += _y;
	this->z += _z;
}
