using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace _2DPixelArtEngine
{
    public static class ContentManager
    {
        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, Texture2D> _loadedAssets;
        public static Texture2D Pixel;
        public static string AssetDirectory;

        public static void Initialize(GraphicsDevice graphicsDevice, string assetDirectory)
        {
            _graphicsDevice = graphicsDevice;
            _loadedAssets = new Dictionary<string, Texture2D>();

            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            Pixel.SetData(data);

            AssetDirectory = assetDirectory;
        }

        public static Texture2D Load(string fileLocation)
        {
            if (_loadedAssets.ContainsKey(fileLocation.ToLower())) return _loadedAssets[fileLocation.ToLower()];

            FileStream fileStream = new FileStream(AssetDirectory + fileLocation, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);
            fileStream.Dispose();

            _loadedAssets.Add(fileLocation.ToLower(), texture);

            return texture;
        }

        public static void Unload(string fileLocation)
        {
            if (!_loadedAssets.ContainsKey(fileLocation.ToLower())) return;
            _loadedAssets.Remove(fileLocation.ToLower());
        }
    }
}
