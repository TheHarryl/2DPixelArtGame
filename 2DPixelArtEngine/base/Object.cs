using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2DPixelArtEngine
{
    public class Object
    {
        public ChunkManager Parent;
        public ChunkPosition Chunk;

        public Sprite Sprite;
        private Controller _controller;
        public Controller Controller
        {
            get => _controller;
            set
            {
                value.Parent = this;
                _controller = value;
            }
        }

        public Vector2 Position;
        public Color Color = Color.White;

        private Vector2 _scale;
        private Vector2 _targetSize;
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _targetSize = new Vector2(Sprite.Texture.Bounds.Width * _scale.X, Sprite.Texture.Bounds.Height * _scale.Y);
            }
        }

        public RectangleF Hitbox;
        public Vector2 Direction;
        public float Speed;
        public bool Collideable;

        public Object(RectangleF hitbox, Sprite sprite, Vector2 position = new Vector2(), Controller? controller = null, float speed = 0f, bool collideable = true)
        {
            Sprite = sprite;
            if (controller == null)
                Controller = new Controller();
            Scale = Vector2.One;
            Position = position;
            Hitbox = hitbox;
            Direction = Vector2.Zero;
            Speed = speed;
            Collideable = collideable;
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Controller.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            spriteBatch.Draw(Sprite.Texture, new Rectangle((int)(Position + Sprite.Offset + offset).X, (int)(Position + Sprite.Offset + offset).Y, (int)_targetSize.X, (int)_targetSize.Y), Sprite.Cropping, Color);
        }

        public RectangleF GetBounds(Vector2 offset = new Vector2())
        {
            return new RectangleF((Position + offset).X, (Position + offset).Y, (float)Math.Ceiling(_targetSize.X), (float)Math.Ceiling(_targetSize.Y));
        }

        public RectangleF GetHitboxBounds(Vector2 offset = new Vector2())
        {
            return new RectangleF((Position + offset).X + Hitbox.X, (Position + offset).Y + Hitbox.Y, Hitbox.Width, Hitbox.Height);
        }
    }
}
