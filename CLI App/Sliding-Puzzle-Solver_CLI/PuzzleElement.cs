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

        public void CalcManhattanDistance(Point currentPosition, Point desiredPosition)
        {
            ManhattanDistance = Math.Abs(currentPosition.X - desiredPosition.X) + Math.Abs(currentPosition.Y - desiredPosition.Y);
        }
    }
}
