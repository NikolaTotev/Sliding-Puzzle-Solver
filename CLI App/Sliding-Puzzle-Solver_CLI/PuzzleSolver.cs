using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sliding_Puzzle_Solver_CLI
{
    class PuzzleSolver
    {
        private int m_PuzzleSize;
        private List<List<PuzzleElement>> m_ConfigurationMatrix;
        private int[,] m_ParentMatrix;
        private Dictionary<int, PuzzleElement> m_ConfigurationList;
        private List<Movable> m_MovableElements = new List<Movable>();
        private Movable m_ResponsibleMove;

        public int currentThreshold { get; set; }
        private int m_CurrentDepth = 0;
        private int m_HnCoef = 0;
        public PuzzleSolver(int puzzleSize, List<List<PuzzleElement>> configurationMatrix, Dictionary<int, PuzzleElement> configurationList)
        {
            m_PuzzleSize = puzzleSize;
            m_ConfigurationMatrix = configurationMatrix;
            m_ConfigurationList = configurationList;
            currentThreshold = 0;
            m_ParentMatrix = new int[puzzleSize,puzzleSize];

            //Deep copy code for the Puzzle Matrix
            m_ConfigurationMatrix = new List<List<PuzzleElement>>();

            foreach (List<PuzzleElement> puzzleElements in configurationMatrix)
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
            foreach (KeyValuePair<int, PuzzleElement> puzzleElement in configurationList)
            {
                PuzzleElement newElement = new PuzzleElement(puzzleElement.Value.ElementNumber,
                    puzzleElement.Value.CurrentPosition, puzzleElement.Value.DesiredPosition,
                    puzzleElement.Value.ManhattanDistance);
                m_ConfigurationList.Add(puzzleElement.Key, newElement);
            }
        }

        //Calculates the Manhattan Distance for the current configuration.
        //This should be called before IDA* start and after every move.
        //This is not an optimized calculation!
        public void CalcHnCoef()
        {
            // Console.WriteLine();
            // Console.WriteLine("======== Recalculating Hn ========");
            m_HnCoef = 0;
            foreach (var element in m_ConfigurationList)
            {
                element.Value.CalcManhattanDistance(); //This can be optimized to calculate ManhattanDist only when a piece moves.

                m_HnCoef += element.Value.ManhattanDistance;
                //Console.WriteLine(
                //$"{element.Value.ElementNumber} -> hn = {element.Value.ManhattanDistance} || New total: {hnCoef}");
            }

            currentThreshold = m_HnCoef;
        }


        public void FindMovable()
        {
            //Console.WriteLine();
            //Console.WriteLine("======= Finding movable =======");
            bool canMoveLeft = m_ConfigurationList[0].CurrentPosition.X >= 1;
            int number = m_ConfigurationList[0].ElementNumber;
            bool canMoveRight = m_ConfigurationList[0].CurrentPosition.X < 2;
            bool canMoveUp = m_ConfigurationList[0].CurrentPosition.Y >= 1;
            bool canMoveDown = m_ConfigurationList[0].CurrentPosition.Y < 2;

            if (canMoveLeft)
            {
                //Console.WriteLine("Left movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X - 1;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Right));
            }

            if (canMoveRight)
            {
                //Console.WriteLine("Right movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X + 1;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Left));
            }

            if (canMoveUp)
            {
                //Console.WriteLine("Up movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y - 1;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Down));
            }

            if (canMoveDown)
            {
                //Console.WriteLine("Down movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y + 1;
                m_MovableElements.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Up));
            }
        }


        private void Move(bool reverse)
        {
            MoveDirection direction;
            if (reverse)
            {
                switch (m_ResponsibleMove.Direction)
                {
                    case MoveDirection.Up:
                        direction = MoveDirection.Down;
                        break;
                    case MoveDirection.Down:
                        direction = MoveDirection.Up;
                        break;
                    case MoveDirection.Left:
                        direction = MoveDirection.Right;
                        break;
                    case MoveDirection.Right:
                        direction = MoveDirection.Left;
                        break;
                    case MoveDirection.None:
                        direction = MoveDirection.None;
                        break;
                    default:
                        direction = MoveDirection.None;
                        break;
                }
            }
            else
            {
                direction = m_ResponsibleMove.Direction;
            }

            PuzzleElement elementToMove = m_ConfigurationList[m_ResponsibleMove.PieceNumber];
            Point elementCoords = elementToMove.CurrentPosition;
            Point zeroCoords = m_ConfigurationList[0].CurrentPosition;

            switch (direction)
            {
                case MoveDirection.Up:
                    //Console.WriteLine($"Moving {m_ResponsibleMove.PieceNumber} UP");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                             m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[m_ResponsibleMove.PieceNumber].CurrentPosition.Y -= 1;
                    m_ConfigurationList[0].CurrentPosition.Y += 1;
                    break;
                case MoveDirection.Down:
                    //Console.WriteLine($"Moving {m_ResponsibleMove.PieceNumber} DOWN");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[m_ResponsibleMove.PieceNumber].CurrentPosition.Y += 1;
                    m_ConfigurationList[0].CurrentPosition.Y -= 1;
                    break;
                case MoveDirection.Left:
                    //Console.WriteLine($"Moving {m_ResponsibleMove.PieceNumber} LEFT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[m_ResponsibleMove.PieceNumber].CurrentPosition.X -= 1;
                    m_ConfigurationList[0].CurrentPosition.X += 1;
                    break;
                case MoveDirection.Right:
                    //Console.WriteLine($"Moving {m_ResponsibleMove.PieceNumber} RIGHT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[m_ResponsibleMove.PieceNumber].CurrentPosition.X += 1;
                    m_ConfigurationList[0].CurrentPosition.X -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(m_ResponsibleMove.Direction), m_ResponsibleMove.Direction, null);
            }
        }

        private void SaveParentMatrix()
        {
            for (int i = 0; i < m_ConfigurationMatrix.Count; i++)
            {
                for (int j = 0; j < m_ConfigurationMatrix[i].Count; j++)
                {
                    m_ParentMatrix[i, j] = m_ConfigurationMatrix[i][j].ElementNumber;
                }

            }
        }

        bool WillLoop()
        {
            for (int i = 0; i < m_ConfigurationMatrix.Count; i++)
            {
                for (int j = 0; j < m_ConfigurationMatrix[i].Count; j++)
                {
                    if (m_ConfigurationMatrix[i][j].ElementNumber != m_ParentMatrix[i, j])
                    {
                        return false;
                    }
                }

            }

            return true;
        }
    }
}
