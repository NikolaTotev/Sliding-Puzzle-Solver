using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sliding_Puzzle_Solver_CLI
{
 

    public enum MoveDirection { Up, Down, Left, Right }
    public struct Movable
    {
        public Movable(int pieceNumber, MoveDirection direction)
        {
            PieceNumber = pieceNumber;
            Direction = direction;
        }
        public int PieceNumber;
        public MoveDirection Direction;
    }
    class PuzzleConfiguration
    {
        private int ConfigID;
        private List<PuzzleConfiguration> m_ChidConfigurations = new List<PuzzleConfiguration>();
        private List<List<PuzzleElement>> m_ConfigurationMatrix;
        private Dictionary<int, PuzzleElement> m_ConfigurationList;
        private List<Movable> m_Moves;
        public int hnCoef;
        private List<Movable> m_MovableElements = new List<Movable>();
        private bool hasBeenVisited = false;
        public PuzzleConfiguration(List<List<PuzzleElement>> inputMatrixConfig, Dictionary<int, PuzzleElement> inputListConfig, List<Movable> moves)
        {
            //Deep copy code for the Puzzle Matrix
            m_ConfigurationMatrix = new List<List<PuzzleElement>>();

            foreach (List<PuzzleElement> puzzleElements in inputMatrixConfig)
            {
                List<PuzzleElement> newList = new List<PuzzleElement>();

                foreach (PuzzleElement puzzleElement in puzzleElements)
                {
                    PuzzleElement newElement = new PuzzleElement(puzzleElement.ElementNumber,
                        puzzleElement.CurrentPosition, puzzleElement.DesiredPosition, puzzleElement.ManhattanDistance);
                    newList.Add(newElement);
                }
                m_ConfigurationMatrix.Add(newList);
            }

            //Deep copy code for the Puzzle List
            m_ConfigurationList = new Dictionary<int, PuzzleElement>();
            foreach (KeyValuePair<int, PuzzleElement> puzzleElement in inputListConfig)
            {
                PuzzleElement newElement = new PuzzleElement(puzzleElement.Value.ElementNumber,
                    puzzleElement.Value.CurrentPosition, puzzleElement.Value.DesiredPosition,
                    puzzleElement.Value.ManhattanDistance);
                m_ConfigurationList.Add(puzzleElement.Key, newElement);
            }

            if (hnCoef == 0)
            {
                CalcHnCoef();
            }

            m_Moves = moves;
        }

        private void Move(Movable movableElement)
        {

            PuzzleElement elementToMove = m_ConfigurationList[movableElement.PieceNumber];
            Point elementCoords = elementToMove.CurrentPosition;
            Point zeroCoords = m_ConfigurationList[0].CurrentPosition;
            Console.WriteLine();
            PrintMatrix(m_ConfigurationMatrix);
            Console.WriteLine();
            m_Moves.Add(movableElement);
            switch (movableElement.Direction)
            {
                case MoveDirection.Up:
                    Console.WriteLine($"Moving {movableElement.PieceNumber} UP");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                             m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[movableElement.PieceNumber].CurrentPosition.Y -= 1;
                    m_ConfigurationList[0].CurrentPosition.Y += 1;
                    break;
                case MoveDirection.Down:
                    Console.WriteLine($"Moving {movableElement.PieceNumber} DOWN");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[movableElement.PieceNumber].CurrentPosition.Y += 1;
                    m_ConfigurationList[0].CurrentPosition.Y -= 1;
                    break;
                case MoveDirection.Left:
                    Console.WriteLine($"Moving {movableElement.PieceNumber} LEFT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[movableElement.PieceNumber].CurrentPosition.X -= 1;
                    m_ConfigurationList[0].CurrentPosition.X += 1;
                    break;
                case MoveDirection.Right:
                    Console.WriteLine($"Moving {movableElement.PieceNumber} RIGHT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[movableElement.PieceNumber].CurrentPosition.X += 1;
                    m_ConfigurationList[0].CurrentPosition.X -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(movableElement.Direction), movableElement.Direction, null);
            }
            CalcHnCoef();
        }



        public void CalcHnCoef()
        {
            Console.WriteLine();
            Console.WriteLine("======== Recalculating Hn ========");
            hnCoef = 0;
            foreach (var element in m_ConfigurationList)
            {

                element.Value.CalcManhattanDistance(); //This can be optimized to calculate ManhattanDist only when a piece moves.

                hnCoef += element.Value.ManhattanDistance;
                Console.WriteLine($"{element.Value.ElementNumber} -> hn = {element.Value.ManhattanDistance} || New total: {hnCoef}");
            }
        }

        public void FindMovable()
        {
            Console.WriteLine();
            Console.WriteLine("======= Finding movable =======");
            bool canMoveLeft = m_ConfigurationList[0].CurrentPosition.X > 1;
            bool canMoveRight = m_ConfigurationList[0].CurrentPosition.X < 2;
            bool canMoveUp = m_ConfigurationList[0].CurrentPosition.Y > 1;
            bool canMoveDown = m_ConfigurationList[0].CurrentPosition.Y < 2;

            if (canMoveLeft)
            {
                Console.WriteLine("Left movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X - 1;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Right));
            }

            if (canMoveRight)
            {
                Console.WriteLine("Right movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X + 1;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Left));
            }

            if (canMoveUp)
            {
                Console.WriteLine("Up movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y - 1;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Down));
            }

            if (canMoveDown)
            {
                Console.WriteLine("Down movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y + 1;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Up));
            }
        }

        public void GenerateChildConfigurations()
        {
            Console.WriteLine();
            Console.WriteLine("======== Generating children ========");
            foreach (Movable movableElement in m_MovableElements)
            {
                Console.WriteLine($"Generating child, difference in {movableElement.PieceNumber} ");
                PuzzleConfiguration newChildConfiguration =
                    new PuzzleConfiguration(m_ConfigurationMatrix, m_ConfigurationList, m_Moves);
                newChildConfiguration.Move(movableElement);
                newChildConfiguration.CalcHnCoef();
                m_ChidConfigurations.Add(newChildConfiguration);
                if (newChildConfiguration.hnCoef == 0)
                {
                    return;
                }
            }
            hasBeenVisited = true;
        }

        public void IDATraversal(int currentThreshold)
        {
            Console.WriteLine();
            Console.WriteLine($">> Current Threshold {currentThreshold}");
            if (hnCoef == 0)
            {
                return;
            }
            FindMovable();
            if (!hasBeenVisited)
            {
                GenerateChildConfigurations();
            }
            foreach (PuzzleConfiguration child in m_ChidConfigurations)
            {
                if (child.hnCoef <= currentThreshold)
                {
                    child.IDATraversal(currentThreshold);
                }
                else
                {
                    return;
                }
            }
        }

        public static void PrintMatrix(List<List<PuzzleElement>> matrixToPrint)
        {
            for (int i = 0; i < matrixToPrint.Count; i++)
            {
                for (int j = 0; j < matrixToPrint[i].Count; j++)
                {
                    Console.Write($"{matrixToPrint[i][j].ElementNumber} ");
                }
                Console.WriteLine();
            }
        }
    }
}
