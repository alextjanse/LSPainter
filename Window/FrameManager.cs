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
        public float[] Vertices { get; }
        public uint[] Indices { get; }

        Frame[,] frames;

        public FrameManager(WindowLayout windowLayout, ImageHandler originalImage, Texture painting)
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

            List<float> verticesList = new List<float>();
            List<uint> indicesList = new List<uint>();

            int frameIndex = 0;
            uint indexOffset = 0;

            if (windowLayout.ShowOriginal)
            {
                /*
                float z = 0.0f;

                float[] frameVertices = new float[] {
                    xCoords[0], yCoords[0], z, 0.0f, 0.0f,
                    xCoords[1], yCoords[0], z, 1.0f, 0.0f,
                    xCoords[0], yCoords[1], z, 0.0f, 1.0f,
                    xCoords[1], yCoords[1], z, 1.0f, 1.0f,
                };

                uint[] frameIndices = new uint[] {
                    0 + indexOffset, 1 + indexOffset, 3 + indexOffset,
                    0 + indexOffset, 2 + indexOffset, 3 + indexOffset
                };

                indexOffset += (uint)frameVertices.Length;

                verticesList.AddRange(frameVertices);
                indicesList.AddRange(frameIndices);

                frames[0, 0] = new Frame(originalImage, frameVertices, frameIndices, frameIndices.Length * frameIndex++);
                */
            }

            int remainingFrames = Math.Min(windowLayout.NCanvases, windowLayout.Columns * windowLayout.Rows + frameIndex);

            for (int i = 0; i < remainingFrames; i++)
            {
                int xI = frameIndex % windowLayout.Columns;
                int yI = frameIndex / windowLayout.Columns;

                float z = 0.0f;

                float[] frameVertices = new float[] {
                    xCoords[xI],     yCoords[yI],     z, 0.0f, 0.0f,
                    xCoords[xI + 1], yCoords[yI],     z, 1.0f, 0.0f,
                    xCoords[xI],     yCoords[yI + 1], z, 0.0f, 1.0f,
                    xCoords[xI + 1], yCoords[yI + 1], z, 1.0f, 1.0f,
                };

                uint[] frameIndices = new uint[] {
                    0 + indexOffset, 1 + indexOffset, 3 + indexOffset,
                    0 + indexOffset, 2 + indexOffset, 3 + indexOffset
                };

                indexOffset += (uint)frameVertices.Length;

                verticesList.AddRange(frameVertices);
                indicesList.AddRange(frameIndices);

                frames[xI, yI] = new Frame(painting, frameVertices, frameIndices, frameIndices.Length * frameIndex++);
            }

            Vertices = verticesList.ToArray();
            Indices = indicesList.ToArray();
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