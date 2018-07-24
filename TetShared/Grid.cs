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
    class Grid : Drawable
    {
        public int GridWidth;
        public int GridHeight;
        public int BlockSize;
        public Vector2 Location;

        public int[,] GameGrid;

        public Grid(int _GridWidth, int _GridHeight, int _BlockSize)
        {
            GridWidth = _GridWidth;
            GridHeight = _GridHeight;
            BlockSize = _BlockSize;
            Location = Vector2.Zero;

            GameGrid = ResetGrid();
        }


        // Reset het grid zodat alle plaatsen weer een 0 als waarde krijgen
        public int[,] ResetGrid()
        {
            int[,] EmptyGrid = new int[GridHeight, GridWidth];
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    EmptyGrid[y, x] = 0;
                }
            }

            return EmptyGrid;
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.onGrid(this);
        }
    }
}
