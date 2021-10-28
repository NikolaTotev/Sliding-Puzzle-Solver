# 3x3 Auto Solver
## Introduction
This is my solution for the first homework for the course Intelligent Systems at FMI year 4.
The goal is to write an algorithm that automatically solved a 3x3 sliding puzzle. The best algorithm to use is IDA*. 
## Contents
This repo contains two VS 2019 projects. One contains a CLI solution and the other one is the GUI version build using Blazor. 

## Try it out
The GUI version of this solution can be found [here](#http://nikolatotev-001-site1.ctempurl.com/).

## Program Elements

### Class Solver
This class contains all of the puzzle solving logic. To create an instance of Solver you need:
`int puzzleSize`
`List<List<PuzzleElement>> configurationMatrix`
`Dictionary<int, PuzzleElement> configurationList`
What these arguemnts are for will be discussed later in this file.
### Functions:

`public int CalcHnCoef()`
This function calculates the Manhattan distance of the current configuration.

`public void Solve()`
This function is called by the parent program when the puzzle needs to be solved;

`private void Search(int currentDepth)`
This function reccursively traverses the possible states. The exact algorithm will be described in a later point.

```
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
```


`public List<Movable> FindMovable()`
This function returns a list of the struct `Movable` . This list contains all possible moves for a the current configuration.

`private void Move(bool 
reverse, Movable responsibleMove)`
This function executes the move that is passed in. the reverse flag is used when popping out of reccursion to revert to the parent configuration before executing the next move.

`private int [,] SaveParentMatrix()`
This function copies only the numbers from the parent matrix. The parent matrix is used as an anti-local loop check to avoid the following case:
R= root node

R -> A, B 
A -> A1, B1
and A1 = R in terms of number configuration.
The parent matrix allows for this to be detected and stops the algorithm from going into A1 from A.

`bool WillLoop(int [,] m_ParentMatrix)`
This function performs the check  is A1 = R? 

## Algorithm
Important variables
currentThreshold
m_IsSolved
Heuristic function: Heur(n) = h(n) + g(n)
Where g(n) is the number of "hops" from one state to another.
Algorithm start:
1. currentThreshold is initialized as the h(n) of the root + 0;
2. Search function is called with depth 0 `Search(0)`
3. `FindMovable()` is called and the result is saved in `Movables`
4. Foreach loop starts for elements in `Movables`
5. Select next elementin `Movables`
6. `SaveParentMatrix()` is called
7. `Move()` called for current moveable element `Move(false, movableElement)`
8. Loop check performed
	7.1 If will loop, move is reverrsed with `Move(true, movableElement)`. Algorithm returns to step 5 and the next `Movable` is selected.
9. h(n) (manhattan distance sum) for the current configuration is calculated.
10. The check `h(n)+g(n) < currentThreshold`is performed
	10.1 If false the move is reversed and the algorithm 			  goes back to step 5
11. Check for `h(n) == 0`. 
11.1 If this is true, `m_IsSolved` is set to true. The current `movableElement` is pushed into the stack. 
Return is called
12. `Search()` is called: `Search(currentDepth+1)`
13. After popping out of the recursion a check of m_Solved is executed. If the puzzle is solved the steps in 11.1 are executed.
14. If the puzzle is not solved, the move is reversed and the algorithm goes back to step 5.




