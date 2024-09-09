using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace AirDuctSpelunking;

public class AirDuctSpelunking
{
	int matrixRows = 0;
	int matrixColumns = 0;
	char[][] map = null;
	bool[,] visited = null;
	int[,] adjacencyMatrix = null;

	int[,] direction = new int[,]
	{
		{  1,  0 },  // down
		{ -1,  0 },  // upp
		{  0, -1 },	 // left
		{  0,  1 }   // right
	};

	Queue<int[]> dfsQueue = new Queue<int[]>();
	Dictionary<int, List<int>> dfsVisited = new Dictionary<int, List<int>>();

	Node[] mustVisitNodes;

	public AirDuctSpelunking()
	{
		start();
	}

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
			StreamReader sr = new StreamReader("AirDuctSpelunking_Input3.txt");
			this.matrixRows = (File.ReadLines("AirDuctSpelunking_Input3.txt").Count());

			// creates the map md-array with the expected number of columns
			map = new char[this.matrixRows][];


			//Continue to read until you reach end of file
			for (int i = 0; i < this.matrixRows+1; i++)
			{
				//Read the next line
				line = sr.ReadLine();

				// Keeps track of how long the rows will be 
				if (inputSize == -1)
				{
					this.matrixColumns = line.Count();
				}

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
				
				// Resets the column counter for next row
				column = 0;
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

		// Perform a DFS for each node seperatly
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
				
			spelunkingDFS(i, mustVisitNodes[i].Row, mustVisitNodes[i].Column);		
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
		primsAlgorithm(0, nrMustVisit);
		
		Console.WriteLine("hihi");
	}

	// Find shortest path from start node to reach all other nodes using prims algorithm
	private void primsAlgorithm(int startNode,  int nrOfVertices)
	{
		// Array that will contain the created minimal spanning tree.
		int[] parent = new int[nrOfVertices];

		// Values used to pick the minimal weighted edge 
		int[] key = new int[nrOfVertices];

		bool[] mstSet = new bool[nrOfVertices];

		// Represents the set of currently added vertices in the MST 
		for (int i = 0; i < nrOfVertices; i++)
		{
			key[i] = int.MaxValue;
			mstSet[i] = false;
		}

		// Sets the start node (root) of MST
		key[startNode] = 0;
		parent[startNode] = -1;
		
		for (int i = 0; i < nrOfVertices ; i++)
		{
			// Picks the vertice with lowes distance not yet included in MST
			int pickedVertice = minKey(key, mstSet);

			// Set the picked vertice as true (now part of tree)
			mstSet[pickedVertice] = true;

			//int nrOfConectedNodes = this.mustVisitNodes[pickedVertice].LengthToNeighbor.Count;

			// Updating key values and parent indexes of adjacent vertices of picked vertex.
			// We skip the ones that already are part of the MST
			for (int j = 0; j < nrOfVertices; j++)
			{

				if (mstSet[j] == false &&                                               // Has not been visited yet
					this.adjacencyMatrix[pickedVertice, j] != 0 &&                      // Is a connection to node
					this.adjacencyMatrix[pickedVertice, j] < key[j])					// Has a lower distance then the current distance
				{
					parent[j] = pickedVertice;
					key[j] = this.adjacencyMatrix[pickedVertice, j]; 
				}
			}
		}
		printMST(parent, adjacencyMatrix, nrOfVertices);
		Console.WriteLine("HAHIO");
	}


	// THIS IS ONLY HERE FOR TESTING REASONS 
	static void printMST(int[] parent, int[,] graph, int V)
	{
		Console.WriteLine("Edge \tWeight");
		for (int i = 1; i < V; i++)
			Console.WriteLine(parent[i] + " - " + i + "\t"
							  + graph[i, parent[i]]);
	}

	// Finding the vertice with lowest distance, not yet included in mstSet
	private int minKey(int[] key, bool[] mstSet)
	{
		int arraySize = key.Length;

		// Set min to highest possible value
		int min = int.MaxValue;
		int min_index = -1;

		for (int i = 0; i < arraySize; i++)
		{
			if (mstSet[i] == false && key[i] < min)
			{
				min = key[i];
				min_index = i;
			}
		}

		return min_index;
	}

	

	// A depth first search to find the shortest distance between nodes
	// Recursion moves in all four directions 
	// TODO: make sure that ONLY THE SHORTEST distance is keept
	private void spelunkingDFS(int startNode, int row, int column, int step = 0/*, string movedInDirection = ""*/)
	{
		string currChar = "";

		this.visited[row, column] = true;  

		currChar = map[row][column].ToString();

		//TODO!!! THIS PART OF THE CODE SHOULD ONLY REGISTER IF THE DISTANCE IS LOWER THEN PREVIEUS DISTANCE!!!
		if (!currChar.Equals(".") && step != 0)
		{
			Console.WriteLine("startnode "+ startNode +" reached node " + currChar + " steps taken: "+step);
			if (!mustVisitNodes[startNode].LengthToNeighbor.ContainsKey(int.Parse(currChar)))
			{
				this.mustVisitNodes[startNode].LengthToNeighbor.Add(int.Parse(currChar), step);
				this.visited[row, column] = false;
			}
			
			if (step < mustVisitNodes[startNode].LengthToNeighbor[int.Parse(currChar)])
			{
				this.mustVisitNodes[startNode].LengthToNeighbor[int.Parse(currChar)] = step;
			}

			return; 
		}

		step++;

		// Moves in specified direction, with the exception of the direction it came from
		for (int i = 0; i < 4; i++)
		{
			// Has the cell already been visted?
			if (this.visited[row + direction[i, 0], column + direction[i, 1]] == true)
			{
				continue;
			}

			string nextChar = map[row + direction[i, 0]][column + direction[i, 1]].ToString();

			// Is next step outside map boundaries or a wall?
			if ((row + direction[i, 0]) < 0		||
				(column + direction[i, 1]) < 0	||
				(row + direction[i, 0]) == this.matrixRows ||
				(column + direction[i, 1] == this.matrixColumns || 
				nextChar.Equals("#")))
			{
				continue;
			}

			spelunkingDFS(startNode, row + direction[i, 0], column + direction[i, 1], step);
		}
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
