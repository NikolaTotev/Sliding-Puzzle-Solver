using System;
using System.Collections.Generic;
using System.Linq;
using Console = System.Console;

namespace Sliding_Puzzle_Solver_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<int>> m_PuzzleMatrix = new List<List<int>>();
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine($"Welcome to AutoSolve. Begin by entering the puzzle matrix to be solved!");
            Console.WriteLine($"Please enter numbers with spaces in between. Press enter after entering each row.");
            Console.WriteLine();
            Console.WriteLine($"!!>> There is NO input validation at the moment, so don't mess around <<!!");
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine($"The empty space in the puzzle is marked with 0");
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine($"Enter row {j+1} of the puzzle matrix");
                string currentRow = Console.ReadLine();

                List<int> numbers = currentRow.Split(' ').Select(int.Parse).ToList();
                List<int> rowToAdd = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    rowToAdd.Add(numbers[i]);
                }
                m_PuzzleMatrix.Add(rowToAdd);
            }

            Console.WriteLine($"The matrix that you entered is:");

            for (int i = 0; i < m_PuzzleMatrix.Count; i++)
            {
                for (int j = 0; j < m_PuzzleMatrix[i].Count; j++)
                {
                    Console.Write(m_PuzzleMatrix[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
