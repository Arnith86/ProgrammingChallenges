using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProgrammingChallenges.ClumsyCrucible;

/**
 * Part 2: 
 * 
 * The crucibles of lava simply aren't large enough to provide an adequate supply of lava to the machine parts factory. 
 * Instead, the Elves are going to upgrade to ultra crucibles. Ultra crucibles are even more difficult to steer than 
 * normal crucibles. Not only do they have trouble going in a straight line, but they also have trouble turning!  
 * Once an ultra crucible starts moving in a direction, it needs to move a minimum of four blocks in that direction 
 * before it can turn (or even before it can stop at the end). However, it will eventually start to get wobbly: an ultra 
 * crucible can move a maximum of ten consecutive blocks without turning. 
 * 
 */

public class ClumsyCrucible
{
	int NR_OF_ROWS;
	int NR_OF_COLUMNS;

	int[,] heatMap;

	public ClumsyCrucible()
	{
		readInput();
		
		Console.WriteLine(dijkstrasAlgorithm(1, 3));
		Console.WriteLine(dijkstrasAlgorithm(4, 10));
	}

	// Converts the int to only positive values
	private int directionToPosetiveInt (int direction)
	{
		int positiveIntiger = 0;
		
		switch (direction)
		{
			case -1: positiveIntiger = 0;
				break;
			case 0: positiveIntiger = 1;
				break;
			case 1: positiveIntiger = 2;
				break;
			default:
				Console.WriteLine("Invalid direction.");
				break;
		}
		return positiveIntiger;
	}

	private int dijkstrasAlgorithm(int minMoves, int maxMoves)
	{
		int leastHeatlossValue = 0;
		int loopCount = 1;

		// Keeps the node with the shortest distance at the top
		PriorityQueue<Node, int> leastHeatLoss = new PriorityQueue<Node, int>();

		// Keeps track of which nodes have already been visited.
		bool[,,,,] visited = new bool[NR_OF_ROWS, NR_OF_COLUMNS, 3, 3, maxMoves + 1 ];
		
		// Keeps track of the lowest heatloss accumulated to nodes and which node was the previously visited node.
		Dictionary<Node, int> moveSequance = new Dictionary<Node, int>();
		Dictionary<Node, Node> prevNodes = new Dictionary<Node, Node>();

		
		// Adds start nodes to queue (first move directions, down and right)
		Node currNode = new Node(0, 0, 1, 0, 0);
		Node firstNode1 = new Node(1, 0, 1, 0, 1);
		Node firstNode2 = new Node(0, 1, 0, 1, 1);

		leastHeatLoss.Enqueue(firstNode1, heatMap[1, 0]);
		leastHeatLoss.Enqueue(firstNode2, heatMap[0, 1]);
		moveSequance.Add(firstNode1, heatMap[1, 0]);
		moveSequance.Add(firstNode2, heatMap[0, 1]);

		prevNodes.Add(firstNode1, currNode);
		prevNodes.Add(firstNode2, currNode);

		int[,] direction = new int[,]
		{
			{  1,  0 },  // down
			{ -1,  0 },  // upp
			{  0, -1 },	 // left
			{  0,  1 }   // right
		};
		
		while (true)
		{
			if (leastHeatLoss.Count != 0) currNode = leastHeatLoss.Dequeue();
			else break; 

			int stepsTaken = currNode.Steps;

			// Final node was found 
			if (currNode.Row == NR_OF_ROWS - 1 && currNode.Column == NR_OF_COLUMNS - 1 && stepsTaken >= minMoves)
			{
				leastHeatlossValue = moveSequance[currNode];
				break;
			}
			

			int row = currNode.Row;
			int column = currNode.Column;
			int heatLossToCurrNode = moveSequance[currNode];
			int directionTakenX = currNode.DirectionX;
			int directionTakenY = currNode.DirectionY;
			

			// Checks if the node with its current state has already been visited, skips node if it has
			if (visited[row, column, directionToPosetiveInt(directionTakenX), directionToPosetiveInt(directionTakenY), stepsTaken]) continue;
			visited[row, column, directionToPosetiveInt(directionTakenX), directionToPosetiveInt(directionTakenY), stepsTaken] = true;

			for (int i = 0; i < 4; i++)
			{
				// Possible next nodes values
				int tryMoveInDirX = direction[i, 0];
				int tryMoveInDirY = direction[i, 1];
				int nextRow = currNode.Row + tryMoveInDirX;
				int nextColumn = currNode.Column + tryMoveInDirY;
				int nextStepCount = 1;
				bool sameDirection = (tryMoveInDirX == directionTakenX) && (tryMoveInDirY == directionTakenY); 

				// Hinders return to previus node
				if ((tryMoveInDirX + directionTakenX) == 0 && 
					(tryMoveInDirY + directionTakenY) == 0) continue;

				// Hinders movement outside of the matrix
				if (nextRow == NR_OF_ROWS || nextColumn == NR_OF_COLUMNS || nextRow < 0 || nextColumn < 0) continue;

				// Total heatloss from start to next node
				int heatLossToNextNode = heatLossToCurrNode + heatMap[nextRow, nextColumn];

				// Step count gets incremented if movement continues in same direction otherwise unchanged (1)
				if (sameDirection)	nextStepCount = stepsTaken + 1;

				// Prevents further movements in any direction exept the direction the previusly taken direction
				if (stepsTaken < minMoves && !sameDirection) continue;
				// Prevents further movements if max steps in the same direction has been taken
				else if (nextStepCount > maxMoves) continue;
				
				Node nextNode = new Node(nextRow, nextColumn, tryMoveInDirX, tryMoveInDirY, nextStepCount);

				// If heatloss is less to nextNode then current heatloss (or no registered value),set this node as new least heatloss node
				if (!moveSequance.ContainsKey(nextNode) || (moveSequance[nextNode] > heatLossToNextNode))
				{
					leastHeatLoss.Enqueue(nextNode, heatLossToNextNode);
					moveSequance[nextNode] = heatLossToNextNode;
				}
			}
		}

		return leastHeatlossValue;
	}

	// Gets the proportion of the input matrix
	private void getInputSize()
	{
		StreamReader sr = new StreamReader("ClumsyCrucibleInput.txt");
		NR_OF_ROWS = (File.ReadLines("ClumsyCrucibleInput.txt").Count());
		NR_OF_COLUMNS = sr.ReadLine().Count();
	}

	// Creates the map that will be used to map the possible routes.
	private void readInput()
	{
		getInputSize();
		string[] readAll = File.ReadAllLines("ClumsyCrucibleInput.txt");
		
		heatMap = new int[NR_OF_ROWS, NR_OF_COLUMNS];

		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			for (int j = 0; j < NR_OF_COLUMNS; j++)
			{
				heatMap[i, j] = readAll[i][j] - '0';
			}
		}
	}
}

// Node will contain state information 
public struct Node
{
	public int Row { get; set; }
	public int Column { get; set; }
	public int DirectionX { get; set; }
	public int DirectionY { get; set; }
	public int Steps { get; set; }
		
	public Node(int row, int column, int directionX, int directionY,int steps)
	{
		Row = row;
		Column = column;
		DirectionX = directionX;
		DirectionY = directionY;
		Steps = steps;
	}
}
