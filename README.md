**Generating Sudoku puzzles** <br>
**Daniela Luuová**
<br>
<br>
**Abstract** <br>
	This project focuses on optimal generation of Sudoku puzzles with a unique solution. Includes an analysis of existing approaches, the design of a custom solution, and its implementation in C#. The project is presented in the form of a game for hands-on experimentation.

**Goals:**
-	Creation of a uniquely solvable Sudoku
-	Ability to try out the generated Sudoku
-	Practicing the design and impelentation of algorithms
-	Development and completion of more complex programs
-	Working with a scientific paper (hereinafter reffered to as "paper")
<br>

**Used technologies** <br> <br>
**Programming language:** C# <br>
**Environment:** .NET 9 <br>
**Application type:** Console application <br>
**Libraries:** System (Random, Console, ...), System.Diagnostics (Stopwatch), custom namespaces (Full_Sudoku, Difficulties) <br>
**Concepts:** Sudoku generation, solving, recursion, matrix manipulation, console colors, input/output. <br>
**Algorithms:** Las Vegas algorithm, recursive backtracking, global number randomization, jumping one cell, wandering along "S", left to right then top to bottom, pruning technique <br>
**Paper:** [Generating sudoku from easy to evil](http://zhangroup.aporc.org/images/files/Paper_3485.pdf) <br>
**AI:** minor details for better code readability, assistance with two methods (recursive backtracking, recursive backtracking with pruning)


**Program Description:**

**Full_Sudoku.cs** <br>
This file is responsible for generating a fully filled Sudoku grid according to the basic rules. I used Las Vegas algorithm, which combines partial pre-filling of the 9x9 grid (hereinafter reffered to as the grid) with recursive backtracking.

The methods are executed repeatedly until a valid Sudoku is generated (usually on the first attempt, probability of not generating a valid Sudoku is approximately 4%)

The approximate execution time is 10ms.  

**Difficulties.cs** <br>
	This file defines five difficulty levels. It works with a completed grid and gradually removes numbers using the dig-hole strategy.
 
**Program.cs** <br>
This is the main program where all algorithms are combined and gamified. It handles user input (input validation and subsequent processing of multiple inputs).

**Gameplay Flow:**
1.	The player selects a difficulty level or chooses to view instructions and rules.
2.	The game starts with the selected difficulty
3.	After finishing, the player can choose whether to continue or exit the game.

**Sources:** <br>
**1.**	Paper - [Generating sudoku from easy to evil](http://zhangroup.aporc.org/images/files/Paper_3485.pdf) <br>
**2.**	AI – ChatGPT, github copilot <br>
**3.**	Youtube - [Green Code - sudoku](https://youtu.be/0roAZFaqSjw?si=eT139kOMlojjBHTx)
