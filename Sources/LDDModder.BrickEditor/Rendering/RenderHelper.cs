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

        public static ModelShaderProgram ModelShader { get; private set; }



        public static void InitializeShaders()
        {
            ColorShader = ProgramFactory.Create<ColorShaderProgram>();
            WireframeShader = ProgramFactory.Create<WireframeShaderProgram>();
            ModelShader = ProgramFactory.Create<ModelShaderProgram>();
        }

        public static void DisposeShaders()
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

            if (ModelShader != null)
            {
                ModelShader.Dispose();
                ModelShader = null;
            }
        }

        #endregion

        public static void InitializeViewProjection(Matrix4 view, Matrix4 projection)
        {
            WireframeShader.Use();
            WireframeShader.ViewMatrix.Set(view);
            WireframeShader.Projection.Set(projection);

            ColorShader.Use();
            ColorShader.ViewMatrix.Set(view);
            ColorShader.Projection.Set(projection);

            ModelShader.Use();
            ModelShader.ViewMatrix.Set(view);
            ModelShader.Projection.Set(projection);

            GL.UseProgram(0);
        }

        public static void InitializeMatrices(Camera camera)
        {
            var viewMatrix = camera.GetViewMatrix();
            var projection = camera.GetProjectionMatrix();

            WireframeShader.Use();
            WireframeShader.ViewMatrix.Set(viewMatrix);
            WireframeShader.Projection.Set(projection);

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
            BeginDrawColorModel(vertexBuffer, transform, material.Diffuse);
        }

        public static void BeginDrawColorModel(IVertexBuffer vertexBuffer, Matrix4 transform, Vector4 color)
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

            //GL.PopAttrib();
        }

        public static void EndDrawWireframe()
        {
            GL.PopAttrib();
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
