using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2DPixelArtGame
{
    public class FloorController : BaseController
    {
        public float DecayStart;
        public float DecayLength;
        public Sprite Sprite;
        public Color[] PixelData;
        public float[] PixelTimestamps;

        public FloorController(Sprite sprite, float decayStart, float decayLength) : base("floor")
        {
            Sprite = sprite;

            int amountPixels = Sprite.Texture.Width * Sprite.Texture.Height;
            PixelData = new Color[amountPixels];
            PixelTimestamps = new float[amountPixels];
            Sprite.Texture.GetData<Color>(PixelData);

            DecayStart = decayStart;
            DecayLength = decayLength;

            Random rand = new Random();
            float step = DecayLength / Sprite.Texture.Height;
            for (int y = 0; y < Sprite.Texture.Height; y++) {
                for (int x = 0; x < Sprite.Texture.Width; x++) {
                    PixelTimestamps[(Sprite.Texture.Height - 1 - y) * Sprite.Texture.Width + x] = (step * y) + ((float)rand.NextDouble() * step);
                }
            }

        }
        
        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= DecayStart + DecayLength + 1f)
            {
                Parent.Delete();
                return;
            } else if (gameTime.TotalGameTime.TotalSeconds >= DecayStart)
            {
                Parent.Hitbox = new RectangleF(0, 0, Parent.Sprite.Cropping.Width, Parent.Sprite.Cropping.Height * (1f - ((float)gameTime.TotalGameTime.TotalSeconds - DecayStart) / DecayLength));
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            float timeSinceDecay = (float)GlobalTime.Timestamp.TotalSeconds - DecayStart;
            if (timeSinceDecay < 0f) return;
            for (int i = PixelTimestamps.Length - 1; i >= 0; i--)
            {
                if (timeSinceDecay < PixelTimestamps[i]) continue;
                float interpolant = (timeSinceDecay - PixelTimestamps[i]);
                Vector2 position = offset + new Vector2(i % Sprite.Texture.Width, i / Sprite.Texture.Width) * Parent.Scale;
                spriteBatch.Draw(ContentManager.Pixel, new Rectangle((int)position.X, (int)position.Y, (int)Parent.Scale.X, (int)Parent.Scale.Y), Color.Black);
                if (timeSinceDecay > PixelTimestamps[i] + 1f) continue;
                spriteBatch.Draw(ContentManager.Pixel, new Rectangle((int)position.X, (int)position.Y + (int)TweenService.Tween(0, 400, interpolant, EasingDirection.In, EasingStyle.Quart), (int)Parent.Scale.X, (int)Parent.Scale.Y), Color.Lerp(PixelData[i], Color.Transparent, interpolant * 0.5f));
            }
        }
    }
}
