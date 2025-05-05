using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[, ] sudoku = new int[9, 9];
        List<int> seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
        int sez = seznam.Count-1;

        for (int i = 0; i < 9; i += 3) {
            for (int j = i; j < (i+3); j++) {
                for (int k = i; k < (i+3); k++) {
                    sudoku[j, k] = seznam[random.Next(sez)];
                    seznam.Remove(sudoku[j, k]);
                    sez--;
                }
            }
            seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
            sez = seznam.Count-1;
        }


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