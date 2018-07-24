using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TetShared
{
    class FallingPiece : Drawable
    {
        public int[,] Shape;
        public int Number;
        public Vector2 Location;

        public FallingPiece()
        {
            Shape = null;
            Number = 0;
            Location = Vector2.Zero;
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.onFallingPiece(this);
        }
    }
}
