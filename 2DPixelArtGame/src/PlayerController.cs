using _2DPixelArtEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Object = _2DPixelArtEngine.Object;

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
            if (Parent.Parent.Parent.KeyboardState != null)
            {
                KeyboardState keyboardState = (KeyboardState)Parent.Parent.Parent.KeyboardState;
                Parent.Direction = new Vector2();
                if (keyboardState.IsKeyDown(Keys.Up))
                    Parent.Direction += new Vector2(0, -1);
                if (keyboardState.IsKeyDown(Keys.Down))
                    Parent.Direction += new Vector2(0, 1);
                if (keyboardState.IsKeyDown(Keys.Left))
                    Parent.Direction += new Vector2(-1, 0);
                if (keyboardState.IsKeyDown(Keys.Right))
                    Parent.Direction += new Vector2(1, 0);
                if (Parent.Direction != Vector2.Zero)
                    Parent.Direction = Vector2.Normalize(Parent.Direction);
                if (keyboardState.IsKeyDown(Keys.Enter) && !_attacking)
                {
                    Attack();
                    Vector2 direction = Vector2.Zero;
                    if (Parent.Sprite == AttackUp)
                        direction = new Vector2(0, -1);
                    else if (Parent.Sprite == AttackDown)
                        direction = new Vector2(0, 1);
                    else if (Parent.Sprite == AttackLeft)
                        direction = new Vector2(-1, 0);
                    else if (Parent.Sprite == AttackRight)
                        direction = new Vector2(1, 0);
                    Object projectile = new Object(new RectangleF(0, 0, 20, 20), new Sprite(ContentManager.LoadTexture("projectile.png")), Parent.Position + direction * 20, new Vector2(-10, -10), new ProjectileController("playerprojectile", 5, 10, 2, 0.1f, 0), 0);
                    projectile.Direction = direction;
                    projectile.Scale = new Vector2(2, 2);
                    projectile.Color = Color.Transparent;
                    Parent.Parent.Add(projectile);
                }
            }

            if (Parent.Parent.Parent.MouseState != null)
            {
                MouseState mouseState = (MouseState)Parent.Parent.Parent.MouseState;
                //MouseState lastMouseState = (MouseState)Parent.Parent.Parent.LastMouseState;
                if (mouseState.LeftButton == ButtonState.Pressed)// && lastMouseState.LeftButton == ButtonState.Released)
                {
                    Vector2 direction = Vector2.Normalize(new Vector2(mouseState.X, mouseState.Y) - Parent.ScreenPosition);
                    Object projectile = new Object(new RectangleF(0, 0, 20, 20), new Sprite(ContentManager.LoadTexture("projectile.png")), Parent.Position + direction * 30f, new Vector2(-10, -10), new ProjectileController("playerprojectile", 5, 10, 2, 0.3f, 5), 800);
                    projectile.Direction = direction;
                    projectile.AlwaysOnTop = true;
                    Parent.Parent.Add(projectile);
                }
            }

            base.Update(gameTime);
        }
    }
}
