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
        private Dictionary<int, PuzzleElement> m_ConfigurationList;
        
        public Stack<Movable> Moves
        {
            get;
        }

        public int m_CurrentThreshold { get; set; }
        private int m_CurrentDepth = 0;
        private int m_HnCoef = 0;
        private bool m_IsSolved = false;
        public PuzzleSolver(int puzzleSize, List<List<PuzzleElement>> configurationMatrix, Dictionary<int, PuzzleElement> configurationList)
        {
            m_PuzzleSize = puzzleSize;
            m_ConfigurationMatrix = configurationMatrix;
            m_ConfigurationList = configurationList;
            m_CurrentThreshold = 0;
            Moves = new Stack<Movable>();

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

            m_CurrentThreshold = CalcHnCoef() + 0;
        }

        //Calculates the Manhattan Distance for the current configuration.
        //This should be called before IDA* start and after every move.
        //This is not an optimized calculation!
        public int CalcHnCoef()
        {
            // Console.WriteLine();
            // Console.WriteLine("======== Recalculating Hn ========");
            int hnCoef = 0;
            foreach (var element in m_ConfigurationList)
            {
                element.Value.CalcManhattanDistance(); //This can be optimized to calculate ManhattanDist only when a piece moves.

                hnCoef += element.Value.ManhattanDistance;
                //Console.WriteLine(
                //$"{element.Value.ElementNumber} -> hn = {element.Value.ManhattanDistance} || New total: {hnCoef}");
            }

           return hnCoef;
        }


        public void Solve()
        {
            while (!m_IsSolved)
            {
                Console.WriteLine(m_CurrentThreshold);
                Search(0);
                m_CurrentThreshold += 2;
                
            }
        }

        private void Search(int currentDepth)
        {
            List<Movable> movables = FindMovable();
            int[,] parentMatrix;
            foreach (Movable movableElement in movables)
            {

                parentMatrix = SaveParentMatrix();

                Move(false, movableElement);

                if (WillLoop(parentMatrix))
                {
                    Move(true, movableElement);
                }
                else
                {
                    int currentHnCoef = CalcHnCoef();
                    
                    if (currentHnCoef + currentDepth <= m_CurrentThreshold)
                    {
                        if (currentHnCoef == 0)
                        {
                            m_IsSolved = true;
                            Moves.Push(movableElement);
                            return;
                        }

                        
                        Search(currentDepth+1);
                        
                        if (m_IsSolved)
                        {
                            Moves.Push(movableElement);
                            return;
                        }
                        Move(true, movableElement);
                    }
                    else
                    {
                        Move(true, movableElement);
                    }
                }
            }
        }

        public List<Movable> FindMovable()
        {
            List<Movable> movables = new List<Movable>();

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
                movables.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Right));
            }

            if (canMoveRight)
            {
                //Console.WriteLine("Right movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X + 1;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y;
                movables.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Left));
            }

            if (canMoveUp)
            {
                //Console.WriteLine("Up movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y - 1;
                movables.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Down));
            }

            if (canMoveDown)
            {
                //Console.WriteLine("Down movement possible.");
                int movableYCoord = m_ConfigurationList[0].CurrentPosition.X;
                int movableXCoord = m_ConfigurationList[0].CurrentPosition.Y + 1;
                movables.Add(new Movable(m_ConfigurationMatrix[movableXCoord][movableYCoord].ElementNumber, MoveDirection.Up));
            }

            return movables;
        }


        private void Move(bool reverse, Movable responsibleMove)
        {
            MoveDirection direction;
            if (reverse)
            {
                switch (responsibleMove.Direction)
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
                direction = responsibleMove.Direction;
            }

            PuzzleElement elementToMove = m_ConfigurationList[responsibleMove.PieceNumber];
            Point elementCoords = elementToMove.CurrentPosition;
            Point zeroCoords = m_ConfigurationList[0].CurrentPosition;

            switch (direction)
            {
                case MoveDirection.Up:
                    //Console.WriteLine($"Moving {responsibleMove.PieceNumber} UP");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                             m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[responsibleMove.PieceNumber].CurrentPosition.Y -= 1;
                    m_ConfigurationList[0].CurrentPosition.Y += 1;
                    break;
                case MoveDirection.Down:
                    //Console.WriteLine($"Moving {responsibleMove.PieceNumber} DOWN");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[responsibleMove.PieceNumber].CurrentPosition.Y += 1;
                    m_ConfigurationList[0].CurrentPosition.Y -= 1;
                    break;
                case MoveDirection.Left:
                    //Console.WriteLine($"Moving {responsibleMove.PieceNumber} LEFT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[responsibleMove.PieceNumber].CurrentPosition.X -= 1;
                    m_ConfigurationList[0].CurrentPosition.X += 1;
                    break;
                case MoveDirection.Right:
                    //Console.WriteLine($"Moving {responsibleMove.PieceNumber} RIGHT");
                    m_ConfigurationMatrix[elementCoords.Y][elementCoords.X] =
                        m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X];
                    m_ConfigurationMatrix[zeroCoords.Y][zeroCoords.X] = elementToMove;

                    m_ConfigurationList[responsibleMove.PieceNumber].CurrentPosition.X += 1;
                    m_ConfigurationList[0].CurrentPosition.X -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(responsibleMove.Direction), responsibleMove.Direction, null);
            }
        }

        private int [,] SaveParentMatrix()
        {
            int[,] newParent = new int[m_PuzzleSize, m_PuzzleSize];
            for (int i = 0; i < m_ConfigurationMatrix.Count; i++)
            {
                for (int j = 0; j < m_ConfigurationMatrix[i].Count; j++)
                {
                    newParent[i, j] = m_ConfigurationMatrix[i][j].ElementNumber;
                }

            }

            return newParent;
        }

        bool WillLoop(int [,] m_ParentMatrix)
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
