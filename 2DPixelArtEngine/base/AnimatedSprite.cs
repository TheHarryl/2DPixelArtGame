using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class AnimatedSprite : Sprite
    {
        public List<Sprite> SpriteSheet;
        public int Columns;
        public int Frames;
        public float FramesPerSecond;

        private TimeSpan _timeStarted;

        public AnimatedSprite(Texture2D texture, Rectangle? startingCropping, int columns, int frames, float framesPerSecond) : base(texture, startingCropping)
        {
            Columns = columns;
            Frames = frames;
            FramesPerSecond = framesPerSecond;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
