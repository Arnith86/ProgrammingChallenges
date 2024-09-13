
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace AirDuctSpelunking2;
/**
 * --- Part Two ---
 * Of course, if you leave the cleaning robot somewhere weird, someone is bound to notice.	
 * What is the fewest number of steps required to start at 0, visit every non-0 number marked on the map at least once, and then return to 0?
 **/


/**	
 * Part two of Air Duct Spelunking essentially transforms the problem from a traveling salesman-like problem with a caveat, to a complete version of TSP.
 * To solve this extended problem from part one, we use the same BFS and traveling salesman algorithms as in part one.
 * The only difference is that we now keep track of the start node. With this information, we simply add the distance from the last visited node
 * to the start node, as recorded in the adjacency matrix, when the FINISHED_STATE is reached.
 **/



public class AirDuctSpelunking2
{
	int matrixRows = 0;
	int matrixColumns = 0;
	char[][] map = null;
	bool[,] visited = null;
	int[,] adjacencyMatrix = null;

	int FINAL_STATE = 0;
	int START_NODE = 0;

	int[,] direction = new int[,]
	{
		{  1,  0 },  // down
		{ -1,  0 },  // upp
		{  0, -1 },	 // left
		{  0,  1 }   // right
	};

	Queue<int[]> bfsQueue = new Queue<int[]>();
	Dictionary<int, List<int>> bfsVisited = new Dictionary<int, List<int>>();

	Node[] mustVisitNodes;

	public AirDuctSpelunking2(int startNode)
	{
		START_NODE = startNode;
		start();
	}

	// Reads the input
	private char[][] readInput()
	{
		char[][] map = null;

		int row = 0;
		int column = 0;
		bool columnSet = false;

		char[] mapRow = null;

		int inputSize = -1;
		string line;

		try
		{
			//Pass the file path and file name to the StreamReader constructor
			StreamReader sr = new StreamReader("AirDuctSpelunking_Input.txt");
			this.matrixRows = (File.ReadLines("AirDuctSpelunking_Input.txt").Count());

			// creates the map md-array with the expected number of columns
			map = new char[this.matrixRows][];


			//Continue to read until you reach end of file
			for (int i = 0; i < this.matrixRows + 1; i++)
			{
				//Read the next line
				line = sr.ReadLine();

				// Keeps track of how long the rows will be 
				if (inputSize == -1) this.matrixColumns = line.Count();

				// Only creates the row array on second loop
				mapRow = new char[this.matrixColumns];

				// Fills the row array with char values, but only in second loop.
				foreach (char charInString in line)
				{
					mapRow[column] = charInString;
					column++;
				}

				map[row] = mapRow;
				row++;

				column = 0;  // Resets the column counter for next row
			}
			//close the file
			sr.Close();
		}
		catch (Exception e)
		{
			Console.WriteLine("Exception: " + e.Message);
		}

		return map;
	}


	private void start()
	{
		string input;

		// Gather input
		this.map = readInput();
		Dictionary<int, Node> mustVisit = new Dictionary<int, Node>();

		int width = this.matrixColumns;
		int hight = this.matrixRows;

		// Finds the nodes that must be visited
		for (int row = 0; row < hight; row++)
		{
			for (int column = 0; column < width; column++)
			{
				if (Char.IsNumber(map[row][column]))
				{
					// Converts the nodes char value to an int, and use it as key that stores coordinants
					int key = int.Parse(map[row][column].ToString());
					mustVisit.Add(key, new Node(key, row, column));
				}
			}
		}

		int nrMustVisit = mustVisit.Count();
		this.mustVisitNodes = new Node[nrMustVisit];

		// Converts the Dictionery to an array for ease of access of specific nodes
		foreach (KeyValuePair<int, Node> item in mustVisit)
		{
			mustVisitNodes[item.Key] = item.Value;
		}

		// Initiates the matrix of bools used to keep track of visited cells
		visited = new bool[this.matrixRows, this.matrixColumns];


		// Perform a BFS for each node seperatly
		for (int i = 0; i < nrMustVisit; i++)
		{
			// Resets the matrix
			for (int j = 0; j < this.matrixRows; j++)
			{
				for (int k = 0; k < this.matrixColumns; k++)
				{
					visited[j, k] = false;
				}
			}

			Console.WriteLine("key: " + mustVisitNodes[i].NodeNr + " row index: " + mustVisitNodes[i].Row + " column index: " + mustVisitNodes[i].Column);

			// A bredth first search to find the shortest distance between nodes
			// moves in all four directions 
			string currChar = "";
			int step = 0;

			// Enqueues the start node
			this.bfsQueue.Enqueue(new int[] { mustVisitNodes[i].Row, mustVisitNodes[i].Column, step });

			//int startNode = mustVisitNodes[i].NodeNr;
			int startNode = mustVisitNodes[i].NodeNr;

			while (true)
			{
				if (!(bfsQueue.Count > 0)) break;

				int[] positionInformation = bfsQueue.Dequeue();
				int row = positionInformation[0];
				int column = positionInformation[1];
				int currentSteps = positionInformation[2];
				currChar = map[row][column].ToString();

				// Regesters the shortest distance to a node from the start node
				if (!currChar.Equals(".") && currentSteps != 0 && currChar != startNode.ToString())
				{
					Console.WriteLine("startnode " + startNode + " reached node " + currChar + " steps taken: " + currentSteps);
					if (!mustVisitNodes[startNode].LengthToNeighbor.ContainsKey(int.Parse(currChar)))
					{
						this.mustVisitNodes[startNode].LengthToNeighbor.Add(int.Parse(currChar), currentSteps);
					}

					if (currentSteps < mustVisitNodes[startNode].LengthToNeighbor[int.Parse(currChar)])
					{
						this.mustVisitNodes[startNode].LengthToNeighbor[int.Parse(currChar)] = currentSteps;
					}

					continue;
				}

				// Moves in specified direction, with the exception of the direction it came from
				for (int j = 0; j < 4; j++)
				{
					int nextRow = row + direction[j, 0];
					int nextColum = column + direction[j, 1];

					// Has the cell already been visted?
					if (this.visited[nextRow, nextColum] == true) continue;

					string nextChar = map[nextRow][nextColum].ToString();

					// Is next step outside map boundaries or a wall?
					if (nextRow < 0 || nextColum < 0 || nextRow == this.matrixRows || nextColum == this.matrixColumns || nextChar.Equals("#"))
					{
						continue;
					}

					// Expands search
					bfsQueue.Enqueue(new int[] { nextRow, nextColum, (currentSteps + 1) });
					this.visited[nextRow, nextColum] = true;
				}
			}
		}

		// Creates an adjacency matrix from the data registered in the nodes
		this.adjacencyMatrix = new int[nrMustVisit, nrMustVisit];

		foreach (Node node in this.mustVisitNodes)
		{
			foreach (KeyValuePair<int, int> kvp in node.LengthToNeighbor)
			{
				adjacencyMatrix[node.NodeNr, kvp.Key] = kvp.Value;
			}
		}

		// Find shortest path from start node to all other nodes using dijkstras algorithm
		int result = dynamicProgramingSolveTSP(nrMustVisit);

		Console.WriteLine($"Shortest path distance: {result}");
	}


	/**---------------------------------------------------------------------------------------------
		
		The finished state is when the finished state mask has all bits set to 1
		(meaning all the nodes have been visited). maybe? :S
			
		Example:
		If N = 4: (where N is the number of nodes)
		1 << 4 results in 10000 (which is 16 in decimal).
		Subtracting 1 gives 01111 (which is 15 in decimal).
		Therefore, FINISHED_STATE = 15, or 1111 in binary, meaning that all 4 bits are set.

		1 << n : Represents all possible bitmask states (can range from 0 to 2^n -1)

		the "OR" "|" operand evaluates each bit in two masks, if one or both has the value 1 then it gets the value 1.
		Take these two binery numbers, 0001 and 1000, with the "OR" operand the answer will be 1001.

		the "AND" "&" operand evaluates if a specific bit is 1 or not. Take these two binery numbers, 1001 and 1000, and that 
		1001 & (1 << 3) is asked. when combined with the "AND" operand we get, 1000, showing that the bit is 1, or in the case
		of this bitmask, the node has already been visited.
	-------------------------------------------------------------------------------------------------**/

	// Initializes the traveling salesman recuresion
	private int dynamicProgramingSolveTSP(int N)
	{
		this.FINAL_STATE = (1 << N) - 1;
		// This initializes the state to binery of only zeros (we always start at node zero)
		int startState = 1 << 0;
		// The dimentions of the array is N x 2^n it will contain the cost for subpaths
		int[,] memo = new int[N, 1 << N];
		// Contains sets of last visited nodes  (will be used to regenerate the path later) 
		int[,] prevVisNode = new int[N, 1 << N];

		// Initializes all cells to -1, -1 = not computed
		for (int i = 0; i < N; i++)
		{
			for (int j = 0; j < (1 << N); j++)
			{
				memo[i, j] = -1;
			}
		}

		int minTourCost = tsp(N, 0, startState, memo, prevVisNode);

		//only returns minTour for the time beeing, consider regenerating path
		return minTourCost;

	}

	// Traveling Salesman Problem recursion checking each branch for shortest distance 
	private int tsp(int N, int node, int state, int[,] memo, int[,] prevVisNode)
	{
		// If the final state has been reached, end recursion with the shortest path to the start node from current node   
		if (state == FINAL_STATE) return adjacencyMatrix[START_NODE, node];

		// Return the chached answer if it has already been computed.
		if (memo[node, state] != -1) return memo[node, state];

		int minCost = int.MaxValue;
		int index = -1;
		int nrVisitedNodesOfBranch = 0;

		// Loops trough all nodes, skiping already visited nodes
		// Which has the shortest distance?
		for (int next = 0; next < N; next++)
		{
			// The new state is specified, by combining the current state with the next node to be visited.
			int nextState = state | (1 << next);

			// Checks if the next:th bit (node) has been visited. (1 = visited, 0 = not) 
			if ((state & (1 << next)) != 0) continue;


			// Visit next branch node 
			int newCost = adjacencyMatrix[node, next] + tsp(N, next, nextState, memo, prevVisNode);

			// We only keep the value of the branch with shorest distance
			if (newCost < minCost)
			{
				minCost = newCost;
				index = next;
			}
		}

		// Registers the last visited node in series 
		prevVisNode[node, state] = index;
		memo[node, state] = minCost;

		return minCost; ;
	}
}

public class Node
{
	public int NodeNr { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }

	public Dictionary<int, int> LengthToNeighbor { get; set; }

	public Node(int nodeNr, int row, int column)
	{
		NodeNr = nodeNr;
		Row = row;
		Column = column;
		LengthToNeighbor = new Dictionary<int, int>();
	}
}


