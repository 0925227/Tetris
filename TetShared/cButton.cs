using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TetShared
{
    public class cButton
    {
        Texture2D texture;
        //Vector2 position;
        Vector2 bPosition;

        Rectangle rectangle;
        Color colour = new Color(255, 255, 255, 255);

        public Vector2 size;

        public cButton(ContentManager Content, string file, GraphicsDevice graphics, int bposx, int bposy)
        {
            bPosition = new Vector2(bposx, bposy);
            texture = Content.Load<Texture2D>(file);
            size = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 12);
        }
        bool down;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)bPosition.X, (int)bPosition.Y,
            (int)size.X, (int)size.Y);

            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(rectangle))
            {
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 4; else colour.A -= 4;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;

            }
            else if (colour.A < 255)
            {
                colour.A = 255;
                isClicked = false;
            }
        }
        //public void setPosition(Vector2 bPosition)
        //{
        //    position = bPosition;
        //}
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
