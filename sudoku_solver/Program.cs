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
        int sez = seznam.Count-1;
        int temp_rnd;

        List<int> temporary_seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};

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

        int start_r;
        int start_c;


        for (int i = 0; i < 9; i += 3) {
            for (int j = 0; j < 9; j += 3) {
                start_r = i;
                start_c = j;

                seznam = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};
                sez = seznam.Count-1;
                for (int k = 0; k < 3; k++) {
                    for (int l = 0; l < 3; l++) {
                        if (sudoku[start_r + k, start_c + l] == 0) {
                            sudoku[start_r + k, start_c + l] = seznam[random.Next(sez)];
                            seznam.Remove(sudoku[start_r + k, start_c + l]);
                            sez--;

                            
                        }
                    }
                }
            }
        }


        static bool Rada(int zacatek, int random_cislo, int[,] sudoku){
            for (int i = 0; i < 9; i++) {
                if (sudoku[i, zacatek] == random_cislo) {
                    return false;
                }
            }
            return true;
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

//Rada(i+l, sudoku[start_r + k, start_c + l], sudoku)