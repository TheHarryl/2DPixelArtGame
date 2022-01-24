using Microsoft.Xna.Framework;
using System;

namespace _2DPixelArtEngine
{
    public static class GlobalService
    {
        public static TimeSpan Timestamp;
        public static Random Random;

        public static void Initialize(Random random)
        {
            Random = random;
        }

        public static void Update(GameTime gameTime)
        {
            Timestamp = gameTime.TotalGameTime;
        }
    }
}
