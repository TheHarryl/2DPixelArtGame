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
        public int TeamID;

        public BaseController(int teamID = 0)
        {
            TeamID = teamID;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
