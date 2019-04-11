#pragma once
class MyVector
{
public:
	MyVector();
	MyVector(float _x, float y, float _z);
	~MyVector();

	void operator += (const MyVector &_myVector);
	void Divide (const float &_div);
	void Add(float  _x, float _y, float _z);

private:
	float x, y, z;
};

