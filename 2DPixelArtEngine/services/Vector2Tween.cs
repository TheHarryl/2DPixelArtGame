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
            get => new Vector2(TweenService.Tween(InitialValue.X, FinalValue.X, (float)(GlobalTime.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle), TweenService.Tween(InitialValue.Y, FinalValue.Y, (float)(GlobalTime.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle));
            private set { }
        }
        public EasingStyle EasingStyle;
        public EasingDirection EasingDirection;
        public float Duration;

        public Vector2Tween(Vector2 initialValue, Vector2 finalValue, EasingStyle easingStyle, EasingDirection easingDirection, float duration)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
            Timestamp = GlobalTime.Timestamp;
            EasingStyle = easingStyle;
            EasingDirection = easingDirection;
            Duration = duration;
        }

        public bool Done()
        {
            return (GlobalTime.Timestamp - Timestamp).TotalSeconds >= Duration;
        }
    }
}
