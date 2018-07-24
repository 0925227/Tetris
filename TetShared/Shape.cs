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
    public class Shape
    {
        public int[][,] Pieces;

        int[,] Piece1;
        int[,] Piece2;
        int[,] Piece3;
        int[,] Piece4;
        int[,] Piece5;
        int[,] Piece6;
        int[,] Piece7;

        public Color[] BlockColors;

        public Shape()
        {
            // Alle tetrominos worden opgeslagen in een multidimensional array
            // Op deze manier worden ze op het scherm getekent, kunnen we ze roteren en is collision detection mogelijk
            // Een 0 staat voor een lege plek, bij een hoger getal word er een blokje getekent in de bijpassende kleur

            Piece1 = new int[2, 2]     // O Tetromino
            {
                    {1,1},
                    {1,1}
            };

            Piece2 = new int[4, 4]     // I Tetromino
            {
                    {0,0,0,0},
                    {2,2,2,2},
                    {0,0,0,0},
                    {0,0,0,0}
            };

            Piece3 = new int[3, 3]     // J Tetromino
            {
                    {3,0,0},
                    {3,3,3},
                    {0,0,0}
            };

            Piece4 = new int[3, 3]     // L Tetromino
            {
                    {0,0,4},
                    {4,4,4},
                    {0,0,0}
            };

            Piece5 = new int[3, 3]     // S Tetromino
            {
                    {0,5,5},
                    {5,5,0},
                    {0,0,0}
            };

            Piece6 = new int[3, 3]     // Z Tetromino
            {
                    {6,6,0},
                    {0,6,6},
                    {0,0,0}
            };

            Piece7 = new int[3, 3]     // T Tetromino
            {
                    {0,7,0},
                    {7,7,7},
                    {0,0,0}
            };

            // Alle tetrominos worden opgeslagen in een Array
            // Aan de hand van het indexcijfer kunnen ze random worden gespawned
            Pieces = new int[][,]
            {
                    Piece1,    // 0
                    Piece2,    // 1
                    Piece3,    // 2
                    Piece4,    // 3
                    Piece5,    // 4
                    Piece6,    // 5
                    Piece7     // 6
            };

            // De bijbehorende kleuren worden ook opgeslagen in een Array
            // Transparent is voor de achtergrond
            BlockColors = new Color[]
            {
                    Color.Transparent,      //0
                    Color.Orange,           //1
                    Color.Blue,             //2
                    Color.Red,              //3
                    Color.LightSkyBlue,     //4
                    Color.Yellow,           //5
                    Color.Magenta,          //6
                    Color.LimeGreen         //7
            };
        }


        // Een gegeven Piece word met de klok mee gedraaid
        public int[,] Rotate(int[,] Piece)
        {
            int dim = Piece.GetLength(0);
            int[,] npiece = new int[dim, dim];

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    npiece[i, j] = Piece[dim - 1 - j, i];
                }
            }

            return npiece;
        }
    }
}
