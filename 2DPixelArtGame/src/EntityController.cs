using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtGame
{
    public class EntityController : BaseController
    {
        public AnimatedSprite IdleUp;
        public AnimatedSprite IdleDown;
        public AnimatedSprite IdleLeft;
        public AnimatedSprite IdleRight;

        public AnimatedSprite MoveUp;
        public AnimatedSprite MoveDown;
        public AnimatedSprite MoveLeft;
        public AnimatedSprite MoveRight;

        public bool Attacking = false;
        public AnimatedSprite AttackUp;
        public AnimatedSprite AttackDown;
        public AnimatedSprite AttackLeft;
        public AnimatedSprite AttackRight;

        public EntityController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, int teamID = 0) : base(teamID)
        {
            IdleUp = idleUp;
            IdleDown = idleDown;
            IdleLeft = idleLeft;
            IdleRight = idleRight;

            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;

            AttackUp = attackUp;
            AttackDown = attackDown;
            AttackLeft = attackLeft;
            AttackRight = attackRight;
        }

        public EntityController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, int teamID = 0) : base(teamID)
        {
            IdleUp = idleUp;
            IdleDown = idleDown;
            IdleLeft = idleLeft;
            IdleRight = idleRight;

            MoveUp = moveUp;
            MoveDown = moveDown;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Attacking)
            {
                if (Parent.Direction == Vector2.Zero)
                {
                    if (Parent.Sprite == MoveUp)
                        Parent.Sprite = IdleUp;
                    else if (Parent.Sprite == MoveDown)
                        Parent.Sprite = IdleDown; 
                    else if (Parent.Sprite == MoveLeft)
                        Parent.Sprite = IdleLeft;
                    else if (Parent.Sprite == MoveRight)
                        Parent.Sprite = IdleRight;
                } else {
                    if (Math.Abs(Parent.Direction.X) >= Math.Abs(Parent.Direction.Y))
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
}
