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
            var board = new Board(10);
            var game = new Game();
            board.Fields[5, 4] = new Pawn("black");
            game.Start(board);
            //ConsoleColor backgroundColor = Console.BackgroundColor;
            //ConsoleColor foregroundColor = Console.ForegroundColor;
            //Console.WriteLine("hello kurwa");
            //Board board = new Board(20);
            //board.InitBoard();
            //board.choosingPawn();
            //Console.BackgroundColor = backgroundColor;
            //Console.ForegroundColor = foregroundColor;
        }
    }
}
