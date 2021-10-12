using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Rewind
    {
        private Stack<Turn> _turns = new Stack<Turn>();

        public void AddTurn(Coords startingPos, Coords endingPos, string killedColor)
        {
            _turns.Push(new Turn(startingPos, endingPos, killedColor));
        }
        public Turn GetLastMove()
        {
            return _turns.Pop();
        }

        public bool IsEmpty()
        {
            return _turns.Count == 0;
        }
    }
}
