using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Object
    {
        public Sprite Sprite;

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

        public bool Collideable;

        Object(Sprite sprite, Vector2 position = new Vector2())
        {
            Sprite = sprite;
            Scale = Vector2.One;
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            spriteBatch.Draw(Sprite.Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)_targetSize.X, (int)_targetSize.Y), Sprite.Cropping, Color);
        }
    }
}
