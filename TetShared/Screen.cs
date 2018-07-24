using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace TetShared
{
    public class Screen
    {

        Texture2D bg;
        public int width;
        public int height;
        Vector2 position;

        public Screen(ContentManager Content, string BG)
        {
            this.width = 600;
            this.height = 496;
            this.bg = Content.Load<Texture2D>(BG);
            this.position = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bg, position, Color.White);
        }

    }
}