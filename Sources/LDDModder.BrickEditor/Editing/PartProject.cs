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
        public RootNode Models { get; private set; }
        public RootNode Collisions { get; private set; }
        public RootNode Connections { get; private set; }

        public PartProject()
        {
            Models = new RootNode("Models", this);
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

            foreach (string meshFilePath in Directory.GetFiles(meshesPath, $"{partID}.g*"))
            {
                var mesh = MeshFile.Read(meshFilePath);

                var modelNode = PartNode.Create<PartModelNode>();
                var fileExt = Path.GetExtension(meshFilePath).ToLower();
                
                if (!fileExt.EndsWith("g"))
                {
                    fileExt = fileExt.Substring(fileExt.IndexOf('g') + 1);
                    modelNode.DecorationNumber = int.Parse(fileExt);
                }

                foreach (var culling in mesh.Cullings)
                {
                    var subMesh = mesh.GetCullingGeometry(culling, false);
                    var meshNode = new ModelMeshNode(subMesh) { MeshType = culling.Type };
                    modelNode.Add(meshNode);
                }

                partProject.Models.Add(modelNode);
            }

            foreach (var coll in primitive.Collisions)
                partProject.Collisions.Add(CollisionNode.Create(coll));

            foreach (var conn in primitive.Connectors)
                partProject.Connections.Add(ConnectionNode.Create(conn));


            return partProject;
        }

        public void Save(string filename)
        {
            filename = Path.GetFullPath(filename);
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var buffer = new byte[4096];

            using (var fs = File.Open(filename, FileMode.Create))
            using (var zipStream = new ZipOutputStream(fs))
            {
                zipStream.SetLevel(3);
                var rootNode = new XElement("LDP");
                var projectXml = new XDocument(rootNode);
                rootNode.Add(Models.SerializeHierarchy());
                rootNode.Add(Collisions.SerializeHierarchy());
                rootNode.Add(Connections.SerializeHierarchy());


                using (var tmpMS = new MemoryStream())
                {
                    projectXml.Save(tmpMS);
                    tmpMS.Seek(0, SeekOrigin.Begin);

                    zipStream.PutNextEntry(new ZipEntry("Project.xml")
                    {
                        DateTime = DateTime.Now,
                        Size = tmpMS.Length
                    });
                    
                    StreamUtils.Copy(tmpMS, zipStream, buffer);

                    zipStream.CloseEntry();
                }

                foreach (var modelNode in Models.Nodes.OfType<PartModelNode>())
                {
                    foreach (var meshNode in modelNode.Nodes.OfType<ModelMeshNode>())
                    {
                        using (var tmpMS = new MemoryStream())
                        {
                            meshNode.Mesh.Save(tmpMS);
                            tmpMS.Seek(0, SeekOrigin.Begin);

                            zipStream.PutNextEntry(new ZipEntry($"Models\\{modelNode.ID}\\{meshNode.ID}.geom")
                            {
                                DateTime = DateTime.Now,
                                Size = tmpMS.Length
                            });

                            StreamUtils.Copy(tmpMS, zipStream, buffer);

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
