using OpenTK.Graphics.OpenGL4;
using OpenTK;
using SixLabors.ImageSharp;

namespace LSPainter
{
    public class Frame
    {
        public float[] Vertices { get; }

        public uint[] Indices { get; }
        int indexOffset;

        Texture texture;

        public Frame(Texture texture, float[] vertices, uint[] indices, int indexOffset)
        {
            this.texture = texture;
            Vertices = vertices;
            Indices = indices;
            this.indexOffset = indexOffset;
        }

        public void Load(Shader shader)
        {
            texture.Load(shader);
        }

        public void Update()
        {
            texture.Update();
        }

        public void Draw()
        {
            texture.Use();
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, indexOffset);
        }
    }

    // TODO: add two subclasses of Frame
    // public class LiveCanvas : Frame // Update()
    // public class StillFrame : Frame // Static
}