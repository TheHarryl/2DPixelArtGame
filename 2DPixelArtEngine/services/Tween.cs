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
            get => TweenService.Tween(InitialValue, FinalValue, (float)(GlobalTime.Timestamp - Timestamp).TotalSeconds / Duration, EasingDirection, EasingStyle);
            private set { }
        }
        public EasingStyle EasingStyle;
        public EasingDirection EasingDirection;
        public float Duration;

        public Tween(float initialValue, float finalValue, EasingStyle easingStyle, EasingDirection easingDirection, float duration)
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
