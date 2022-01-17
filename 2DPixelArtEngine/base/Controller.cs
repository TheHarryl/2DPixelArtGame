using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2DPixelArtEngine
{
    public class Controller
    {
        public Object Parent;
        public int TeamID;

        public Controller(int teamID = 0)
        {
            TeamID = teamID;
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
