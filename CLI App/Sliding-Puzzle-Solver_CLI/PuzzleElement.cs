using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sliding_Puzzle_Solver_CLI
{
    class PuzzleElement
    {
        public int ElementNumber;
        public Point CurrentPosition;
        public Point DesiredPosition;
        public int ManhattanDistance;

        public PuzzleElement(int elementNumber, Point currentPosition, Point desiredPosition)
        {
            ElementNumber = elementNumber;
            CurrentPosition = currentPosition;
            DesiredPosition = desiredPosition;
            CalcManhattanDistance();
        }

        public PuzzleElement(int elementNumber, Point currentPosition, Point desiredPosition, int manhattanDistance)
        {
            ElementNumber = elementNumber;
            CurrentPosition = currentPosition;
            DesiredPosition = desiredPosition;
            ManhattanDistance = manhattanDistance;
        }

        public void CalcManhattanDistance()
        {
            ManhattanDistance = Math.Abs(CurrentPosition.X - DesiredPosition.X) + Math.Abs(CurrentPosition.Y - DesiredPosition.Y);
        }
    }
}
