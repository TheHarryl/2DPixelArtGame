using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Tween
    {
        public TimeSpan Timestamp;
        public float InitialValue;
        public float FinalValue;
        public float CurrentValue
        {
            get => TweenService.Tween(InitialValue, FinalValue, (float)(GlobalService.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle);
            private set { }
        }
        public EasingStyle EasingStyle;
        public EasingDirection EasingDirection;
        public float Duration;

        public Tween(float initialValue, float finalValue, EasingStyle easingStyle, EasingDirection easingDirection, float duration)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
            Timestamp = GlobalService.Timestamp;
            EasingStyle = easingStyle;
            EasingDirection = easingDirection;
            Duration = duration;
        }

        public static float Lerp(float startingValue, float endingValue, float interpolant, EasingDirection easingDirection, EasingStyle easingStyle)
        {
            return TweenService.Tween(startingValue, endingValue, interpolant, easingDirection, easingStyle);
        }

        public bool Done()
        {
            return (GlobalService.Timestamp - Timestamp).TotalSeconds >= Duration;
        }
    }
}
