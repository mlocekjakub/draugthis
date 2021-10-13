using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    static class Program
    {
        public static void Main(string[] args)
        {
            //var board = new Board(10);
            var game = new Game();
            //Menu menu = new Menu();
            //var board = menu.RunMenu();
            var board = new Board(10);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board.Fields[i, j] = null;
                }
            }
            //board.Fields[9, 4] = new Pawn("black", 9, 4);
            board.Fields[8, 3] = new Pawn("white",8,3);
            board.Fields[6, 3] = new Pawn("white",6,3);
            board.Fields[6, 3].IsCrowned = true;
            board.Fields[6, 5] = new Pawn("white",6,5);
            board.Fields[8, 5] = new Pawn("white",8,5);
            board.Fields[5, 4] = new Pawn("black",5,4);
            board.Fields[2, 1] = new Pawn("white",2,1);
            board.Fields[7, 9] = new Pawn("black", 7, 9);
            game.Start(board);

        }

    }

    
}
