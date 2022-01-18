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
    public class PixelEngine
    {
        private GraphicsDevice _graphicsDevice;

        private int _width;
        private int _height;
        //private int _pixelWidth;
        //private int _pixelHeight;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                //_pixelWidth = (int)Math.Ceiling((float)_width / PixelRatio);
            }
        }
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                //_pixelHeight = (int)Math.Ceiling((float)_height / PixelRatio);
            }
        }

        //public int PixelRatio;

        public Camera Camera;
        public ChunkManager Scene;
        public ChunkManager Background;

        //public Color[] Screen;
        //private Texture2D _screen;

        public int UpdateRadiusInChunks;

        public PixelEngine(GraphicsDevice graphicsDevice, int width, int height, int updateRadiusInChunks = 5, int chunkSize = 320)//, int pixelRatio = 3)
        {
            _graphicsDevice = graphicsDevice;

            //PixelRatio = pixelRatio;
            Width = width;
            Height = height;
            UpdateRadiusInChunks = updateRadiusInChunks;

            //Screen = new Color[_pixelWidth * _pixelHeight];
            //_screen = new Texture2D(_graphicsDevice, _pixelWidth, _pixelHeight);

            Scene = new ChunkManager(chunkSize);
            Background = new ChunkManager(chunkSize);
        }

        public void Update(GameTime gameTime, Vector2 offset = new Vector2())
        {
            GlobalTime.Update(gameTime);
            ChunkPosition originChunk = Scene.GetChunkPosition(offset - Camera.Position);
            for (int y = originChunk.Y - UpdateRadiusInChunks; y <= originChunk.Y + UpdateRadiusInChunks; y++)
            {
                for (int x = originChunk.X - UpdateRadiusInChunks; x <= originChunk.X + UpdateRadiusInChunks; x++)
                {
                    Scene.UpdateChunk(gameTime, new ChunkPosition(x, y));
                    Background.UpdateChunk(gameTime, new ChunkPosition(x, y));
                }
            }
            Scene.ReindexChunks();
            Background.ReindexChunks();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            if (Camera == null)
                Camera = new Camera();
            Vector2 origin = offset - Camera.Position;
            ChunkPosition originChunk = Scene.GetChunkPosition(origin);
            RectangleF screenBounds = new RectangleF(origin.X - Width / 2f, origin.Y - Height / 2f, Width, Height);
            for (int y = originChunk.Y - UpdateRadiusInChunks; y <= originChunk.Y + UpdateRadiusInChunks; y++)
            {
                for (int x = originChunk.X - UpdateRadiusInChunks; x <= originChunk.X + UpdateRadiusInChunks; x++)
                {
                    Background.DrawChunk(spriteBatch, screenBounds, new ChunkPosition(x, y), offset, true);
                }
            }
            for (int y = originChunk.Y - UpdateRadiusInChunks; y <= originChunk.Y + UpdateRadiusInChunks; y++)
            {
                for (int x = originChunk.X - UpdateRadiusInChunks; x <= originChunk.X + UpdateRadiusInChunks; x++)
                {
                    Scene.DrawChunk(spriteBatch, screenBounds, new ChunkPosition(x, y), offset, true);
                }
            }
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
