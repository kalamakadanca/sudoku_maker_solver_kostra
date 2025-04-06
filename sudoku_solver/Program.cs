using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int[, ] sudoku = new int[9, 9];
        HashSet<int> usedNumbers = new HashSet<int>();
        Dictionary<int, HashSet<int>> col = new Dictionary<int, HashSet<int>>();
        for (int i = 0; i < 9; i++)
        {
            col[i] = new HashSet<int>();
        }
        HashSet<int> rand_pick = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };



        // col[j] ma nekonecnou smycku - nevim proc - uz vim proc - ne vsechno je sudoku 🥀


        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                rand_pick = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                do {
                    sudoku[i, j] = random.Next(rand_pick.Count()) + 1;

                }  while (usedNumbers.Contains(sudoku[i, j]) || col[j].Contains(sudoku[i, j]));
                rand_pick.Remove(sudoku[i, j]);

                usedNumbers.Add(sudoku[i, j]);
                col[j].Add(sudoku[i, j]);
                Console.WriteLine($"i: {i}, j: {j}, sudoku[i, j]: {sudoku[i, j]}, usedNumbers: {string.Join(", ", usedNumbers)}, col[j]: {string.Join(", ", col[j])}");
            }
            usedNumbers.Clear();
        }

        Console.WriteLine("Sudoku Puzzle:");
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}