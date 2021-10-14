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

        public int AmountOfWhitePawns { get; set; }
        public int AmountOfBlackPawns { get; set; }

        public bool IsAiWhite { get; set; }

        public bool IsAiBlack { get; set; }
        public Pawn[,] Fields { get; set; }

        public Coords WhiteCursor { get; set; }

        public Coords BlackCursor { get; set; }

        public Board(int n)
        {
            AmountOfWhitePawns = n * 2;
            AmountOfBlackPawns = n * 2;
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
                            Fields[i, j] = new Pawn("black", new Coords(i, j));
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
                            Fields[i, j] = new Pawn("white", new Coords(i, j));
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

        public void MovePawn(Board board, Coords startingPos, Coords endingPos,  Pawn killedPawn = null, bool chainKill=false)
        {
            Console.WriteLine(board);
            Console.WriteLine($"spos {startingPos.XPos} {startingPos.YPos}");
            Console.WriteLine($"epos {endingPos.XPos} {endingPos.YPos}");
            Console.WriteLine(killedPawn);
            
            if (chainKill)
            {
                _rewind.AddMove(new Move(startingPos, endingPos, killedPawn));
            }
            else
            {
                _rewind.AddTurn(new Move(startingPos, endingPos, killedPawn));
            }

            Fields[endingPos.YPos, endingPos.XPos] = Fields[startingPos.YPos, startingPos.XPos];
            Fields[endingPos.YPos, endingPos.XPos].Position.YPos = endingPos.YPos;
            Fields[endingPos.YPos, endingPos.XPos].Position.XPos = endingPos.XPos;
            RemovePawn(startingPos);
            UnhighlightPawn(endingPos);
            if (killedPawn != null)
            {
                RemovePawn(new Coords(killedPawn.Position.YPos, killedPawn.Position.XPos));
            }
            if (board.Fields[endingPos.YPos, endingPos.XPos].Color == "white" && endingPos.YPos == 0 ||
                board.Fields[endingPos.YPos, endingPos.XPos].Color == "black" && endingPos.YPos == board.Fields.GetLength(0) - 1)
            {
                board.Fields[endingPos.YPos, endingPos.XPos].IsCrowned = true;
            }
        }

        public void MoveBack(Board board, Coords startingPos, Coords endingPos)
        {
            Fields[endingPos.YPos, endingPos.XPos] = Fields[startingPos.YPos, startingPos.XPos];
            if (board.Fields[startingPos.YPos, startingPos.XPos].Color == "white" && startingPos.YPos == 0 ||
                board.Fields[startingPos.YPos, startingPos.XPos].Color == "black" && startingPos.YPos == board.Fields.GetLength(0) - 1)
            {
                board.Fields[startingPos.YPos, startingPos.XPos].IsCrowned = false;
            }
            RemovePawn(startingPos);
        }

        private void RemovePawn(Coords Pos)
        {
            Fields[Pos.YPos, Pos.XPos] = null;
        }

        public void HighlightPawn(Coords Pos)

        {
            Fields[Pos.YPos, Pos.XPos].Highlight = true;
        }

        public void UnhighlightPawn(Coords Pos)
        {
            if (Fields[Pos.YPos, Pos.XPos] != null)
            {
                Fields[Pos.YPos, Pos.XPos].Highlight = false;
            }
        }

        public void Undo(Board board)
        {
            if (!_rewind.IsEmpty())
            {
                var turn = _rewind.GetLastTurn();
                while (turn.Moves.Count != 0)
                {
                    Move move = turn.Moves.Pop();
                    MoveBack(board, move.EndingPos, move.StartingPos);
                    if (move.KilledPawn != null)
                    {
                        var pawnToRestore = move.KilledPawn;
                        Fields[pawnToRestore.Position.YPos, pawnToRestore.Position.XPos] = pawnToRestore;
                    }
                }
            }
        }

        public void PrintBoard(Coords cursor = null)
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            char columnLetter = 'A';
            Console.Write("    ");
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                Console.Write($" {columnLetter++} ");
            }

            Console.WriteLine();
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                if (i + 1 < 10)
                {
                    Console.Write($"  {i + 1} ");
                }
                else
                {
                    Console.Write($" {i + 1} ");
                }

                for (int j = 0; j < Fields.GetLength(0); j++)
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

                    if (cursor != null && i == cursor.YPos & j == cursor.XPos)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                    }

                    if (Fields[i, j] != null)
                    {
                        if (Fields[i, j].Highlight)
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                        }

                        Console.ForegroundColor = Fields[i, j].FontColor;

                        Console.Write(Fields[i, j].IsCrowned ? " ♀ " : " ○ ");
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