using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sliding_Puzzle_Solver_CLI
{

    enum MoveDirection {Up, Down, Left, Right}
    class PuzzleConfiguration
    {
        private int ConfigID;
        private List<PuzzleConfiguration> m_ChidConfigurations = new List<PuzzleConfiguration>();
        private List<List<int>> m_ConfigurationMatrix;
        private Point[] m_CurrentCoordinates = new Point[9];

        public PuzzleConfiguration(List<List<int>> inputConfig)
        {
            m_ConfigurationMatrix = inputConfig;
            for (int i = 0; i < inputConfig.Count; i++)
            {
                for (int j = 0; j < inputConfig[i].Count; j++)
                {
                    int currentNumber = inputConfig[i][j];
                    if (currentNumber != 0)
                    {
                        m_CurrentCoordinates[inputConfig[i][j] - 1] = new Point(i, j);
                    }
                    else
                    {
                        m_CurrentCoordinates[8] = new Point(i, j);
                    }
                    
                }
            }
        }

        private void Move(int blockToMove, MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    break;
                case MoveDirection.Down:
                    break;
                case MoveDirection.Left:
                    break;
                case MoveDirection.Right:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        struct Movable
        {
            public int PieceNumber;
            public MoveDirection Direction;
        }

        public void GenerateChildConfigurations()
        {
            //Check which pieces around the 0 can be moved. 
            //Do this by subtracting/adding 1 to the coordinates of the 0
            //Do this for all 4 sides.

            //If a side has a piece that can be moved save it to a list of Movable

            //Iterate through the list of "Moveable", for each element, create a child configuration.
            //Call "Move" on the child configuration
            //Add child configuration to the list of children.

            //ATTENTION: This algorithm might cause duplicate configurations
        }
    }
}
