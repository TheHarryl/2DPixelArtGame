using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.IO;
using System.Xml;
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

        private int _frameCount = 0;
        private DateTime _startFrameCount = DateTime.Now;

        private string _interactionName = "";
        private int _interactionStep = 1;

        private string _directory;
        private Object _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() + "\\assets\\";
            ContentManager.Initialize(GraphicsDevice, _directory);
            Texture2D playerTexture = ContentManager.LoadTexture("dude.png");
            Texture2D grassTexture = ContentManager.LoadTexture("grass.png");
            _pixelEngine = new PixelEngine(800, 480);
            GlobalService.Initialize(new Random());

            float secondsPerTile = 60f;
            int z = 0;
            for (int y = 5; y >= -5; y--)
            {
                for (int x = -30; x <= 30; x++)
                {
                    Sprite floorSprite = new Sprite(ContentManager.LoadTexture("tileset.png"));
                    Object floor = new Object(new RectangleF(0, 0, 32, 32), floorSprite, new Vector2(x * floorSprite.Cropping.Width * 2, y * floorSprite.Cropping.Height * 2), new Vector2(-16, 16), new FloorController(floorSprite, z * secondsPerTile + (float)GlobalService.Random.NextDouble() * secondsPerTile / 10, secondsPerTile));
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
                Object grass = new Object(new RectangleF(4, 33, 17, 12), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(-300 + GlobalService.Random.Next(0, 300), -300 + GlobalService.Random.Next(0, 800)), new Vector2(-12.5f, -42), new GrassController(
                    new AnimatedSprite(grassTexture, new Rectangle(0, 0, 25, 42)),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 42, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 0, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 0, 25, 42), 4, 4, 15, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 42, 25, 42), 4, 4, 15, false)
                ));
                grass.Scale = new Vector2(2, 2);
                _pixelEngine.Scene.Add(grass);
            }
            
            _player = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)), new Vector2(100, 200), new Vector2(-16, -30), new PlayerController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(160, 234, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 273, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 156, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 195, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 78, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 117, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 39, 32, 39), 4, 4, 16, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 0, 32, 39), 4, 4, 16, false)
            ), 300, true);
            _player.Scale = new Vector2(2, 2);
            _pixelEngine.Camera.CameraSubject = _player;
            _pixelEngine.Scene.Add(_player);

            Object dummy = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)), new Vector2(400, 200), new Vector2(-16, -30), new EnemyController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(160, 234, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 273, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 156, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160, 195, 32, 39), 4, 4, 24, false),
                new AnimatedSprite(playerTexture, new Rectangle(160 + 64, 78, 32, 39), 1, 1, 4, false),
                new AnimatedSprite(playerTexture, new Rectangle(160 + 64, 117, 32, 39), 1, 1, 4, false),
                new AnimatedSprite(playerTexture, new Rectangle(160 + 64, 39, 32, 39), 1, 1, 4, false),
                new AnimatedSprite(playerTexture, new Rectangle(160 + 64, 0, 32, 39), 1, 1, 4, false),
                100
            ), 300);
            dummy.Scale = new Vector2(2, 2);
            dummy.Color = new Color(255, 200, 200);
            _pixelEngine.Scene.Add(dummy);

            Object npc = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)), new Vector2(100, 0), new Vector2(-16, -30), new EntityController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8)
            ), 300, true);
            npc.Scale = new Vector2(2, 2);
            //npc.Color = new Color(200, 200, 200);
            _pixelEngine.Scene.Add(npc);
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
            if (_pixelEngine.KeyboardState != null && _pixelEngine.LastKeyboardState != null)
            {
                KeyboardState keyboardState = (KeyboardState)_pixelEngine.KeyboardState;
                KeyboardState lastKeyboardState = (KeyboardState)_pixelEngine.LastKeyboardState;
                if (keyboardState.IsKeyDown(Keys.Enter) && !lastKeyboardState.IsKeyDown(Keys.Enter))
                {
                    /*Object collision;
                    XmlTextReader textReader = new XmlTextReader(_directory + "dialogue.xml");
                    textReader.Read();
                    // If the node has value  
                    while (textReader.Read())
                    {
                        // Move to fist element  
                        textReader.MoveToElement();
                        Console.WriteLine("XmlTextReader Properties Test");
                        Console.WriteLine("===================");
                        // Read this element's properties and display them on console
                        Console.WriteLine("Name:" + textReader.Name);
                        Console.WriteLine("Base URI:" + textReader.BaseURI);
                        Console.WriteLine("Local Name:" + textReader.LocalName);
                        Console.WriteLine("Attribute Count:" + textReader.AttributeCount.ToString());
                        Console.WriteLine("Depth:" + textReader.Depth.ToString());
                        Console.WriteLine("Line Number:" + textReader.LineNumber.ToString());
                        Console.WriteLine("Node Type:" + textReader.NodeType.ToString());
                        Console.WriteLine("Attribute Count:" + textReader.Value.ToString());
                    }
                    if (_player.CollisionX != null && )*/
                }
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _pixelEngine.Draw(_spriteBatch);

            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(18, 308, 764, 154), Color.Black);
            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(20, 310, 760, 150), Color.White);
            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(24, 314, 752, 142), Color.Black);

            FontSprite _font = new FontSprite(ContentManager.LoadFont("eight-bit-dragon.ttf"), "Yo do u know where my cheese went");
            _font.Draw(_spriteBatch, Color.White, new Vector2(0.75f), new Vector2(31 + 5, 325 + 5));
            Sprite npcSprite = new Sprite(ContentManager.LoadTexture("testnpc.png"), new Rectangle(0, 0, 60, 60));
            npcSprite.Draw(_spriteBatch, Color.White, new Vector2(2, 2), new Vector2(800 - 11 - 24 - 120, 325));

            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(18, 308 - 90, 300 + 4, 80 + 4), Color.Black);
            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(20, 310 - 90, 300, 80), Color.White);
            _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(24, 314 - 90, 300 - 8, 80 - 8), Color.Black);

            _spriteBatch.Draw(ContentManager.LoadTexture("pointer.png"), new Rectangle(34, 314 - 80, 20, 20), Color.White);


            _font.Text = "yes i do sir or madam";
            _font.Draw(_spriteBatch, Color.White, new Vector2(0.75f), new Vector2(60, 325 - 90));

            _font.Text = "not a clue OG";
            _font.Draw(_spriteBatch, Color.White, new Vector2(0.75f), new Vector2(60, 325 - 90 + 30));

            _spriteBatch.End();

            _frameCount++;
            if (DateTime.Now - _startFrameCount >= new TimeSpan(0, 0, 1))
            {
                System.Diagnostics.Debug.WriteLine(_frameCount + " FPS | " + (_pixelEngine.Scene.GetAllChunks().Count + _pixelEngine.Background.GetAllChunks().Count) + " Objects");
                _frameCount = 0;
                _startFrameCount = DateTime.Now;
            }

            base.Draw(gameTime);
        }
    }
}
