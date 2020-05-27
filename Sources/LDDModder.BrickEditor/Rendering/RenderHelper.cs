﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.BrickEditor.Rendering.Models;
using LDDModder.BrickEditor.Rendering.Shaders;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using ObjectTK.Textures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LDDModder.BrickEditor.Rendering
{
    public static class RenderHelper
    {

        #region Shaders

        public static ColorShaderProgram ColorShader { get; private set; }

        public static WireframeShaderProgram WireframeShader { get; private set; }

        public static WireframeShader2Program WireframeShader2 { get; private set; }

        public static ModelShaderProgram ModelShader { get; private set; }

        public static StudConnectionShaderProgram StudConnectionShader { get; private set; }

        public static SimpleTextureShaderProgram SimpleTextureShader { get; private set; }

        public static IndexedVertexBuffer<Vector3> BoundingBoxBufffer;

        //public static Buffer<StudGridCell> StudGridBuffer { get; private set; }

        #endregion

        public static void InitializeResources()
        {
            ColorShader = ProgramFactory.Create<ColorShaderProgram>();
            WireframeShader = ProgramFactory.Create<WireframeShaderProgram>();
            ModelShader = ProgramFactory.Create<ModelShaderProgram>();
            WireframeShader2 = ProgramFactory.Create<WireframeShader2Program>();
            StudConnectionShader = ProgramFactory.Create<StudConnectionShaderProgram>();
            SimpleTextureShader = ProgramFactory.Create<SimpleTextureShaderProgram>();

            BoundingBoxBufffer = new IndexedVertexBuffer<Vector3>();
            var box = BBox.FromCenterSize(Vector3.Zero, Vector3.One);

            BoundingBoxBufffer.SetVertices(box.GetCorners());
            var bboxIndices = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                bboxIndices.Add((i * 2));
                bboxIndices.Add((i * 2) + 1);
                bboxIndices.Add((i * 2));
                bboxIndices.Add(((i + 1) * 2) % 8);
                bboxIndices.Add((i * 2) + 1);
                bboxIndices.Add((((i + 1) * 2) + 1) % 8);
            }

            BoundingBoxBufffer.SetIndices(bboxIndices);

            //StudGridBuffer = new Buffer<StudGridCell>();
            

            CollisionMaterial = new MaterialInfo
            {
                Diffuse = new Vector4(1f, 0.05f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            ConnectionMaterial = new MaterialInfo
            {
                Diffuse = new Vector4(0.95f, 0.95f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            MaleConnectorMaterial = new MaterialInfo
            {
                Diffuse = new Vector4(0.05f, 0.05f, 0.95f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            FemaleConnectorMaterial = new MaterialInfo
            {
                Diffuse = new Vector4(0.05f, 0.95f, 0.05f, 1f),
                Specular = new Vector3(1f),
                Shininess = 2f
            };

            WireframeColor = new Vector4(0, 0, 0, 1f);
            WireframeColorAlt = new Vector4(0.85f, 0.85f, 0.85f, 1f);
            SelectionOutlineColor = new Vector4(1f);
        }

        public static void ReleaseResources()
        {
            if (ColorShader != null)
            {
                ColorShader.Dispose();
                ColorShader = null;
            }

            if (WireframeShader != null)
            {
                WireframeShader.Dispose();
                WireframeShader = null;
            }

            if (WireframeShader2 != null)
            {
                WireframeShader2.Dispose();
                WireframeShader2 = null;
            }

            if (ModelShader != null)
            {
                ModelShader.Dispose();
                ModelShader = null;
            }

            if (BoundingBoxBufffer != null)
            {
                BoundingBoxBufffer.Dispose();
                BoundingBoxBufffer = null;
            }

            if (StudConnectionShader != null)
            {
                StudConnectionShader.Dispose();
                StudConnectionShader = null;
            }

            if (SimpleTextureShader != null)
            {
                SimpleTextureShader.Dispose();
                SimpleTextureShader = null;
            }
        }

        public static void InitializeMatrices(Camera camera)
        {
            var viewMatrix = camera.GetViewMatrix();
            var projection = camera.GetProjectionMatrix();

            WireframeShader.Use();
            WireframeShader.ViewMatrix.Set(viewMatrix);
            WireframeShader.Projection.Set(projection);

            WireframeShader2.Use();
            WireframeShader2.ViewMatrix.Set(viewMatrix);
            WireframeShader2.Projection.Set(projection);

            ColorShader.Use();
            ColorShader.ViewMatrix.Set(viewMatrix);
            ColorShader.Projection.Set(projection);

            ModelShader.Use();
            ModelShader.ViewMatrix.Set(viewMatrix);
            ModelShader.Projection.Set(projection);
            ModelShader.ViewPosition.Set(camera.Position);

            StudConnectionShader.Use();
            StudConnectionShader.ViewMatrix.Set(viewMatrix);
            StudConnectionShader.Projection.Set(projection);

            SimpleTextureShader.Use();
            SimpleTextureShader.ViewMatrix.Set(viewMatrix);
            SimpleTextureShader.Projection.Set(projection);

            GL.UseProgram(0);
        }

        public static void BindModelTexture(Texture2D texture, TextureUnit textureUnit)
        {
            ModelShader.Use();
            texture.Bind(textureUnit);
            ModelShader.Texture.BindTexture(textureUnit, texture);
        }

        public static void UnbindModelTexture()
        {
            ModelShader.Use();
            ModelShader.Texture.Set(TextureUnit.Texture0);
            ModelShader.UseTexture.Set(false);
        }

        #region Draw Model

        public static void BeginDrawModel(PartialModel model, Matrix4 transform, MaterialInfo material)
        {
            BeginDrawModel(model.VertexBuffer, transform, material);
        }

        public static void BeginDrawModel(IVertexBuffer vertexBuffer, Matrix4 transform, MaterialInfo material)
        {
            ModelShader.Use();
            ModelShader.ModelMatrix.Set(transform);
            ModelShader.Material.Set(material);

            vertexBuffer.Bind();
            vertexBuffer.BindAttribute(ModelShader.Position, 0);
            vertexBuffer.BindAttribute(ModelShader.Normal, 12);
            vertexBuffer.BindAttribute(ModelShader.TexCoord, 24);
        }

        public static void EndDrawModel(IVertexBuffer vertexBuffer)
        {
            vertexBuffer.UnbindAttribute(ModelShader.Position);
            vertexBuffer.UnbindAttribute(ModelShader.Normal);
            vertexBuffer.UnbindAttribute(ModelShader.TexCoord);
        }

        public static void EndDrawModel(PartialModel model)
        {
            EndDrawModel(model.VertexBuffer);
        }

        #endregion


        public static void DrawStudConnector2(Matrix4 transform, LDDModder.LDD.Primitives.Connectors.Custom2DFieldConnector connector)
        {
            float offset = connector.SubType == 23 ? 0.0001f : -0.0001f;
            DrawTexturedQuad(transform, TextureManager.StudConnectionGrid,
                new Vector2(connector.StudWidth * 0.8f, connector.StudHeight * 0.8f),
                new Vector4(0, 0, connector.StudWidth, connector.StudHeight), offset);
        }

        public static void DrawStudConnector(Matrix4 transform, LDDModder.LDD.Primitives.Connectors.Custom2DFieldConnector connector)
        {
            bool wasTexEnabled = GL.IsEnabled(EnableCap.Texture2D);
            GL.Enable(EnableCap.Texture2D);
            //GL.DepthMask(false);
            StudConnectionShader.Use();
            StudConnectionShader.ModelMatrix.Set(transform);
            var cellSize = new Vector2(0.8f) * new Vector2(connector.StudWidth, connector.StudHeight);
            cellSize.X /= connector.ArrayWidth;
            cellSize.Y /= connector.ArrayHeight;
            StudConnectionShader.CellSize.Set(cellSize);
            StudConnectionShader.IsMale.Set(connector.SubType == 23);
            TextureManager.StudGridTexture.Bind(TextureUnit.Texture5);
            StudConnectionShader.Texture.Set(TextureUnit.Texture5);

            var items = new List<StudGridCell>();

            for (int y = 0; y < connector.ArrayHeight; y++)
            {
                for (int x = 0; x < connector.ArrayWidth; x++)
                {
                    var node = connector[x, y];
                    items.Add(new StudGridCell()
                    {
                        Position = new Vector3(x, y, 0),
                        Values = new Vector3(node.Value1, node.Value2, node.Value3)
                    });
                }
            }

            var gridBuffer = new Buffer<StudGridCell>();
            gridBuffer.Init(BufferTarget.ArrayBuffer, items.ToArray());
            var vao = new VertexArray();
            vao.Bind();
            vao.BindAttribute(StudConnectionShader.Position, gridBuffer, 0);
            vao.BindAttribute(StudConnectionShader.Values, gridBuffer, 12);
            vao.DrawArrays(PrimitiveType.Points, 0, items.Count);
            vao.UnbindAttribute(StudConnectionShader.Position);
            vao.UnbindAttribute(StudConnectionShader.Values);
            gridBuffer.Dispose();
            vao.Dispose();

            StudConnectionShader.Texture.Set(TextureUnit.Texture0);
            TextureManager.StudGridTexture.Bind(TextureUnit.Texture0);
            //GL.DepthMask(true);
            if (!wasTexEnabled)
                GL.Disable(EnableCap.Texture2D);
        }

        public static void BeginDrawColorModel(IVertexBuffer vertexBuffer, Matrix4 transform, MaterialInfo material)
        {
            BeginDrawColor(vertexBuffer, transform, material.Diffuse);
        }

        public static void BeginDrawColor(IVertexBuffer vertexBuffer, Matrix4 transform, Vector4 color)
        {
            ColorShader.Use();
            ColorShader.ModelMatrix.Set(transform);
            ColorShader.Color.Set(color);

            vertexBuffer.Bind();
            vertexBuffer.BindAttribute(ColorShader.Position, 0);
        }

        public static void EndDrawColor(IVertexBuffer vertexBuffer)
        {
            vertexBuffer.UnbindAttribute(ColorShader.Position);
        }

        public static void BeginDrawWireframe(IVertexBuffer vertexBuffer, Matrix4 transform, float thickness, Vector4 color)
        {
            WireframeShader.Use();
            WireframeShader.Color.Set(color);
            WireframeShader.ModelMatrix.Set(transform);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(thickness);

            vertexBuffer.Bind();
            vertexBuffer.BindAttribute(WireframeShader.Position, 0);
            vertexBuffer.BindAttribute(WireframeShader.Normal, 12);

        }

        public static void BeginDrawWireframe2(IVertexBuffer vertexBuffer, Matrix4 transform, float thickness, Vector4 color)
        {
            WireframeShader2.Use();
            WireframeShader2.Color.Set(color);
            WireframeShader2.ModelMatrix.Set(transform);
            WireframeShader2.Size.Set(thickness);

            vertexBuffer.Bind();
            vertexBuffer.BindAttribute(WireframeShader2.Position, 0);
        }

        public static void EndDrawWireframe(IVertexBuffer vertexBuffer)
        {
            GL.PopAttrib();
            vertexBuffer.UnbindAttribute(WireframeShader.Position);
            vertexBuffer.UnbindAttribute(WireframeShader.Normal);
        }

        public static void DrawLine(Vector4 color, Vector3 p1, Vector3 p2, float thickness = 1f)
        {
            DrawLine(Matrix4.Identity, color, p1, p2, thickness);
        }

        public static void DrawLine(Matrix4 transform, Vector4 color, Vector3 p1, Vector3 p2, float thickness = 1f)
        {
            ColorShader.Use();
            ColorShader.ModelMatrix.Set(transform);
            ColorShader.Color.Set(color);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(thickness);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(p1);
            GL.Vertex3(p2);
            GL.End();
            GL.PopAttrib();
        }

        public static void DrawTexturedQuad(Matrix4 transform, Texture2D texture, Vector2 size, Vector4 texCoords, float yOffset = 0)
        {
            bool wasTexEnabled = GL.IsEnabled(EnableCap.Texture2D);
            GL.Enable(EnableCap.Texture2D);

            var positions = new Vector4(0, 0, size.X, size.Y);

            VertVT CreateVert(Vector2 pos, Vector2 tex)
            {
                return new VertVT(new Vector3(pos.X, yOffset, pos.Y), tex);
            }

            var quadVerts = new VertVT[]
            {
                CreateVert(positions.Xy, texCoords.Xy),
                CreateVert(positions.Xw, texCoords.Xw),
                CreateVert(positions.Zw, texCoords.Zw),
                CreateVert(positions.Zy, texCoords.Zy)
            };

            SimpleTextureShader.Use();
            SimpleTextureShader.ModelMatrix.Set(transform);
            texture.Bind(TextureUnit.Texture5);
            SimpleTextureShader.Texture.Set(TextureUnit.Texture5);
            //SimpleTextureShader.Texture.BindTexture(TextureUnit.Texture5, texture);

            var vertBuffer = new VertexArrayBuffer<VertVT>();
            
            vertBuffer.SetElements(quadVerts);
            vertBuffer.Bind();
            vertBuffer.BindAttribute(SimpleTextureShader.Position, 0);
            vertBuffer.BindAttribute(SimpleTextureShader.TexCoord, 12);

            vertBuffer.DrawArray(PrimitiveType.Quads, 0, 4);
            vertBuffer.Dispose();

            SimpleTextureShader.Texture.Set(TextureUnit.Texture0);
            texture.Bind(TextureUnit.Texture0);

            if (!wasTexEnabled)
                GL.Disable(EnableCap.Texture2D);
        }

        public static void DrawRectangle(Matrix4 transform, Vector2 size, Vector4 color, float thickness = 1f)
        {
            ColorShader.Use();
            ColorShader.ModelMatrix.Set(transform);
            ColorShader.Color.Set(color);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(thickness);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(Vector3.Zero);
            GL.Vertex3(Vector3.UnitZ * size.Y);
            GL.Vertex3(Vector3.UnitX * size.X + Vector3.UnitZ * size.Y);
            GL.Vertex3(Vector3.UnitX * size.X);
            GL.Vertex3(Vector3.Zero);
            GL.End();
            GL.PopAttrib();
        }

        public static void DrawBoundingBox(Matrix4 transform, Vector3 pos, Vector3 size, Vector4 color, float thickness = 1f)
        {
            ColorShader.Use();

            ColorShader.ModelMatrix.Set(Matrix4.CreateScale(size) * Matrix4.CreateTranslation(pos) * transform);
            ColorShader.Color.Set(color);

            BoundingBoxBufffer.Bind();
            BoundingBoxBufffer.BindAttribute(ColorShader.Position, 0);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(thickness);
            BoundingBoxBufffer.DrawElements(PrimitiveType.Lines);
            GL.PopAttrib();
        }

        public static void DrawBoundingBox(Matrix4 transform, BBox box, Vector4 color, float thickness = 1f)
        {
            ColorShader.Use();
            
            ColorShader.ModelMatrix.Set(Matrix4.CreateScale(box.Size) * Matrix4.CreateTranslation(box.Center) * transform);
            ColorShader.Color.Set(color);

            BoundingBoxBufffer.Bind();
            BoundingBoxBufffer.BindAttribute(ColorShader.Position, 0);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(thickness);
            BoundingBoxBufffer.DrawElements(PrimitiveType.Lines);
            GL.PopAttrib();
        }

        public static void DrawGizmoAxes(Matrix4 transform, float size, Vector4 color, float lineThickness = 1f)
        {
            ColorShader.Use();
            ColorShader.ModelMatrix.Set(transform);
            ColorShader.Color.Set(color);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(lineThickness);

            for (int i = 0; i < 3; i++)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                var axisVector = new Vector3();
                axisVector[i] = 1f;
                GL.Vertex3(axisVector * size);
                GL.End();
            }

            GL.PopAttrib();
        }

        public static void DrawGizmoAxes(Matrix4 transform, float size, float lineThickness = 1f)
        {
            ColorShader.Use();
            ColorShader.ModelMatrix.Set(transform);

            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(lineThickness);

            for (int i = 0; i < 3; i++)
            {
                ColorShader.Color.Set(DefaultAxisColors[i]);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Vector3.Zero);
                var axisVector = new Vector3();
                axisVector[i] = 1f;
                GL.Vertex3(axisVector * size);
                GL.End();
            }

            GL.PopAttrib();
        }

        public static void DrawGizmoAxes(Matrix4 transform, float size, bool selected = false)
        {
            if (!selected)
                DrawGizmoAxes(transform, size, 2f);
            else
            {
                RenderWithStencil(() =>
                {
                    DrawGizmoAxes(transform, size, 2f);
                },
                () =>
                {
                    DrawGizmoAxes(transform, size, new Vector4(1f, 1f, 1f, 1f), 3f);
                });
            }
        }

        #region Default Materials and Colors (TODO: maybe put this elsewhere)


        public static MaterialInfo CollisionMaterial { get; set; }
        public static MaterialInfo ConnectionMaterial { get; set; }
        public static MaterialInfo MaleConnectorMaterial { get; set; }
        public static MaterialInfo FemaleConnectorMaterial { get; set; }

        public static Vector4 WireframeColor { get; set; }
        public static Vector4 WireframeColorAlt { get; set; }
        public static Vector4 SelectionOutlineColor { get; set; }

        public static Vector4[] DefaultAxisColors = new Vector4[]
            {
                new Vector4(1f,0.09f,0.26f,1f),
                new Vector4(0.58f, 0.898f, 0.156f, 1f),
                new Vector4(0.156f,0.564f,1f,1f)
            };

        #endregion


        #region Stencil Buffer

        public static void RenderWithStencil(Action renderPass1, Action renderPass2)
        {
            bool wasEnabled = GL.IsEnabled(EnableCap.StencilTest);
            if (!wasEnabled)
                EnableStencilTest();

            EnableStencilMask();

            renderPass1();

            ApplyStencilMask();

            renderPass2();

            DisableStencilMask();

            if (!wasEnabled)
                DisableStencilTest();
        }

        public static void RenderWithStencil(bool useStencil, Action renderPass1, Action renderPass2)
        {
            bool wasEnabled = GL.IsEnabled(EnableCap.StencilTest);
            
            if (useStencil)
            {
                if (!wasEnabled)
                    EnableStencilTest();

                EnableStencilMask();
            }

            renderPass1();

            if (useStencil)
            {
                ApplyStencilMask();

                renderPass2();

                DisableStencilMask();

                if (!wasEnabled)
                    DisableStencilTest();
            }
        }

        public static void EnableStencilTest()
        {
            GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0);
            GL.Clear(ClearBufferMask.StencilBufferBit);
            //GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
            GL.StencilFunc(StencilFunction.Always, 1, 0xFFFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        }

        public static void DisableStencilTest()
        {
            GL.Disable(EnableCap.StencilTest);
            GL.Clear(ClearBufferMask.StencilBufferBit);
        }

        public static void EnableStencilMask()
        {
            GL.StencilFunc(StencilFunction.Always, 1, 0xFFFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        }

        public static void ApplyStencilMask()
        {
            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFFFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        }

        public static void DisableStencilMask()
        {
            GL.StencilFunc(StencilFunction.Always, 1, 0xFFFF);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
        }

        public static void ClearStencil()
        {
            GL.Clear(ClearBufferMask.StencilBufferBit);
        }

        #endregion

    }
}
