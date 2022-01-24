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
    public class EnemyController : EntityController
    {
        public int Health;

        public EnemyController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, AnimatedSprite knockUp, AnimatedSprite knockDown, AnimatedSprite knockLeft, AnimatedSprite knockRight, int health, string classifier = "enemy") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, attackUp, attackDown, attackLeft, attackRight, knockUp, knockDown, knockLeft, knockRight, classifier)
        {
            Health = health;
        }

        public EnemyController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, int health, string classifier = "enemy") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, classifier)
        {
            Health = health;
        }

        public override void Update(GameTime gameTime)
        {
            Object obj = Parent.Parent.GetNearbyChunks(Parent.Chunk).Find(o => o.GetHitboxBounds().IntersectsWith(Parent.GetHitboxBounds()) && o.Controller.Classifier == "playerprojectile");
            if (obj != null)
            {
                Health -= (int)obj.Controller.Flags["Damage"];
                DamageTextController.Control(Parent, (int)obj.Controller.Flags["Damage"], (bool)obj.Controller.Flags["CriticalHit"]);
                Parent.Direction = obj.Direction;// / Parent.Speed * 100f;
                Knock();
                obj.Delete();
                //if (Health <= 0)
                //    Parent.Delete();
            }
            base.Update(gameTime);
        }
    }
}
