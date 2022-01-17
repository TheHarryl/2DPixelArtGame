using Microsoft.Xna.Framework;
using System;

namespace _2DPixelArtEngine
{
    static class GlobalTime
    {
        public static TimeSpan Timestamp;

        public static void Update(GameTime gameTime)
        {
            Timestamp = gameTime.TotalGameTime;
        }
    }
}
