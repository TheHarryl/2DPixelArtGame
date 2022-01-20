﻿using Microsoft.Xna.Framework;
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

        public Sprite(Texture2D texture, Rectangle? cropping = null)
        {
            Texture = texture;
            if (cropping != null)
                Cropping = (Rectangle)cropping;
            else
                Cropping = Texture.Bounds;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
