using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

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
                PrintSudoku(sudoku);
            }
        }
        answer = false;


        static int[,] CopySudoku(int[,] original)
        {
            int[,] copy = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    copy[i, j] = original[i, j];
            return copy;
        }


        int[,] sudoku_copy = CopySudoku(sudoku);

        bool end_game = false;
        int difficulty = 0;
        //MAIN GAME LOOP
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
                    difficulty_1(sudoku_copy, random);
                    PrintSudoku(sudoku_copy);
                    end_game = true;
                    break;
                case 2:
                    Console.WriteLine("Vybrali jste si snadnou obtížnost.");
                    difficulty_2(sudoku_copy, random);
                    PrintSudoku(sudoku_copy);
                    end_game = true;
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
                    difficulty_3(sudoku_copy);
                    PrintSudoku(sudoku_copy);
                    end_game = true;
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
        //



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
                    for (int j = 0; j < 9; j++)
                    {
                        if (j % 2 == 0)
                        {
                            original_number = sudoku[i, j];
                            sudoku[j, i] = 0;

                            validRow_Col(j, i, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = CopySudoku(sudoku);
                            if (CountSolutions(sudoku_copy_temp) == 1)
                            {
                                if (count_row < 3 || count_col < 3)
                                {
                                    sudoku[j, i] = original_number;
                                }
                            }
                            else
                            {
                                sudoku[j, i] = original_number;
                            }
                        }
                    }
                }
                else if (i % 2 != 0 || second_going_through == true && i % 2 == 0)
                {
                    for (int k = 8; k >= 0; k--)
                    {
                        if (k % 2 != 0)
                        {
                            original_number = sudoku[i, k];
                            sudoku[k, i] = 0;

                            validRow_Col(k, i, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = CopySudoku(sudoku);
                            if (CountSolutions(sudoku_copy_temp) == 1)
                            {
                                if (count_row < 3 || count_col < 3)
                                {
                                    sudoku[k, i] = original_number;
                                }
                            }
                            else
                            {
                                sudoku[k, i] = original_number;
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
        //
        // difficulty - 5 - nejtěžší
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

        // counting possible solutions
        static int CountSolutions(int[,] sudoku)
        {
            int count = 0;
            SolveMultiple(sudoku, ref count, 2);
            return count;
        }

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