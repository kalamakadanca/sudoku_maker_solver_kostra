**Generování sudoku** <br>
**Daniela Luuová**
<br>
<br>
**Abstrakt** <br>
	Tato práce je zaměřená na optimální generování jednoznačně řešitelného sudoku. Zahrnuje analýzu existujících přístupů, návrh vlastního řešení a jeho realizaci v jazyce C#. Práce je formou hry pro vyzkoušení.

**Cíle:**
-	Vytvoření jednoznačně řešitelného sudoku
-	Možnost vyzkoušení si vygenerovaného sudoku

-	Vyzkoušet si návrh a implementaci algoritmů
-	Realizace a kompletizace obtížnějších programů
-	Práce s odborným článkem (dále jen článek)
<br>

**Použité technologie** <br>
**Jazyk:** C# <br>
**Prostředí:** .NET 9 <br>
**Typ aplikace:** konzolová <br>
**Knihovny:** System (Random, Console, ...), System.Diagnostics (Stopwatch), vlastní jmenné prostory (Full_Sudoku, Difficulties) <br>
**Koncepty:** generování sudoku, řešení, rekurze, práce s maticemi, konzolové barvy, vstup/výstup. <br>
**Algoritmy:** Las Vegas algoritmus, rekurzivní backtracking, globální randomizace čísel, přeskakování buněk, procházení ve tvaru S, procházení zleva doprava, technika prořezávání <br>
**Článek:** [Generating sudoku from easy to evil](http://zhangroup.aporc.org/images/files/Paper_3485.pdf) <br>
**AI:** malé detaily pro lepší přehlednost kódu, nápomoc se dvěma metodami (rekurzivní backtracking, rekurzivní backtracking s technikou prořezávání)


**Popis fungování programu:**

**Full_Sudoku.cs** <br>
V tomto souboru se vytváří vyplněné sudoku podle základních pravidel. Použila jsem Las Vegas algoritmus, ve kterém se používá předvyplnění 9x9 mřížky (dále jen mřížka) a rekurzivní backtracking.

Metody se pouští do doby, dokud nevznikne platné sudoku. (většinou jednou, šance pro nevytvoření validního sudoku jsou přibližně 4 %). 

Přibližný čas je 10ms.  

**Difficulties.cs** <br>
	V tomto souboru se tvoří 5 obtížnostních levelů. Pracuje se s vyplněnou mřížkou a postupně se odstraňují čísla pomocí dig-hole strategy (strategie vykopávání buněk).
 
**Program.cs** <br>
Toto je hlavní program, ve kterém tyto algoritmy spojuji a gamifikuji. Probíhá zde práce s uživatelskými vstupy (validace vstupu, následné nakládání s vícero vstupy).

**Průběh hry probíhá následovně:**
1.	Hráč si vybere obtížnost, popř. si nechá vysvětlit ovládací prvky a pravidla.
2.	Zapne se mu samotná hra s danou obtížností.
3.	Po dokončení si může vybrat, zda chce pokračovat, nebo ukončit hru.

**Zdroje:** <br>
**1.**	Článek - [Generating sudoku from easy to evil](http://zhangroup.aporc.org/images/files/Paper_3485.pdf) <br>
**2.**	Ai – ChatGPT, github copilot <br>
**3.**	Youtube - [Green Code - sudoku](https://youtu.be/0roAZFaqSjw?si=eT139kOMlojjBHTx)
