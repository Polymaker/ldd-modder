using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering.Models
{
    public class EditorGridModel : ModelBase
    {
        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = float.NaN;
            return false;
        }

        public override void RenderModel(Camera camera)
        {
            RenderHelper.GridShader.MVMatrix.Set(camera.GetViewMatrix());
            RenderHelper.GridShader.PMatrix.Set(camera.GetProjectionMatrix());
            RenderHelper.GridShader.FadeDistance.Set(camera.IsPerspective ? 20f : 0f);

            GL.DepthMask(false);

            GL.DepthMask(true);
        }
    }
}
