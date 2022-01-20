using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class BaseController
    {
        public Object Parent;
        public string Classifier;
        public int ActionTag;

        public BaseController(string classifier = "none")
        {
            Classifier = classifier;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
