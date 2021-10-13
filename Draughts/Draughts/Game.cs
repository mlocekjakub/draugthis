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
                MakeAMove(board, "black");
            }
        }

        private void MakeAMove(Board board, string color)
        {
            Coords startingPos = new Coords(-1, -1);
            Coords endingPos;
            bool success = false;
            bool undo = false;
            string enemy = color == "white" ? "black" : "white";
            while (!success)
            {
                startingPos = color == "white" ? board.SelectPosition(board.WhiteCursor) : board.SelectPosition(board.BlackCursor);
                if (startingPos == null)
                {
                    continue;
                }
                else if (startingPos.XPos == -10)
                {
                    board.Undo();
                    undo = true;
                    break;
                }
                else if (board.CheckIfPawnIsPresent(color, startingPos))
                {
                    board.HighlightPawn(startingPos);
                    success = true;
                }
            }
            success = false;
            while (!success && !undo)
            {
                endingPos = color == "white" ? board.SelectPosition(board.WhiteCursor) : board.SelectPosition(board.BlackCursor);
                if (endingPos == null)
                {
                    board.UnhighlightPawn(startingPos);
                    MakeAMove(board, color);
                    break;
                }
                else if (endingPos.XPos == -10)
                {
                    board.UnhighlightPawn(startingPos);
                    board.Undo();
                    break;
                }
                if (board.Fields[endingPos.YPos, endingPos.XPos] == null)
                {
                    success = ValidateMove(board, startingPos, endingPos, success, enemy);
                }
            }
        }

        private bool ValidateMove(Board board, Coords startingPos, Coords endingPos, bool success, string enemy)
        {
            if (enemy == "black")
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
                    bool killed = true;
                    while (killed)
                    {
                        CheckForChainKill(board, ref endingPos, enemy, ref killed);
                    }
                    success = true;
                }
            }
            else
            {
                if (endingPos.YPos == startingPos.YPos + 1 && Math.Abs(endingPos.XPos - startingPos.XPos) == 1)
                {
                    board.MovePawn(startingPos, endingPos);
                    success = true;
                }
                else if ((endingPos.YPos == startingPos.YPos + 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2] is Pawn &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2].Color == enemy)
                {
                    Coords toRemove = new Coords((startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2);
                    board.RemovePawn(toRemove);
                    board.MovePawn(startingPos, endingPos, enemy);
                    bool killed = true;
                    while (killed)
                    {
                        CheckForChainKill(board, ref endingPos, enemy, ref killed);
                    }
                    success = true;
                }
            }

            return success;
        }

        private void CheckForChainKill(Board board, ref Coords endingPos, string enemy, ref bool killed)
        {
            bool statement1 = endingPos.YPos - 2 >= 0 && endingPos.XPos - 2 >= 0 && board.Fields[endingPos.YPos - 1, endingPos.XPos - 1] is Pawn &&
                board.Fields[endingPos.YPos - 1, endingPos.XPos - 1].Color == enemy &&
                board.Fields[endingPos.YPos - 2, endingPos.XPos - 2] == null;
            bool statement2 = endingPos.XPos + 2 < board.Fields.GetLength(0) && endingPos.YPos - 2 >= 0 && board.Fields[endingPos.YPos - 1, endingPos.XPos + 1] is Pawn &&
                board.Fields[endingPos.YPos - 1, endingPos.XPos + 1].Color == enemy &&
                board.Fields[endingPos.YPos - 2, endingPos.XPos + 2] == null;
            bool statement3 = endingPos.XPos + 2 < board.Fields.GetLength(0) && endingPos.YPos + 2 < board.Fields.GetLength(0) && board.Fields[endingPos.YPos + 1, endingPos.XPos + 1] is Pawn &&
                board.Fields[endingPos.YPos + 1, endingPos.XPos + 1].Color == enemy &&
                board.Fields[endingPos.YPos + 2, endingPos.XPos + 2] == null;
            bool statement4 = endingPos.XPos - 2 >= 0 && endingPos.YPos + 2 < board.Fields.GetLength(0) && board.Fields[endingPos.YPos + 1, endingPos.XPos - 1] is Pawn &&
                board.Fields[endingPos.YPos + 1, endingPos.XPos - 1].Color == enemy &&
                board.Fields[endingPos.YPos + 2, endingPos.XPos - 2] == null;
            if (statement1 || statement2 || statement3 || statement4)
            {
                endingPos = ChainKill(board, endingPos, enemy);
            }
            else
            {
                killed = false;
            }
        }

        private Coords ChainKill(Board board, Coords startingPos, string enemy)
        {
            Coords cursor = enemy == "white" ? board.BlackCursor : board.WhiteCursor;
            board.HighlightPawn(startingPos);
            Coords endingPos;
            while (true)
            {
                endingPos = board.SelectPosition(cursor);
                if (endingPos == null)
                {
                    continue;
                }
                if (((endingPos.YPos == startingPos.YPos - 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) ||
                     (endingPos.YPos == startingPos.YPos + 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) &&
                      board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2] is Pawn &&
                      board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2].Color == enemy))
                {
                    Coords toRemove = new Coords((startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2);
                    board.RemovePawn(toRemove);
                    board.MovePawn(startingPos, endingPos, enemy, chainKill:true);
                    break;
                }
            }
            return endingPos;
        }
    }
}
