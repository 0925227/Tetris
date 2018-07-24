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
    class Board
    {
        public int ScreenWidth;
        public int ScreenHeight;

        public int GridWidth;
        public int GridHeight;
        public int BlockSize;

        SpriteBatch spriteBatch;
        Texture2D block;
        Texture2D NextPieceCanvas;

        public Grid grid;
        public Shape shape;
        public NextPiece nextPiece;
        public FallingPiece fallingPiece;
        public Random Rand;
        DrawVisitor drawVisitor;

        public bool TimeReset;
  

        public int Score;

        public Board(SpriteBatch _spriteBatch, Texture2D _block, Texture2D _NextPieceCanvas, int _ScreenWidth, int _ScreenHeight, int _GridWidth, int _GridHeight, int _BlockSize)
        {
            ScreenWidth = _ScreenWidth;
            ScreenHeight = _ScreenHeight;

            GridWidth = _GridWidth;
            GridHeight = _GridHeight;
            BlockSize = _BlockSize;

            spriteBatch = _spriteBatch;
            block = _block;
            NextPieceCanvas = _NextPieceCanvas;

            grid = new Grid(GridWidth, GridHeight, BlockSize);
            shape = new Shape();
            Rand = new Random();
            nextPiece = new NextPiece();
            fallingPiece = new FallingPiece();

            grid.GameGrid = grid.ResetGrid();
            SpawnNext();
            SpawnCurrent();

            TimeReset = false;
   

            Score = 0;

            drawVisitor = new DrawVisitor(spriteBatch, shape.BlockColors, block, grid.Location, BlockSize, GridWidth, GridHeight, ScreenWidth, ScreenHeight, NextPieceCanvas);
        }

        // Random number generator voor de tetromino's
        public int GenerateNumber()
        {
            return Rand.Next(0, 7);
        }


        // Vormt een tetromino array om zodat deze een kleur krijgt
        public int[,] GenerateArray(int Color)
        {
            int[,] Piece = shape.Pieces[Color];

            int dim = Piece.GetLength(0);

            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim; y++)
                {
                    Piece[y, x] *= (Color + 1);
                }
            }

            return Piece;
        }


        // Genereert de volgende tetromino
        public void SpawnNext()
        {
            nextPiece.Number = GenerateNumber();
            nextPiece.Shape = (int[,])shape.Pieces[nextPiece.Number].Clone();
        }

        // Pakt de NextPiece en maakt hiervan de CurrentPiece
        public void SpawnCurrent()
        {
            fallingPiece.Shape = (int[,])nextPiece.Shape.Clone();
            fallingPiece.Location = new Vector2(GridWidth / 2 - 1, 0);

            SpawnNext();
        }

        // Checkt of er complete lijnen zijn zodat deze verwijderd kunnen worden en de score omhoog kan
        public void RemoveCompleteLines()
        {
            int CompletedLines = 0;
            for (int y = GridHeight - 1; y >= 0; y--)
            {
                bool isComplete = true;
                for (int x = 0; x < GridWidth; x++)
                {
                    if (grid.GameGrid[y, x] == 0)
                    {
                        isComplete = false;
                    }
                }

                if (isComplete)
                {
                    for (int yc = y; yc > 0; yc--)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            grid.GameGrid[yc, x] = grid.GameGrid[yc - 1, x];
                        }
                    }
                    y++;

                    CompletedLines += 1;

                }
            }
            if (CompletedLines == 1)
            {
                Score += 100;           // Score voor 1 lijn
            }

            if (CompletedLines == 2)
            {
                Score += 300;           // Score voor 2 lijnen
            }

            if (CompletedLines == 3)
            {
                Score += 800;           // Score voor 3 lijnen
            }

            if (CompletedLines == 4)
            {
                Score += 2000;          // Score voor 4 lijnen
            }
        }


        // Zodra de vallende tetromino geland is word deze op de grid geschreven
        public void Place(int[,] Piece, int x, int y)
        {
            int dim = Piece.GetLength(0);

            for (int px = 0; px < dim; px++)
            {
                for (int py = 0; py < dim; py++)
                {
                    int coordx = px + x;
                    int coordy = py + y;

                    if (Piece[py, px] != 0)
                    {
                        grid.GameGrid[coordy, coordx] = Piece[py, px];
                    }
                }
            }

            RemoveCompleteLines();
        }

        // Checkt 
        public enum PlaceStates
        {
            CAN_PLACE,      // De tetromino kan geplaatst worden
            BLOCKED,        // De tetromino word tegen gehouden door een andere tetromino
            OFFSCREEN       // De tetromino gaat buiten de grid
        }

        public PlaceStates CanPlace(int[,] Piece, int x, int y)
        {
            int dim = Piece.GetLength(0);

            for (int px = 0; px < dim; px++)
            {
                for (int py = 0; py < dim; py++)
                {
                    int coordx = x + px;
                    int coordy = y + py;

                    if (Piece[py, px] != 0)
                    {
                        if (coordx < 0 || coordx >= grid.GridWidth)
                        {
                            return PlaceStates.OFFSCREEN;
                        }

                        if (coordy >= GridHeight || grid.GameGrid[coordy, coordx] != 0)
                        {
                            return PlaceStates.BLOCKED;
                        }
                    }
                }
            }
            return PlaceStates.CAN_PLACE;
        }


        // De tetromino beweegt recht naar beneden
        public void BlockFall()
        {
            TimeReset = false;
            Vector2 NewLocation = fallingPiece.Location + new Vector2(0, 1);

            PlaceStates ps = CanPlace(fallingPiece.Shape, (int)NewLocation.X, (int)NewLocation.Y);
            if (ps != PlaceStates.CAN_PLACE)
            {
                Place(fallingPiece.Shape, (int)fallingPiece.Location.X, (int)fallingPiece.Location.Y);
                SpawnCurrent();

                ps = CanPlace(fallingPiece.Shape, (int)fallingPiece.Location.X, (int)fallingPiece.Location.Y);
                if (ps == PlaceStates.BLOCKED)
                {
                    // Hier ben je game over
                    grid.GameGrid = grid.ResetGrid();
                    //GameOver = true;                  
                }
            }
            else
            {
                fallingPiece.Location = NewLocation;
            }
            TimeReset = true;
        }

        // Checkt of een tetromino naar links of rechts mag bewegen aan de hand van input, zoja dan word de opdracht uitgevoerd
        public void Move(string direction)
        {
            if (direction == "left")
            {
                Vector2 NewLocation = fallingPiece.Location + new Vector2(-1, 0);

                PlaceStates ps = CanPlace(fallingPiece.Shape, (int)NewLocation.X, (int)NewLocation.Y);
                if (ps == PlaceStates.CAN_PLACE)
                {
                    fallingPiece.Location = NewLocation;
                }

            }

            if (direction == "right")
            {
                Vector2 NewLocation = fallingPiece.Location + new Vector2(1, 0);

                PlaceStates ps = CanPlace(fallingPiece.Shape, (int)NewLocation.X, (int)NewLocation.Y);
                if (ps == PlaceStates.CAN_PLACE)
                {
                    fallingPiece.Location = NewLocation;
                }

            }
        }

        // Checkt of een tetromino mag draaien, zoja dan word de opdracht uitgevoerd
        public void RotatePiece()
        {
            int[,] newPiece = shape.Rotate(fallingPiece.Shape);

            PlaceStates ps = CanPlace(newPiece, (int)fallingPiece.Location.X, (int)fallingPiece.Location.Y);
            if (ps == PlaceStates.CAN_PLACE)
            {
                fallingPiece.Shape = newPiece;
            }
        }

        // Tekent de verschillende componenten van het bord via een Visitor pattern
        public void Draw()
        {
            grid.Draw(drawVisitor);
            fallingPiece.Draw(drawVisitor);
            nextPiece.Draw(drawVisitor);
        }

    }
}
