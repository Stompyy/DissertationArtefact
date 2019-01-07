# Comp320_1_1607539

## DissertationArtefact

csgo demoParser

Included is a single sample .dem file to test functionality (I have 160+ currently but that's too much too include)

I included a couple of screenshots to show the raw data if you're interested.

The sample .dem file is in the build folder \csgoDemoParser\bin\Debug\sampleDemFile.dem to easily find when running the program.

Also in the build folder is the fully operational VelocityTrendFinal.csv - The result of much parsing! Also a .png visualisation of this data as a vector map visualisationFinal.png. Although a new image will be generated on each velocityTrend.csv parse. 

As shown by pressing the winforms 'help' button, a rough help/guide/readme is:



## Help

Start by loading a .dem file.

The resulting rawData.csv can then be parsed into players. This can be used to prune empty or non existent players such as spectators or coaches. (easily identifiable as each game can only have 10 players. More player files imply non valid players present - ignore)

The 'Combine multiple .csv files into master.csv' button will allow the selection of all potential data, and will write a master.csv file. This will likely be too large to load entirely into a program for analysis (164 .dem files ~= 8GB master.csv file). Instead, transfer this data into an SQL database table with the 'Connect to database -> Create new database from master.csv file' button.

To identify velocity trends within the map, use 'Create average velocity trend MasterVelocity.csv from database'. This will query the database for position values falling within tiles of a grid, aggregating and averaging the velocity values for each grid tile. This grid is bound by values determined by 'Query min max position values' and split by a value: Experiment.LevelAxisSubdivisions = 100 (default) vertically and horizontally to subdivide the map into 10,000 tiles.

For a more lightweight approach this program writes a seperate velocity#.csv file per 100 queries. Use 'Combine velocityColumn#.csv files' to select all 100 generated velocity files and combine them into one masterVelocity#.csv file. This is the first goal of this program, and serves as a culmination of all the velocity trends in a small file.

This data can be visualised with the 'Load LedReckoning MasterVelocity.CSV' button. Both in the winforms app and saved as a .png file.

Please bear in mind that this is a collection of tools for the collection, parsing, and summarising of data trends. User experience has not been a strong design factor as this tool is only expected to be used by myself currently.

Using 'Seperate player#.csv file(s) into paths' button will write out every seperate path from a player file. The experiment will ask for a selection of path files, from which it will randomly choose a non zero length path to experiment upon.

Performing an experiment will output comparison statistics between traditional threshold based dead reckoning and the proposed led reckoning approach using the derived velocity trend map.