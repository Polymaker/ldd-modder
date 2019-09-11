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
        public int PartID { get; set; }
        public string Description { get; set; }

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

            var primitive = Primitive.Load(primitiveFile);

            var partProject = new PartProject()
            {
                PartID = partID,
                Description = primitive.Name
            };

            int collisionCount = 0;
            foreach (var coll in primitive.Collisions)
            {
                var colNode = CollisionNode.Create(coll);
                colNode.ID = GenerateUUID($"P{partID}_C1{collisionCount}");
                colNode.Description += $" {collisionCount+1}";
                partProject.Collisions.Add(colNode);
                collisionCount++;
            }

            int connectionCount = 0;
            foreach (var conn in primitive.Connectors)
            {
                var conNode = ConnectionNode.Create(conn);
                conNode.ID = GenerateUUID($"P{partID}_C2{connectionCount}");
                partProject.Connections.Add(conNode);
                connectionCount++;
            }

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
                    ID = GenerateUUID($"P{partID}_S{surfaceNumber}"),
                    SurfaceID = surfaceNumber,
                    IsTextured = meshFile.IsTextured,
                    IsFlexible = meshFile.IsFlexible,
                    SubMaterialID = materialIndex
                };

                partProject.Surfaces.Add(surface);
                int meshCounter = 0;

                foreach (var culling in meshFile.Cullings)
                {
                    SurfaceComponent component = new SurfaceComponent()
                    {
                        ID = GenerateUUID($"P{partID}_S{surfaceNumber}_C{surface.Components.Count}"),
                        ComponentType = culling.Type
                    };

                    //if (culling.Type == LDD.Meshes.MeshCullingType.MainModel &&
                    //    surface.Components.Any(x => x.ComponentType == LDD.Meshes.MeshCullingType.MainModel))
                    //{
                    //    component = surface.Components
                    //        .FirstOrDefault(x => x.ComponentType == LDD.Meshes.MeshCullingType.MainModel);
                    //}

                    surface.Components.Add(component);

                    var subMesh = meshFile.GetCullingGeometry(culling, false);

                    component.Meshes.Add(new MeshElement(subMesh)
                    {
                        ID = GenerateUUID($"P{partID}_S{surfaceNumber}_C{surface.Components.Count}_M{meshCounter++}")
                    });

                    if (culling.ReplacementMesh != null)
                    {
                        component.AlternateMeshes.Add(new MeshElement(culling.ReplacementMesh)
                        {
                            ID = GenerateUUID($"P{partID}_S{surfaceNumber}_C{surface.Components.Count}_M{meshCounter++}")
                        });
                    }


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


        #region Save

        private void UpdateMeshFilenames()
        {
            var allMeshes = Surfaces.SelectMany(s => s.Components.SelectMany(c => c.GetAllMeshes()));
            foreach (var elem in allMeshes)
                elem.UpdateFilename();
        }

        private XDocument GenerateXmlHierarchy()
        {
            var rootNode = new XElement("LDP");
            var projectXml = new XDocument(rootNode);

            var infoNode = new XElement("Info");
            rootNode.Add(infoNode);
            infoNode.Add(new XElement("PartID", PartID));
            infoNode.Add(new XElement("Description", Description));


            if (Surfaces.Any())
            {
                var surfaceElem = new XElement("Surfaces");
                foreach (var surf in Surfaces)
                    surfaceElem.Add(surf.SerializeHierarchy());

                rootNode.Add(surfaceElem);
            }

            UpdateMeshFilenames();

            var allMeshes = Surfaces.SelectMany(s => s.Components.SelectMany(c => c.GetAllMeshes()));
            if (allMeshes.Any())
            {
                var meshesElem = new XElement("Meshes");
                foreach (var elem in allMeshes)
                {
                    meshesElem.Add(new XElement("Mesh", 
                        new XAttribute("ID", elem.ID),
                        new XAttribute("File", elem.Filename)
                        ));
                }
                rootNode.Add(meshesElem);
            }

            rootNode.Add(Collisions.SerializeHierarchy());
            rootNode.Add(Connections.SerializeHierarchy());
            return projectXml;
        }

        public void SaveCompressed(string filename)
        {
            filename = Path.GetFullPath(filename);
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fs = File.Open(filename, FileMode.Create))
            using (var zipStream = new ZipOutputStream(fs))
            {
                zipStream.SetLevel(3);
                var projectXml = GenerateXmlHierarchy();

                zipStream.PutNextEntry(new ZipEntry("Project.xml"));
                projectXml.Save(zipStream);
                zipStream.CloseEntry();

                foreach (var surface in Surfaces)
                {
                    foreach (var component in surface.Components)
                    {
                        foreach (var meshElem in component.GetAllMeshes())
                        {
                            //zipStream.PutNextEntry(new ZipEntry($"Meshes\\Surface_{surface.ID}\\{component.ID}_{meshElem.ID}.geom"));
                            zipStream.PutNextEntry(new ZipEntry(meshElem.Filename));
                            meshElem.Geometry.Save(zipStream);
                            zipStream.CloseEntry();
                        }
                    }
                }
            }
        }

        public void SaveUncompressed(string directory)
        {
            directory = Path.GetFullPath(directory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var projectXml = GenerateXmlHierarchy();
            projectXml.Save(Path.Combine(directory, "Project.xml"));

            var allMeshes = Surfaces.SelectMany(s => s.Components.SelectMany(c => c.GetAllMeshes()));

            foreach (var elem in allMeshes)
            {
                var fullPath = Path.Combine(directory, elem.Filename);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                elem.Geometry.Save(fullPath);
            }
        }

        #endregion

        public static PartProject Load(string path)
        {
            return null;
        }

        internal static string GenerateUUID(string uniqueID)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(uniqueID);
            byte[] hashedBytes = new System.Security.Cryptography
                .SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            var guid = new Guid(hashedBytes);
            return guid.ToString("N").Substring(0, 10);
        }
    }
}
