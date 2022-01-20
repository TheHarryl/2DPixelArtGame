using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Object = _2DPixelArtEngine.Object;

namespace _2DPixelArtGame
{
    public class GrassController : BaseController
    {
        public AnimatedSprite Normal;
        public AnimatedSprite BrushLeft;
        public AnimatedSprite BrushRight;
        public AnimatedSprite UnbrushLeft;
        public AnimatedSprite UnbrushRight;

        public GrassController(AnimatedSprite normal, AnimatedSprite brushLeft, AnimatedSprite brushRight, AnimatedSprite unbrushLeft, AnimatedSprite unbrushRight) : base("grass")
        {
            Normal = normal;
            BrushLeft = brushLeft;
            BrushRight = brushRight;
            UnbrushLeft = unbrushLeft;
            UnbrushRight = unbrushRight;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Parent.Sprite == Normal || Parent.Sprite == BrushLeft || Parent.Sprite == BrushRight)
            {
                List<Object> objects = Parent.Parent.GetNearbyChunks(Parent.Chunk);
                RectangleF hitbox = Parent.GetHitboxBounds();
                Object obj = objects.Find(o => o.GetHitboxBounds().IntersectsWith(hitbox) && o.Controller.Classifier != "grass");
                if (obj != null)
                {
                    RectangleF objectHitbox = obj.GetHitboxBounds();
                    if (objectHitbox.X + (objectHitbox.Width / 2) > hitbox.X + (hitbox.Width / 2) && Parent.Sprite != BrushLeft)
                    {
                        Parent.Sprite = BrushLeft;
                        BrushLeft.Restart();
                    }
                    else if (objectHitbox.X + (objectHitbox.Width / 2) <= hitbox.X + (hitbox.Width / 2) && Parent.Sprite != BrushRight)
                    {
                        Parent.Sprite = BrushRight;
                        BrushRight.Restart();
                    }
                    return;
                }
            }
            if (Parent.Sprite == BrushLeft || Parent.Sprite == BrushRight)
            {
                List<Object> objects = Parent.Parent.GetNearbyChunks(Parent.Chunk);
                RectangleF hitbox = Parent.GetHitboxBounds();
                Object obj = objects.Find(o => o.GetHitboxBounds().IntersectsWith(hitbox) && o.Controller.Classifier != "grass");
                if (obj != null) return;
                if (Parent.Sprite == BrushLeft)
                {
                    Parent.Sprite = UnbrushLeft;
                    UnbrushLeft.Restart();
                } else
                {
                    Parent.Sprite = UnbrushRight;
                    UnbrushRight.Restart();
                }
            } else if (Parent.Sprite == UnbrushLeft)
            {
                if (!UnbrushLeft.Done) return;
                Parent.Sprite = Normal;
                Normal.Restart();
            } else if (Parent.Sprite == UnbrushRight)
            {
                if (!UnbrushRight.Done) return;
                Parent.Sprite = Normal;
                Normal.Restart();
            } else
            {
                Parent.Sprite = Normal;
            }
        }
    }
}
