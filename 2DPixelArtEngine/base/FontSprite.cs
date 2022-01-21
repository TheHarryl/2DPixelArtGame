using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static Microsoft.Xna.Framework.Graphics.SpriteFont;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2DPixelArtEngine
{
    public class FontSprite : Sprite
    {
        public SpriteFont Font;
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                float width = 0f;
                float height = 0f;
                for (int i = 0; i < Text.Length; i++)
                {
                    Glyph characterGlyph = Font.GetGlyphs()[Text[i]];
                    if (characterGlyph.BoundsInTexture.Height > height)
                        height = characterGlyph.BoundsInTexture.Height;
                    width += characterGlyph.BoundsInTexture.Width + SpacingX;
                }
                Cropping = new Rectangle(0, 0, (int)Math.Round(width), (int)Math.Round(height));
            }
        }
        public int SpacingX;
        public int SpacingY;

        public FontSprite(SpriteFont font, string text = "Sample Text", int spacingX = 0, int spacingY = 0) : base(font.Texture)
        {
            Font = font;
            SpacingX = spacingX;
            SpacingY = spacingY;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 scale, Color color, Vector2 offset = new Vector2())
        {
            List<Object> characters = new List<Object>();
            Vector2 position = new Vector2();
            for (int i = 0; i < Text.Length; i++)
            {
                Glyph characterGlyph = Font.GetGlyphs()[Text[i]];
                spriteBatch.Draw(Texture, new Rectangle((int)(position + offset).X, (int)(position + offset).Y, (int)(characterGlyph.BoundsInTexture.Width * scale.X), (int)(characterGlyph.BoundsInTexture.Height * scale.Y)), characterGlyph.BoundsInTexture, color);
                position.X += characterGlyph.BoundsInTexture.Width + SpacingX;
            }
        }
    }
}
