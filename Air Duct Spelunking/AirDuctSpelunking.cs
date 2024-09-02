using System;
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
			StreamReader sr = new StreamReader("AirDuctSpelunking_Input.txt");
			this.matrixRows = (File.ReadLines("AirDuctSpelunking_Input.txt").Count());

			Console.WriteLine(this.matrixRows);
			// creates the map md-array with the expected number of columns
			map = new char[this.matrixRows][];

			////Read the first line of text
			//line = sr.ReadLine();

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

	

	// Find the distance between nodes
	// Recursion moves in all four directions 
	private void spelunkingDFS(int startNode, int row, int column, int step = 0, string movedInDirection = "")
	{
		string currChar = "";
		
		// Check if we have visited this coordinant, exits if we have.
		if (this.visited[row, column] == true)
		{
			return;
		}
		// Else Regesters that the node now has been visited
		else
		{
			this.visited[row, column] = true;
		}

		// End current branch of recursion, outside of map border or wall was hit.
		if (row < 0 || column < 0 || row == this.matrixRows || column == this.matrixColumns || currChar.Equals("#"))
		{
			return;
		}
		else
		{
			currChar = map[row][column].ToString();
			
			if (currChar.Equals("#"))
			{
				return;
			}
		} 	


		if (!currChar.Equals(".") && step != 0)
		{
			Console.WriteLine("startnode "+ startNode +" reached node " + currChar + " steps taken: "+step);
			if (!mustVisitNodes[startNode].LengthToNeighbor.ContainsKey(int.Parse(currChar)))
			{
				this.mustVisitNodes[startNode].LengthToNeighbor.Add(int.Parse(currChar), step);
				this.mustVisitNodes[int.Parse(currChar)].LengthToNeighbor.Add(startNode, step);
			}
		
			return;
		}

		step++;
					
		// Moves in specified direction, with the exception of the direction it came from
		spelunkingDFS(startNode, row + 1, column, step, "down");
		spelunkingDFS(startNode, row - 1, column, step, "upp");
		spelunkingDFS(startNode, row, column - 1, step, "left");
		spelunkingDFS(startNode, row, column + 1, step, "right");
	
		return;
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
