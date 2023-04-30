using OpenTK.Graphics.OpenGL4;

namespace LSPainter
{
    public enum FrameType
    {
        Original,
        LiveCanvas,
        StillSolution
    }

    public class FrameManager
    {
        Frame[,] frames;

        public FrameManager(WindowLayout windowLayout, ImageHandler originalImage, IEnumerable<Painting> paintings)
        {
            frames = new Frame[windowLayout.Columns, windowLayout.Rows];

            float[] xCoords = new float[windowLayout.Columns + 1];
            float[] yCoords = new float[windowLayout.Rows + 1];

            float frameWidth = 2.0f / windowLayout.Columns;
            float frameHeight = 2.0f / windowLayout.Rows;
 
            for (int x = 0; x < windowLayout.Columns + 1; x++)
            {
                xCoords[x] = -1.0f + frameWidth * x;
            }

            for (int y = 0; y < windowLayout.Rows + 1; y++)
            {
                yCoords[y] = 1.0f - frameHeight * y;
            }

            int frameIndex = 0;

            if (windowLayout.ShowOriginal)
            {
                float z = 0.0f;

                float[] frameVertices = new float[] {
                    xCoords[0], yCoords[0], z, 0.0f, 0.0f, // top-left
                    xCoords[1], yCoords[0], z, 1.0f, 0.0f, // top-right
                    xCoords[0], yCoords[1], z, 0.0f, 1.0f, // bottom-left
                    xCoords[1], yCoords[1], z, 1.0f, 1.0f, // bottom-right
                };

                uint[] frameIndices = new uint[] {
                    0, 3, 1,
                    0, 2, 3
                };

                frames[0, 0] = new Frame(originalImage, frameVertices, frameIndices);

                frameIndex++;
            }

            int remainingFrames = Math.Min(windowLayout.NCanvases, windowLayout.Columns * windowLayout.Rows + frameIndex);

            Painting[] paintingArray = paintings.ToArray();

            for (int i = 0; i < remainingFrames; i++)
            {
                int xI = frameIndex % windowLayout.Columns;
                int yI = frameIndex / windowLayout.Columns;

                float z = 0.0f;

                float[] frameVertices = new float[] {
                    xCoords[xI],     yCoords[yI],     z, 0.0f, 0.0f, // top-left
                    xCoords[xI + 1], yCoords[yI],     z, 1.0f, 0.0f, // top-right
                    xCoords[xI],     yCoords[yI + 1], z, 0.0f, 1.0f, // bottom-left
                    xCoords[xI + 1], yCoords[yI + 1], z, 1.0f, 1.0f, // bottom-right
                };

                uint[] frameIndices = new uint[] {
                    0, 3, 1,
                    0, 2, 3
                };

                Painting painting = paintingArray[i];

                frames[xI, yI] = new Frame(painting, frameVertices, frameIndices);

                frameIndex++;
            }
        }

        public void Load(Shader shader)
        {
            foreach (Frame frame in frames)
            {
                frame.Load(shader);
            }
        }

        public void Update()
        {
            foreach (Frame frame in frames)
            {
                frame.Update();
            }
        }

        public void Draw()
        {
            foreach (Frame frame in frames)
            {
                frame.Draw();
            }
        }
    }
}