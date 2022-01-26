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
    public class DamageTextController : BaseController
    {
        private TimeSpan _timestamp;
        private bool _criticalHit;

        private bool _cacheOriginal = false;
        private Color _originalColor;
        private Vector2 _originalScale;
        private Vector2 _originalPosition;

        private Vector2 _shake;
        private int _shakeStep;

        public DamageTextController(bool criticalHit = false) : base()
        {
            _criticalHit = criticalHit;
            _timestamp = GlobalService.Timestamp;
            _shakeStep = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_cacheOriginal) {
                _cacheOriginal = true;
                _originalColor = Parent.Color;
                _originalScale = Parent.Scale;
                _originalPosition = Parent.Position;
            }
            float interpolant = (float)(gameTime.TotalGameTime - _timestamp).TotalSeconds;
            if (_criticalHit) {
                int shakeStep = (int)(GlobalService.Timestamp.TotalSeconds * 30);
                if (_shakeStep != shakeStep)
                {
                    _shake = new Vector2(GlobalService.Random.Next(-2, 2), GlobalService.Random.Next(-2, 2)) * 1.5f;
                    _shakeStep = shakeStep;
                }
                Parent.Position = _shake + Vector2Tween.Lerp(_originalPosition, _originalPosition + new Vector2(0, -40), interpolant * 1.3f, EasingDirection.Out, EasingStyle.Back);
                Parent.Scale = Vector2Tween.Lerp(_originalScale * 0.5f, _originalScale, interpolant * 4f > 1f ? 1f : interpolant * 4f, EasingDirection.In, EasingStyle.Bounce);
            } else {
                Parent.Position = Vector2Tween.Lerp(_originalPosition, _originalPosition + new Vector2(0, -40), interpolant * 1.3f, EasingDirection.Out, EasingStyle.Quart);
                Parent.Scale = Vector2.Lerp(_originalScale * 0.5f, _originalScale, interpolant * 4f > 1f ? 1f : interpolant * 4f);
            }
            Parent.Color = Color.Lerp(Color.Transparent, _originalColor, interpolant * 4f > 1f ? 1f : interpolant * 4f);

            if (interpolant >= 0.7f)
                Parent.Delete();
        }

        public static void Control(Object obj, int damage, bool criticalHit)
        {
            RectangleF bounds = obj.GetBounds();
            Object text = new Object(new RectangleF(), new FontSprite(ContentManager.LoadFont("pixelsplitter.ttf"), Math.Abs(damage).ToString(), 3), new Vector2(bounds.X, bounds.Y) + new Vector2(GlobalService.Random.Next(0, (int)bounds.Width), bounds.Height * (GlobalService.Random.Next(25, 75) / 100f)), new Vector2(), new DamageTextController(criticalHit));
            text.AlwaysOnTop = true;
            if (damage < 0f) {
                if (criticalHit)
                    text.Color = new Color(0, 150, 0);
                else
                    text.Color = new Color(70, 255, 70);
            } else if (criticalHit)
                text.Color = new Color(255, 70, 70);
            text.Origin = new Vector2(text.Sprite.Cropping.Width / 2, text.Sprite.Cropping.Height / 2);
            text.Scale = new Vector2(0.9f, 0.9f);
            obj.Parent.Add(text);
        }
    }
}
