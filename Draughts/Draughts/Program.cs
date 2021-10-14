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
            var game = new Game();
            //board.Fields[5, 4] = new Pawn("black");
            Menu menu = new Menu();
            var board = menu.RunMenu();
            game.Start(board);

        }

    }

    
}
