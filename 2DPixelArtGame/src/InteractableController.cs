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
    public class InteractableController : BaseController
    {
        protected TimeSpan? _startedFalling = null;

        public InteractableController(string classifier) : base(classifier)
        {
        }
    }
}
