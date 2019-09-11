using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
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
    public class PartPackage
    {
        public const string PACKAGE_XML_FILENAME = "package.xml";
        public const string MESH_FOLDER = "Meshes\\";
        public const string DECO_FOLDER = "Decorations\\";

        public int PartID { get; set; }
        
        public string Description { get; set; }

        public Primitive Primitive { get; set; }

        public List<PartMesh> Meshes { get; set; }

        public List<LDD.Palettes.Palette.Brick> Configurations { get; set; }

        public List<DecorationImage> DecorationImages { get; set; }

        public List<LDD.Palettes.Decoration> DecorationMappings { get; set; }

        public PartPackage()
        {
            Meshes = new List<PartMesh>();
            Configurations = new List<LDD.Palettes.Palette.Brick>();
            DecorationMappings = new List<LDD.Palettes.Decoration>();
            DecorationImages = new List<DecorationImage>();
        }

        public void Save(string filename)
        {
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
                var decoElem = new XElement("Decorations");
                packageXml.Root.Add(decoElem);
                foreach (var decImg in DecorationImages)
                    decoElem.Add(decImg.Serialize());
            }

            using (var zipStream = new ZipOutputStream(stream))
            {
                zipStream.SetLevel(3);

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
                    partMesh.Mesh.Save(zipStream);
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
                if (Image.RawFormat == System.Drawing.Imaging.ImageFormat.Png)
                    return $"{DecorationID}.png";
                else if (Image.RawFormat == System.Drawing.Imaging.ImageFormat.Jpeg)
                    return $"{DecorationID}.jpg";
                else if (Image.RawFormat == System.Drawing.Imaging.ImageFormat.Bmp)
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
    }
}
