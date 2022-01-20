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
        public Dictionary<string, object> Properties;

        public BaseController(string classifier = "none")
        {
            Classifier = classifier;
            Properties = new Dictionary<string, object>();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset = new Vector2())
        {
            
        }
    }
}
