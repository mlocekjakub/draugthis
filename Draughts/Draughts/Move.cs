using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Move
    {
        public Coords StartingPos { get; set; }
        public Coords EndingPos { get; set; }
        public Pawn KilledPawn { get; set; }
        public Move(Coords startingPos, Coords endingPos, Pawn killedPawn = null)
        {
            StartingPos = startingPos;
            EndingPos = endingPos;
            KilledPawn = killedPawn;
        }
        //public Coords GetKilledPawnCoords()
        //{
        //    if (KilledColour != null)
        //    {
        //        return new Coords((StartingPos.YPos + EndingPos.YPos) / 2, (StartingPos.XPos + EndingPos.XPos) / 2);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
