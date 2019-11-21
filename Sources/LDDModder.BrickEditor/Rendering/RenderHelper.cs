using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.BrickEditor.Rendering.Shaders;
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

        public static void InitializeResources()
        {
            ColorShader = ProgramFactory.Create<ColorShaderProgram>();
            WireframeShader = ProgramFactory.Create<WireframeShaderProgram>();
            ModelShader = ProgramFactory.Create<ModelShaderProgram>();
            WireframeShader2 = ProgramFactory.Create<WireframeShader2Program>();
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
        }

        #endregion

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


        #region Stencil Buffer

        public static void EnableStencilTest()
        {
            GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0);
            GL.Clear(ClearBufferMask.StencilBufferBit);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
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

        public static void RemoveStencilMask()
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
