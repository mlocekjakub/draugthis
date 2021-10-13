using System.Collections.Generic;

namespace Draughts
{
    public class AI
    {
        private List<Pawn> PossiblePawnsToMove;

        public AI(Board board, string color)
        {
            PossiblePawnsToMove = generatePawnsToMove(board, color);
        }
        
        public List<Pawn> generatePawnsToMove(Board board, string color)
        {
            List<Pawn> possiblePawnsToMove = new List<Pawn>();
            foreach (Pawn pawn in board.Fields)
            {
                bool statement1 = board.Fields[pawn.Position.XPos+1,pawn.Position.YPos]!= null;
                bool statement2 = board.Fields[pawn.Position.XPos-1,pawn.Position.YPos]!= null;
                bool statement3 = board.Fields[pawn.Position.XPos,pawn.Position.YPos+1]!= null;
                bool statement4 = board.Fields[pawn.Position.XPos,pawn.Position.YPos-1]!= null;
                
                if (pawn.Color == color)
                {
                    if (statement1 || statement2 || statement3 || statement4)
                    {
                        possiblePawnsToMove.Add(pawn);
                    }
                    
                }
            }

            return possiblePawnsToMove;
        }
    }
}