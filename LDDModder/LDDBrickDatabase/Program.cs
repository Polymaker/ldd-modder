using LDDModder.LDD.Files;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDBrickDatabase
{
    static class Program
    {
        static void Main(string[] args)
        {
            var destinationPath = Path.GetFullPath("Assets");
            //Directory.CreateDirectory(destinationPath);
            //using (var lifFile = LifFile.Open("Assets.lif"))
            //    lifFile.Extract(destinationPath, 1);

            //using (var lifFile = LifFile.Open(Path.Combine(destinationPath, "db.lif")))
            //    lifFile.Extract(Path.Combine(destinationPath, "db"), 2);

            var brickVersionFile = Path.Combine(destinationPath, @"db\info.xml");
            var dbInfoFile = XDocument.Load(brickVersionFile);

            if (int.TryParse(dbInfoFile.Root.Elements().FirstOrDefault()?.Attribute("version")?.Value, out int dbVersion))
            {
                var dbFilePath = Path.GetFullPath($"LDDDB_{dbVersion}.db");
                //if (File.Exists(dbFilePath))
                //    File.Delete(dbFilePath);
                //File.Copy(@"D:\Programming\C#\ldd-modder\LDDModder\LDDBrickDatabase\LDDBricks.db", dbFilePath);

                using (var conn = new SQLiteConnection($"Data Source={dbFilePath}"))
                {
                    conn.Open();

                    //using (var cmd = conn.CreateCommand())
                    //{
                    //    cmd.CommandText = $"INSERT INTO Info (DatabaseVersion) VALUES ({dbVersion})";
                    //    cmd.ExecuteNonQuery();
                    //}

                    using (var db = new LddBrickDB(conn))
                    {
                        //MapBrickModels(db, destinationPath);
                        //MapBricks(db, destinationPath);
                        MapAssemblies(db, destinationPath);
                        //MapPalettes(db, destinationPath);
                    }
                }
            }
        }

        private static void MapBrickModels(LddBrickDB db, string assetsPath)
        {
            var primitiveModelsPath = Path.Combine(assetsPath, @"db\Primitives\LOD0\");
            try
            {
                foreach (var brickModelPath in Directory.GetFiles(primitiveModelsPath, "*.g*"))
                {
                    int surfaceID = 0;
                    if (!brickModelPath.EndsWith("g"))
                    {
                        var ext = Path.GetExtension(brickModelPath);
                        surfaceID = int.Parse(ext.Substring(2));
                    }
                    var model = new BrickModel() { ID = int.Parse(Path.GetFileNameWithoutExtension(brickModelPath)), SurfaceID = surfaceID };
                    db.BrickModels.Add(model);
                }

                db.SaveChanges();
            }
            catch
            {
                var errors = db.GetValidationErrors();
            }
        }

        private static void MapBricks(LddBrickDB db, string assetsPath)
        {
            var primitivePath = Path.Combine(assetsPath, @"db\Primitives\");
            try
            {
                foreach (var brickInfoPath in Directory.GetFiles(primitivePath, "*.xml"))
                {
                    try
                    {
                        var info = Primitive.Load(brickInfoPath);
                        var brickID = int.Parse(Path.GetFileNameWithoutExtension(brickInfoPath));

                        if (brickID != info.Id)
                        {
                            Console.WriteLine($"The primitive ID's differ from the file name ({brickID}.xml <> {info.Id})");
                            if (!info.Aliases.Contains(info.Id))
                                info.Aliases.Add(info.Id);
                            if (!info.Aliases.Contains(brickID))
                                info.Aliases.Add(brickID);

                            info.Id = brickID;
                            /*
                            var model1exist = db.BrickModels.Any(m => m.ID == info.Id);
                            var model2exist = db.BrickModels.Any(m => m.ID == brickID);

                            if (!model1exist && !model2exist)
                            {
                                Console.WriteLine("No brick model exist for either of them!");
                            }
                            else if (model1exist == model2exist && model1exist)
                            {
                                Console.WriteLine("A brick model exist for both of them!");
                                //info.Id = brickID;
                            }
                            else if (model2exist)
                            {
                                //info.Id = brickID;
                            }
                            */
                        }

                        var brick = new Brick()
                        {
                            ID = info.Id,
                            Name = info.Name,
                            GroupID = info.Group?.Id ?? 0,
                            PlatformID = info.Platform?.Id ?? 0,
                            Version = info.DesignVersion ?? 0,
                            VersionMajor = info.Version.Major,
                            VersionMinor = info.Version.Minor
                        };
                        

                        db.Bricks.Add(brick);

                        if (info.Aliases.Any())
                        {
                            foreach (var aliasID in info.Aliases)
                            {
                                if (aliasID != brick.ID)
                                    brick.Aliases.Add(new BrickAlias() { Brick = brick, BrickID = brick.ID, Alias = aliasID });
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                    }
                }

                db.SaveChanges();
            }
            catch(Exception ex)
            {
                var errors = db.GetValidationErrors();
            }
        }

        class XmlNodeInfo
        {
            public string NodeName { get; set; }
            public XmlNodeInfo Parent { get; set; }
            public int Level => Parent == null ? 0 : Parent.Level + 1;

            public List<XmlNodeInfo> SubNodes { get; } = new List<XmlNodeInfo>();
            public List<string> Attributes { get; } = new List<string>();
            public int MaxElements { get; set; }

            public XmlNodeInfo(string nodeName)
            {
                NodeName = nodeName;
            }

            public XmlNodeInfo(XmlNodeInfo parent, string nodeName)
            {
                Parent = parent;
                NodeName = nodeName;
            }

            public XmlNodeInfo AddNode(string name)
            {
                var newNode = new XmlNodeInfo(this, name);
                SubNodes.Add(newNode);
                return newNode;
            }

            public bool Contains(string nodeName)
            {
                return SubNodes.Any(n => n.NodeName == nodeName);
            }

            public override string ToString()
            {
                return NodeName;
            }
        }

        private static void MapAssemblies(LddBrickDB db, string assetsPath)
        {
            var assembliesPath = Path.Combine(assetsPath, @"db\Assemblies\");
            var allNodes = new List<XmlNodeInfo>();
            var nodesPerLevel = new Dictionary<int, List<XmlNodeInfo>>();

            foreach(var assemFile in Directory.GetFiles(assembliesPath, "*.lxfml"))
            {
                var doc = XDocument.Load(assemFile);
                foreach(var node in doc.Descendants())
                {
                    var depth = node.GetDepth();
                    if (!nodesPerLevel.ContainsKey(depth))
                        nodesPerLevel.Add(depth, new List<XmlNodeInfo>());
                    
                    if (depth == 0)
                    {
                        if(!nodesPerLevel[0].Any(n=>n.NodeName == node.Name.LocalName))
                        {
                            var nodeInfo = new XmlNodeInfo(node.Name.LocalName);
                            nodesPerLevel[depth].Add(nodeInfo);
                            allNodes.Add(nodeInfo);
                        }
                    }
                    else
                    {
                        var parentNode = nodesPerLevel[depth - 1].FirstOrDefault(n => n.NodeName == node.Parent.Name.LocalName);
                        if(parentNode != null && !parentNode.Contains(node.Name.LocalName))
                        {
                            var nodeInfo = parentNode.AddNode(node.Name.LocalName);
                            nodesPerLevel[depth].Add(nodeInfo);
                            allNodes.Add(nodeInfo);
                        }
                    }

                    var myNode = nodesPerLevel[depth].FirstOrDefault(n => n.NodeName == node.Name.LocalName);

                    if (myNode != null)
                    {
                        if(node.Parent != null)
                        {
                            myNode.MaxElements = Math.Max(myNode.MaxElements, node.Parent.Elements().Count(e => e.Name == node.Name));
                        }
                        

                        if (node.Attributes().Any())
                        {
                            foreach (var attr in node.Attributes())
                            {
                                if (!myNode.Attributes.Contains(attr.Name.LocalName))
                                    myNode.Attributes.Add(attr.Name.LocalName);
                            }
                        }
                    }
                }
            }

            foreach(var nodeInfo in allNodes)
            {
                if (nodeInfo.Attributes.Count == 0 && nodeInfo.SubNodes.Count == 1)
                    continue;

                string nodeCode = $"[XmlRoot(\"{nodeInfo.NodeName}\")]\r\n";
                nodeCode += $"public class {nodeInfo.NodeName}\r\n{{\r\n";
                foreach (var attr in nodeInfo.Attributes)
                {
                    nodeCode += $"\t[XmlAttribute(\"{attr}\")]\r\n";
                    nodeCode += $"\tpublic string {attr.Capitalize()} {{ get; set; }}\r\n\r\n";
                }

                foreach(var subNode in nodeInfo.SubNodes)
                {
                    if(subNode.MaxElements > 1 || (subNode.Attributes.Count == 0 && subNode.SubNodes.Count == 1 && subNode.SubNodes[0].MaxElements > 1))
                    {
                        var subClassName = subNode.NodeName.Capitalize();
                        if (subClassName.EndsWith("s"))
                            subClassName = subClassName.Substring(0, subClassName.Length - 1);

                        nodeCode += $"\tpublic List<{subClassName}> {subClassName}s {{ get; set; }}\r\n\r\n";
                    }
                    else
                    {
                        //nodeCode += $"\t[XmlAttribute(\"{subNode.NodeName}\")]\r\n";
                        nodeCode += $"\tpublic {subNode.NodeName} {subNode.NodeName} {{ get; set; }}\r\n\r\n";
                    }
                }

                nodeCode = nodeCode.TrimEnd() + "\r\n";
                nodeCode += "}\r\n";
                Trace.WriteLine(nodeCode);
            }
        }

        private static void MapPalettes(LddBrickDB db, string assetsPath)
        {
            var palettesPath = Path.Combine(assetsPath, @"Palettes\");

            try
            {

                foreach (var paletteFilename in Directory.GetFiles(palettesPath, "*.lif"))
                {
                    try
                    {
                        var fileInfo = LDDModder.LDD.Palettes.PaletteManager.LoadFromLif(paletteFilename);

                        if (fileInfo.Palettes.Count == 0)
                            continue;

                        foreach(var lddPalette in fileInfo.Palettes)
                        {
                            var palette = new Palette()
                            {
                                Name = Path.GetFileNameWithoutExtension(lddPalette.OriginFileName),
                                BagName = fileInfo.Name,
                                Version = fileInfo.Info.PaletteVersion,
                                VersionMajor = fileInfo.Info.FileVersion.Major,
                                VersionMinor = fileInfo.Info.FileVersion.Minor
                            };
                            Console.WriteLine(palette.Name);
                            db.Palettes.Add(palette);

                            foreach (var item in lddPalette.Items)
                            {
                                if (item is LDDModder.LDD.Palettes.Brick brick)
                                {
                                    palette.Bricks.Add(new PaletteBrick()
                                    {
                                        BagName = palette.BagName,
                                        PaletteName = palette.Name,
                                        DesignID = brick.DesignID,
                                        ItemNos = int.Parse(brick.ElementID),
                                        MaterialID = brick.MaterialID
                                    });
                                }
                                else if (item is LDDModder.LDD.Palettes.Assembly assem)
                                {
                                    palette.Assemblies.Add(new PaletteAssembly()
                                    {
                                        BagName = palette.BagName,
                                        PaletteName = palette.Name,
                                        DesignID = assem.DesignID,
                                        ItemNos = int.Parse(assem.ElementID)
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var errors = db.GetValidationErrors();
            }
        }
    }
}
