using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Sprite
    {
        public Texture2D Texture;
        public Rectangle Cropping;
        public Vector2 Offset;

        public Sprite(Texture2D texture, Rectangle? cropping, Vector2 offset = new Vector2())
        {
            Texture = texture;
            if (cropping != null)
                Cropping = (Rectangle)cropping;
            else
                Cropping = Texture.Bounds;

            Offset = offset;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
