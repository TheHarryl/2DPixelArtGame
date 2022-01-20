using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;

namespace _2DPixelArtGame
{
    public class ProjectileController : BaseController
    {
        public float Duration;

        private TimeSpan Timestamp;

        public ProjectileController(float duration, string classifier) : base(classifier)
        {
            Duration = duration;
            Timestamp = GlobalTime.Timestamp;
        }
        
        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - Timestamp).TotalSeconds >= Duration)
                Parent.Delete();
        }
    }
}
