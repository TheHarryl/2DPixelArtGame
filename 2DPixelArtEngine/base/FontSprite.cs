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
                float height = 1f;
                for (int i = 0; i < _text.Length; i++)
                {
                    if (Text[i] == '\n') height++;
                    if (!Font.GetGlyphs().ContainsKey(Text[i])) continue;
                    Glyph characterGlyph = Font.GetGlyphs()[Text[i]];
                    width += characterGlyph.BoundsInTexture.Width + SpacingX + Font.Spacing;
                }
                height *= Font.LineSpacing;
                Cropping = new Rectangle(0, 0, (int)Math.Round(width), (int)Math.Round(height));
            }
        }

        public int OutlineSize;
        public int SpacingX;
        public int SpacingY;

        public FontSprite(SpriteFont font, string text = "Sample Text", int outlineSize = 0, int spacingX = 0, int spacingY = 0) : base(font.Texture)
        {
            Font = font;
            OutlineSize = outlineSize;
            SpacingX = spacingX;
            SpacingY = spacingY;
            Text = text;
        }

        private void DrawText(Glyph characterGlyph, SpriteBatch spriteBatch, Color color, Vector2 scale, Vector2 offset = new Vector2(), int letterSizeOffset = 0)
        {
            //characterGlyph.Cropping.Height - Font.MeasureString(" ").Y
            spriteBatch.Draw(Texture, new Rectangle((int)offset.X - letterSizeOffset, (int)(offset.Y - (characterGlyph.Cropping.Height - Font.MeasureString(" ").Y) * scale.Y) - letterSizeOffset, (int)(characterGlyph.BoundsInTexture.Width * scale.X + letterSizeOffset * 2), (int)(characterGlyph.BoundsInTexture.Height * scale.Y + letterSizeOffset * 2)), characterGlyph.BoundsInTexture, color);
        }

        public override void Draw(SpriteBatch spriteBatch, Color color, Vector2 scale, Vector2 offset = new Vector2())
        {
            Vector2 position = new Vector2();
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n')
                {
                    position.Y += Font.LineSpacing * scale.Y + SpacingY;
                    position.X = 0;
                }
                if (!Font.GetGlyphs().ContainsKey(Text[i])) continue;
                Glyph characterGlyph = Font.GetGlyphs()[Text[i]];
                if (OutlineSize != 0)
                {
                    DrawText(characterGlyph, spriteBatch, Color.Black, scale, position + offset, (int)(-OutlineSize * scale.X));
                    DrawText(characterGlyph, spriteBatch, Color.Black, scale, position + offset, (int)(OutlineSize * scale.X));
                }
                DrawText(characterGlyph, spriteBatch, color, scale, position + offset);
                position.X += characterGlyph.WidthIncludingBearings * scale.X + SpacingX;
            }
        }
    }
}
