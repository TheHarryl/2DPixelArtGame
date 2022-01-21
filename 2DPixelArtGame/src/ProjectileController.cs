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

        public ProjectileController(string classifier, int minDamage, int maxDamage, float duration = 999) : base(classifier)
        {
            Properties.Add("MinDamage", minDamage);
            Properties.Add("MaxDamage", maxDamage);
            Duration = duration;
            Timestamp = GlobalTime.Timestamp;
        }
        
        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - Timestamp).TotalSeconds >= Duration || Vector2.Distance(Parent.Position, Parent.Parent.Parent.Camera.Position) > 2000f)
                Parent.Delete();
        }
    }
}
