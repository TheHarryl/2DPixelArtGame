﻿using Microsoft.Xna.Framework;
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
        private BaseController _controller;
        public BaseController Controller
        {
            get => _controller;
            set
            {
                value.Parent = this;
                _controller = value;
            }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get => new Vector2((float)Math.Round(_position.X), (float)Math.Round(_position.Y));
            set
            {
                _position = value;
                Parent.Reindex(this);
            }
        }
        public Vector2 ScreenPosition
        {
            get => Position - Parent.Parent.Camera.Position + new Vector2(Parent.Parent.Width / 2f, Parent.Parent.Height / 2f);
            private set { }
        }
        public Vector2 SpriteOffset;
        public Color Color;

        private Vector2 _scale;
        private Vector2 _scaledTexture;
        private RectangleF _scaledHitbox;
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                _scaledTexture = new Vector2(Sprite.Cropping.Width * _scale.X, Sprite.Cropping.Height * _scale.Y);
                _scaledHitbox = new RectangleF(Hitbox.X * Scale.X, Hitbox.Y * Scale.Y, Hitbox.Width * Scale.X, Hitbox.Height * Scale.Y);
            }
        }

        private RectangleF _hitbox;
        public RectangleF Hitbox
        {
            get => _hitbox;
            set
            {
                _hitbox = value;
                _scaledHitbox = new RectangleF(Hitbox.X * Scale.X, Hitbox.Y * Scale.Y, Hitbox.Width * Scale.X, Hitbox.Height * Scale.Y);
            }
        }
        public Vector2 Direction;
        public float Speed;
        public bool Collideable;

        public Object(RectangleF hitbox, Sprite sprite, Vector2 position = new Vector2(), Vector2 spriteOffset = new Vector2(), BaseController? controller = null, float speed = 0f, bool collideable = true)
        {
            Sprite = sprite;
            Controller = controller;
            if (Controller == null)
                Controller = new BaseController();
            _position = position;
            SpriteOffset = spriteOffset;
            Hitbox = hitbox;
            Direction = Vector2.Zero;
            Speed = speed;
            Collideable = collideable;
            Scale = Vector2.One;
            Color = Color.White;
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Controller.Update(gameTime);

            _position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            Vector2 position = Position + SpriteOffset * Scale + offset;
            spriteBatch.Draw(Sprite.Texture, new Rectangle((int)position.X, (int)position.Y, (int)_scaledTexture.X, (int)_scaledTexture.Y), Sprite.Cropping, Color);
            Controller.Draw(spriteBatch, position);
            if (Parent.Parent.DisplayHitboxes)
                spriteBatch.Draw(ContentManager.Pixel, new Rectangle((int)GetHitboxBounds(offset).X, (int)GetHitboxBounds(offset).Y, (int)GetHitboxBounds(offset).Width, (int)GetHitboxBounds(offset).Height), Color.Red * 0.5f);
        }

        public RectangleF GetBounds(Vector2 offset = new Vector2())
        {
            return new RectangleF((Position + SpriteOffset * Scale + offset).X, (Position + SpriteOffset * Scale + offset).Y, (float)Math.Ceiling(_scaledTexture.X), (float)Math.Ceiling(_scaledTexture.Y));
        }

        public RectangleF GetHitboxBounds(Vector2 offset = new Vector2())
        {
            return new RectangleF((Position + SpriteOffset * Scale + offset).X + _scaledHitbox.X, (Position + SpriteOffset * Scale + offset).Y + _scaledHitbox.Y, _scaledHitbox.Width, _scaledHitbox.Height);
        }

        public void Delete()
        {
            Parent.Remove(this);
        }
    }
}
