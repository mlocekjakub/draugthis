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
        
        public int amountOfWhitePawns { get; set; }
        public int amountOfBlackPawns { get; set; }

        public bool IsAIWhite { get; set; }
        public bool IsAIBlack { get; set; }
        public Pawn[,] Fields { get; set; }

        public Coords WhiteCursor { get; set; }

        public Coords BlackCursor { get; set; }

        public Board(int n)
        {
            amountOfWhitePawns = n * 2;
            amountOfBlackPawns = n * 2;
            _rewind = new Rewind();
            Fields = new Pawn[n, n];
            BoardInit();
        }

        public void BoardInit()
        {
            int boardSize = Fields.GetLength(1);
            if (boardSize % 2 == 0)
            {
                WhiteCursor = new Coords(boardSize - 1, 0);
                BlackCursor = new Coords(0, boardSize - 1);
            }
            else
            {
                WhiteCursor = new Coords(boardSize - 1, 1);
                BlackCursor = new Coords(0, boardSize - 2);
            }
            

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (i < 4)
                    {
                        if (i % 2 == 0 & j % 2 == 1 || i % 2 == 1 & j % 2 == 0)
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
                        if (i % 2 == 0 & j % 2 == 1 || i % 2 == 1 & j % 2 == 0)
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

        public Coords SelectPosition(Coords cursor)
        {
            ConsoleKeyInfo _Key;
            while (true)
            {
                Console.Clear();
                PrintBoard(cursor);
                _Key = Console.ReadKey();
                switch (_Key.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (cursor.XPos + 2 < Fields.GetLength(0))
                        {
                            cursor.XPos += 2;
                        }

                        break;
                    case ConsoleKey.LeftArrow:
                        if (cursor.XPos - 2 >= 0)
                        {
                            cursor.XPos -= 2;
                        }

                        break;
                    case ConsoleKey.UpArrow:
                        if (cursor.YPos - 1 >= 0)
                        {
                            cursor.YPos--;
                            if (cursor.XPos - 1 >= 0)
                            {
                                cursor.XPos--;
                            }
                            else
                            {
                                cursor.XPos++;
                            }
                        }

                        break;
                    case ConsoleKey.DownArrow:
                        if (cursor.YPos + 1 < Fields.GetLength(0))
                        {
                            cursor.YPos++;
                            if (cursor.XPos + 1 < this.Fields.GetLength(0))
                            {
                                cursor.XPos++;
                            }
                            else
                            {
                                cursor.XPos--;
                            }
                        }

                        break;
                    case ConsoleKey.Enter:
                        return new Coords(cursor.YPos, cursor.XPos);
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

        public void MovePawn(Coords startingPos, Coords endingPos, string killedColor = "none", bool chainKill=false)
        {
            if (chainKill)
            {
                _rewind.AddMove(new Move(startingPos, endingPos, killedColor));
            }
            else
            {
                _rewind.AddTurn(new Move(startingPos, endingPos, killedColor));
            }
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
                var turn = _rewind.GetLastTurn();
                while (turn.Moves.Count != 0)
                {
                    Move move = turn.Moves.Pop();
                    MoveBack(move.EndingPos, move.StartingPos);
                    if (move.KilledColour != "none")
                    {
                        var pos = move.GetKilledPawnCoords();
                        Fields[pos.YPos, pos.XPos] = new Pawn(move.KilledColour);
                    }
                }
            }
        }


        public void PrintBoard(Coords cursor)
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            char columnLetter= 'A';
            Console.Write("    ");
            for (int i = 0; i < this.Fields.GetLength(0); i++)
            {
                Console.Write($" {columnLetter++} ");
            }
            Console.WriteLine();
            for (int i = 0; i < this.Fields.GetLength(0); i++)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                if (i+1 < 10)
                {
                    Console.Write($"  {i+1} ");
                }
                else
                {
                    Console.Write($" {i+1} ");
                }

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

                    if (i == cursor.YPos & j == cursor.XPos)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                    }

                    if (this.Fields[i, j] != null)
                    {
                        if (this.Fields[i, j].Highlight)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }

                        Console.ForegroundColor = this.Fields[i, j].FontColor;
                        
                        Console.Write(this.Fields[i, j].isCrowned?" ♀ ":" ○ ");
                        
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }

                Console.WriteLine();
            }

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }
    }
}