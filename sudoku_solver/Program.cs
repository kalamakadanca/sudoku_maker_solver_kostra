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


        static bool ContinueToPlay()
        {
            Console.WriteLine("Chceš pokračovat ve hře? (A/N)");
            string answer = Console.ReadLine().ToUpper();

            if (answer == "A")
            {
                return false;
            }
            else if (answer == "N")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Děkují za hraní!");
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.WriteLine("Neplatná volba. Zadej 'A' pro pokračování nebo 'N' pro ukončení.");
                return ContinueToPlay();
            }
        }


        int[,] sudoku_for_user = new int[9, 9];

        bool end_game = false;
        int difficulty = 0;
        bool hra = true;

        int[,] sudoku_for_user_starter;

        Console.WriteLine("Vítej v sudoku!");
        Console.WriteLine();

        //MAIN GAME LOOP
        while (!end_game)
        {
            if (difficulty != 7)
                Console.Clear();
            difficulty = 0;
            // choosing the difficulty
            while (difficulty == 0)
            {
                while (!answer)
                {
                    Array.Clear(sudoku, 0, sudoku.Length);
                    Full_Sudoku.Las_Vegas.Sprinkler(sudoku, random);
                    answer = Full_Sudoku.Las_Vegas.Solve(sudoku, answer);
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

                switch (difficulty)
                {
                    case 1:
                        Difficulties.Difficulty_Levels.Difficulty_1(sudoku_for_user, random);
                        hra = true;
                        break;
                    case 2:
                        Difficulties.Difficulty_Levels.Difficulty_2(sudoku_for_user, random);
                        hra = true;
                        break;
                    case 3:
                        Difficulties.Difficulty_Levels.Difficulty_3(sudoku_for_user);
                        hra = true;
                        break;
                    case 4:
                        Difficulties.Difficulty_Levels.Difficulty_4(sudoku_for_user);
                        hra = true;
                        break;
                    case 5:
                        Difficulties.Difficulty_Levels.Difficulty_5(sudoku_for_user);
                        hra = true;
                        break;
                    case 6:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Děkuji za návštěvu!");
                        Console.ResetColor();
                        hra = false;
                        end_game = true;
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Pravidla hry:");
                        Console.WriteLine("1. Cílem hry je vyplnit mřížku 9x9 čísly od 1 do 9 tak, aby v každém řádku, sloupci a 3x3 čtverci bylo každé číslo právě jednou.");
                        Console.WriteLine("2. Některá čísla jsou již předvyplněná, tyto buňky nelze měnit.");
                        Console.WriteLine("3. Pokud chceš vymazat číslo, zadej 0. <pouze červená čísla lze vymazat>");
                        Console.WriteLine("4. Pokud chceš vyřešit sudoku, zadej 20 jako řádek. (nesmí obsahovat žádné červené číslo)");
                        Console.ResetColor();
                        hra = false;
                        break;

                    default:
                        Console.WriteLine("Neplatná volba. Zkus to znovu.");
                        break;
                }
            }

            sudoku_for_user_starter = CopySudoku(sudoku_for_user);

            Console.WriteLine();

            while (hra)
            {
                Console.Clear();
                PrintSudokuUser(sudoku, sudoku_for_user, sudoku_for_user_starter);

                if (NumberOfFullNumbers(sudoku_for_user) == 81 && FullSudoku(sudoku, sudoku_for_user))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Gratuluji! Vyřešil jsi sudoku!");
                    Console.ResetColor();
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
                    Console.WriteLine("Stistni libovolnou klávesu pro pokračování...");
                    Console.ReadKey(true);
                    continue;
                }

                Console.WriteLine("Zadej číslo (1-9): ");
                int number;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out number) && number >= 1 && number <= 9) break;
                    else if (number == 0)
                    {
                        sudoku_for_user[row, col] = 0;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Neplatný vstup! Zadej prosím číslo buňky od 1-9.");
                    }
                }
                if (row != 19) sudoku_for_user[row, col] = number;
                else
                {
                    Full_Sudoku.Las_Vegas.Solve(sudoku_for_user, false);
                }
            }
        }
        // printing the time
        sw.Stop();
        Console.WriteLine("Time taken: " + sw.ElapsedMilliseconds + " ms");
    }


    // checking if the sudoku is full
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
    //

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

    // copying the sudoku
    public static int[,] CopySudoku(int[,] original)
    {
        int[,] copy = new int[9, 9];
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                copy[i, j] = original[i, j];
        return copy;
    }
    //

    // counting the number of full numbers in the sudoku
    public static int NumberOfFullNumbers(int[,] sudoku)
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
}