using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Game
    {

        public void Start(Board board)
        {

            while (true)
            {
                MakeAMove(board, "white");
            }
        }

        private void MakeAMove(Board board, string color)
        {
            Coords startingPos = new Coords(-1, -1);
            Coords endingPos;
            bool success = false;
            string enemy = (color == "white") ? "black" : "white";
            while (!success)
            {
                startingPos = board.SelectPosition();
                if (startingPos == null)
                {
                    continue;
                }
                else if (startingPos.XPos == -10)
                {
                    board.Undo();
                }
                else if (board.CheckIfPawnIsPresent(color, startingPos))
                {
                    board.HighlightPawn(startingPos);
                    success = true;
                }
            }
            success = false;
            while (!success)
            {
                endingPos = board.SelectPosition();
                if (endingPos == null)
                {
                    board.UnhighlightPawn(startingPos);
                    success = true;
                    break;
                }
                else if (endingPos.XPos == -10)
                {
                    board.UnhighlightPawn(startingPos);
                    board.Undo();
                    success = true;
                    break;
                }
                if (board.Fields[endingPos.YPos, endingPos.XPos] == null)
                {
                    if (endingPos.YPos == startingPos.YPos - 1 && Math.Abs(endingPos.XPos - startingPos.XPos) == 1)
                    {
                        board.MovePawn(startingPos, endingPos);
                        success = true;
                    }
                    else if ((endingPos.YPos == startingPos.YPos - 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) &&
                              board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2] is Pawn &&
                              board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2].Color == enemy)
                    {
                        Coords toRemove = new Coords((startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2);
                        board.RemovePawn(toRemove);
                        board.MovePawn(startingPos, endingPos, enemy);
                        success = true;
                    }
                }
            }
        }
    }
}
