using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Object = _2DPixelArtEngine.Object;

namespace _2DPixelArtGame
{
    public class EnemyController : EntityController
    {
        public EnemyController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, AnimatedSprite knockUp, AnimatedSprite knockDown, AnimatedSprite knockLeft, AnimatedSprite knockRight, string classifier = "enemy") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, attackUp, attackDown, attackLeft, attackRight, knockUp, knockDown, knockLeft, knockRight, classifier)
        {
        }

        public EnemyController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, string classifier = "enemy") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, classifier)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Object obj = Parent.Parent.GetNearbyChunks(Parent.Chunk).Find(o => o.GetHitboxBounds().IntersectsWith(Parent.GetHitboxBounds()) && o.Controller.Classifier == "playerprojectile");
            if (obj != null)
            {
                Parent.Direction = obj.Direction / Parent.Speed * 100f;
                Knock();
                obj.Delete();
            }
            base.Update(gameTime);
        }
    }
}
