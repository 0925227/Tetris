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
    class DrawVisitor : IDrawVisitor
    {
        SpriteBatch spriteBatch;
        Color[] BlockColors;
        Texture2D block;
        Vector2 BoardLocation;
        int BlockSize;
        int GridWidth;
        int GridHeight;
        int ScreenWidth;
        int ScreenHeight;
        Texture2D NextPieceCanvas;

        public DrawVisitor(SpriteBatch _spriteBatch, Color[] _BlockColors, Texture2D _block, Vector2 _BoardLocation, int _BlockSize, int _GridWidth, int _GridHeight, int _ScreenWidth, int _ScreenHeight, Texture2D _NextPieceCanvas)
        {
            spriteBatch = _spriteBatch;
            BlockColors = _BlockColors;
            block = _block;
            BoardLocation = _BoardLocation;
            BlockSize = _BlockSize;
            GridWidth = _GridWidth;
            GridHeight = _GridHeight;
            ScreenWidth = _ScreenWidth;
            ScreenHeight = _ScreenHeight;
            NextPieceCanvas = _NextPieceCanvas;
        }

        public void onFallingPiece(FallingPiece fallingPiece) // Tekent de Falling Piece
        {
            int dim = fallingPiece.Shape.GetLength(0);

            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    if (fallingPiece.Shape[y, x] != 0)
                    {
                        Color tintColor = BlockColors[fallingPiece.Shape[y, x]];

                        spriteBatch.Draw(block, new Vector2((int)BoardLocation.X + ((int)fallingPiece.Location.X + x) * BlockSize, (int)BoardLocation.Y + ((int)fallingPiece.Location.Y + y) * BlockSize), tintColor);
                    }
                }
            }
        }

        public void onGrid(Grid grid) // Tekent de grid met alle tetromino's die al geplaatst zijn
        {
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    Color tintColor = BlockColors[grid.GameGrid[y, x]];
                    if (grid.GameGrid[y, x] == 0)
                    {
                        tintColor = Color.FromNonPremultiplied(50, 50, 50, 50);
                    }

                    spriteBatch.Draw(block, new Vector2((int)BoardLocation.X + x * BlockSize, (int)BoardLocation.Y + y * BlockSize), tintColor);
                }
            }
        }

        public void onNextPiece(NextPiece nextPiece) // Tekent de Next Piece met het canvas aan de zijkant
        {
            int dim = nextPiece.Shape.GetLength(0);
            int DrawPosition = (dim * BlockSize) / 2;
            float Bx = (float)ScreenWidth * (float)0.75 - DrawPosition;
            float By = (float)ScreenHeight * (float)0.05 + (47 + 16) - DrawPosition;
            if (dim == 2)
            {
                By = (float)ScreenHeight * (float)0.05 + 47 - DrawPosition;
            }

            float Cx = (float)ScreenWidth * (float)0.75 - 76;
            float Cy = (float)ScreenHeight * (float)0.05;

            spriteBatch.Draw(NextPieceCanvas, new Vector2(Cx, Cy), Color.White);

            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim; x++)
                {
                    if (nextPiece.Shape[y, x] != 0)
                    {
                        Color tintColor = BlockColors[nextPiece.Shape[y, x]];

                        //spriteBatch.Draw(block, new Vector2(460 + (x * BlockSize) - DrawPosition, 45 + (y * BlockSize)), tintColor);
                        spriteBatch.Draw(block, new Vector2(Bx + (x * BlockSize), By + (y * BlockSize)), tintColor);
                    }
                }
            }
        }
    }
}
