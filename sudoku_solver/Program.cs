using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[, ] sudoku = new int[9, 9];
        bool answer = false;

        while (!answer) {
            Array.Clear(sudoku, 0, sudoku.Length);
            sprinkler(sudoku, random);
            answer = Solve(sudoku, answer);
        }

        // printing the sudoku
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }
        //



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
    }
}