using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.IO;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2DPixelArtGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PixelEngine _pixelEngine;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            System.Diagnostics.Debug.WriteLine(Directory.GetCurrentDirectory());
            _pixelEngine = new PixelEngine(_graphics.GraphicsDevice, 800, 480);
            Texture2D grassTexture = _pixelEngine.ImportTexture("C:/Users/harry/Desktop/2DPixelArtGame/2DPixelArtGame/sprites/grass.png");
            Random random = new Random();
            for (int i = 0; i < 80; i++)
            {
                Object grass = new Object(new RectangleF(4, 33, 17, 12), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(200 + random.Next(0, 200), 100 + random.Next(0, 200)), new Vector2(-12.5f, -42), new GrassController(
                    new AnimatedSprite(grassTexture, new Rectangle(0, 0, 25, 42)),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 42, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 0, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 0, 25, 42), 4, 4, 15, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 42, 25, 42), 4, 4, 15, false)
                ));
                grass.Scale = new Vector2(2, 2);
                _pixelEngine.Scene.Add(grass);
            }
            
            Texture2D playerTexture = _pixelEngine.ImportTexture("C:/Users/harry/Desktop/2DPixelArtGame/2DPixelArtGame/sprites/dude.png");
            Object player = new Object(new RectangleF(17, 59, 30, 13), new AnimatedSprite(playerTexture, new Rectangle(0, 234, 64, 78)), new Vector2(0, 0), new Vector2(-32, -78), new PlayerController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 234, 64, 78)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 156, 64, 78)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 64, 78)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 64, 78)),
                new AnimatedSprite(playerTexture, new Rectangle(64, 234, 64, 78), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(64, 156, 64, 78), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(64, 0, 64, 78), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(64, 78, 64, 78), 4, 4, 8)
            ), 300);
            _pixelEngine.Camera.CameraSubject = player;
            _pixelEngine.Scene.Add(player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _pixelEngine.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(32, 85, 58));

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _pixelEngine.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
