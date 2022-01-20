using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Object = _2DPixelArtEngine.Object;

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

        protected bool _attacking = false;
        public AnimatedSprite AttackUp;
        public AnimatedSprite AttackDown;
        public AnimatedSprite AttackLeft;
        public AnimatedSprite AttackRight;

        protected bool _knocked = false;
        public AnimatedSprite KnockUp;
        public AnimatedSprite KnockDown;
        public AnimatedSprite KnockLeft;
        public AnimatedSprite KnockRight;

        public EntityController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, AnimatedSprite knockUp, AnimatedSprite knockDown, AnimatedSprite knockLeft, AnimatedSprite knockRight, string classifier = "entity") : base(classifier)
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

            KnockUp = knockUp;
            KnockDown = knockDown;
            KnockLeft = knockLeft;
            KnockRight = knockRight;
        }

        public EntityController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, string classifier = "entity") : base(classifier)
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
            if (!_attacking && !_knocked)
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
            } else if (_attacking)
            {
                if (Parent.Direction != Vector2.Zero)
                    Parent.Direction = Vector2.Normalize(Parent.Direction) / 2f;
                if (Parent.Sprite == AttackUp && AttackUp.Done)
                {
                    _attacking = false;
                    Parent.Sprite = IdleUp;
                } else if (Parent.Sprite == AttackDown && AttackDown.Done)
                {
                    _attacking = false;
                    Parent.Sprite = IdleDown;
                } else if (Parent.Sprite == AttackLeft && AttackLeft.Done)
                {
                    _attacking = false;
                    Parent.Sprite = IdleLeft;
                } else if (Parent.Sprite == AttackRight && AttackRight.Done)
                {
                    _attacking = false;
                    Parent.Sprite = IdleRight;
                }
            } else if (_knocked)
            {
                if (Parent.Sprite == KnockUp && KnockUp.Done)
                {
                    _knocked = false;
                    Parent.Sprite = IdleDown;
                    Parent.Direction = Vector2.Zero;
                }
                else if (Parent.Sprite == KnockDown && KnockDown.Done)
                {
                    _knocked = false;
                    Parent.Sprite = IdleUp;
                    Parent.Direction = Vector2.Zero;
                }
                else if (Parent.Sprite == KnockLeft && KnockLeft.Done)
                {
                    _knocked = false;
                    Parent.Sprite = IdleRight;
                    Parent.Direction = Vector2.Zero;
                }
                else if (Parent.Sprite == KnockRight && KnockRight.Done)
                {
                    _knocked = false;
                    Parent.Sprite = IdleLeft;
                    Parent.Direction = Vector2.Zero;
                }
            }
        }

        protected void Attack()
        {
            if (_attacking || _knocked) return;
            _attacking = true;
            /*List<Object> objects = Parent.Parent.GetNearbyChunks(Parent.Chunk);
            RectangleF hitbox = Parent.GetHitboxBounds();
            for (int i = 0; i < objects.Count; i++)
            {
                RectangleF objectHitbox = objects[i].GetHitboxBounds();
                if (!hitbox.IntersectsWith(objectHitbox) || objects[i].Controller.TeamID != -1) continue;
                objects[i].Controller.ActionTag = 1;
                objects[i].Direction = Parent.Direction;
                return;
            }*/
            if (Parent.Sprite == IdleUp || Parent.Sprite == MoveUp)
            {
                Parent.Sprite = AttackUp;
                AttackUp.Restart();
            } else if (Parent.Sprite == IdleDown || Parent.Sprite == MoveDown)
            {
                Parent.Sprite = AttackDown;
                AttackDown.Restart();
            } else if (Parent.Sprite == IdleLeft || Parent.Sprite == MoveLeft)
            {
                Parent.Sprite = AttackLeft;
                AttackLeft.Restart();
            } else if (Parent.Sprite == IdleRight || Parent.Sprite == MoveRight)
            {
                Parent.Sprite = AttackRight;
                AttackRight.Restart();
            } else
            {
                _attacking = false;
            }
        }

        protected void Knock()
        {
            _attacking = false;
            _knocked = true;
            if (Math.Abs(Parent.Direction.X) >= Math.Abs(Parent.Direction.Y))
            {
                if (Parent.Direction.X <= 0f) {
                    Parent.Sprite = KnockLeft;
                    KnockLeft.Restart();
                } else {
                    Parent.Sprite = KnockRight;
                    KnockRight.Restart();
                }
            } else
            {
                if (Parent.Direction.Y <= 0f) {
                    Parent.Sprite = KnockUp;
                    KnockUp.Restart();
                } else {
                    Parent.Sprite = KnockDown;
                    KnockDown.Restart();
                }
            }
        }
    }
}
