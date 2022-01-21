using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
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
        private static Dictionary<string, Texture2D> _loadedTextures;
        public static Dictionary<string, SpriteFont> _loadedFonts;
        public static Texture2D Pixel;
        public static string AssetDirectory;

        public static void Initialize(GraphicsDevice graphicsDevice, string assetDirectory)
        {
            _graphicsDevice = graphicsDevice;
            _loadedTextures = new Dictionary<string, Texture2D>();
            _loadedFonts = new Dictionary<string, SpriteFont>();

            Pixel = new Texture2D(_graphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            Pixel.SetData(data);

            AssetDirectory = assetDirectory;
        }

        public static Texture2D LoadTexture(string fileLocation)
        {
            if (_loadedTextures.ContainsKey(fileLocation.ToLower())) return _loadedTextures[fileLocation.ToLower()];

            FileStream fileStream = new FileStream(AssetDirectory + fileLocation, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);
            fileStream.Dispose();

            _loadedTextures.Add(fileLocation.ToLower(), texture);

            return texture;
        }

        public static SpriteFont LoadFont(string fileLocation)
        {
            if (_loadedFonts.ContainsKey(fileLocation.ToLower())) return _loadedFonts[fileLocation.ToLower()];

            TtfFontBakerResult fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(AssetDirectory + fileLocation),
                25,
                1024,
                1024,
                new[]
                {
                    CharacterRange.BasicLatin,
                    CharacterRange.Latin1Supplement,
                    CharacterRange.LatinExtendedA,
                    CharacterRange.Cyrillic
                }
            );

            SpriteFont font = fontBakeResult.CreateSpriteFont(_graphicsDevice);

            _loadedFonts.Add(fileLocation.ToLower(), font);

            return font;
        }

        public static void Unload(string fileLocation)
        {
            if (!_loadedTextures.ContainsKey(fileLocation.ToLower())) return;
            _loadedTextures.Remove(fileLocation.ToLower());
        }
    }
}
