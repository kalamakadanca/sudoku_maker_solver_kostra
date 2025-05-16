using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[, ] sudoku = new int[9, 9];
        List<int> seznam;
        List<int> seznam_small_grid = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        int temp_rnd = 0;
        int random_x;
        int random_y;
        int sprinkler_random_number;
        bool sprinkler = true;

        for (int i = 0; i < 11; i++)
        {
            sprinkler_random_number = random.Next(1, 10);
            random_y = random.Next(0, 9);
            random_x = random.Next(0, 9);
            if (IsValidPlacement(random_y, random_x, sprinkler_random_number, sudoku, seznam_small_grid, sprinkler) && sudoku[random_y, random_x] == 0)
            {
                sudoku[random_y, random_x] = sprinkler_random_number;
            }
            else
            {
                i--;
            }
        }
        sprinkler = false;
        // printing the sudoku
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();
        

        // filling sudoku
        for (int start_r = 0; start_r < 9; start_r++)
        {
            seznam = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int start_c = 0; start_c < 9; start_c++)
            {
                if (sudoku[start_r, start_c] == 0)
                {
                    temp_rnd = seznam[random.Next(seznam.Count)];
                    if (IsValidPlacement(start_r, start_c, temp_rnd, sudoku, seznam_small_grid, sprinkler))
                    {
                        sudoku[start_r, start_c] = temp_rnd;
                        seznam.Remove(temp_rnd);
                    }
                }
            }
        }
        //

        // validating the chosen number
        static bool IsValidPlacement(int startRow, int startCol, int random_number, int[,] sudoku, List<int> seznam,bool sprinkler){
            bool isValid = true;
            int row_grid;
            int col_grid;
            for (int i = 0; i < 9; i++) {
                if (sudoku[startRow, i] == random_number) {
                    isValid = false;
                }
            }
            for (int i = 0; i < 9; i++) {
                if (sudoku[i, startCol] == random_number) {
                    isValid = false;
                }
            }
            if (startRow < 3) row_grid = 0;
            else if (startRow < 6) row_grid = 3;
            else row_grid = 6;

            if (startCol < 3) col_grid = 0;
            else if (startCol < 6) col_grid = 3;
            else col_grid = 6;

            seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
            for (int i = row_grid; i < row_grid + 3; i++) {
                for (int j = col_grid; j < col_grid + 3; j++)
                {
                    if (sudoku[i, j] != 0)
                    {
                        seznam.Remove(sudoku[i, j]);
                    }
                }
            }
            //Console.WriteLine("seznam: " + string.Join(", ", seznam));
            for (int i = row_grid; i < row_grid + 3; i++) {
                for (int j = col_grid; j < col_grid + 3; j++) {
                    if (sudoku[i, j] == random_number) {
                        isValid = false;
                    }
                }
            }
            //Console.WriteLine("seznam: " + string.Join(", ", seznam));



            return isValid;
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
    }
}

// backtracking algorithm


/* TODO:
* 1. backtracking 🥀🥀🥀
*/
