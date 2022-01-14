using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2DPixelArtEngine
{
    public class PixelEngine
    {
        private GraphicsDevice _graphicsDevice;

        private int _width;
        private int _height;
        private int _pixelWidth;
        private int _pixelHeight;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                _pixelWidth = (int)Math.Ceiling((float)_width / PixelRatio);
            }
        }
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                _pixelHeight = (int)Math.Ceiling((float)_height / PixelRatio);
            }
        }

        public int PixelRatio;

        public List<Object> Scene;

        private Texture2D _rectangle;

        public Color[] Screen;

        public PixelEngine(GraphicsDevice graphicsDevice, int width, int height, int pixelRatio = 3)
        {
            _graphicsDevice = graphicsDevice;

            _rectangle = new Texture2D(_graphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            _rectangle.SetData(data);

            PixelRatio = pixelRatio;
            Width = width;
            Height = height;

            Screen = new Color[_pixelWidth * _pixelHeight];
            _screen = new Texture2D(_graphicsDevice, _pixelWidth, _pixelHeight);

            Scene = new List<Object>() { };
        }

        public Texture2D ImportTexture(string fileLocation)
        {
            FileStream fileStream = new FileStream(fileLocation, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(_graphicsDevice, fileStream);
            fileStream.Dispose();

            return texture;
        }
    }
}
