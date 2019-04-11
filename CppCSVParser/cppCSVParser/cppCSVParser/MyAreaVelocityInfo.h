#pragma once
class MyAreaVelocityInfo
{
public:
	MyAreaVelocityInfo();
	~MyAreaVelocityInfo();

	void Add(float _x, float _y, float _z);

	MyVector* Average();

private:
	MyVector* cumulativeVelocity;
	int count;
};

