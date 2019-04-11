
#include "pch.h"
#include <iostream>


struct AreaVelocityInfo
{
	MyVector* cumulativeVelocity = new MyVector();
	int count = 0;
};

AreaVelocityInfo* cumalativeVelocityTable;

int main()
{
	// Singleton
	InfernoLevelData* levelData = new InfernoLevelData();

	// Declare data structure here. Different to the C# method we will use a 1D array here, but treat it as a 2D array. Looking up into it with
	// offsets i.e. i = x + y*width
	cumalativeVelocityTable = new AreaVelocityInfo[100 * 100];

	// The location of the master.csv file (for me)
	std::ifstream file("D:/Uni/Dissertation/demParser/master.csv");

	// Declare these outside of the loop to avoid memory leak
	std::string line;
	XYCoords lookUpCoords;
	std::string* splitLine = new std::string[10];

	// First line is the column headers - ignore
	std::getline(file, line);

	// Read each line of the file into 'line'
	while (std::getline(file, line))
	{
		// Split the line by commas into the splitLine array
		SplitString(line, splitLine);

		// Try catch needed here as incomplete lines are occasionally included in the replay file - see blog 'Stream reading'
		try
		{
			// Get the array look up coords based upon the location values. Have to convert string to float using std::stof()
			lookUpCoords = levelData->TranslatePositionIntoLookUpCoordinates(std::stof(splitLine[2]), std::stof(splitLine[3]));

			// Add the currentLine velocity value onto the looked up cumalulative velocity
			cumalativeVelocityTable[lookUpCoords.x + lookUpCoords.y * 100].cumulativeVelocity->Add(std::stof(splitLine[8]), std::stof(splitLine[9]), 0.0f); //std::stof(splitLine[10]));
			
			// And increment the count
			cumalativeVelocityTable[lookUpCoords.x + lookUpCoords.y * 100].count++;
		}
		catch(const std::exception& e)
		{
			
		}
	}
	
	// The CSV file has been completely parsed. Good point to breakpoint and benchmark
	// Average the velocity values out and write to an output stream perhaps?
	for (int i = 0; i < 100 * 100; i++)
	{
		cumalativeVelocityTable[i].cumulativeVelocity->Divide(cumalativeVelocityTable[i].count);
		// 
	}

	return 1;
}

// My C++ implementation of C# string.Split(','); function
// Fills the array with the comma seperated values of the _line
void SplitString(std::string _line, std::string* _outputArray)
{
	// Start from -1 as the substring function uses i+1 and we want to start at 0
	int i = -1;
	// Find the first ','
	int j = _line.find(',');

	// Try catch in case of incomplete line
	try
	{
		// Coupled approach here, as we never look past these first values in the line
		for (int index = 0; index < 10; index++)
		{
			// Has taken some tweaking here to start and end the substring after and before each comma found by j
			_outputArray[index] = _line.substr(i+1, j - (i+1));

			// Then move it all up to find the next comma and substring
			i = j++;
			j = _line.find(',', j);
		}
	}
	catch (const std::exception& e)
	{

	}
}
