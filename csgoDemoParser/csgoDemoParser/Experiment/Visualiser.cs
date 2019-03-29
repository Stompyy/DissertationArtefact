using System.Drawing;
using System.Windows.Forms;

namespace csgoDemoParser
{
    class Visualiser
    {
        PictureBox VectorMapPictureBox;
        Vector[,] m_LedReckoningLevelDataTable;

        int imageWidth, imageHeight;

        const int imageScale = 2;

        const bool drawBackground = true;
        const bool drawDead = true;
        const bool drawLed = true;


        public Visualiser(PictureBox _VectorMapPictureBox, Vector[,] _LedReckoningLevelDataTable)
        {
            VectorMapPictureBox = _VectorMapPictureBox;
            m_LedReckoningLevelDataTable = _LedReckoningLevelDataTable;

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

            DrawTrendMap(returnGraphics);

            // Save the new image as a .png
            returnBMP.Save("visualisation.png", System.Drawing.Imaging.ImageFormat.Png);

            // Set the image of the Winforms picture box to the new image
            VectorMapPictureBox.Image = returnBMP;

            // Success message
            MessageBox.Show("Image saved as visualisation.png.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * 
         */
        private void DrawTrendMap(Graphics returnGraphics)
        {
            //
            float xStep = (float)imageWidth / Experiment.LevelAxisSubdivisions;
            float yStep = (float)imageHeight / Experiment.LevelAxisSubdivisions;

            // The max size of a velocity arrow is the game rule's max allowed speed. An average cannot be higher
            float maxSize = InfernoLevelData.maxPlayerSpeed;

            // Create the pen
            Pen pen = new Pen(Color.Black, 1.0f)
            {
                // Make the line look nice with an arrow pointing the relevant direction
                //     StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor,
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
                        // Set the colour as a grayscale currently ish (/2)
                        pen.Color = Color.FromArgb(
                            128,
                            (int)velocityTrend.Length / 2,
                            (int)velocityTrend.Length / 2,
                            (int)velocityTrend.Length / 2);

                        // Start drawing from the centre of the tile
                        float startX = (x + 0.5f) * xStep;
                        float startY = (y + 0.5f) * yStep;

                        // * Step / (2.0f * maxSize) limits the draw to the edge of the tile
                        float endX = startX + (velocityTrend.X * xStep / (0.1f * maxSize));
                        float endY = startY + (velocityTrend.Y * yStep / (0.1f * maxSize));

                        // Draw it
                        returnGraphics.DrawLine(pen, startX, startY, endX, endY);
                    }
                }
            }

            // Dispose the pen
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

            // draw the background first as the trend map
            if (drawBackground)
                DrawTrendMap(returnGraphics);

            // Create the pen
            Pen pen = new Pen(Color.Green, 1.0f);

            // 
            for (int i = 1; i < experiment.actualPositions.Length; i++)
            {
                VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.actualPositions[i - 1].X, experiment.actualPositions[i - 1].Y, imageWidth, imageHeight);
                VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.actualPositions[i].X, experiment.actualPositions[i].Y, imageWidth, imageHeight);

                // Draw it
                returnGraphics.DrawLine(pen,
                    startPos.X,
                    startPos.Y,
                    endPos.X,
                    endPos.Y
                    );
            }

            pen.Color = Color.Red;

            // 
            if (drawDead)
            {
                for (int i = 1; i < experiment.deadReckonedPositions.Length; i++)
                {
                    VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.deadReckonedPositions[i - 1].X, experiment.deadReckonedPositions[i - 1].Y, imageWidth, imageHeight);
                    VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.deadReckonedPositions[i].X, experiment.deadReckonedPositions[i].Y, imageWidth, imageHeight);

                    // Draw it
                    returnGraphics.DrawLine(pen,
                        startPos.X,
                        startPos.Y,
                        endPos.X,
                        endPos.Y
                        );
                }
            }

            pen.Color = Color.Blue;

            // 
            if (drawLed)
            {
                for (int i = 1; i < experiment.ledReckonedPositions.Length; i++)
                {
                    VisualisationData startPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.ledReckonedPositions[i - 1].X, experiment.ledReckonedPositions[i - 1].Y, imageWidth, imageHeight);
                    VisualisationData endPos = InfernoLevelData.TranslatePositionIntoRenderCoordinates(experiment.ledReckonedPositions[i].X, experiment.ledReckonedPositions[i].Y, imageWidth, imageHeight);

                    // Draw it
                    returnGraphics.DrawLine(pen,
                        startPos.X,
                        startPos.Y,
                        endPos.X,
                        endPos.Y
                        );
                }
            }

            // Dispose the pen
            pen.Dispose();

            // Save the new image as a .png
            returnBMP.Save("path." + experiment.experimentName + ".png", System.Drawing.Imaging.ImageFormat.Png);

            // Set the image of the Winforms picture box to the new image
            VectorMapPictureBox.Image = returnBMP;

            // Success message
            MessageBox.Show("Image saved as path." + experiment.experimentName + ".png.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
