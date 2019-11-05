using LDDModder.BrickEditor.Rendering.Shaders;
using LDDModder.LDD.Meshes;
using LDDModder.Modding.Editing;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLSurfaceModel : IDisposable
    {
        public PartSurface Surface { get; set; }

        public IndexedVertexBuffer VertexBuffer { get; private set; }

        public List<LddMeshModel> MeshModels { get; private set; }

        public MaterialInfo Material { get; set; }

        public GLSurfaceModel()
        {
            VertexBuffer = new IndexedVertexBuffer();
            MeshModels = new List<LddMeshModel>();
        }

        public GLSurfaceModel(PartSurface surface)
        {
            Surface = surface;
            VertexBuffer = new IndexedVertexBuffer();
            MeshModels = new List<LddMeshModel>();
        }

        public void RebuildPartModels()
        {
            MeshModels.Clear();
            VertexBuffer.ClearBuffers();

            var indexList = new List<int>();
            var vertexList = new List<VertVNT>();

            var distinctMeshes = new List<ModelMesh>();
            foreach (var surfComp in Surface.Components)
            {
                foreach (var meshRef in surfComp.Meshes)
                {
                    var addedModel = AddMeshGeometry(meshRef, indexList, vertexList);
                    addedModel.Visible = true;

                    if (!distinctMeshes.Contains(meshRef.ModelMesh))
                        distinctMeshes.Add(meshRef.ModelMesh);
                }

                if (surfComp is FemaleStudModel femaleStud)
                {
                    foreach (var meshRef in femaleStud.ReplacementMeshes)
                    {
                        var addedModel = AddMeshGeometry(meshRef, indexList, vertexList);
                        addedModel.Visible = false;

                        if (!distinctMeshes.Contains(meshRef.ModelMesh))
                            distinctMeshes.Add(meshRef.ModelMesh);
                    }
                }
            }

            distinctMeshes.ForEach(x => x.UnloadModel());

            VertexBuffer.SetIndices(indexList);
            VertexBuffer.SetVertices(vertexList);
        }

        private LddMeshModel AddMeshGeometry(ModelMeshReference modelMesh, List<int> indexList, List<VertVNT> vertexList)
        {
            var geometry = modelMesh.GetGeometry();
            int indexOffset = indexList.Count;
            int vertexOffset = vertexList.Count;
            var triangleIndices = geometry.GetTriangleIndices();
            indexList.AddRange(triangleIndices);

            foreach (var vert in geometry.Vertices)
            {
                vertexList.Add(new VertVNT()
                {
                    Position = vert.Position.ToGL(),
                    Normal = vert.Normal.ToGL(),
                    TexCoord = geometry.IsTextured ? vert.TexCoord.ToGL() : Vector2.Zero
                });
            }

            var model = new LddMeshModel(modelMesh, indexOffset, geometry.IndexCount, vertexOffset);
            MeshModels.Add(model);
            return model;
        }

        #region Shader Binding

        public void BindToShader(ModelShaderProgram modelShader)
        {
            modelShader.Use();
            VertexBuffer.Bind();
            VertexBuffer.BindAttribute(modelShader.Position, 0);
            VertexBuffer.BindAttribute(modelShader.Normal, 12);
            VertexBuffer.BindAttribute(modelShader.TexCoord, 24);
            modelShader.ModelMatrix.Set(Matrix4.Identity);
            //modelShader.ModelMatrix.Set(Transform);
            modelShader.Material.Set(Material);
        }

        public void UnbindShader(ModelShaderProgram modelShader)
        {
            VertexBuffer.Bind();
            VertexBuffer.UnbindAttribute(modelShader.Position);
            VertexBuffer.UnbindAttribute(modelShader.Normal);
            VertexBuffer.UnbindAttribute(modelShader.TexCoord);
        }

        public void BindToShader(WireframeShaderProgram wireframeShader)
        {
            wireframeShader.Use();

            VertexBuffer.Bind();
            VertexBuffer.BindAttribute(wireframeShader.Position, 0);
            wireframeShader.ModelMatrix.Set(Matrix4.Identity);
            //wireframeShader.ModelMatrix.Set(Transform);
        }

        public void UnbindShader(WireframeShaderProgram wireframeShader)
        {
            VertexBuffer.Bind();
            VertexBuffer.UnbindAttribute(wireframeShader.Position);
        }

        #endregion

        public void Draw()
        {
            VertexBuffer.Vao.DrawElements(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, VertexBuffer.IndexBuffer.ElementCount);
        }

        public void DrawMesh(LddMeshModel model)
        {
            VertexBuffer.Vao.DrawElementsBaseVertex(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles,
                model.StartVertex, model.IndexCount,
                OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, 
                model.StartIndex * 4);
        }


        public void Dispose()
        {
            if (VertexBuffer != null)
                VertexBuffer.Dispose();
        }
    }
}
