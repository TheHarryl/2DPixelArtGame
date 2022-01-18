using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Object = _2DPixelArtEngine.Object;

namespace _2DPixelArtGame
{
    public class GrassController : Controller
    {
        public AnimatedSprite Normal;
        public AnimatedSprite BrushLeft;
        public AnimatedSprite BrushRight;
        public AnimatedSprite UnbrushLeft;
        public AnimatedSprite UnbrushRight;

        public GrassController(AnimatedSprite normal, AnimatedSprite brushLeft, AnimatedSprite brushRight, AnimatedSprite unbrushLeft, AnimatedSprite unbrushRight)
        {
            Normal = normal;
            BrushLeft = brushLeft;
            BrushRight = brushRight;
            UnbrushLeft = unbrushLeft;
            UnbrushRight = unbrushRight;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Parent.Sprite == Normal)
            {
                List<Object> objects = Parent.Parent.GetNearbyChunks(Parent.Chunk);
                RectangleF hitbox = Parent.GetHitboxBounds();
                for (int i = 0; i < objects.Count; i++)
                {
                    RectangleF objectHitbox = objects[i].GetHitboxBounds();
                    if (!hitbox.IntersectsWith(objectHitbox)) continue;
                    if (objectHitbox.X + (objectHitbox.Width / 2) < hitbox.X + (hitbox.Width / 2))
                    {
                        Parent.Sprite = BrushLeft;
                        BrushLeft.Restart();
                    } else
                    {
                        Parent.Sprite = BrushRight;
                        BrushRight.Restart();
                    }
                    return;
                }
            } else if (Parent.Sprite == BrushLeft || Parent.Sprite == BrushRight)
            {
                List<Object> objects = Parent.Parent.GetNearbyChunks(Parent.Chunk);
                RectangleF hitbox = Parent.GetHitboxBounds();
                for (int i = 0; i < objects.Count; i++)
                {
                    RectangleF objectHitbox = objects[i].GetHitboxBounds();
                    if (hitbox.IntersectsWith(objectHitbox))
                        return;
                }
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
            }
        }
    }
}
