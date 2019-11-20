using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.BrickEditor.Rendering.UI;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics;
using QuickFont;
using QuickFont.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    static class UIRenderHelper
    {
        public static Matrix4 ProjectionMatrix { get; set; }
        public static Matrix4 TextMatrix { get; set; }

        public static Vector2 ViewSize { get; set; }

        private static QFontDrawing TextRenderer;
        private static QFont RenderFont;

        public static UIShaderProgram UIShader { get; private set; }

        private static VertexArray VAO;
        private static Buffer<VertVT> VBO;

        private static List<SpriteElement> SpritesToRender;
        private static List<VertVT> VertexList;

        static UIRenderHelper()
        {
            SpritesToRender = new List<SpriteElement>();
            VertexList = new List<VertVT>();
        }

        public static void InitializeResources()
        {
            RenderFont = new QFont("C:\\Windows\\Fonts\\segoeui.ttf", 10,
                new QFontBuilderConfiguration(true));
            TextRenderer = new QFontDrawing();
            UIShader = ProgramFactory.Create<UIShaderProgram>();

            UIShader.Use();
            VAO = new VertexArray();
            VBO = new Buffer<VertVT>();
            VAO.Bind();
            VAO.BindAttribute(UIShader.Position, VBO);
            VAO.BindAttribute(UIShader.TexCoord, VBO, 12);
        }

        public static void ReleaseResources()
        {
            VBO.Dispose();
            VAO.Dispose();

            RenderFont.Dispose();
            TextRenderer.Dispose();
            UIShader.Dispose();
        }

        public static void SetupShaders()
        {
            RenderHelper.UIShader.Use();
            RenderHelper.UIShader.Projection.Set(ProjectionMatrix);

        }

        public static void InitializeMatrices(Camera camera)
        {
            ViewSize = new Vector2(camera.Viewport.Width, camera.Viewport.Height);
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(
                0, camera.Viewport.Width, 
                camera.Viewport.Height, 0, 
                -1.0f, 1.0f);

            TextMatrix = Matrix4.CreateOrthographicOffCenter(
                0, camera.Viewport.Width,
                0, camera.Viewport.Height, 
                -1.0f, 1.0f);

            TextRenderer.ProjectionMatrix = TextMatrix;
            
        }

        public static void DrawSprite(Texture2D texture, Vector4 destination, SpriteBounds spriteBounds)
        {
            var verts = GetElementVertices(destination, spriteBounds);
            SpritesToRender.Add(new SpriteElement()
            {
                Texture = texture,
                Offset = VertexList.Count,
                ElemCount = verts.Length
            });
            VertexList.AddRange(verts);
        }

        public static void DrawText(string text, Vector4 color, Vector2 position)
        {
            var col = Color.FromArgb((byte)(color.W * 255),
                (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
            DrawText(text, col, new RectangleF(position.X, position.Y, 9999, 9999));
        }

        public static void DrawText(string text, Color color, Vector2 position)
        {
            DrawText(text, color, new RectangleF(position.X, position.Y, 9999, 9999));
        }

        public static void DrawText(string text, Vector4 color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            var col = Color.FromArgb((byte)(color.W * 255), 
                (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
            DrawText(text, col, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }

        public static void DrawText(string text, Color color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            DrawText(text, color, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }

        public static void DrawText(string text, Color color, RectangleF bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {

            var textSize = RenderFont.Measure(text, bounds.Size, QFontAlignment.Left);
            var textPos = Vector2.Zero;

            switch (hAlign)
            {
                case StringAlignment.Near:
                    textPos.X = bounds.X;
                    break;
                case StringAlignment.Far:
                    textPos.X = bounds.Right - textSize.Width;
                    break;
                case StringAlignment.Center:
                    textPos.X = bounds.Left + ((bounds.Width - textSize.Width) / 2f);
                    break;
            }
            switch (vAlign)
            {
                case StringAlignment.Near:
                    textPos.Y = bounds.Y;
                    break;
                case StringAlignment.Far:
                    textPos.Y = bounds.Y + bounds.Height - textSize.Height;
                    break;
                case StringAlignment.Center:
                    textPos.Y = bounds.Y + ((bounds.Height - (textSize.Height + 2)) / 2f);
                    break;
            }

            textPos.Y = ViewSize.Y - textPos.Y;

            var dp = new QFontDrawingPrimitive(RenderFont, new QFontRenderOptions() { Colour = color, LockToPixel = true });
            dp.Print(text, new Vector3(textPos.X, textPos.Y, 0f), bounds.Size, QFontAlignment.Left);
            TextRenderer.DrawingPrimitives.Add(dp);
        }

        public static void IntializeBeforeRender()
        {
            TextRenderer.DrawingPrimitives.Clear();
            VBO.Clear(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer);
            SpritesToRender.Clear();
            VertexList.Clear();
        }

        public static void RenderElements()
        {
            UIShader.Use();
            UIShader.Projection.Set(ProjectionMatrix);

            VAO.Bind();
            VBO.Init(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VertexList.ToArray());

            foreach (var spriteGroup in SpritesToRender.GroupBy(x => x.Texture))
            {
                spriteGroup.Key.Bind(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
                UIShader.Texture.BindTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1, spriteGroup.Key);
                foreach(var sprite in spriteGroup)
                {
                    VAO.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, sprite.Offset, sprite.ElemCount);
                }
            }

            TextRenderer.RefreshBuffers();
            TextRenderer.Draw();
            TextRenderer.DisableShader();
        }

        private class SpriteElement
        {
            public Texture2D Texture { get; set; }
            public int Offset { get; set; }
            public int ElemCount { get; set; }
        }

        public static VertVT[] GetElementVertices(Vector4 elementBounds, SpriteBounds texCoordBounds)
        {
            var verts = new Vector2[]
            {
                new Vector2(elementBounds.X, elementBounds.Y),
                new Vector2(elementBounds.X, elementBounds.Y + elementBounds.W),
                new Vector2(elementBounds.X + elementBounds.Z, elementBounds.Y + elementBounds.W),
                new Vector2(elementBounds.X + elementBounds.Z, elementBounds.Y)
            };
            var coords = new Vector2[]
            {
                new Vector2(texCoordBounds.X, texCoordBounds.Y),
                new Vector2(texCoordBounds.X, texCoordBounds.Y + texCoordBounds.Height),
                new Vector2(texCoordBounds.X + texCoordBounds.Width, texCoordBounds.Y + texCoordBounds.Height),
                new Vector2(texCoordBounds.X + texCoordBounds.Width, texCoordBounds.Y)
            };
            return new VertVT[]
            {
                new VertVT(new Vector3(verts[0]), coords[0]),
                new VertVT(new Vector3(verts[1]), coords[1]),
                new VertVT(new Vector3(verts[2]), coords[2]),
                new VertVT(new Vector3(verts[3]), coords[3])
            };
        }
    }
}
