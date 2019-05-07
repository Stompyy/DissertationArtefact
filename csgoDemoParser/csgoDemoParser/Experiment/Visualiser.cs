using System.Drawing;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Handles the drawing of paths and the trend data onto the WinForms picture box
     */
    class Visualiser
    {
        // A reference to the PictureBox drawing target
        PictureBox VectorMapPictureBox;

        // A reference to the trend data
        Vector[,] m_LedReckoningLevelDataTable;

        // The dimensions of the image
        int imageWidth, imageHeight;

        // The defaullt scale of the image before zooming
        const int imageScale = 2;

        // Control which of the paths will be rendered. Used for fine tuning the dead reckoning values
        const bool drawBackground = true;
        const bool drawDead = true;
        const bool drawLed = true;

        /*
         * Constructor
         */
        public Visualiser(PictureBox _VectorMapPictureBox, Vector[,] _LedReckoningLevelDataTable)
        {
            // Sets the references
            VectorMapPictureBox = _VectorMapPictureBox;
            m_LedReckoningLevelDataTable = _LedReckoningLevelDataTable;

            // Determine the image dimensions given the scale and PictureBox dimensions
            imageWidth = VectorMapPictureBox.Width * imageScale;
            imageHeight = VectorMapPictureBox.Height * imageScale;
        }

        /*
         * Draw and save the data visualisation
         */
        public void DrawAndSaveDataVisualisation()
        {
            // Create the graphics objects needed for the displaying and saving of the data visualisation
            Bitmap returnBMP = new Bitmap(imageWidth, imageHeight);
            Graphics returnGraphics = Graphics.FromImage(returnBMP);

            // Draw the trend map
            DrawTrendMap(returnGraphics);

            // Save the new image as a .png
            returnBMP.Save("visualisation.png", System.Drawing.Imaging.ImageFormat.Png);

            // Set the image of the Winforms picture box to the new image
            VectorMapPictureBox.Image = returnBMP;
        }

        /*
         * Draws the trend map onto the PictureBox reference
         */
        private void DrawTrendMap(Graphics returnGraphics)
        {
            // Calculate the step for the grid
            float xStep = (float)imageWidth / Experiment.LevelAxisSubdivisions;
            float yStep = (float)imageHeight / Experiment.LevelAxisSubdivisions;

            // Create the pen. Black colour with a quarter alpha channel shows overlay nicely
            Pen pen = new Pen(Color.FromArgb(64, 0, 0, 0), 1.0f)
            {
                // Make the line look nice with an arrow pointing the relevant direction
                EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
            };

            // For each potential tile, look up in velocity trend in m_LedReckoningLevelDataTable and try to draw it
            for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
            {
                for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                {
                    // Draw the map the right way around, reverse the 0-99 y value - Just aesthetic convention shows Map level De_Inferno being this way up
                    // Get the velocity trend value stored in the Vector[,] m_LedReckoningLevelDataTable for that tile
                    Vector velocityTrend = m_LedReckoningLevelDataTable[x, Experiment.LevelAxisSubdivisions - y - 1];

                    // Only draw a line if there is a non zero vector there
                    if (velocityTrend.Length != 0.0)
                    {
                        // Start drawing from the centre of the tile
                        float startX = (x + 0.5f) * xStep;
                        float startY = (y + 0.5f) * yStep;

                        // Calculate the end point. As these lines can appear quite small, extend out a reasonable amount to draw a good representation
                        float endX = startX + (velocityTrend.X * xStep / (0.1f * InfernoLevelData.maxPlayerSpeed));
                        float endY = startY - (velocityTrend.Y * yStep / (0.1f * InfernoLevelData.maxPlayerSpeed));

                        // Draw the line
                        returnGraphics.DrawLine(pen, startX, startY, endX, endY);
                    }
                }
            }

            // Dispose of the pen
            pen.Dispose();
        }


        /*
         * Draw and save path visualisation
         */
        public void DrawAndSavePathVisualisation(Experiment experiment)
        {
            // Create the graphics objects needed for the displaying and saving of the data visualisation
            Bitmap returnBMP = new Bitmap(imageWidth, imageHeight);
            Graphics returnGraphics = Graphics.FromImage(returnBMP);

            // Draw the background first as the trend map
            if (drawBackground)
            {
                DrawTrendMap(returnGraphics);
            }

            // Create the pen, Set to green for the Actual path which is drawn first
            Pen pen = new Pen(Color.Green, 1.0f);

            // For each frame of the path draw line
            // As we draw from the previous position to the current position, start at the second frame
            for (int i = 1; i < experiment.actualPositions.Length; i++)
            {
                // Get the start and end positions for the line
                VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.actualPositions[i - 1].X, experiment.actualPositions[i - 1].Y, imageWidth, imageHeight);
                VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.actualPositions[i].X, experiment.actualPositions[i].Y, imageWidth, imageHeight);

                // For visual comparison of packet update moments, if a dead reckoned packet update was prompted, draw a round anchor
                if (experiment.deadReckonedPositions[i].packetUpdate)
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                }
                else
                {
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                }


                // Draw the line
                returnGraphics.DrawLine(pen,
                    startPos.X,
                    startPos.Y,
                    endPos.X,
                    endPos.Y
                    );
            }

            // If we are drawing the dead reckoned path
            if (drawDead)
            {
                // Set the pen colour to red for the dead reckoning representation
                pen.Color = Color.Red;

                // For each frame of the path draw line
                // As we draw from the previous position to the current position, start at the second frame
                for (int i = 1; i < experiment.deadReckonedPositions.Length; i++)
                {
                    // Get the start and end positions for the line
                    VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.deadReckonedPositions[i - 1].X, experiment.deadReckonedPositions[i - 1].Y, imageWidth, imageHeight);
                    VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.deadReckonedPositions[i].X, experiment.deadReckonedPositions[i].Y, imageWidth, imageHeight);

                    // For visual comparison of packet update moments, if a dead reckoned packet update was prompted, draw a round anchor
                    if (experiment.deadReckonedPositions[i].packetUpdate)
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                    }
                    else
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                    }


                    // Draw the line
                    returnGraphics.DrawLine(pen,
                        startPos.X,
                        startPos.Y,
                        endPos.X,
                        endPos.Y
                        );
                }
            }

            // If we are drawing the led reckoning path
            if (drawLed)
            {
                // Set the pen colour to blue for the led reckoning path
                pen.Color = Color.Blue;

                // For each frame of the path draw line
                // As we draw from the previous position to the current position, start at the second frame
                for (int i = 1; i < experiment.ledReckonedPositions.Length; i++)
                {
                    // Get the start and end positions for the line
                    VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.ledReckonedPositions[i - 1].X, experiment.ledReckonedPositions[i - 1].Y, imageWidth, imageHeight);
                    VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.ledReckonedPositions[i].X, experiment.ledReckonedPositions[i].Y, imageWidth, imageHeight);

                    // If a led reckoning packet update is prompted, draw a round anchor
                    if (experiment.ledReckonedPositions[i].packetUpdate)
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                    }
                    else
                    {
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                    }

                    // Draw the line
                    returnGraphics.DrawLine(pen,
                        startPos.X,
                        startPos.Y,
                        endPos.X,
                        endPos.Y
                        );
                }
            }

            // Dispose of the pen
            pen.Dispose();

            // Save the new image as a .png appropriately named
            returnBMP.Save("path." + experiment.experimentName + ".png", System.Drawing.Imaging.ImageFormat.Png);

            // Set the image of the Winforms picture box to the new image
            VectorMapPictureBox.Image = returnBMP;
        }
    }
}
