using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[,] sudoku = new int[9, 9];
        bool answer = false;
        Stopwatch sw = Stopwatch.StartNew();

        


        static int[,] CopySudoku(int[,] original)
        {
            int[,] copy = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    copy[i, j] = original[i, j];
            return copy;
        }
        static bool ContinueToPlay()
        {
            Console.WriteLine("Chceš pokračovat ve hře? (A/N)");
            string answer = Console.ReadLine().ToUpper();

            if (answer == "A")
            {
                return true;
            }
            else if (answer == "N")
            {
                Console.WriteLine("Děkujeme za hraní!");
                return false;
            }
            else if (answer is null)
            {
                Console.WriteLine("Neplatná volba. Zadej 'A' pro pokračování nebo 'N' pro ukončení.");
                return ContinueToPlay();
            }
            else
            {
                Console.WriteLine("Neplatná volba. Zadej 'A' pro pokračování nebo 'N' pro ukončení.");
                return ContinueToPlay();
            }
        }


        int[,] sudoku_for_user = new int[9, 9];

        bool end_game = false;
        int difficulty;
        bool hra = true;

        int[,] sudoku_for_user_starter = new int[9, 9];
        //MAIN GAME LOOP
        while (!end_game)
        {
            Console.WriteLine("Vítej v sudoku!");
            Console.WriteLine();

            difficulty = 0;
            // choosing the difficulty
            while (difficulty == 0)
            {
                while (!answer)
                {
                    Array.Clear(sudoku, 0, sudoku.Length);
                    sprinkler(sudoku, random);
                    answer = Solve(sudoku, answer);
                }
                answer = false;

                sudoku_for_user = CopySudoku(sudoku);

                Console.WriteLine("Vyber si úroveň obtížnosti:");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("1. Nejsnadnější");
                Console.WriteLine("2. Snadná");
                Console.WriteLine("3. Střední");
                Console.WriteLine("4. Těžká");
                Console.WriteLine("5. Nejtěžší");
                Console.WriteLine("6. Vypnout hru");
                Console.WriteLine("7. Pravidla hry");
                Console.ResetColor();
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out difficulty)) break;
                    else
                    {
                        Console.WriteLine("Neplatný vstup! Zadej prosím číslo od 1-6.");
                    }
                }
            }
            switch (difficulty)
            {
                case 1:
                    Console.WriteLine("Vybral sis nejsnadnější obtížnost.");
                    difficulty_1(sudoku_for_user, random);
                    hra = true;
                    break;
                case 2:
                    Console.WriteLine("Vybral sis snadnou obtížnost.");
                    difficulty_2(sudoku_for_user, random);
                    hra = true;
                    break;
                case 3:
                    Console.WriteLine("Vybrali sis střední obtížnost.");
                    difficulty_3(sudoku_for_user);
                    hra = true;
                    break;
                case 4:
                    Console.WriteLine("Vybrali sis těžkou obtížnost.");
                    difficulty_4(sudoku_for_user);
                    hra = true;
                    break;
                case 5:
                    Console.WriteLine("Vybrali sis nejtěžší obtížnost.");
                    difficulty_5(sudoku_for_user);
                    hra = true;
                    break;
                case 6:
                    hra = false;
                    end_game = true;
                    break;
                default:
                    Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                    break;
            }

            hra = true;

            sudoku_for_user_starter = CopySudoku(sudoku_for_user);

            Console.WriteLine();
            //

            while (hra)
            {
                PrintSudokuUser(sudoku, sudoku_for_user, sudoku_for_user_starter);

                if (number_of_full_numbers(sudoku_for_user) == 81 && FullSudoku(sudoku, sudoku_for_user))
                {
                    Console.WriteLine("Gratulujeme! Vyřešil jsi sudoku!");
                    hra = false;
                    end_game = ContinueToPlay();
                    break;
                }

                Console.WriteLine("Zadej řádek (1-9): ");
                int row;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out row) && row >= 1 && row <= 9 || row == 20) break;
                    else
                    {
                        Console.WriteLine("Neplatný vstup! Zadej prosím řádek jako číslo od 1-9.");
                    }
                }
                row--;

                Console.WriteLine("Zadej sloupec (1-9): ");
                int col;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out col) && col >= 1 && col <= 9) break;
                    else
                    {
                        Console.WriteLine("Neplatný vstup! Zadej prosím sloupec jako číslo od 1-9.");
                    }
                }
                col--;

                if (row != 19 && sudoku_for_user[row, col] == sudoku[row, col] && sudoku_for_user[row, col] != 0)
                {
                    Console.WriteLine("Toto pole již obsahuje číslo. Zkus to znovu.");
                    continue;
                }

                Console.WriteLine("Zadej číslo (1-9): ");
                int number;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out number) && number >= 1 && number <= 9) break;
                    else
                    {
                        Console.WriteLine("Neplatný vstup! Zadej prosím číslo buňky od 1-9.");
                    }
                }
                if (row != 19) sudoku_for_user[row, col] = number;
                else
                {
                    Solve(sudoku_for_user, false);
                }
            }
        }



        static bool FullSudoku(int[,] sudoku, int[,] sudoku_for_user)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != sudoku_for_user[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }




        // PRINTING THE SUDOKU
        // printing the sudoku for the user
        static void PrintSudokuUser(int[,] sudoku, int[,] sudoku_for_user, int[,] sudoku_for_user_starter)
        {
            Console.WriteLine("Sudoku:");

            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    Console.WriteLine("------+-------+------");
                }

                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        Console.Write("| ");
                    }

                    int number = sudoku_for_user[i, j];

                    if (number == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(". ");
                        Console.ResetColor();
                    }
                    else if (sudoku_for_user_starter[i, j] == 0 && sudoku[i, j] == number)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(number + " ");
                        Console.ResetColor();
                    }
                    else if (number == sudoku[i, j]) Console.Write(number + " ");
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(number + " ");
                        Console.ResetColor();
                    }

                }
                Console.WriteLine();
            }
        }
        //

        // printing the sudoku
        static void PrintSudoku(int[,] sudoku)
        {
            Console.WriteLine("Sudoku:");

            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                {
                    Console.WriteLine("------+-------+------");
                }
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        Console.Write("| ");
                    }

                    int number = sudoku[i, j];
                    Console.Write(number == 0 ? ". " : number + " ");
                }
                Console.WriteLine();
            }
        }
        //
        // ###

        // DIFFICULTY ALGORITHMS
        // difficulty - 1 - nejjednodušší
        static void difficulty_1(int[,] sudoku, Random random)
        {
            int i = 0;
            while (i < 30 || number_of_full_numbers(sudoku) > 61)
            {
                i++;

                int random_x = random.Next(0, 9);
                int random_y = random.Next(0, 9);
                int original_number = sudoku[random_x, random_y];

                if (original_number == 0)
                {
                    i--;
                    continue;
                }

                sudoku[random_x, random_y] = 0;
                int[,] copy_sudoku_temp = CopySudoku(sudoku);
                validRow_Col(random_x, random_y, sudoku, out int count_row, out int count_col);

                if (CountSolutions(copy_sudoku_temp) == 1)
                {
                    if (count_row < 5 || count_col < 5)
                    {
                        sudoku[random_x, random_y] = original_number;
                        i--;
                    }

                }
                else
                {
                    sudoku[random_x, random_y] = original_number;
                }
            }

        }
        //

        // difficulty - 2 - snadná
        static void difficulty_2(int[,] sudoku, Random random)
        {
            int i = 0;
            while (i < 45 || number_of_full_numbers(sudoku) > 49)
            {
                i++;
                
                int random_x = random.Next(0, 9);
                int random_y = random.Next(0, 9);
                int original_number = sudoku[random_x, random_y];

                if (sudoku[random_x, random_y] == 0)
                {
                    i--;
                    continue;
                }

                sudoku[random_x, random_y] = 0;
                int[,] copy_sudoku_temp = CopySudoku(sudoku);
                validRow_Col(random_x, random_y, sudoku, out int count_row, out int count_col);

                if (CountSolutions(copy_sudoku_temp) == 1)
                {
                    if (count_row < 4 || count_col < 4)
                    {
                        sudoku[random_x, random_y] = original_number;
                        i--;
                    }

                }
                else
                {
                    sudoku[random_x, random_y] = original_number;
                }
            }
        }
        //

        // difficulty - 3 - střední
        static void difficulty_3(int[,] sudoku)
        {
            bool second_going_through = false;
            int i = 0;
            int original_number;
            
            while (i < 9 || second_going_through == true && i < 9)
            {
                if (i % 2 == 0 || second_going_through == true && i % 2 != 0)
                {
                    for (int j = 8; j >= 0; j--)
                    {
                        if (j % 2 == 0)
                        {
                            original_number = sudoku[i, j];
                            sudoku[i, j] = 0;

                            validRow_Col(i, j, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = CopySudoku(sudoku);
                            if (CountSolutions(sudoku_copy_temp) == 1)
                            {
                                if (count_row < 3 || count_col < 3)
                                {
                                    sudoku[i, j] = original_number;
                                }
                            }
                            else
                            {
                                sudoku[i, j] = original_number;
                            }
                        }
                    }
                }
                else if (i % 2 != 0 || second_going_through == true && i % 2 == 0)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (k % 2 != 0)
                        {
                            original_number = sudoku[i, k];
                            sudoku[i, k] = 0;

                            validRow_Col(i, k, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = CopySudoku(sudoku);
                            if (CountSolutions(sudoku_copy_temp) == 1)
                            {
                                if (count_row < 3 || count_col < 3)
                                {
                                    sudoku[i, k] = original_number;
                                }
                            }
                            else
                            {
                                sudoku[i, k] = original_number;
                            }
                        }
                    }
                }
                i++;
                if (i == 9 && second_going_through == false)
                {
                    second_going_through = true;
                    i = 0;
                }
            }
            Console.WriteLine("Počet čísel: " + number_of_full_numbers(sudoku));
        }
        //

        // difficulty - 4 - těžká
        static void difficulty_4(int[,] sudoku)
        {
            int original_number;
            for (int i = 0; i < 9; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 8; j >= 0; j--)
                    {
                        original_number = sudoku[i, j];
                        sudoku[i, j] = 0;
                        int[,] sudoku_copy_temp = CopySudoku(sudoku);
                        validRow_Col(i, j, sudoku, out int count_row, out int count_col);
                        if (CountSolutions(sudoku_copy_temp) == 1)
                        {
                            if (count_row < 2 || count_col < 2)
                            {
                                sudoku[i, j] = original_number;
                            }
                        }
                        else
                        {
                            sudoku[i, j] = original_number;
                        }

                    }
                }
                else
                {
                    for (int j = 0; j < 9; j++)
                    {
                        original_number = sudoku[i, j];
                        sudoku[i, j] = 0;
                        int[,] sudoku_copy_temp = CopySudoku(sudoku);
                        validRow_Col(i, j, sudoku, out int count_row, out int count_col);
                        if (CountSolutions(sudoku_copy_temp) == 1)
                        {
                            if (count_row < 2 || count_col < 2)
                            {
                                sudoku[i, j] = original_number;
                            }
                        }
                        else
                        {
                            sudoku[i, j] = original_number;
                        }

                    }
                }
                if (number_of_full_numbers(sudoku) < 23) break;
            }
            Console.WriteLine("Počet čísel: " + number_of_full_numbers(sudoku));
        }
        //

        // difficulty - 5 - nejtěžší
        static void difficulty_5(int[,] sudoku)
        {
            int original_number;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    original_number = sudoku[i, j];
                    sudoku[i, j] = 0;
                    int[,] sudoku_copy_temp = CopySudoku(sudoku);
                    if (CountSolutions(sudoku_copy_temp) != 1) sudoku[i, j] = original_number;
                    if (number_of_full_numbers(sudoku) < 17) break;
                }
                if (number_of_full_numbers(sudoku) < 17) break;
            }
            Console.WriteLine("Počet čísel: " + number_of_full_numbers(sudoku));
        }
        //



        // counting the number of full numbers in the sudoku
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
        //

        // validating the row and column
        static void validRow_Col(int row, int col, int[,] sudoku, out int count_row, out int count_col)
        {
            count_row = 0;
            count_col = 0;
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[row, i] != 0)
                {
                    count_row++;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[i, col] != 0)
                {
                    count_col++;
                }
            }
        }
        //

        // counting possible solutions
        static int CountSolutions(int[,] sudoku)
        {
            int count = 0;
            SolveMultiple(sudoku, ref count, 2);
            return count;
        }
        //

        // recursive backtracking for more than one solution
        static bool SolveMultiple(int[,] sudoku, ref int count, int maxCount)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (sudoku[r, c] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValidPlacement(r, c, num, sudoku))
                            {
                                sudoku[r, c] = num;
                                if (SolveMultiple(sudoku, ref count, maxCount))
                                    return true;
                                sudoku[r, c] = 0;
                            }
                        }
                        return false;
                    }
                }
            }

            count++;
            return count >= maxCount;
        }
        //
        // ###




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