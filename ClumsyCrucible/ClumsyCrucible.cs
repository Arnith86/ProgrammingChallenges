using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;


namespace ProgrammingChallenges.ClumsyCrucible;


public class ClumsyCrucible
{
	int NR_OF_ROWS;
	int NR_OF_COLUMNS;

	int[,] heatMap;

	public ClumsyCrucible()
	{
		readInput();
		Console.WriteLine(dijkstrasAlgorithm());
	}

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

	private int dijkstrasAlgorithm()
	{
		int leastHeatlossValue = 0;
		int loopCount = 1;

		// Keeps the node with the shortest distance at the top
		PriorityQueue<Node, int> leastHeatLoss = new PriorityQueue<Node, int>();

		// Keeps track of which nodes have already been visited.
		bool[,,,,] visited = new bool[NR_OF_ROWS, NR_OF_COLUMNS, 3, 3, 4];
		
		// Keeps track of the lowest heatloss accumulated to nodes and which node was the previously visited node.
		Dictionary<Node, int> moveSequance = new Dictionary<Node, int>();
		Dictionary<Node, Node> prevNodes = new Dictionary<Node, Node>();

		
		// Adds start node to queue
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
			Console.WriteLine($"still working: loop count {loopCount++}");
			if (leastHeatLoss.Count > 0) currNode = leastHeatLoss.Dequeue();

			// Final node was found 
			if (currNode.Row == NR_OF_ROWS - 1 && currNode.Column == NR_OF_COLUMNS - 1) 
			{
				leastHeatlossValue = moveSequance[currNode];
				break;
			}

			int row = currNode.Row;
			int column = currNode.Column;
			int heatLossToCurrNode = moveSequance[currNode];
			int directionTakenX = currNode.DirectionX;
			int directionTakenY = currNode.DirectionY;
			int stepsTaken = currNode.Steps;

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

				// Hinders return to previus node
				if ((tryMoveInDirX + directionTakenX) == 0 && 
					(tryMoveInDirY + directionTakenY) == 0) continue;


				// Hinders movement outside of the matrix
				if (nextRow == NR_OF_ROWS || nextColumn == NR_OF_COLUMNS || nextRow < 0 || nextColumn < 0) continue;

				// Total heatloss from start to next node
				int heatLossToNextNode = heatLossToCurrNode + heatMap[nextRow, nextColumn];

				// Step count gets incremented if movement continues in same direction otherwise unchanged (1)
				if ((tryMoveInDirX == directionTakenX) && (tryMoveInDirY == directionTakenY)) nextStepCount = stepsTaken + 1;
				
				// Prevents further movements if three steps in the same direction has been taken
				if (nextStepCount > 3) continue;
				
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
		//StreamReader sr = new StreamReader("ExampleInput.txt");
		//NR_OF_ROWS = (File.ReadLines("ExampleInput.txt").Count());
		//StreamReader sr = new StreamReader("singleSolution.txt");
		//NR_OF_ROWS = (File.ReadLines("singleSolution.txt").Count());

		NR_OF_COLUMNS = sr.ReadLine().Count();
	}

	// Creates the map that will be used to map the possible routes.
	private void readInput()
	{
		getInputSize();
		string[] readAll = File.ReadAllLines("ClumsyCrucibleInput.txt");
		//string[] readAll = File.ReadAllLines("ExampleInput.txt");
		//string[] readAll = File.ReadAllLines("singleSolution.txt");


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
