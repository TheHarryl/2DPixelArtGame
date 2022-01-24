using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;

namespace _2DPixelArtGame
{
    public class EntityController : InteractableController
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

        private TimeSpan _lastMove;

        private Object? _outline;

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

            _lastMove = GlobalService.Timestamp;
        }

        public override void Update(GameTime gameTime)
        {
            List<Object> backgroundObjects = Parent.Parent.Parent.Background.GetNearbyChunks(Parent.Chunk);
            RectangleF hitbox = Parent.GetHitboxBounds();
            Object floorObj = backgroundObjects.Find(o => o.GetHitboxBounds().Contains(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height * 0.75f));
            if (floorObj == null || _startedFalling != null)
            {
                if (_startedFalling == null)
                    _startedFalling = gameTime.TotalGameTime;
                Parent.Position += new Vector2(0, 800) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Parent.Color = Color.Lerp(Parent.Color, Color.Transparent, 5 * (float)gameTime.ElapsedGameTime.TotalSeconds);

                if ((gameTime.TotalGameTime - (TimeSpan)_startedFalling).TotalSeconds > 1f)
                    Parent.Delete();
            }
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
                } else if (Parent.Sprite != KnockUp && Parent.Sprite != KnockDown && Parent.Sprite != KnockLeft & Parent.Sprite != KnockRight)
                {
                    _knocked = false;
                }
            }

            if (Parent.Direction != Vector2.Zero)
                _lastMove = GlobalService.Timestamp;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset = default)
        {
            List<Object> sceneObjects = Parent.Parent.Parent.Scene.GetNearbyChunks(Parent.Chunk);
            RectangleF hitbox = Parent.GetHitboxBounds();
            Object inanimateObj = sceneObjects.Find(o => o.Controller.Classifier == "inanimate" && o.GetHitboxBounds().IntersectsWith(hitbox));
            if (inanimateObj != null && _outline == null)
            {
                _outline = new Object(Parent.Hitbox, Parent.Sprite);
                _outline.AlwaysOnTop = true;
                Parent.Parent.Add(_outline);
            } else if (inanimateObj == null && _outline != null)
            {
                Parent.Parent.Remove(_outline);
                _outline = null;
            }
            if (_outline != null)
            {
                float interpolant = (float)(GlobalService.Timestamp - _lastMove).TotalSeconds;
                if (interpolant > 1f)
                    interpolant = 1f;
                _outline.Hitbox = Parent.Hitbox;
                _outline.Sprite = Parent.Sprite;
                _outline.Position = Parent.Position;
                _outline.SpriteOffset = Parent.SpriteOffset;
                _outline.Scale = Parent.Scale;
                _outline.Color = Color.Lerp(new Color(255, 255, 255, 0) * 0f, new Color(255, 255, 255, 0) * 0.15f, interpolant);
            }
        }

        protected void Attack()
        {
            if (_attacking || _knocked) return;
            _lastMove = GlobalService.Timestamp;
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
            _lastMove = GlobalService.Timestamp;
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
            Parent.TweenPosition(new Vector2Tween(Vector2.Zero, Parent.Direction * 70f, EasingStyle.Quint, EasingDirection.Out, 1f));
            Parent.Direction = Vector2.Zero;
        }
    }
}
