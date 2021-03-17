using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace LDDModder.BrickEditor.Rendering.Models
{
    public class HardEdgesModel : ModelBase
    {
        public List<Vector3> EdgePoints { get; set; }

        public HardEdgesModel()
        {
            EdgePoints = new List<Vector3>();
        }

        public override bool RayIntersects(Ray ray, out float distance)
        {
            distance = 0;
            return false;
        }

        public override void RenderModel(Camera camera, MeshRenderMode mode = MeshRenderMode.Solid)
        {
            base.RenderModel(camera, mode);
            RenderHelper.ColorShader.Use();
            RenderHelper.ColorShader.ModelMatrix.Set(Transform);
            RenderHelper.ColorShader.Color.Set(new Vector4(1,0,1,1));
            GL.PushAttrib(AttribMask.LineBit);
            GL.LineWidth(2f);
            GL.Begin(PrimitiveType.Lines);
            foreach (var v in EdgePoints)
                GL.Vertex3(v);
            GL.End();
            GL.PopAttrib();
        }

        public override void Dispose()
        {
            base.Dispose();

        }
    }
}
