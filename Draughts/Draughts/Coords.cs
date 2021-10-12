using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Coords
    {
        public int YPos{ get; set; }
        public int XPos { get; set; }

        public Coords(int yPos, int xPos)
        {
            YPos = yPos;
            XPos = xPos;
        }
        public Coords(Coords coords)
        {
            YPos = coords.YPos;
            XPos = coords.XPos;
        }
    }
}
