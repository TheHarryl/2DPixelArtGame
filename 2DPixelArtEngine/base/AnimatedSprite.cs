using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class AnimatedSprite : Sprite
    {
        Rectangle _startingRectangle;

        public int FramesPerRow;
        public int TotalFrames;
        public float FramesPerSecond;

        private TimeSpan _timeStarted;
        private TimeSpan _timePaused;
        public bool Paused;

        public int SpacingX;
        public int SpacingY;

        public bool Looped;
        public bool Done;

        private int _cachedFrame = -1;
        private Rectangle _cachedCropping;

        public AnimatedSprite(Texture2D texture, Rectangle? startingCropping, int framesPerRow, int totalFrames, float framesPerSecond, bool looped = true, int spacingX = 0, int spacingY = 0) : base(texture, startingCropping)
        {
            _startingRectangle = Cropping;

            FramesPerRow = framesPerRow;
            TotalFrames = totalFrames;
            FramesPerSecond = framesPerSecond;
            Paused = false;
            _timeStarted = new TimeSpan(0);
            _timePaused = new TimeSpan(0);

            Looped = looped;
            Done = false;

            SpacingX = spacingX;
            SpacingY = spacingY;
        }

        public AnimatedSprite(Texture2D texture, Rectangle? cropping) : base(texture, cropping)
        {
            _startingRectangle = Cropping;

            FramesPerRow = 1;
            TotalFrames = 1;
            FramesPerSecond = 1;
            Paused = false;
            _timeStarted = new TimeSpan(0);
            _timePaused = new TimeSpan(0);

            Looped = true;
            Done = false;

            SpacingX = 0;
            SpacingY = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (_timeStarted == new TimeSpan(0))
                _timeStarted = gameTime.TotalGameTime;
            if (Paused && _timePaused == new TimeSpan(0))
                _timePaused = gameTime.TotalGameTime;
            else if (!Paused && _timePaused != new TimeSpan(0))
            {
                _timeStarted = gameTime.TotalGameTime - (_timePaused - _timeStarted);
                _timePaused = new TimeSpan(0);
            }
            if (Paused) return;

            int currentFrame = (int)((gameTime.TotalGameTime - _timeStarted).TotalSeconds * FramesPerSecond);
            if (currentFrame >= TotalFrames)
            {
                Done = true;
                if (Looped)
                    currentFrame %= TotalFrames;
                else
                    currentFrame = TotalFrames - 1;
            }
            if (currentFrame == _cachedFrame)
            {
                Cropping = _cachedCropping;
            } else
            {
                int column = currentFrame % FramesPerRow;
                int row = currentFrame / FramesPerRow;
                Cropping = new Rectangle(_startingRectangle.X + column * (_startingRectangle.Width + SpacingX), _startingRectangle.Y + row * (_startingRectangle.Height + SpacingY), _startingRectangle.Width, _startingRectangle.Height);

                _cachedFrame = currentFrame;
                _cachedCropping = Cropping;
            }
        }

        public void Restart()
        {
            _timeStarted = GlobalTime.Timestamp;
            _timePaused = new TimeSpan(0);
            Paused = false;
            Done = false;
        }
    }
}
