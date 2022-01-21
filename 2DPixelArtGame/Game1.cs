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

        private int frameCount = 0;
        private DateTime startFrameCount = DateTime.Now;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "\\sprites\\";
            ContentManager.Initialize(GraphicsDevice, directory);
            Texture2D playerTexture = ContentManager.Load("dude.png");
            Texture2D grassTexture = ContentManager.Load("grass.png");
            _pixelEngine = new PixelEngine(800, 480);
            Random random = new Random((int)GlobalTime.Timestamp.TotalMilliseconds);

            int z = 0;
            for (int y = 5; y >= -5; y--)
            {
                for (int x = -30; x <= 30; x++)
                {
                    Sprite floorSprite = new Sprite(ContentManager.Load("tileset.png"));
                    Object floor = new Object(new RectangleF(0, 0, 32, 32), floorSprite, new Vector2(x * floorSprite.Cropping.Width * 2, y * floorSprite.Cropping.Height * 2), new Vector2(-25, 25), new FloorController(floorSprite, z * 60 + (float)random.NextDouble() * 6, 60));
                    floor.Scale = new Vector2(2, 2);
                    _pixelEngine.Background.Add(floor);
                }
                z++;
            }

            /*for (int i = 0; i < 80; i++)
            {
                Object grass = new Object(new RectangleF(4, 28, 17, 17), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(200 + random.Next(0, 200), 100 + random.Next(0, 200)), new Vector2(-12.5f, -42), new GrassController(
                    new AnimatedSprite(grassTexture, new Rectangle(0, 0, 25, 42)),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 42, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 0, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 0, 25, 42), 4, 4, 15, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 42, 25, 42), 4, 4, 15, false)
                ));
                grass.Scale = new Vector2(2, 2);
                _pixelEngine.Scene.Add(grass);
            }*/
            
            for (int i = 0; i < 160*3; i++)
            {
                Object grass = new Object(new RectangleF(4, 33, 17, 12), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(-300 + random.Next(0, 300), -100 + random.Next(0, 800)), new Vector2(-12.5f, -42), new GrassController(
                    new AnimatedSprite(grassTexture, new Rectangle(0, 0, 25, 42)),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 42, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 0, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 0, 25, 42), 4, 4, 15, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 42, 25, 42), 4, 4, 15, false)
                ));
                grass.Scale = new Vector2(2, 2);
                _pixelEngine.Scene.Add(grass);
            }
            
            Object player = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)), new Vector2(100, 200), new Vector2(-16, -30), new PlayerController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(160, 234, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 273, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 156, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 195, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 78, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 117, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 39, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 0, 32, 39), 4, 4, 16, false)
            ), 300);
            player.Scale = new Vector2(2, 2);
            _pixelEngine.Camera.CameraSubject = player;
            _pixelEngine.Scene.Add(player);

            Object dummy = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)), new Vector2(400, 200), new Vector2(-16, -30), new EnemyController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(160, 234, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 273, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 156, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 195, 32, 39), 4, 4, 8, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 78, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 117, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 39, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 0, 32, 39), 4, 4, 16, false)
            ), 300);
            dummy.Scale = new Vector2(2, 2);
            _pixelEngine.Scene.Add(dummy);
            //_pixelEngine.DisplayHitboxes = true;

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);//(new Color(37, 146, 79));

            // TODO: Add your drawing code here
            _pixelEngine.Update(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _pixelEngine.Draw(_spriteBatch);

            _spriteBatch.End();

            frameCount++;
            if (DateTime.Now - startFrameCount >= new TimeSpan(0, 0, 1))
            {
                System.Diagnostics.Debug.WriteLine(frameCount + " FPS");
                frameCount = 0;
                startFrameCount = DateTime.Now;
            }

            base.Draw(gameTime);
        }
    }
}
