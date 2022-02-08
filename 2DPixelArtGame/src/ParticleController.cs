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
    public class ParticleController : BaseController
    {
        private bool _cachedOriginal = false;

        private Color _initialColor;
        private Vector2 _initialScale;
        private float _initialSpeed;

        private Color _finalColor;
        private Vector2 _finalScale;
        private float _finalSpeed;

        private Tween _animationPosition;

        public ParticleController(string classifier, float duration, EasingDirection easingDirection, EasingStyle easingStyle, Color finalColor, Vector2 finalScale, float finalSpeed) : base(classifier)
        {
            _animationPosition = new Tween(0, 1, easingStyle, easingDirection, duration);

            _finalColor = finalColor;
            _finalScale = finalScale;
            _finalSpeed = finalSpeed;
        }
        
        public override void Update(GameTime gameTime)
        {
            float interpolant = _animationPosition.CurrentValue;

            if (!_cachedOriginal)
            {
                _cachedOriginal = true;
                _initialColor = Parent.Color;
                _initialScale = Parent.Scale;
                _initialSpeed = Parent.Speed;
            }
            
            Parent.Color = Color.Lerp(_initialColor, _finalColor, interpolant);
            Parent.Scale = Vector2.Lerp(_initialScale, _finalScale, interpolant);
            Parent.Speed = _initialSpeed * (1f - interpolant) + _finalSpeed * interpolant;

            if (interpolant >= 1f || Vector2.Distance(Parent.Position, Parent.Parent.Parent.Camera.Position) > 2000f)
                Parent.Delete();
        }
    }
}
