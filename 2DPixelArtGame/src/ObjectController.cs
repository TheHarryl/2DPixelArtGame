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
    public class ObjectController : BaseController
    {
        private TimeSpan? _startedFalling;

        public ObjectController(string classifier) : base(classifier)
        {
            _startedFalling = null;
        }
        
        public override void Update(GameTime gameTime)
        {
            List<Object> objects = Parent.Parent.Parent.Background.GetNearbyChunks(Parent.Chunk);
            RectangleF hitbox = Parent.GetHitboxBounds();
            Object obj = objects.Find(o => o.GetHitboxBounds().Contains(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height * 0.75f));
            if (obj == null)
            {
                if (_startedFalling == null)
                    _startedFalling = gameTime.TotalGameTime;
                Parent.Position += new Vector2(0, 800) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Parent.Color = Color.Lerp(Parent.Color, Color.Transparent, 10 * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if ((gameTime.TotalGameTime - (TimeSpan)_startedFalling).TotalSeconds > 1f)
                    Parent.Delete();
            }
        }
    }
}
