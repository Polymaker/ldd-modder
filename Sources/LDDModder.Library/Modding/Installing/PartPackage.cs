using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class PartPackage : IDisposable
    {
        public const string PACKAGE_XML_FILENAME = "package.xml";
        public const string MESH_FOLDER = "Meshes\\";
        public const string DECO_FOLDER = "Decorations\\";

        public int PartID { get; set; }
        
        public string Description { get; set; }

        public Primitive Primitive { get; set; }

        public List<PartMesh> Meshes { get; set; }

        public List<LDD.Palettes.Brick> Configurations { get; set; }

        public List<DecorationImage> DecorationImages { get; set; }

        public List<LDD.Palettes.Decoration> DecorationMappings { get; set; }

        public PartPackage()
        {
            Meshes = new List<PartMesh>();
            Configurations = new List<LDD.Palettes.Brick>();
            DecorationMappings = new List<LDD.Palettes.Decoration>();
            DecorationImages = new List<DecorationImage>();
        }

        public void Save(string filename)
        {
            filename = Path.GetFullPath(filename);
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (var fs = File.Open(filename, FileMode.Create))
                Save(fs);
        }

        public void Save(Stream stream)
        {
            var packageXml = new XDocument(new XElement("PartPackage"));
            packageXml.Root.Add(new XElement("Info", 
                new XElement("Part", PartID),
                new XElement("Description", Description)
                ));

            if (Meshes.Any())
            {
                var meshesElem = new XElement("Meshes");
                packageXml.Root.Add(meshesElem);
                foreach (var partMesh in Meshes)
                    meshesElem.Add(partMesh.Serialize());
            }

            if (DecorationImages.Any())
            {
                var decoElem = packageXml.Root.AddElement("Decorations");
                foreach (var decImg in DecorationImages)
                    decoElem.Add(decImg.Serialize());
            }

            if (Configurations.Any())
            {
                var brickElem = packageXml.Root.AddElement("Configurations");
                foreach(var brick in Configurations)
                {
                    var elem = new XElement("Brick",
                        new XAttribute("ElementID", brick.ElementID),
                        new XAttribute("MaterialID", brick.MaterialID));

                    
                    foreach (var dec in brick.Decorations)
                        elem.Add(XmlHelper.DefaultSerialize(dec));

                    foreach (var subMat in brick.SubMaterials)
                        elem.Add(XmlHelper.DefaultSerialize(subMat));

                    brickElem.Add(elem);
                }
            }

            using (var zipStream = new ZipOutputStream(stream))
            {
                zipStream.SetLevel(1);

                zipStream.PutNextEntry(new ZipEntry(PACKAGE_XML_FILENAME));
                packageXml.Save(zipStream);
                zipStream.CloseEntry();

                if (Primitive != null)
                {
                    zipStream.PutNextEntry(new ZipEntry("primitive.xml"));
                    Primitive.Save(zipStream);
                    zipStream.CloseEntry();
                }

                foreach (var partMesh in Meshes)
                {
                    zipStream.PutNextEntry(new ZipEntry($"{MESH_FOLDER}{partMesh.GetFileName()}"));
                    using (var ms = new MemoryStream())
                    {
                        partMesh.Mesh.Save(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.CopyTo(zipStream);
                    }
                    zipStream.CloseEntry();
                }

                foreach (var decImg in DecorationImages)
                {
                    zipStream.PutNextEntry(new ZipEntry($"{DECO_FOLDER}{decImg.GetFileName()}"));
                    decImg.Image.Save(zipStream, decImg.Image.RawFormat);
                    zipStream.CloseEntry();
                }
            }
        }

        public static PartPackage Read(Stream stream)
        {
            using (var zipFile = new ZipFile(stream))
            {
                var entry = zipFile.GetEntry(PACKAGE_XML_FILENAME);
                if (entry == null)
                    throw new InvalidDataException($"'{PACKAGE_XML_FILENAME}' file not found");

                XDocument packageXml = null;
                using (var zs = zipFile.GetInputStream(entry))
                    packageXml = XDocument.Load(zs);

                
            }

            return null;
        }

        public class DecorationImage
        {
            public string DecorationID { get; set; }
            public System.Drawing.Image Image { get; set; }

            public DecorationImage()
            {
            }

            public DecorationImage(string decorationID, Image image)
            {
                DecorationID = decorationID;
                Image = image;
            }

            public string GetFileName()
            {
                if (Image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                    return $"{DecorationID}.png";
                else if (Image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                    return $"{DecorationID}.jpg";
                else if (Image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                    return $"{DecorationID}.bmp";

                return string.Empty;
            }

            public XElement Serialize()
            {
                return new XElement("Decoration",
                    new XAttribute("DecorationID", DecorationID),
                    new XAttribute("File", DECO_FOLDER + GetFileName()));
            }
        }

        public class PartMesh
        {
            public int PartID { get; set; }
            public int SurfaceID { get; set; }
            public MeshFile Mesh { get; set; }

            public PartMesh()
            {
            }

            public PartMesh(int partID, int surfaceID, MeshFile mesh)
            {
                PartID = partID;
                SurfaceID = surfaceID;
                Mesh = mesh;
            }

            public string GetFileName()
            {
                if (SurfaceID > 0)
                    return $"{PartID}.g{SurfaceID}";

                return $"{PartID}.g";
            }

            public XElement Serialize()
            {
                return new XElement("Mesh", 
                    new XAttribute("SurfaceID", SurfaceID), 
                    new XAttribute("File", MESH_FOLDER + GetFileName()));
            }
        }


        public static void CreateLDDPackages()
        {
            string lddDir = Environment.ExpandEnvironmentVariables(@"%appdata%\LEGO Company\LEGO Digital Designer\");
            string primitiveDir = Path.Combine(lddDir, "db\\Primitives");
            string meshDir = Path.Combine(lddDir, "db\\Primitives\\LOD0");
            string decorationDir = Path.Combine(lddDir, "db\\Decorations");
            var lddPalette = PaletteFile.FromLif(Path.Combine(lddDir, "Palettes\\LDD.lif")).Palettes[0];
            var xmlDecMap = XDocument.Load(Path.Combine(lddDir, "db\\DecorationMapping.xml"));
            var decMappings = new List<LDD.Data.DecorationMapping>();
            foreach (var elem in xmlDecMap.Root.Elements("Mapping"))
                decMappings.Add(XmlHelper.DefaultDeserialize<LDD.Data.DecorationMapping>(elem));

            foreach (var primitivePath in Directory.GetFiles(primitiveDir, "*.xml")
                .OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x))))
            {
                var primitive = Primitive.Load(primitivePath);

                var package = new PartPackage()
                {
                    PartID = int.Parse(Path.GetFileNameWithoutExtension(primitivePath)),
                    Primitive = primitive
                };
                var myDecorations = decMappings.Where(x => x.DesignID == package.PartID);

                if (myDecorations.Any())
                {
                    package.DecorationMappings.AddRange(
                        myDecorations.Select(x =>
                            new LDD.Palettes.Decoration(x.SurfaceID, x.DecorationID)));

                    foreach (string decID in package.DecorationMappings.Select(x => x.DecorationID).Distinct())
                    {
                        var imagePath = Directory.EnumerateFiles(decorationDir, decID + ".*").FirstOrDefault();
                        if (string.IsNullOrEmpty(imagePath))
                            continue;
                        var img = Image.FromFile(imagePath);
                        package.DecorationImages.Add(new DecorationImage(decID, img));
                    }
                }

                var myElements = lddPalette.Bricks.Where(x => x.DesignID == package.PartID);
                if (myElements.Any())
                {
                    package.Configurations.AddRange(myElements);
                }

                int surfaceId = 0;
                foreach (var meshPath in Directory.GetFiles(meshDir, package.PartID + ".g*").OrderBy(x => x))
                {
                    var mesh = MeshFile.Read(meshPath);
                    package.Meshes.Add(new PartMesh(package.PartID, surfaceId++, mesh));
                }

                string folderName = package.PartID.ToString()[0].ToString().PadRight(package.PartID.ToString().Length, '0');

                package.Save($"LPI TEST\\{folderName}\\{package.PartID}.lpi");

                package.Dispose();
            }
        }

        public void Dispose()
        {
            Meshes.Clear();
            DecorationImages.ForEach(x => x.Image.Dispose());
            DecorationImages.Clear();
            Configurations.Clear();

        }
    }
}
