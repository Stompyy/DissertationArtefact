#pragma once

// Used to look up values in a 2D array
struct XYCoords
{
	int x, y;
};

// The level relevant data such as dimensions
class InfernoLevelData
{
public:
	InfernoLevelData();
	~InfernoLevelData();

	// Returns the appropriate 2D array data structure look up indices given a set of in game x and y values
	XYCoords TranslatePositionIntoLookUpCoordinates(float _x, float _y);

private:
	// Copied from the C# project that this is attepting to optimise
	// Found by performing a min max query on master table, returns -1964, 2704, -791.9999, 3600
	// Rounded these numbers
	const float minimumXValue = -1960.0f;
	const float maximumXValue = 2710.0f;
	const float minimumYValue = -790.0f;
	const float maximumYValue = 3605.0f;

	const float mapTotalX = maximumXValue - minimumXValue;
	const float mapTotalY = maximumYValue - minimumYValue;

	// The size of each grid piece
	const float subdivisionSizeX = mapTotalX / 100.0f;
	const float subdivisionSizeY = mapTotalY / 100.0f;
};

