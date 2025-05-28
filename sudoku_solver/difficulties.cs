namespace Difficulties
{
    public static class Difficulty_Levels
    {
        // DIFFICULTY VALIDATION SYSTEM
        // validating the row and column
        static void ValidRow_Col(int row, int col, int[,] sudoku, out int count_row, out int count_col)
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

        // recursive backtracking for more than one solution
        static bool SolveMultiple(int[,] sudoku, ref int count, int maxCount)
        {
            int minOptions = 10;
            int minRow = -1, minCol = -1;

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (sudoku[r, c] == 0)
                    {
                        int options = 0;
                        for (int num = 1; num <= 9; num++)
                        {
                            if (Full_Sudoku.Las_Vegas.IsValidPlacement(r, c, num, sudoku))
                                options++;
                        }

                        if (options < minOptions)
                        {
                            minOptions = options;
                            minRow = r;
                            minCol = c;
                        }

                        if (minOptions == 1) break;
                    }
                }
                if (minOptions == 1) break;
            }

            if (minRow == -1)
            {
                count++;
                return count >= maxCount;
            }

            for (int num = 1; num <= 9; num++)
            {
                if (Full_Sudoku.Las_Vegas.IsValidPlacement(minRow, minCol, num, sudoku))
                {
                    sudoku[minRow, minCol] = num;
                    if (SolveMultiple(sudoku, ref count, maxCount))
                        return true;
                    sudoku[minRow, minCol] = 0;
                }
            }

            return false;
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
        // ###

        // DIFFICULTY ALGORITHMS
        // difficulty - 1 - nejjednodušší
        public static void Difficulty_1(int[,] sudoku, Random random)
        {
            int i = 0;
            while (i < 30 || Program.NumberOfFullNumbers(sudoku) > 61)
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
                int[,] copy_sudoku_temp = Program.CopySudoku(sudoku);
                ValidRow_Col(random_x, random_y, sudoku, out int count_row, out int count_col);

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
        public static void Difficulty_2(int[,] sudoku, Random random)
        {
            int i = 0;
            while (i < 45 || Program.NumberOfFullNumbers(sudoku) > 49)
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
                int[,] copy_sudoku_temp = Program.CopySudoku(sudoku);
                ValidRow_Col(random_x, random_y, sudoku, out int count_row, out int count_col);

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
        public static void Difficulty_3(int[,] sudoku)
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

                            ValidRow_Col(i, j, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = Program.CopySudoku(sudoku);
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

                            ValidRow_Col(i, k, sudoku, out int count_row, out int count_col);
                            int[,] sudoku_copy_temp = Program.CopySudoku(sudoku);
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
        }
        //

        // difficulty - 4 - těžká
        public static void Difficulty_4(int[,] sudoku)
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
                        int[,] sudoku_copy_temp = Program.CopySudoku(sudoku);
                        ValidRow_Col(i, j, sudoku, out int count_row, out int count_col);
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
                        int[,] sudoku_copy_temp = Program.CopySudoku(sudoku);
                        ValidRow_Col(i, j, sudoku, out int count_row, out int count_col);
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
                if (Program.NumberOfFullNumbers(sudoku) < 23) break;
            }
        }
        //

        // difficulty - 5 - nejtěžší
        public static void Difficulty_5(int[,] sudoku)
        {
            int original_number;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    original_number = sudoku[i, j];
                    sudoku[i, j] = 0;
                    int[,] sudoku_copy_temp = Program.CopySudoku(sudoku);
                    if (CountSolutions(sudoku_copy_temp) != 1) sudoku[i, j] = original_number;
                    if (Program.NumberOfFullNumbers(sudoku) < 17) break;
                }
                if (Program.NumberOfFullNumbers(sudoku) < 17) break;
            }
        }
        //
    }
}