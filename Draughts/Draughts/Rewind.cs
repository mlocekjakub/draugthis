using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Rewind
    {
        public Stack<Turn> _turns = new Stack<Turn>();

        public void AddTurn(Move move)
        {
            _turns.Push(new Turn(move));
        }
        public Turn GetLastTurn()
        {
            return _turns.Pop();
        }

        public void AddMove(Move move)
        {
            _turns.Peek().AddMove(move);
        }

        public bool IsEmpty()
        {
            return _turns.Count == 0;
        }
    }
}
