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
        //private GraphicsDevice _graphicsDevice;

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
        private ChunkManager _scene;
        private ChunkManager _background;
        public ChunkManager Scene
        {
            get => _scene;
            set
            {
                _scene = value;
                _scene.Parent = this;
            }
        }
        public ChunkManager Background
        {
            get => _background;
            set
            {
                _background = value;
                _background.Parent = this;
            }
        }

        public bool DisplayHitboxes;

        //public Color[] Screen;
        //private Texture2D _screen;

        public int UpdateRadiusInChunks;

        public PixelEngine(/*GraphicsDevice graphicsDevice, */int width, int height, int updateRadiusInChunks = 5, int chunkSize = 320)//, int pixelRatio = 3)
        {
            //_graphicsDevice = graphicsDevice;

            //PixelRatio = pixelRatio;
            Width = width;
            Height = height;
            UpdateRadiusInChunks = updateRadiusInChunks;

            //Screen = new Color[_pixelWidth * _pixelHeight];
            //_screen = new Texture2D(_graphicsDevice, _pixelWidth, _pixelHeight);

            Camera = new Camera();
            Scene = new ChunkManager(chunkSize);
            Background = new ChunkManager(chunkSize);

            DisplayHitboxes = false;
        }

        public void Update(GameTime gameTime, Vector2 offset = new Vector2())
        {
            GlobalTime.Update(gameTime);
            ChunkPosition originChunk = Scene.GetChunkPosition(Camera.Position);
            List<Object> objects = Scene.GetNearbyChunks(originChunk, UpdateRadiusInChunks).Concat(Background.GetNearbyChunks(originChunk, UpdateRadiusInChunks)).ToList();
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            if (Camera == null)
                Camera = new Camera();
            RectangleF screenBounds = new RectangleF(Camera.Position.X - Width, Camera.Position.Y - Height, Width * 2, Height * 2);
            ChunkPosition topLeftChunk = Scene.GetChunkPosition(screenBounds.X, screenBounds.Y);
            ChunkPosition bottomRightChunk = Scene.GetChunkPosition(screenBounds.Right, screenBounds.Bottom);
            int width = bottomRightChunk.X - topLeftChunk.X;
            int height = bottomRightChunk.Y - topLeftChunk.Y;

            Vector2 topLeft = offset - Camera.Position + new Vector2(Width / 2f, Height / 2f);
            List<Object> renderableBackground = Background.GetChunksInRange(topLeftChunk.X, topLeftChunk.Y, width, height);
            renderableBackground = renderableBackground.OrderByDescending(o => -o.GetPriority(true)).ToList();
            for (int i = renderableBackground.Count - 1; i >= 0; i--)
            {
                if (renderableBackground[i].GetBounds().IntersectsWith(screenBounds))
                    renderableBackground[i].Draw(spriteBatch, topLeft);
            }
            List<Object> renderableScene = Scene.GetChunksInRange(topLeftChunk.X, topLeftChunk.Y, width, height);
            renderableScene = renderableScene.OrderByDescending(o => o.GetPriority()).ToList();
            for (int i = renderableScene.Count - 1; i >= 0; i--)
            {
                if (renderableScene[i].GetBounds().IntersectsWith(screenBounds))
                    renderableScene[i].Draw(spriteBatch, topLeft);
            }
        }
    }
}
