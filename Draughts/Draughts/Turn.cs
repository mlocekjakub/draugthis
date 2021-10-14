using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Turn
    {
        public Stack<Move> Moves { get; set; } = new Stack<Move>();
        public Turn(Move move)
        {
            Moves.Push(move);
        }
        public void AddMove(Move move)
        {
            Moves.Push(move);
        }
    }
}
