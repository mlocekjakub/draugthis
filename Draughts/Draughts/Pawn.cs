using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Pawn
    {
        public string Color { get; set; }
        public ConsoleColor FontColor { get; set; }
        public bool Highlight { get; set; } = false;

        public Pawn(string color)
        {
            Color = color;
            if (color == "white")
            {
                FontColor = ConsoleColor.White;
            }
            else
            {
                FontColor = ConsoleColor.Black;
            }
        }
    }
}
