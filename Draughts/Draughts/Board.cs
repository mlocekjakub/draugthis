using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Board
    {
        // public Pawn[,] Fields { get; set; }

        public string[,] Fields { get; set; }

        public (int, int) Cursor;
        public Board(int n)
        {
            Cursor = (0, 1);
            Fields = new string[n, n];
            // Fields = new Pawn[n, n];
            
        }

        public void InitBoard()
        {
            int numberOfRowsWithPawns = 4;
            int numberOfRowsWithoutPawns = this.Fields.GetLength(0) - numberOfRowsWithPawns * 2;
            
            
            for (int i = 0; i < numberOfRowsWithPawns; i++)
            {
                for (int j = 0; j < this.Fields.GetLength(0); j++)
                {

                    if (i % 2 == 0 & j % 2 == 1 || i % 2 == 1 & j % 2 == 0)
                    {
                        this.Fields[i, j] = "W";
                    }
                    else
                    {
                        this.Fields[i, j] = null;
                    }
                }
            }
            for (int i = numberOfRowsWithPawns; i < numberOfRowsWithPawns+numberOfRowsWithoutPawns; i++)
            {
                for (int j = 0; j < this.Fields.GetLength(0); j++)
                {
                    this.Fields[i, j] = null;
                }
            }
            for (int i = numberOfRowsWithPawns+numberOfRowsWithoutPawns; i < this.Fields.GetLength(0); i++)
            {
                for (int j = 0; j < this.Fields.GetLength(0); j++)
                {

                    if (i % 2 == 0 & j % 2 == 1 || i % 2 == 1 & j % 2 == 0)
                    {
                        this.Fields[i, j] = "B";
                    }
                    else
                    {
                        this.Fields[i, j] = null;
                    }
                }
            }
        }

        public void PrintBoard()
        {
            
            for (int i = 0; i < this.Fields.GetLength(0); i++) {
                for (int j = 0; j < this.Fields.GetLength(0); j++) {
                    
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

                    if (i == this.Cursor.Item1 & j == this.Cursor.Item2)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    } 
                    if (this.Fields[i, j] != null)
                    {
                        Console.Write($" {this.Fields[i, j]} ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                }

                Console.WriteLine();
            }
        }

        public (int, int) choosingPawn()
        {
            ConsoleColor backgroundColor = Console.BackgroundColor;
            ConsoleColor foregroundColor = Console.ForegroundColor;
            ConsoleKey key = ConsoleKey.Backspace;
            while (key != ConsoleKey.Enter)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.Clear();
                PrintBoard();
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        if (Cursor.Item2 - 2 >= 0)
                        {
                            Cursor.Item2 -= 2;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Cursor.Item2 + 2 < this.Fields.GetLength(0))
                        {
                            Cursor.Item2 += 2;
                        }

                        break;
                    case ConsoleKey.UpArrow:
                        if (Cursor.Item1 - 1 >= 0)
                        {
                            Cursor.Item1--;
                            if (Cursor.Item2 - 1 >= 0)
                            {
                                Cursor.Item2--;
                            }
                            else
                            {
                                Cursor.Item2++;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Cursor.Item1 + 1 < this.Fields.GetLength(0))
                        {
                            Cursor.Item1++;
                            if (Cursor.Item2 + 1 < this.Fields.GetLength(0))
                            {
                                Cursor.Item2++;
                            }
                            else
                            {
                                Cursor.Item2--;
                            }
                        }

                        break;
                }
            }

            return Cursor;
        }
    }
}