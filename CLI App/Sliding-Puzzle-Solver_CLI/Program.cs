using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Console = System.Console;

namespace Sliding_Puzzle_Solver_CLI
{
    class Program
    {
        private static List<List<int>> ThreeXThreeMatrix = new()
        {
            new List<int>() { 1, 2, 3 },
            new List<int>() { 4, 5, 6 },
            new List<int>() { 7, 8, 0 }
        };

        private static List<List<int>> FourXFourMatrix = new()
        {
            new List<int>() { 1, 2, 3, 4 },
            new List<int>() { 5, 6, 7, 8 },
            new List<int>() { 9, 10, 11, 12 },
            new List<int>() { 13, 14, 15, 0 }
        };

        private static List<List<int>> FiveXFiveMatrix = new()
        {
            new List<int>() { 1, 2, 3, 4, 5 },
            new List<int>() { 6, 7, 8, 9, 10, },
            new List<int>() { 11, 12, 13, 14, 15 },
            new List<int>() { 16, 17, 18, 19, 20 },
            new List<int>() { 21, 22, 23, 24, 0 }
        };


        struct CheckMatrixElement
        {
            //A flag that shows if a given matrix element has been zeroed.
            public bool HasBeenZeroedFlag;
            //The ID of the configuration that set the flag.
            public int ConfigID;
        }

        static void Main(string[] args)
        {
            List<List<PuzzleElement>> puzzleMatrix = new List<List<PuzzleElement>>();
            Dictionary<int, string> moves = new Dictionary<int, string>();
            Dictionary<int, Point> targetNumberPosition = new Dictionary<int, Point>();
            Dictionary<int, PuzzleElement> puzzleList = new Dictionary<int, PuzzleElement>();
            int puzzleSize;

            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine($"Welcome to AutoSolve. Begin by entering the puzzle matrix to be solved!");
            Console.WriteLine($"Please enter numbers with spaces in between. Press enter after entering each row.");
            Console.WriteLine();
            Console.WriteLine($"!!>> There is NO input validation at the moment, so don't mess around <<!!");
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine($"The empty space in the puzzle is marked with 0");
            Console.WriteLine($"=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.WriteLine();
            Console.WriteLine("Input puzzle size. Supported sizes are 3, 4, 5");

            try
            {
                puzzleSize = int.Parse(Console.ReadLine() ?? string.Empty);
            }
            catch (FormatException)
            {
                Console.WriteLine("Do you know what numbers look like?");
                return;
            }

            if (puzzleSize is > 5 or < 3)
            {
                Console.WriteLine("I think I told you what the supported sizes were...");
                return;
            }


            if (puzzleSize == 3)
            {
                targetNumberPosition = GenTargetPositionList(ThreeXThreeMatrix);
            }

            if (puzzleSize == 4)
            {
                targetNumberPosition = GenTargetPositionList(FourXFourMatrix);
            }

            if (puzzleSize == 5)
            {
                targetNumberPosition = GenTargetPositionList(FiveXFiveMatrix);
            }


            for (int j = 0; j < puzzleSize; j++)
            {
                Console.WriteLine($"Enter row {j + 1} of the puzzle matrix");
                string currentRow = Console.ReadLine();
                List<int> numbers;
                try
                {
                    if (currentRow != null)
                    {
                        numbers = currentRow.Split(' ').Select(int.Parse).ToList();
                    }
                    else
                    {
                        Console.WriteLine("You do know what enter means... right?");
                        return;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Really, you think that {currentRow} is a row of {puzzleSize} numbers?");
                    return;
                }

                List<PuzzleElement> rowToAdd = new List<PuzzleElement>();

                for (int i = 0; i < numbers.Count; i++)
                {
                    Point currentCoord = new Point(i, j);
                    PuzzleElement elementToAdd =
                        new PuzzleElement(numbers[i], currentCoord, targetNumberPosition[numbers[i]]);
                    rowToAdd.Add(elementToAdd);
                    puzzleList.Add(numbers[i], elementToAdd);
                }
                puzzleMatrix.Add(rowToAdd);
            }

            Console.WriteLine($"The matrix that you entered is:");
            PrintMatrix(puzzleMatrix);

            Console.WriteLine();
            Console.WriteLine("The target matrix is:");
            PrintMatrix();
        }

        public static Dictionary<int, Point> GenTargetPositionList(List<List<int>> templateMatrix)
        {
            Dictionary<int, Point> targetPositionList = new Dictionary<int, Point>();

            for (int i = 0; i < templateMatrix.Count; i++)
            {
                for (int j = 0; j < templateMatrix[i].Count; j++)
                {
                    Point currentCoord = new Point(j, i);
                    targetPositionList.Add(templateMatrix[i][j], currentCoord);
                }
            }

            return targetPositionList;
        }

        public static void PrintMatrix(List<List<PuzzleElement>> matrixToPrint)
        {
            for (int i = 0; i < matrixToPrint.Count; i++)
            {
                for (int j = 0; j < matrixToPrint[i].Count; j++)
                {
                    Console.Write(matrixToPrint[i][j].ElementNumber);
                }
                Console.WriteLine();
            }
        }

        public void PrintMatrix(List<List<int>> matrixToPrint)
        {
            for (int i = 0; i < matrixToPrint.Count; i++)
            {
                for (int j = 0; j < matrixToPrint[i].Count; j++)
                {
                    Console.Write(matrixToPrint[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
