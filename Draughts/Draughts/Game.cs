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
        public string Winner { get; set; } = "tie";
        public void Start(Board board)
        {
            while (!CheckForWinnersOrTie(board))
            {
                MakeAMove(board, "white");
                MakeAMove(board, "black");
            }
            Console.WriteLine(Winner != "tie" ? $"{Winner} wins!!!" : $"There's a {Winner}!!!");
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
                    board.Undo(board);
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
                    board.Undo(board);
                    break;
                }
                if (board.Fields[endingPos.YPos, endingPos.XPos] == null)
                {
                    if (board.Fields[startingPos.YPos, startingPos.XPos].IsCrowned)
                    {
                        success = ValidateQueenMove(board, startingPos, endingPos, success, enemy);
                    }
                    else
                    {
                        success = ValidatePawnMove(board, startingPos, endingPos, success, enemy);
                    }
                }
            }
        }

        private bool ValidateQueenMove(Board board, Coords startingPos, Coords endingPos, bool success, string enemy)
        {
            if (Math.Abs(startingPos.YPos - endingPos.YPos) == Math.Abs(startingPos.XPos - endingPos.XPos))
            {
                List<Coords> fieldsInBetween = GetAllCoordsInBetween(startingPos, endingPos);
                bool simpleMove = true;
                foreach (Coords field in fieldsInBetween)
                {
                    if (board.Fields[field.YPos, field.XPos] is Pawn)
                    {
                        simpleMove = false;
                    }
                }
                if (simpleMove)
                {
                    board.MovePawn(board, startingPos, endingPos);
                    return true;
                }
                Coords lastElement = fieldsInBetween[fieldsInBetween.Count - 1];
                if (board.Fields[lastElement.YPos, lastElement.XPos] is Pawn)
                {
                    fieldsInBetween.Remove(lastElement);
                    bool kill = true;
                    foreach (Coords field in fieldsInBetween)
                    {
                        if (board.Fields[field.YPos, field.XPos] is Pawn)
                        {
                            kill = false;
                        }
                    }
                    if (kill)
                    {
                        board.MovePawn(board, startingPos, endingPos, board.Fields[lastElement.YPos, lastElement.XPos]);
                        success = true;
                    }
                }
            }
            return success;
        }

        public List<Coords> GetAllCoordsInBetween(Coords startingPos, Coords endingPos)
        {
            List<Coords> output = new List<Coords>();
            if (startingPos.YPos < endingPos.YPos && startingPos.XPos < endingPos.XPos)
            {
                int y = startingPos.YPos + 1;
                int x = startingPos.XPos + 1;
                while (y != endingPos.YPos)
                {
                    output.Add(new Coords(y, x));
                    y++;
                    x++;
                }

                return output;
            }
            else if (startingPos.YPos > endingPos.YPos && startingPos.XPos > endingPos.XPos)
            {
                int y = startingPos.YPos - 1;
                int x = startingPos.XPos - 1;
                while (y != endingPos.YPos)
                {
                    output.Add(new Coords(y, x));
                    y--;
                    x--;
                }

                return output;
            }
            else if (startingPos.YPos < endingPos.YPos && startingPos.XPos > endingPos.XPos)
            {
                int y = startingPos.YPos + 1;
                int x = startingPos.XPos - 1;
                while (y != endingPos.YPos)
                {
                    output.Add(new Coords(y, x));
                    y++;
                    x--;
                }

                return output;
            }
            else if (startingPos.YPos > endingPos.YPos && startingPos.XPos < endingPos.XPos)
            {
                int y = startingPos.YPos - 1;
                int x = startingPos.XPos + 1;
                while (y != endingPos.YPos)
                {
                    output.Add(new Coords(y, x));
                    y--;
                    x++;
                }

                return output;
            }

            return output;
        }

        private bool ValidatePawnMove(Board board, Coords startingPos, Coords endingPos, bool success, string enemy)
        {
            if (enemy == "black")
            {
                if (endingPos.YPos == startingPos.YPos - 1 && Math.Abs(endingPos.XPos - startingPos.XPos) == 1)
                {
                    board.MovePawn(board, startingPos, endingPos);
                    success = true;
                }
                else if ((endingPos.YPos == startingPos.YPos - 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2] is Pawn &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2].Color == enemy)
                {
                    Coords toRemove = new Coords((startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2);
                    board.MovePawn(board, startingPos, endingPos, board.Fields[toRemove.YPos, toRemove.XPos]);
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
                    board.MovePawn(board, startingPos, endingPos);
                    success = true;
                }
                else if ((endingPos.YPos == startingPos.YPos + 2 && Math.Abs(endingPos.XPos - startingPos.XPos) == 2) &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2] is Pawn &&
                          board.Fields[(startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2].Color == enemy)
                {
                    Coords toRemove = new Coords((startingPos.YPos + endingPos.YPos) / 2, (startingPos.XPos + endingPos.XPos) / 2);
                    board.MovePawn(board, startingPos, endingPos, board.Fields[toRemove.YPos, toRemove.XPos]);
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
                board.Fields[endingPos.YPos, endingPos.XPos].IsCrowned = false;
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
                    board.MovePawn(board, startingPos, endingPos, board.Fields[toRemove.YPos, toRemove.XPos], chainKill:true);
                    break;
                }
            }
            return endingPos;
        }

        public bool CheckForWinnersOrTie(Board board)
        {
            bool isThereWinOrTie = false;

            if (board.amountOfWhitePawns == 0 || board.amountOfBlackPawns == 0)
            {
                isThereWinOrTie = true;
                Winner = board.amountOfWhitePawns == 0 ? "black" : "white";
            } else if (board.amountOfWhitePawns == 1 && board.amountOfBlackPawns == 1)
            {
                int counter = 0;
                foreach (Pawn boardField in board.Fields)
                {
                    if (boardField is Pawn && boardField.IsCrowned)
                    {
                        counter++;
                    }
                }
                isThereWinOrTie = counter == 2 ? true : false;
            }

            return isThereWinOrTie;
        }
    }
}
