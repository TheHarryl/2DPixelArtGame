using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2DPixelArtGame
{
    public class Dialogue
    {
        public int ID;
        public bool Starter;
        public string Condition;
        public string Flag;
        public int GoTo;

        public string Text;
        public int TextSpeed;

        public Sprite Avatar;
        public int AvatarScale;

        public List<DialogueResponse> Responses = new List<DialogueResponse>();

        public Dialogue()
        {

        }

        public Dialogue(int id, bool starter, string condition, string flag, string text, int textSpeed, Sprite avatar, int avatarScale, List<DialogueResponse> responses)
        {
            ID = id;
            Starter = starter;
            Condition = condition;
            Flag = flag;
            Text = text;
            TextSpeed = textSpeed;
            Avatar = avatar;
            AvatarScale = avatarScale;
            Responses = responses;
        }

        public Dialogue Clone()
        {
            return new Dialogue(ID, Starter, Condition, Flag, Text, TextSpeed, Avatar, AvatarScale, new List<DialogueResponse>(Responses));
        }
    }

    public class DialogueResponse
    {
        public int ID;
        public int GoTo;
        public string Text;

        public DialogueResponse(int id, int goTo, string text)
        {
            ID = id;
            GoTo = goTo;
            Text = text;
        }

        public DialogueResponse()
        {

        }
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PixelEngine _pixelEngine;

        private int _frameCount = 0;
        private DateTime _startFrameCount = DateTime.Now;


        private string _directory;
        private Object _player;

        private Dictionary<string, List<Dialogue>> _dialogue = new Dictionary<string, List<Dialogue>>();
        private string _interactionName = "";
        private int _interactionStep = 1;
        private int _interactionResponse = 0;
        private TimeSpan _interactionStart;

        private KeyboardState _lastKeyboardState;
        private KeyboardState _keyboardState;
        private MouseState _lastMouseState;
        private MouseState _mouseState;

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

            using (XmlReader reader = XmlReader.Create(_directory + "dialogue.xml"))
            {
                string name = "";
                List<Dialogue> dialogue = new List<Dialogue>();
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        System.Diagnostics.Debug.WriteLine(reader.Name.ToString());
                        switch (reader.Name.ToString())
                        {
                            case "interactable":
                                if (name != "")
                                {
                                    _dialogue.Add(name, dialogue);
                                }
                                name = reader.GetAttribute("name");
                                dialogue = new List<Dialogue>();
                                break;
                            case "dialogue":
                                dialogue.Add(new Dialogue());
                                string id = reader.GetAttribute("id");
                                string starter = reader.GetAttribute("starter");
                                string condition = reader.GetAttribute("condition");
                                string flag = reader.GetAttribute("flag");
                                string goTo = reader.GetAttribute("goto");
                                dialogue[dialogue.Count - 1].ID = int.Parse(id);
                                dialogue[dialogue.Count - 1].Starter = starter == null ? false : bool.Parse(starter);
                                dialogue[dialogue.Count - 1].Condition = condition == null ? "" : condition;
                                dialogue[dialogue.Count - 1].Flag = flag == null ? "" : flag;
                                break;
                            case "text":
                                string textSpeed = reader.GetAttribute("speed");
                                dialogue[dialogue.Count - 1].TextSpeed = textSpeed == null ? 35 : int.Parse(textSpeed);
                                dialogue[dialogue.Count - 1].Text = reader.ReadString();
                                break;
                            case "avatar":
                                int x = int.Parse(reader.GetAttribute("x"));
                                int y = int.Parse(reader.GetAttribute("y"));
                                int width = int.Parse(reader.GetAttribute("width"));
                                int height = int.Parse(reader.GetAttribute("height"));
                                string scale = reader.GetAttribute("scale");
                                string spriteName = reader.ReadString();
                                dialogue[dialogue.Count - 1].Avatar = new Sprite(ContentManager.LoadTexture(spriteName), new Rectangle(x, y, width, height));
                                dialogue[dialogue.Count - 1].AvatarScale = scale == null ? 2 : int.Parse(scale);
                                break;
                            case "response":
                                string rgoTo = reader.GetAttribute("goto");
                                string text = reader.ReadString();
                                DialogueResponse dialogueResponse = new DialogueResponse();
                                dialogueResponse.GoTo = rgoTo == null ? -1 : int.Parse(rgoTo);
                                dialogueResponse.Text = text;
                                dialogue[dialogue.Count - 1].Responses.Add(dialogueResponse);
                                break;
                        }
                    }
                }
                _dialogue.Add(name, dialogue);
            }

            float secondsPerTile = 60f;
            int z = 0;
            for (int y = 5; y >= -5; y--)
            {
                for (int x = -30; x <= 30; x++)
                {
                    Sprite floorSprite = new Sprite(ContentManager.LoadTexture("tileset.png"));
                    Object floor = new Object(new RectangleF(0, 0, 32, 32), floorSprite, new Vector2(x * floorSprite.Cropping.Width * 2, y * floorSprite.Cropping.Height * 2), new Vector2(16, 16), new FloorController(floorSprite, z * secondsPerTile + (float)GlobalService.Random.NextDouble() * secondsPerTile / 10, secondsPerTile));
                    floor.Scale = new Vector2(2, 2);
                    _pixelEngine.Background.Add(floor);
                }
                z++;
            }

            /*for (int i = 0; i < 80; i++)
            {
                Object grass = new Object(new RectangleF(4, 28, 17, 17), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(-100 + GlobalService.Random.Next(0, 200), -100 + GlobalService.Random.Next(0, 200)), new Vector2(-12.5f, -42), new GrassController(
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
                Object grass = new Object(new RectangleF(4, 33, 17, 12), new Sprite(grassTexture, new Rectangle(0, 0, 25, 42)), new Vector2(-300 + GlobalService.Random.Next(0, 300), -300 + GlobalService.Random.Next(0, 800)), new Vector2(12.5f, 42), new GrassController(
                    new AnimatedSprite(grassTexture, new Rectangle(0, 0, 25, 42)),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 42, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 0, 25, 42), 4, 4, 30, false),
                    new AnimatedSprite(grassTexture, new Rectangle(25, 0, 25, 42), 4, 4, 15, false),
                    new AnimatedSprite(grassTexture, new Rectangle(125, 42, 25, 42), 4, 4, 15, false)
                ));
                grass.Scale = new Vector2(2, 2);
                _pixelEngine.Scene.Add(grass);
            }

            Object box = new Object(new RectangleF(0, 0.2f, 1, 0.8f), new Sprite(ContentManager.Pixel, new Rectangle(0, 0, 1, 1)), new Vector2(-500, 0), new Vector2(0.5f, 0.5f), null, 0, true);
            box.Scale = new Vector2(250, 250);
            box.Color = Color.Gray;
            _pixelEngine.Scene.Add(box);

            _player = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)), new Vector2(100, 200), new Vector2(16, 30), new PlayerController(
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

            Object dummy = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)), new Vector2(400, 200), new Vector2(16, 30), new EnemyController(
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
            //dummy.Color = new Color(255, 200, 200);
            _pixelEngine.Scene.Add(dummy);

            Object npc = new Object(new RectangleF(8.5f, 27.5f, 15, 9), new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)), new Vector2(100, 0), new Vector2(16, 30), new EntityController(
                new AnimatedSprite(playerTexture, new Rectangle(0, 117, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 78, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 0, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(0, 39, 32, 39)),
                new AnimatedSprite(playerTexture, new Rectangle(32, 117, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 78, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 0, 32, 39), 4, 4, 8),
                new AnimatedSprite(playerTexture, new Rectangle(32, 39, 32, 39), 4, 4, 8),
                "entity", "testnpc"
            ), 300, true);
            npc.Scale = new Vector2(2, 2);
            npc.Color = new Color(200, 200, 200);
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

        public void SetInteraction(string interactionName, int interactionStep, GameTime gameTime)
        {
            _interactionName = interactionName;
            _interactionStep = interactionStep;
            _interactionStart = gameTime.TotalGameTime;
            _interactionResponse = 0;
            string flag = _dialogue[_interactionName].Find(o => o.ID == _interactionStep).Flag;
            if (flag != "" && !_player.Controller.Flags.ContainsKey(flag))
                _player.Controller.Flags.Add(flag, true);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);//(new Color(37, 146, 79));

            // TODO: Add your drawing code here
            _lastKeyboardState = _keyboardState;
            _lastMouseState = _mouseState;
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            if (_interactionName != "")
            {
                Dialogue dialogue = _dialogue[_interactionName].Find(o => o.ID == _interactionStep);
                if ((float)(gameTime.TotalGameTime - _interactionStart).TotalSeconds >= (float)dialogue.Text.Length / dialogue.TextSpeed)
                {
                    if (_keyboardState.IsKeyDown(Keys.Up) && !_lastKeyboardState.IsKeyDown(Keys.Up)) {
                        _interactionResponse--;
                    } else if (_keyboardState.IsKeyDown(Keys.Down) && !_lastKeyboardState.IsKeyDown(Keys.Down))
                    {
                        _interactionResponse++;
                    }
                    if (_interactionResponse < 0)
                        _interactionResponse = 0;
                    else if (_interactionResponse >= dialogue.Responses.Count)
                        _interactionResponse = dialogue.Responses.Count - 1;
                    if (_keyboardState.IsKeyDown(Keys.Enter) && !_lastKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (dialogue.Responses[_interactionResponse].GoTo == -1)
                        {
                            _interactionName = "";
                            _pixelEngine.KeyboardInput = true;
                            _pixelEngine.MouseInput = true;
                        } else
                        {
                            SetInteraction(_interactionName, dialogue.Responses[_interactionResponse].GoTo, gameTime);
                        }
                    }
                }
            } else if (_keyboardState.IsKeyDown(Keys.Enter) && !_lastKeyboardState.IsKeyDown(Keys.Enter))
            {
                RectangleF bounds = _player.GetHitboxBounds(Vector2.Zero, Vector2.One);
                Object collision = _pixelEngine.Scene.GetNearbyChunks(_player.Chunk).Find(o => o.Controller.Flags.ContainsKey("Interactable") && _dialogue.ContainsKey((string)o.Controller.Flags["Interactable"]) && o.GetHitboxBounds().IntersectsWith(bounds) && o != _player);
                if (collision != null)
                {
                    _pixelEngine.KeyboardInput = false;
                    _pixelEngine.MouseInput = false;
                    string interactionName = (string)collision.Controller.Flags["Interactable"];
                    SetInteraction(interactionName, _dialogue[interactionName].Find(o => o.Starter && (o.Condition == "" || _player.Controller.Flags.ContainsKey(o.Condition))).ID, gameTime);
                }
            }
            if (_keyboardState.IsKeyDown(Keys.Tab) && !_lastKeyboardState.IsKeyDown(Keys.Tab))
                _pixelEngine.DisplayHitboxes = !_pixelEngine.DisplayHitboxes;
            _pixelEngine.Update(gameTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _pixelEngine.Draw(_spriteBatch);

            if (_interactionName != "")
            {
                Dialogue dialogue = _dialogue[_interactionName].Find(o => o.ID == _interactionStep);
                float timeToFinish = (float)dialogue.Text.Length / dialogue.TextSpeed;
                float interpolant = (float)(gameTime.TotalGameTime - _interactionStart).TotalSeconds / timeToFinish;
                if (interpolant > 1f) interpolant = 1f;

                _spriteBatch.Draw(ContentManager.LoadTexture("dialoguebox.png"), new Rectangle(0, 290, 800, 190), Color.White);

                int characters = (int)(dialogue.Text.Length * interpolant);
                FontSprite _font = new FontSprite(ContentManager.LoadFont("eight-bit-dragon.ttf"), dialogue.Text.Substring(0, characters), 0, 0, 10);
                _font.Draw(_spriteBatch, Color.White, new Vector2(0.75f), new Vector2(31 + 5, 325 + 5));
                dialogue.Avatar.Draw(_spriteBatch, Color.White, new Vector2(2, 2), new Vector2(800 - 11 - 24 - 120, 325));

                if (interpolant == 1f && dialogue.Responses[0].Text != "")
                {
                    _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(18, 308 - 30 * (dialogue.Responses.Count + 1), 300 + 4, 30 * (dialogue.Responses.Count + 1) + -6), Color.Black);
                    _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(20, 310 - 30 * (dialogue.Responses.Count + 1), 300, 30 * (dialogue.Responses.Count + 1) - 10), Color.White);
                    _spriteBatch.Draw(ContentManager.Pixel, new Rectangle(24, 314 - 30 * (dialogue.Responses.Count + 1), 300 - 8, 30 * (dialogue.Responses.Count + 1) - 18), Color.Black);

                    _spriteBatch.Draw(ContentManager.LoadTexture("pointer.png"), new Rectangle(34, 324 - 30 * (dialogue.Responses.Count + 1 - _interactionResponse), 20, 20), Color.White);

                    for (int i = 0; i < dialogue.Responses.Count; i++)
                    {
                        _font.Text = dialogue.Responses[i].Text;
                        _font.Draw(_spriteBatch, Color.White, new Vector2(0.75f), new Vector2(60, 325 - 90 + i * 30));
                    }
                }
            }

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
