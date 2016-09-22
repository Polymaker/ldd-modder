using ICSharpCode.SharpZipLib.Core;
using LDDModder.LDD;
using LDDModder.LDD.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.Modding
{
    public static class PackageInstaller
    {
        static string PrimitivesPath
        {
            get
            {
                return LDDManager.GetDirectory(LDDManager.DbDirectories.Primitives);
            }
        }

        static string ModelsPath
        {
            get
            {
                return LDDManager.GetDirectory(LDDManager.DbDirectories.LOD0);
            }
        }

        static string DecorationsPath
        {
            get
            {
                return LDDManager.GetDirectory(LDDManager.DbDirectories.Decorations);
            }
        }

        public static void InstallPackage(BrickPackage package)
        {
            if (!LDDManager.IsInstalled)
                return;
            if (!LDDManager.IsLifExtracted(LifInstance.Database))
                return;

            foreach (var brick in package.Bricks)
                InstallBrick(package, brick);

            foreach (var decId in package.Decorations)
                InstallDecoration(package, decId);
        }

        private static void InstallBrick(BrickPackage package, BrickPackage.BrickInfo brick)
        {
            string brickFilePath = Path.Combine(PrimitivesPath, string.Format("{0}.xml", brick.Id));
            bool brickNotInstalled = false;
            if (!File.Exists(brickFilePath))
            {
                WriteFile(brickFilePath, package.GetBrick(brick));
            }
            else
            {
                var existingBrick = Primitive.Load(brickFilePath);
                if (brick.Version > existingBrick.Version)
                    WriteFile(brickFilePath, package.GetBrick(brick));
                else
                    brickNotInstalled = true;
            }

            foreach (var modelName in package.Models.Where(name => name.StartsWith(brick.Id.ToString())))
            {
                string modelFilePath = Path.Combine(ModelsPath, modelName);
                if (!brickNotInstalled || !File.Exists(modelFilePath))
                    WriteFile(modelFilePath, package.GetModel(modelName));
            }
        }

        private static void InstallDecoration(BrickPackage package, int decorationId)
        {
            string decFilePath = Path.Combine(DecorationsPath, decorationId + ".png");

        }

        private static void WriteFile(string filepath, Stream stream)
        {
            byte[] buffer = new byte[4096];
            using (FileStream sw = File.Create(filepath))
                StreamUtils.Copy(stream, sw, buffer);
        }
    }
}
