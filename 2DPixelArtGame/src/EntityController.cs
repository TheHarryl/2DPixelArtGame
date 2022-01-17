using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtGame
{
    public class EntityController : Controller
    {
        public AnimatedSprite MoveUp;
        public AnimatedSprite MoveDown;
        public AnimatedSprite MoveLeft;
        public AnimatedSprite MoveRight;

        public bool Attacking = false;
        public AnimatedSprite AttackUp;
        public AnimatedSprite AttackDown;
        public AnimatedSprite AttackLeft;
        public AnimatedSprite AttackRight;

        public EntityController(AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight)
        {
            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;

            AttackUp = attackUp;
            AttackDown = attackDown;
            AttackLeft = attackLeft;
            AttackRight = attackRight;
        }

        public EntityController(AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight)
        {
            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Attacking)
            {
                if (Math.Abs(Parent.Direction.X) > Math.Abs(Parent.Direction.Y))
                {
                    if (Parent.Direction.X <= 0f)
                        Parent.Sprite = MoveLeft;
                    else
                        Parent.Sprite = MoveRight;
                } else
                {
                    if (Parent.Direction.Y <= 0f)
                        Parent.Sprite = MoveUp;
                    else
                        Parent.Sprite = MoveDown;
                }
            }
        }
    }
}
