using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtGame
{
    public class PlayerController : EntityController
    {
        public PlayerController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, AnimatedSprite knockUp, AnimatedSprite knockDown, AnimatedSprite knockLeft, AnimatedSprite knockRight, string classifier = "player") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, attackUp, attackDown, attackLeft, attackRight, knockUp, knockDown, knockLeft, knockRight, classifier)
        {

        }

        public PlayerController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, string classifier = "player") : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, classifier)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            //if (!_attacking && !_knocked)
            //{
                KeyboardState state = Keyboard.GetState();

                Parent.Direction = new Vector2();
                if (state.IsKeyDown(Keys.Up))
                    Parent.Direction += new Vector2(0, -1);
                if (state.IsKeyDown(Keys.Down))
                    Parent.Direction += new Vector2(0, 1);
                if (state.IsKeyDown(Keys.Left))
                    Parent.Direction += new Vector2(-1, 0);
                if (state.IsKeyDown(Keys.Right))
                    Parent.Direction += new Vector2(1, 0);
                if (Parent.Direction != Vector2.Zero)
                    Parent.Direction = Vector2.Normalize(Parent.Direction);
                if (state.IsKeyDown(Keys.Enter))
                    Attack();
            //}
            base.Update(gameTime);
        }
    }
}
