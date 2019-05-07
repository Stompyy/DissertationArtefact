
namespace csgoDemoParser
{
    /*
     * 
     */
    class HelpMessage
    {
        // The message to be displayed
        public static string getMessage()
        {
            return "Start by loading a .dem file \n" +
            "The resulting rawData.csv can then be parsed into players. " +
            "This can be used to manually prune empty or non existent players such as spectators or coaches " +
            "(easily identifiable as each game can only have 10 players. More player files represent non " +
            "valid players present - ignore). Then process the player files into path files, and remove " +
            "any paths less than 5 seconds in length (approximately < 60KB). The remaining path files are " +
            "valid to perform the experiment with.\n\n" +

            "PERFORMING AN EXPERIMENT.\n" +
            "Under the menu bar choose 'Experiment'->'Auto Sampling Experiment' to begin. " +
            "A file explorer prompt will ask the user to select all path.csv files that are intended " +
            "to be used in the experiment. A random path is chosen as the experiment subject, and the remainder " +
            "are processed into a velocity trend data structure. This will take between 1-3 minutes depending upon " +
            "hardware. Once completely processed, an experiment upon the randomly selected experiment subject will " +
            "automatically begin as described in the accompanying dissertation research paper. Upon completion, the " +
            "results will saved to an appropriately named CSV file, an image of the trend map and paths simulated will be " +
            "displayed in the WinForms component, and that image will also be saved as a PNG file alongside the " +
            "results.CSV file and named similarly.\n\n" +

            "TESTING.\n" +
            "Checking the map name of the replay data DEM files is done by 'Debug tools'->'Check map names'. " +
            "This will output a CSV results file to reveal the map name of each replay file.\n\n" +

            "'Test'->'Test Against Control Data' will prompt a usual experiment, however afterwards will prompt " +
            "the loading of a control data results CSV file to check against. For zero velocity and fixed velocity tests " +
            "these are precalculated, or if using the testPath to ensure the program is behaving consistently " +
            "over development iterations, select the additional trend data for both teams to perform the more complex " +
            "recreation, before being prompted to load the results file to compare against. All test files are supplied " +
            "and appropriately named under /TestPaths, although due to file paths differing, the comparisons may not pass " +
            "with differing file locations. Evidence of the tests passing can be seen in Appendix C of the accompanying dissertation.\n\n" +

            "Please bear in mind that this is a collection of tools for the collection, parsing, " +
            "and summarising of data trends. User experience has not been a strong design factor as " +
            "this tool is only expected to be used by myself currently. \n\n";
        }
    }
}
