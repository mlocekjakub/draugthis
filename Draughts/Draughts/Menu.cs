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
                Console.WriteLine("Choose board size between 10 and 20");
                bool success = false;
                string output;
                while (!success)
                {
                    output = Console.ReadLine();
                    int intOutput;
                    if (int.TryParse(output, out intOutput) && intOutput > 9 && intOutput < 21)
                    {
                        board = new Board(int.Parse(output));
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
                            board.IsAIWhite = !board.IsAIWhite;
                            break;
                        case 2:
                            board.IsAIBlack = !board.IsAIBlack;
                            break;
                        case 3:
                            Pointer = 0;
                            board = RunMenu();
                            break;
                    }
                }
            }
            return board;
        }
        
        public void PrintMenu(int menu)
        {
            Console.WriteLine("hello kurwa");
            Console.WriteLine("Choose your kurwa option:"); // usuń kurwa
            if (menu == 0)
            {
                Console.WriteLine(Pointer == 0 ? "> Start <" : "  Start  ");
                Console.WriteLine(Pointer == 1 ? "> Exit <" : "  Exit  ");
            } else if (menu == 1)
            {
                Console.WriteLine(Pointer == 0 ? "> Start <" : "  Start  ");
                Console.Write(Pointer == 1 ? "> White <" : "  White  ");
                Console.WriteLine(board.IsAIWhite ? "  AI  " : "  Player  ");
                Console.Write(Pointer == 2 ? "> Black <" : "  Black  ");
                Console.WriteLine(board.IsAIBlack ? "  AI  " : "  Player  ");
                Console.WriteLine(Pointer == 3 ? "> Back <" : "  Back  ");
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
                    numberOfOptions = 4;
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