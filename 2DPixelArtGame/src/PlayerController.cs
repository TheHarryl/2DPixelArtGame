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
        public PlayerController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, AnimatedSprite attackUp, AnimatedSprite attackDown, AnimatedSprite attackLeft, AnimatedSprite attackRight, int teamID = 0) : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, attackUp, attackDown, attackLeft, attackRight, teamID)
        {

        }

        public PlayerController(AnimatedSprite idleUp, AnimatedSprite idleDown, AnimatedSprite idleLeft, AnimatedSprite idleRight, AnimatedSprite moveUp, AnimatedSprite moveDown, AnimatedSprite moveLeft, AnimatedSprite moveRight, int teamID = 0) : base(idleUp, idleDown, idleLeft, idleRight, moveUp, moveDown, moveLeft, moveRight, teamID)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (!Attacking)
            {
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
            } else
            {
                if (Parent.Direction != Vector2.Zero)
                    Parent.Direction = Vector2.Normalize(Parent.Direction) / 2f;
            }

            base.Update(gameTime);
        }
    }
}
