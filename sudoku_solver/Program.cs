using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

using System.Diagnostics;
using Iced.Intel;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[,] sudoku = new int[9, 9];
        bool answer = false;
        Stopwatch sw = Stopwatch.StartNew();

        while (!answer)
        {
            Array.Clear(sudoku, 0, sudoku.Length);
            sprinkler(sudoku, random);
            answer = Solve(sudoku, answer);
            if (answer)
            {
                Console.WriteLine("Sudoku je vyřešeno.");
                PrintSudoku(sudoku);
            }
        }
        answer = false;

        bool end_game = false;
        int difficulty = 0;
        while (!end_game)
        {
            Console.WriteLine("Vítejte v sudoku!");
            Console.WriteLine();

            // choosing the difficulty
            while (difficulty == 0)
            {
                Console.WriteLine("Vyber si úroveň obtížnosti:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1. Nejsnadnější");
                Console.WriteLine("2. Snadná");
                Console.WriteLine("3. Střední");
                Console.WriteLine("4. Těžká");
                Console.WriteLine("5. Nejtěžší");
                Console.WriteLine("6. Vypnout hru");
                Console.WriteLine("7. zkoušení metody");
                Console.ResetColor();
                difficulty = int.Parse(Console.ReadLine());
            }
            switch (difficulty)
            {
                case 1:
                    Console.WriteLine("Vybrali jste si nejsnadnější obtížnost.");
                    break;
                case 2:
                    Console.WriteLine("Vybrali jste si snadnou obtížnost.");
                    break;
                case 3:
                    Console.WriteLine("Vybrali jste si střední obtížnost.");
                    break;
                case 4:
                    Console.WriteLine("Vybrali jste si těžkou obtížnost.");
                    break;
                case 5:
                    Console.WriteLine("Vybrali jste si nejtěžší obtížnost.");
                    break;
                case 6:
                    end_game = true;
                    break;
                case 7:
                    difficulty_1(sudoku, random, answer);
                    PrintSudoku(sudoku);
                    break;
                default:
                    Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                    break;
            }
            //
            Console.WriteLine();
        }


        // printing the sudoku
        static void PrintSudoku(int[,] sudoku)
        {
            Console.WriteLine("Sudoku:");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(sudoku[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }
        //

        static int number_of_full_numbers(int[,] sudoku)
        {
            int number_of_full_numbers = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != 0)
                    {
                        number_of_full_numbers++;
                    }
                }
            }
            return number_of_full_numbers;
        }




        // difficulty - 1 - nejjednodušší
        static void difficulty_1(int[,] sudoku, Random random, bool answer)
        {
            while (number_of_full_numbers(sudoku) > 50)
            {
                int random_y;
                int random_x;
                int random_number;

                for (int i = 1; i < 10; i++)
                {
                    random_y = random.Next(0, 9);
                    random_x = random.Next(0, 9);
                    random_number = sudoku[random_y, random_x];

                    sudoku[random_y, random_x] = i;
                    Solve(sudoku, answer);
                    if (!Solve(sudoku, answer))
                    {
                        sudoku[random_y, random_x] = 0;
                    }
                    else
                    {
                        sudoku[random_y, random_x] = random_number;
                        i--;
                    }
                }
            }
            //
        }









        // difficulty - 2 - snadná
        //
        // difficulty - 3 - střední
        //
        // difficulty - 4 - těžká
        //
        // difficulty - 5 - nejtěžší













        // MAKING THE SUDOKU
        // filling the sudoku with 11 random numbers
        static void sprinkler(int[,] sudoku, Random random)
        {

            int random_x;
            int random_y;
            int sprinkler_random_number;
            sudoku[0, 0] = random.Next(1, 10);
            for (int i = 0; i < 10; i++)
            {

                sprinkler_random_number = random.Next(1, 10);
                random_y = random.Next(0, 9);
                random_x = random.Next(0, 9);
                if (IsValidPlacement(random_y, random_x, sprinkler_random_number, sudoku) && sudoku[random_y, random_x] == 0)
                {
                    sudoku[random_y, random_x] = sprinkler_random_number;
                }
                else
                {
                    i--;
                }
            }
        }
        //

        // filling sudoku
        static bool Solve(int[,] sudoku, bool answer)
        {
            for (int start_r = 0; start_r < 9; start_r++)
            {
                for (int start_c = 0; start_c < 9; start_c++)
                {
                    if (sudoku[start_r, start_c] == 0)
                    {
                        for (int num = 1; num < 10; num++)
                        {
                            if (IsValidPlacement(start_r, start_c, num, sudoku))
                            {
                                sudoku[start_r, start_c] = num;

                                if (Solve(sudoku, answer)) return true;
                                else
                                {
                                    sudoku[start_r, start_c] = 0;
                                }
                            }
                        }
                        return false;

                    }
                }
            }
            return true;

        }
        //

        // validating the chosen number
        static bool IsValidPlacement(int startRow, int startCol, int random_number, int[,] sudoku)
        {
            bool isValid = true;

            for (int i = 0; i < 9; i++)
            {
                if (sudoku[startRow, i] == random_number)
                {
                    isValid = false;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[i, startCol] == random_number)
                {
                    isValid = false;
                }
            }

            int row_grid = startRow / 3 * 3;
            int col_grid = startCol / 3 * 3;

            for (int i = row_grid; i < row_grid + 3; i++)
            {
                for (int j = col_grid; j < col_grid + 3; j++)
                {
                    if (sudoku[i, j] == random_number)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        //
        // ###

        // printing the time
        sw.Stop();
        Console.WriteLine("Time taken: " + sw.ElapsedMilliseconds + " ms");
    }
}