using OpenTK.Graphics.OpenGL4;
using OpenTK;
using SixLabors.ImageSharp;

namespace LSPainter
{
    public class PictureFrame
    {
        public float[] Vertices { get; }

        public uint[] Indices { get; }

        int vertexArrayObject, vertexBufferObject, elementBufferObject;

        Texture texture;

        public PictureFrame(Texture texture, float[] vertices, uint[] indices)
        {
            this.texture = texture;
            Vertices = vertices;
            Indices = indices;
        }

        public void Load(Shader shader)
        {
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StreamDraw);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StreamDraw);

            texture.Load();

            var vertexSize = 5 * sizeof(float);

            int vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, vertexSize, 3 * sizeof(float));

            // Unbind
            GL.BindVertexArray(0);
        }

        public void Update()
        {
            texture.Update();
        }

        public void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);

            texture.Use();
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }
    }

    // TODO: add two subclasses of Frame
    // public class StillFrame : Frame // Static
    // public class LiveCanvas : Frame // Updating
}