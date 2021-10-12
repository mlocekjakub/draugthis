using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board(10);
            var game = new Game();
            board.Fields[5, 4] = new Pawn("black");
            game.Start(board);
        }
    }
}
