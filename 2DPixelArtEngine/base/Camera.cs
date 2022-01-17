using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Camera
    {
        public Object? CameraSubject;

        private Vector2 _position;
        private Vector2Tween _tween;
        public Vector2 Position
        {
            get
            {
                if (CameraSubject != null)
                    return CameraSubject.Position;
                else
                {
                    if (_tween != null && _tween.Done())
                        _position = _tween.CurrentValue;
                    return _position;
                }
            }
            set => _position = value;
        }

        public Camera(Vector2 position = new Vector2())
        {
            Position = position;
        }

        public void Tween(Vector2Tween tween)
        {
            _tween = tween;
        }
    }
}
