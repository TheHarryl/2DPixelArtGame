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

        private TimeSpan _timestamp;

        public ProjectileController(string classifier, int minDamage, int maxDamage, float criticalMultiplier, float criticalChance, float duration = 999) : base(classifier)
        {
            int damage = GlobalService.Random.Next(minDamage, maxDamage);
            bool criticalHit = GlobalService.Random.Next(0, 1000) <= criticalChance * 1000;
            if (criticalHit)
                damage = (int)(damage * criticalMultiplier);
            Flags.Add("Damage", damage);
            Flags.Add("CriticalHit", criticalHit);
            Duration = duration;
            _timestamp = GlobalService.Timestamp;
        }
        
        public override void Update(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - _timestamp).TotalSeconds >= Duration || Vector2.Distance(Parent.Position, Parent.Parent.Parent.Camera.Position) > 2000f)
                Parent.Delete();
        }
    }
}
