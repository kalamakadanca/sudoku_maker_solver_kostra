using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[, ] sudoku = new int[9, 9];
        List<int> seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        int temp_rnd;

        // pattern filling
        for (int i = 0; i < 9; i += 3) {
            for (int j = i; j < (i+3); j++) {
                for (int k = i; k < (i+3); k++) {
                    sudoku[j, k] = seznam[random.Next(seznam.Count)];
                    seznam.Remove(sudoku[j, k]);
                }
            }
            seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        }
        //

        // filling the rest of the sudoku
        for (int start_r = 0; start_r < 9; start_r += 3) {
            for (int start_c = 0; start_c < 9; start_c += 3) {

                seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
                for (int k = 0; k < 3; k++) {
                    for (int l = 0; l < 3; l++) {
                        if (sudoku[start_r + k, start_c + l] == 0) {
                            temp_rnd = seznam[random.Next(seznam.Count)];
                            if (IsValidPlacement(start_r + k, start_c + l, temp_rnd, sudoku)) {
                                sudoku[start_r + k, start_c + l] = temp_rnd;
                                seznam.Remove(temp_rnd);
                            } 
                        }
                    }
                }
            }
        }
        //

        // validating the chosen number
        static bool IsValidPlacement(int startRow, int startCol, int random_number, int[,] sudoku){
            bool isValid = true;
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

// pattern filling & backtracking algorithm
