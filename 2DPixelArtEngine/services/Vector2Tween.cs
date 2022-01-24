using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Vector2Tween
    {
        public TimeSpan Timestamp;
        public Vector2 InitialValue;
        public Vector2 FinalValue;
        public Vector2 CurrentValue
        {
            get => new Vector2(TweenService.Tween(InitialValue.X, FinalValue.X, (float)(GlobalService.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle), TweenService.Tween(InitialValue.Y, FinalValue.Y, (float)(GlobalService.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle));
            private set { }
        }
        public EasingStyle EasingStyle;
        public EasingDirection EasingDirection;
        public float Duration;

        public Vector2Tween(Vector2 initialValue, Vector2 finalValue, EasingStyle easingStyle, EasingDirection easingDirection, float duration)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
            Timestamp = GlobalService.Timestamp;
            EasingStyle = easingStyle;
            EasingDirection = easingDirection;
            Duration = duration;
        }

        public static Vector2 Lerp(Vector2 startingValue, Vector2 endingValue, float interpolant, EasingDirection easingDirection, EasingStyle easingStyle)
        {
            return new Vector2(TweenService.Tween(startingValue.X, endingValue.X, interpolant, easingDirection, easingStyle), TweenService.Tween(startingValue.Y, endingValue.Y, interpolant, easingDirection, easingStyle));
        }

        public bool Done()
        {
            return (GlobalService.Timestamp - Timestamp).TotalSeconds >= Duration;
        }
    }
}
