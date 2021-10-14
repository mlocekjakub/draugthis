using System;

namespace Draughts
{
    public class Menu
    {
        private Board board { get; set; }
        
        private int Pointer { get; set; }
        public Menu()
        {
            Pointer = 0;
        }
        
        public Board RunMenu()
        {
            int option = SelectOption(0);
            if (option == 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Choose board size between 10 and 20");
                bool success = false;
                string output;
                while (!success)
                {
                    output = Console.ReadLine();
                    int intOutput;
                    if (int.TryParse(output, out intOutput) && intOutput > 9 && intOutput < 21)
                    {
                        board = new Board(intOutput);
                        success = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Choose board size between 10 and 20");
                    }
                }
                bool successed = false;
                while (!successed)
                {
                    option = SelectOption(1);
                    switch (option)
                    {
                        case 0:
                            successed = true;
                            break;
                        case 1:
                            board.IsAiWhite = !board.IsAiWhite;
                            break;
                        case 2:
                            board.IsAiBlack = !board.IsAiBlack;
                            break;
                        case 3:
                            Pointer = 0;
                            return BuildDemoMap();
                        case 4:
                            Pointer = 0;
                            board = RunMenu();
                            break;
                    }
                }
            }
            return board;
        }

        private Board BuildDemoMap()
        {
            var board = new Board(10);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board.Fields[i, j] = null;
                }
            }
            board.Fields[8, 3] = new Pawn("white", new Coords(8, 3));
            board.Fields[6, 3] = new Pawn("white", new Coords(6, 3));
            board.Fields[6, 3].IsCrowned = true;
            board.Fields[6, 5] = new Pawn("white", new Coords(6, 5));
            board.Fields[8, 5] = new Pawn("white", new Coords(8, 5));
            board.Fields[5, 4] = new Pawn("black", new Coords(5, 4));
            board.Fields[2, 1] = new Pawn("white", new Coords(2, 1));
            return board;
        }

        public void PrintMenu(int menu)
        {
            Console.WriteLine("Choose your option:");
            if (menu == 0)
            {
                Console.WriteLine(Pointer == 0 ? "> Start <" : "  Start  ");
                Console.WriteLine(Pointer == 1 ? "> Exit <" : "  Exit  ");
            } else if (menu == 1)
            {
                Console.WriteLine(Pointer == 0 ? "> Start <" : "  Start  ");
                Console.Write(Pointer == 1 ? "> White <" : "  White  ");
                Console.WriteLine(board.IsAiWhite ? "  AI  " : "  Player  ");
                Console.Write(Pointer == 2 ? "> Black <" : "  Black  ");
                Console.WriteLine(board.IsAiBlack ? "  AI  " : "  Player  ");
                Console.WriteLine(Pointer == 3 ? "> DEMO <" : "  DEMO  ");
                Console.WriteLine(Pointer == 4 ? "> Back <" : "  Back  ");
            }
            
        }

        public int SelectOption(int menuOption)
        {
            ConsoleKey key;
            int numberOfOptions = 0;
            switch (menuOption)
            {
                case 0:
                    numberOfOptions = 2;
                    break;
                case 1:
                    numberOfOptions = 5;
                    break;
            }
            while (true)
            {
                Console.Clear();
                PrintMenu(menuOption);
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (Pointer != 0)
                        {
                            Pointer--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Pointer != numberOfOptions-1)
                        {
                            Pointer++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        return Pointer;
                }
            }
        }
    }
}