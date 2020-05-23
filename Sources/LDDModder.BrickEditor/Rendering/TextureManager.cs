using ObjectTK.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using LDDModder.BrickEditor.Resources;

namespace LDDModder.BrickEditor.Rendering
{
    static class TextureManager
    {
        public static Texture2D StudGridTexture { get; private set; }
        public static Texture2D StudConnectionGrid { get; private set; }
        public static void InitializeResources()
        {
            StudGridTexture = LoadTexture("Textures.StudGridTex.png", 
                TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Clamp);

            StudConnectionGrid = LoadTexture("Textures.StudConnectionGrid.png",
                TextureMinFilter.Linear, TextureMagFilter.Linear, TextureWrapMode.Repeat);
        }

        public static void ReleaseResources()
        {
            if (StudGridTexture != null)
            {
                StudGridTexture.Dispose();
                StudGridTexture = null;
            }

            if (StudConnectionGrid != null)
            {
                StudConnectionGrid.Dispose();
                StudConnectionGrid = null;
            }
        }

        public static Texture2D LoadTexture(string imageName, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapMode)
        {
            using (var image = ResourceHelper.GetResourceImage(imageName))
                return LoadTexture(image, minFilter, magFilter, wrapMode);
        }

        public static Texture2D LoadTexture(Bitmap image, TextureMinFilter minFilter, TextureMagFilter magFilter, TextureWrapMode wrapMode)
        {
            BitmapTexture.CreateCompatible(image, out Texture2D tex, 1);
            tex.LoadBitmap(image, 0);
            tex.SetFilter(minFilter, magFilter);
            tex.SetWrapMode(wrapMode);
            return tex;
        }
    }
}
