﻿using System;
using System.Collections.Generic;

namespace Draughts
{
    public class AI
    {
        public string Color { get; set; }
        public string Enemy { get; set; }

        public AI(string color)
        {
            Color = color;
            Enemy = color == "white" ? "black" : "white";
        }

        public List<Pawn> GeneratePawnsToMove(Board board)
        {
            List<Pawn> possiblePawnsToMove = new List<Pawn>();
            foreach (Pawn pawn in board.Fields)
            {
                if (pawn is Pawn)
                {
                    bool statement1 = false;
                    bool statement2 = false;
                    bool statement3 = false;
                    bool statement4 = false;


                    if (pawn.Position.XPos + 2 < board.Fields.GetLength(0) &&
                        pawn.Position.YPos + 2 < board.Fields.GetLength(0) && Color == "black" &&
                        board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1] != null &&
                        board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1].Color == Enemy)
                    {
                        statement1 = board.Fields[pawn.Position.YPos + 2, pawn.Position.XPos + 2] == null;
                    }
                    else if (pawn.Position.XPos + 1 < board.Fields.GetLength(0) &&
                             pawn.Position.YPos + 1 < board.Fields.GetLength(0) && Color == "black")
                    {
                        statement1 = board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1] == null;
                    }

                    if (pawn.Position.XPos - 2 > 0 &&
                        pawn.Position.YPos + 2 < board.Fields.GetLength(0) && Color == "black" &&
                        board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1] != null &&
                        board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1].Color == Enemy)
                    {
                        statement2 = board.Fields[pawn.Position.YPos + 2, pawn.Position.XPos - 2] == null;
                    }
                    else if (pawn.Position.XPos - 1 >= 0 && pawn.Position.YPos + 1 < board.Fields.GetLength(0) &&
                             Color == "black")
                    {
                        statement2 = board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1] == null;
                    }

                    if (pawn.Position.XPos + 2 < board.Fields.GetLength(0) &&
                        pawn.Position.YPos - 2 > 0 && Color == "white" &&
                        board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1] != null &&
                        board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1].Color == Enemy)
                    {
                        statement3 = board.Fields[pawn.Position.YPos - 2, pawn.Position.XPos + 2] == null;
                    }
                    else if (pawn.Position.XPos + 1 < board.Fields.GetLength(0) && pawn.Position.YPos - 1 >= 0 &&
                             Color == "white")
                    {
                        statement3 = board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1] == null;
                    }

                    if (pawn.Position.XPos - 2 > 0 &&
                        pawn.Position.YPos - 2 > 0 && Color == "white" &&
                        board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1] != null &&
                        board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1].Color == Enemy)
                    {
                        statement4 = board.Fields[pawn.Position.YPos - 2, pawn.Position.XPos - 2] == null;
                    }
                    else if (pawn.Position.XPos - 1 >= 0 && pawn.Position.YPos - 1 >= 0 && Color == "white")
                    {
                        statement4 = board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1] == null;
                    }

                    if (pawn.Color == Color)
                    {
                        if (statement1 || statement2 || statement3 || statement4)
                        {
                            possiblePawnsToMove.Add(pawn);
                        }
                    }
                }
            }


            return possiblePawnsToMove;
        }

        public Pawn RandomPawn(List<Pawn> possiblePawnsToMove)
        {
            Random random = new Random();
            if (possiblePawnsToMove.Count != 0)
            {
                return possiblePawnsToMove[random.Next(possiblePawnsToMove.Count)];
            }
            else
            {
                return new Pawn(Color, new Coords(-1, -1));
            }
        }

        public (Coords, bool, Coords?) RandomMove(Board board, Pawn pawn)
        {
            List<(Coords, bool, Coords?)> possibleMoves = new List<(Coords, bool, Coords?)>();

            if (pawn is Pawn && pawn.Position.XPos + 1 < board.Fields.GetLength(0) &&
                pawn.Position.YPos + 1 < board.Fields.GetLength(0) && Color == "black")
            {
                if (board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos + 1, pawn.Position.XPos + 1), false, null));
                }
                else if (board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1] is Pawn &&
                         board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos + 1].Color != pawn.Color &&
                         pawn.Position.XPos + 2 < board.Fields.GetLength(0) &&
                         pawn.Position.YPos + 2 < board.Fields.GetLength(0) &&
                         board.Fields[pawn.Position.YPos + 2, pawn.Position.XPos + 2] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos + 2, pawn.Position.XPos + 2), true,
                        new Coords(pawn.Position.YPos + 1, pawn.Position.XPos + 1)));
                }
            }

            if (pawn is Pawn && pawn.Position.XPos - 1 >= 0 && pawn.Position.YPos + 1 < board.Fields.GetLength(0) &&
                Color == "black")
            {
                if (board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos + 1, pawn.Position.XPos - 1), false, null));
                }
                else if (board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1] is Pawn &&
                         board.Fields[pawn.Position.YPos + 1, pawn.Position.XPos - 1].Color != pawn.Color &&
                         pawn.Position.YPos + 2 < board.Fields.GetLength(0) && pawn.Position.XPos - 2 >= 0 &&
                         board.Fields[pawn.Position.YPos + 2, pawn.Position.XPos - 2] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos + 2, pawn.Position.XPos - 2), true,
                        new Coords(pawn.Position.YPos + 1, pawn.Position.XPos - 1)));
                }
            }

            if (pawn is Pawn && pawn.Position.XPos + 1 < board.Fields.GetLength(0) && pawn.Position.YPos - 1 >= 0 &&
                Color == "white")
            {
                if (board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos - 1, pawn.Position.XPos + 1), false, null));
                }
                else if (board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1] is Pawn &&
                         board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos + 1].Color != pawn.Color &&
                         pawn.Position.XPos + 2 < board.Fields.GetLength(0) && pawn.Position.YPos - 2 >= 0 &&
                         board.Fields[pawn.Position.YPos - 2, pawn.Position.XPos + 2] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos - 2, pawn.Position.XPos + 2), true,
                        new Coords(pawn.Position.YPos - 1, pawn.Position.XPos + 1)));
                }
            }

            if (pawn is Pawn && pawn.Position.XPos - 1 >= 0 && pawn.Position.YPos - 1 >= 0 && Color == "white")
            {
                if (board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos - 1, pawn.Position.XPos - 1), false, null));
                }
                else if (board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1] is Pawn &&
                         board.Fields[pawn.Position.YPos - 1, pawn.Position.XPos - 1].Color != pawn.Color &&
                         pawn.Position.XPos - 2 >= 0 && pawn.Position.YPos - 2 >= 0 &&
                         board.Fields[pawn.Position.YPos - 2, pawn.Position.XPos - 2] == null)
                {
                    possibleMoves.Add((new Coords(pawn.Position.YPos - 2, pawn.Position.XPos - 2), true,
                        new Coords(pawn.Position.YPos - 1, pawn.Position.XPos - 1)));
                }
            }

            Random random = new Random();


            if (possibleMoves.Count != 0)
            {
                return possibleMoves[random.Next(possibleMoves.Count)];
            }
            else
            {
                return (new Coords(-1, -1), false, null);
            }
        }


        public void AiMove(Board board)
        {
            Console.Clear();
            board.PrintBoard(Color);
            List<Pawn> listOfPawns = GeneratePawnsToMove(board);
            Pawn pawn = RandomPawn(listOfPawns);
            (Coords, bool, Coords?) randomMove = RandomMove(board, pawn);
            Coords endingCoords = randomMove.Item1;
            bool killed = randomMove.Item2;
            Coords? toDelete = randomMove.Item3;
            ConsoleKey key = ConsoleKey.A;

            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Backspace)
                {
                    board.Undo(board);
                    Console.Clear();
                    board.PrintBoard(Color);
                }
            }

            if (killed)
            {
                board.MovePawn(board, pawn.Position, endingCoords, board.Fields[toDelete.YPos, toDelete.XPos]);
            }
            else
            {
                board.MovePawn(board,  endingCoords, pawn.Position);
            }

            pawn.Position = endingCoords;
        }
    }
}