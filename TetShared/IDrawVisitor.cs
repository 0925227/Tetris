using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetShared
{
    interface IDrawVisitor
    {
        void onGrid(Grid grid);
        void onFallingPiece(FallingPiece fallingPiece);
        void onNextPiece(NextPiece nextPiece);
    }
}
