using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Game
    {
        public static void Start()
        {
            Console.WriteLine("hello kurwa");
            Console.WriteLine("How big board do you want to have?");
            int n = Convert.ToInt32(Console.ReadLine());
            Board board = new Board(n);
            board.ToString();
        }
    }
}