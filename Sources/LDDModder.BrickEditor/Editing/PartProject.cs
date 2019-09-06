using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Core;

namespace LDDModder.BrickEditor.Editing
{
    public class PartProject
    {
        public List<PartSurface> Surfaces { get; set; }


        public RootNode Collisions { get; private set; }
        public RootNode Connections { get; private set; }

        public PartProject()
        {
            Surfaces = new List<PartSurface>();
            Collisions = new RootNode("Collisions", this);
            Connections = new RootNode("Connections", this);
        }

        public static PartProject CreateFromLdd(string lddDbPath, int partID)
        {
            var primitivesPath = Path.Combine(lddDbPath, "Primitives");
            var meshesPath = Path.Combine(lddDbPath, "Primitives", "LOD0");

            var primitiveFile = Path.Combine(primitivesPath, $"{partID}.xml");

            if (!File.Exists(primitiveFile))
                throw new FileNotFoundException($"Part Info not found. ({partID}.xml)");

            if (!File.Exists(Path.Combine(meshesPath, $"{partID}.g")))
                throw new FileNotFoundException($"Part Mesh not found. ({partID}.g)");

            var primitive = Primitive.FromXmlFile(primitiveFile);

            var partProject = new PartProject();

            int colIdx = 1;
            foreach (var coll in primitive.Collisions)
            {
                var colNode = CollisionNode.Create(coll);
                colNode.Description += $" {colIdx++}";
                partProject.Collisions.Add(colNode);
            }

            foreach (var conn in primitive.Connectors)
                partProject.Connections.Add(ConnectionNode.Create(conn));

            foreach (string meshFilePath in Directory.GetFiles(meshesPath, $"{partID}.g*"))
            {
                var meshFile = MeshFile.Read(meshFilePath);
                var fileExt = Path.GetExtension(meshFilePath).ToLower();

                int surfaceNumber = 0;
                int materialIndex = 0;

                if (!fileExt.EndsWith("g"))
                {
                    fileExt = fileExt.Substring(fileExt.IndexOf('g') + 1);
                    if (!int.TryParse(fileExt, out surfaceNumber))
                        surfaceNumber = partProject.Surfaces.Count;
                }

                if (primitive.SubMaterials != null &&
                    surfaceNumber < primitive.SubMaterials.Length)
                {
                    materialIndex = primitive.SubMaterials[surfaceNumber];
                }

                var surface = new PartSurface
                {
                    SurfaceID = surfaceNumber,
                    SubMaterialID = materialIndex
                };
                surface.GenerateID();

                partProject.Surfaces.Add(surface);

                foreach (var culling in meshFile.Cullings)
                {
                    SurfaceComponent component = new SurfaceComponent() { ComponentType = culling.Type };
                    component.GenerateID();

                    //if (culling.Type == LDD.Meshes.MeshCullingType.MainModel &&
                    //    surface.Components.Any(x => x.ComponentType == LDD.Meshes.MeshCullingType.MainModel))
                    //{
                    //    component = surface.Components
                    //        .FirstOrDefault(x => x.ComponentType == LDD.Meshes.MeshCullingType.MainModel);
                    //}

                    surface.Components.Add(component);

                    var subMesh = meshFile.GetCullingGeometry(culling, false);

                    component.Meshes.Add(new MeshElement(subMesh));

                    if (culling.ReplacementMesh != null)
                        component.AlternateMeshes.Add(new MeshElement(culling.ReplacementMesh));


                    if (culling.Studs != null)
                    {
                        foreach (var stud in culling.Studs)
                        {
                            component.LinkedStuds.Add(new StudReference()
                            {
                                ConnectorNode = (ConnectionNode)partProject.Connections.Nodes[stud.ConnectorIndex],
                                StudIndex = stud.FieldIndices[0].Index,
                                Value1 = stud.FieldIndices[0].Value2,
                                Value2 = stud.FieldIndices[0].Value4
                            });
                        }
                    }
                }
            }


            return partProject;
        }

        public void Save(string filename)
        {
            filename = Path.GetFullPath(filename);
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fs = File.Open(filename, FileMode.Create))
            using (var zipStream = new ZipOutputStream(fs))
            {
                zipStream.SetLevel(3);
                var rootNode = new XElement("LDP");
                var projectXml = new XDocument(rootNode);
                var surfaceElem = new XElement("Surfaces");
                foreach(var surf in Surfaces)
                    surfaceElem.Add(surf.SerializeHierarchy());
                rootNode.Add(surfaceElem);

                rootNode.Add(Collisions.SerializeHierarchy());
                rootNode.Add(Connections.SerializeHierarchy());


                zipStream.PutNextEntry(new ZipEntry("Project.xml"));
                projectXml.Save(zipStream);
                zipStream.CloseEntry();

                foreach (var surface in Surfaces)
                {
                    foreach (var component in surface.Components)
                    {
                        foreach (var meshElem in component.GetAllMeshes())
                        {
                            zipStream.PutNextEntry(new ZipEntry($"Meshes\\Surface_{surface.ID}\\{component.ID}_{meshElem.ID}.geom"));
                            meshElem.Geometry.Save(zipStream);
                            zipStream.CloseEntry();
                        }
                    }
                }
            }
        }

        public static PartProject Load(string path)
        {
            return null;
        }
    }
}
