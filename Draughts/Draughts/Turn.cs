using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Turn
    {
        public Coords StartingPos { get; set; }
        public Coords EndingPos { get; set; }
        public string KilledColour { get; set; } = "none";
        public Turn(Coords startingPos, Coords endingPos, string killedColor = "white")
        {
            StartingPos = startingPos;
            EndingPos = endingPos;
            KilledColour = killedColor;
        }
        public Coords GetKilledPawnCoords()
        {
            if (KilledColour != "none")
            {
                return new Coords((StartingPos.YPos + EndingPos.YPos) / 2, (StartingPos.XPos + EndingPos.XPos) / 2);
            }
            else
            {
                return null;
            }
        }
    }
}
