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

        public static UIShaderProgram UIShader { get; private set; }

        private static VertexArray VAO;
        private static Buffer<VertVT> VBO;

        private static List<SpriteElement> SpritesToRender;
        private static List<VertVT> VertexList;

        public static QFontDrawing TextRenderer { get; private set; }
        public static QFont NormalFont { get; private set; }
        public static QFont SmallFont { get; private set; }
        public static QFont MonoFont { get; private set; }

        static UIRenderHelper()
        {
            SpritesToRender = new List<SpriteElement>();
            VertexList = new List<VertVT>();
        }

        public static void InitializeResources()
        {
            var builderConfig = new QFontBuilderConfiguration(true)
            {
                ShadowConfig =
                {
                    BlurRadius = 2,
                    BlurPasses = 1,
                    Type = ShadowType.Blurred
                },
                TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
                Characters = CharacterSet.General | CharacterSet.Japanese | CharacterSet.Thai | CharacterSet.Cyrillic
            };

            NormalFont = new QFont("C:\\Windows\\Fonts\\segoeui.ttf", 10,
                builderConfig);

            SmallFont = new QFont("C:\\Windows\\Fonts\\segoeui.ttf", 8,
                builderConfig);

            MonoFont = new QFont("C:\\Windows\\Fonts\\consola.ttf", 10,
                builderConfig);

            TextRenderer = new QFontDrawing();

            UIShader = ProgramFactory.Create<UIShaderProgram>();

            UIShader.Use();
            UIShader.Opacity.Set(1f);
        }

        public static void InitializeBuffers()
        {
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

            NormalFont.Dispose();
            SmallFont.Dispose();
            TextRenderer.Dispose();
            UIShader.Dispose();

            NormalFont.Dispose();
            SmallFont.Dispose();
            MonoFont.Dispose();
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

        public static SpriteElement DrawSpriteBox(Texture2D texture, Vector4 destination, BoxSpriteBounds spriteBounds)
        {
            float scale = Math.Min(destination.Z, destination.W);
            return DrawSpriteBox(texture, destination, spriteBounds, scale);
        }

        public static SpriteElement DrawSpriteBox(Texture2D texture, Vector4 destination, BoxSpriteBounds spriteBounds, float edgeScale)
        {
            var allVerts = new List<VertVT>();

            float[] BoxSizeX = new float[]
            {
                (spriteBounds.GetColumnSize(0) / spriteBounds.Width) * edgeScale,
                0,
                (spriteBounds.GetColumnSize(2) / spriteBounds.Width) * edgeScale
            };
            BoxSizeX[1] = destination.Z - BoxSizeX[0] - BoxSizeX[2];

            float[] BoxSizeY = new float[]
            {
                (spriteBounds.GetRowSize(0) / spriteBounds.Height) * edgeScale,
                0,
                (spriteBounds.GetRowSize(2) / spriteBounds.Height) * edgeScale
            };
            BoxSizeY[1] = destination.W - BoxSizeY[0] - BoxSizeY[2];

            float[] BoxPosX = new float[] { 
                destination.X, 
                destination.X + BoxSizeX[0], 
                destination.X + BoxSizeX[0] + BoxSizeX[1] };
            float[] BoxPosY = new float[] {
                destination.Y,
                destination.Y + BoxSizeY[0],
                destination.Y + BoxSizeY[0] + BoxSizeY[1] };

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    var boxSpriteBounds = spriteBounds.GetRegionBounds(x, y);
                    if (boxSpriteBounds.Width > 0 && boxSpriteBounds.Height > 0)
                    {
                        var boxVerts = GetElementVertices(
                            new Vector4(BoxPosX[x], BoxPosY[y], BoxSizeX[x], BoxSizeY[y]), boxSpriteBounds);
                        allVerts.AddRange(boxVerts);
                    }
                }
            }
            var sprite = new SpriteElement()
            {
                Texture = texture,
                Offset = VertexList.Count,
                ElemCount = allVerts.Count
            };
            SpritesToRender.Add(sprite);
            VertexList.AddRange(allVerts);

            return sprite;
        }

        //public static void DrawText(string text, QFont font, Vector4 color, Vector2 position)
        //{
        //    var col = Color.FromArgb((byte)(color.W * 255),
        //        (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
        //    DrawText(text, font, col, new RectangleF(position.X, position.Y, 9999, 9999));
        //}

        public static void DrawText(string text, QFont font, Color color, Vector2 position)
        {
            DrawText(text, font, color, new RectangleF(position.X, position.Y, 9999, 9999));
        }

        public static void DrawShadowText(string text, QFont font, Color color, Vector2 position)
        {
            var dpOpt = new QFontRenderOptions() { Colour = color, LockToPixel = false, DropShadowActive = true };
            DrawText(text, font, new RectangleF(position.X, position.Y, 9999, 9999), 
                StringAlignment.Near, StringAlignment.Near, dpOpt);
        }

        public static void DrawText(string text, QFont font, Vector4 color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            var col = Color.FromArgb((byte)(color.W * 255), 
                (byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
            DrawText(text, font, col, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }

        public static void DrawText(string text, QFont font, Color color, Vector4 bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            DrawText(text, font, color, new RectangleF(bounds.X, bounds.Y, bounds.Z, bounds.W), vAlign, hAlign);
        }

        public static void DrawText(string text, QFont font, Color color, RectangleF bounds,
            StringAlignment vAlign = StringAlignment.Near,
            StringAlignment hAlign = StringAlignment.Near)
        {
            var dpOpt = new QFontRenderOptions() { Colour = color, LockToPixel = true };
            DrawText(text, font, bounds, vAlign, hAlign, dpOpt);
        }

        public static void DrawText(string text, QFont font, RectangleF bounds,
            StringAlignment vAlign,
            StringAlignment hAlign,
            QFontRenderOptions options)
        {

            var textSize = font.Measure(text, bounds.Size, QFontAlignment.Left);
            textSize.Height += 2;
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
                    textPos.Y = bounds.Y + ((bounds.Height - textSize.Height) / 2f);
                    break;
            }

            textPos.Y = ViewSize.Y - textPos.Y;

            var dp = new QFontDrawingPrimitive(font, options);
            dp.Print(text, new Vector3(textPos.X, textPos.Y, 0f), bounds.Size, QFontAlignment.Left);

            TextRenderer.DrawingPrimitives.Add(dp);
        }


        public static void IntializeBeforeRender()
        {
            TextRenderer.DrawingPrimitives.Clear();
            TextRenderer.ProjectionMatrix = TextMatrix;
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
                
                foreach (var sprite in spriteGroup)
                {
                    UIShader.Opacity.Set(sprite.Opacity);
                    VAO.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, sprite.Offset, sprite.ElemCount);
                }
            }

            TextRenderer.RefreshBuffers();
            TextRenderer.Draw();
            TextRenderer.DisableShader();
        }

        public class SpriteElement
        {
            public Texture2D Texture { get; set; }
            public int Offset { get; set; }
            public int ElemCount { get; set; }
            public float Opacity { get; set; } = 1f;
            public OpenTK.Graphics.OpenGL.PrimitiveType PrimitiveType { get; set; } = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;
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
