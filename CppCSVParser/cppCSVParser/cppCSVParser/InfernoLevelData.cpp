#include "pch.h"
#include "InfernoLevelData.h"


InfernoLevelData::InfernoLevelData()
{
}


InfernoLevelData::~InfernoLevelData()
{
}

// Returns the appropriate 2D array data structure look up indices given a set of in game x and y values
XYCoords InfernoLevelData::TranslatePositionIntoLookUpCoordinates(float _x, float _y)
{
	XYCoords returnCoords = XYCoords();

	// Calculate the return value
	returnCoords.x = (int)((_x - this->minimumXValue) / this->subdivisionSizeX);
	returnCoords.y = (int)((_y - this->minimumYValue) / this->subdivisionSizeY);

	return returnCoords;
}
