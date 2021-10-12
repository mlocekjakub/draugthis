using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Board
    {
        Rewind _rewind;

        public bool IsAIWhite { get; set; }
        
        public bool IsAIBlack { get; set; }
        public Pawn[,] Fields { get; set; }

        private Coords Cursor { get; set; }

        public Board(int n)
        {
            _rewind = new Rewind();
            Fields = new Pawn[n, n];
            int boardSize = Fields.GetLength(1);
            Cursor = new Coords(boardSize - 1, 0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i < 4)
                    {
                        if ((i == 0 || i == 2) && j % 2 != 0)
                        {
                            Fields[i, j] = new Pawn("black");
                        }
                        else if ((i == 1 || i == 3) && j % 2 == 0)
                        {
                            Fields[i, j] = new Pawn("black");
                        }
                        else
                        {
                            Fields[i, j] = null;
                        }
                    }
                    else if (i > boardSize - 5)
                    {
                        if ((i == boardSize - 2 || i == boardSize - 4) && j % 2 != 0)
                        {
                            Fields[i, j] = new Pawn("white");
                        }
                        else if ((i == boardSize - 1 || i == boardSize - 3) && j % 2 == 0)
                        {
                            Fields[i, j] = new Pawn("white");
                        }
                        else
                        {
                            Fields[i, j] = null;
                        }
                    }
                    else
                    {
                        Fields[i, j] = null;
                    }
                }
            }
        }

        public void PrintBoarddupa()
        {
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if (i == Cursor.YPos && j == Cursor.XPos)
                    {
                        Console.Write("X");
                    }
                    else if (Fields[i, j] == null)
                    {
                        Console.Write("~");
                    }
                    else if (Fields[i, j].Color == "black")
                    {
                        Console.Write("B");
                    }
                    else if (i == Cursor.YPos && j == Cursor.XPos)
                    {
                        Console.Write("X");
                    }
                    else if (Fields[i, j].Highlight)
                    {
                        Console.Write("H");
                    }
                    else
                    {
                        Console.Write("W");
                    }
                }
                Console.Write("\n");
            }
        }

        public Coords SelectPosition()
        {
            ConsoleKeyInfo _Key;
            while (true)
            {
                Console.Clear();
                PrintBoard();
                _Key = Console.ReadKey();
                switch (_Key.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (Cursor.XPos + 2 < Fields.GetLength(0))
                        {
                            Cursor.XPos += 2;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Cursor.XPos - 2 >= 0)
                        {
                            Cursor.XPos -= 2;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (Cursor.YPos - 1 >= 0)
                        {
                            Cursor.YPos--;
                            if (Cursor.XPos - 1 >= 0)
                            {
                                Cursor.XPos--;
                            }
                            else
                            {
                                Cursor.XPos++;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Cursor.YPos + 1 < Fields.GetLength(0))
                        {
                            Cursor.YPos++;
                            if (Cursor.XPos + 1 < this.Fields.GetLength(0))
                            {
                                Cursor.XPos++;
                            }
                            else
                            {
                                Cursor.XPos--;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        return new Coords(Cursor.YPos, Cursor.XPos);
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.Backspace:
                        return new Coords(-10, -10);
                }

            }
        }

        public bool CheckIfPawnIsPresent(string color, Coords pos)
        {
            if (Fields[pos.YPos, pos.XPos] == null)
            {
                return false;
            }
            else if (Fields[pos.YPos, pos.XPos].Color == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MovePawn(Coords startingPos, Coords endingPos, string killedColor = "none")
        {
            _rewind.AddTurn(startingPos, endingPos, killedColor);
            Fields[endingPos.YPos, endingPos.XPos] = Fields[startingPos.YPos, startingPos.XPos];
            RemovePawn(startingPos);
            UnhighlightPawn(endingPos);
        }

        public void MoveBack(Coords startingPos, Coords endingPos)
        {
            Fields[endingPos.YPos, endingPos.XPos] = Fields[startingPos.YPos, startingPos.XPos];
            RemovePawn(startingPos);
        }

        public void RemovePawn(Coords Pos)
        {
            Fields[Pos.YPos, Pos.XPos] = null;
        }

        public void HighlightPawn(Coords Pos)

        {
            Fields[Pos.YPos, Pos.XPos].Highlight = true;
        }

        public void UnhighlightPawn(Coords Pos)
        {
            Fields[Pos.YPos, Pos.XPos].Highlight = false;
        }

        public void Undo()
        {
            if (!_rewind.IsEmpty())
            {
                var turn = _rewind.GetLastMove();
                MoveBack(turn.EndingPos, turn.StartingPos);
                if (turn.KilledColour != "none")
                {
                    var pos = turn.GetKilledPawnCoords();
                    Fields[pos.YPos, pos.XPos] = new Pawn(turn.KilledColour);
                }
            }
        }
        public void PrintBoard()
        {
            for (int i = 0; i < this.Fields.GetLength(0); i++)
            {
                for (int j = 0; j < this.Fields.GetLength(0); j++)
                {

                    if (i % 2 == 0 & j % 2 == 1 || i % 2 == 1 & j % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    if (i == this.Cursor.YPos & j == this.Cursor.XPos)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    if (this.Fields[i, j] != null)
                    {
                        if (this.Fields[i, j].Highlight)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }
                        Console.Write($" O ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}