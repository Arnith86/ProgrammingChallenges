using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.ClumsyCrucible;

public class ClumsyCrucible
{
	int NR_OF_ROWS;
	int NR_OF_COLUMNS;

	int[,] heatMap;  
	const string ROW = "row";

	public ClumsyCrucible()
	{
		readInput();
		adjacencyMatrixCreator();
		Console.WriteLine( dijkstrasAlgorithm());
	}

	// Make sure this returns an array 
	private void adjacencyMatrixCreator()
	{

		Queue<Node> bfsQueue = new Queue<Node>();
		bool[,] visited = new bool[NR_OF_ROWS, NR_OF_COLUMNS];
		int combination = (NR_OF_ROWS * NR_OF_COLUMNS);
		adjacencyMatrix = new int[combination, combination];

		int[,] direction = new int[,]
		{
			{  1,  0 },  // down
			{ -1,  0 },  // upp
			{  0, -1 },	 // left
			{  0,  1 }   // right
		};

		// Start position of the bfs inserted 
		bfsQueue.Enqueue(heatMap[0,0]);
		visited[0, 0] = true;

		while (true)
		{
			// All nodes that can be visited has been visited
			if (!(bfsQueue.Count > 0)) break;

			Node currNode = bfsQueue.Dequeue();
			
			for (int i = 0; i < 4; i++)
			{
				int nextRow = currNode.Row + direction[i, 0];
				int nextColumn = currNode.Column + direction[i, 1];

				// Next coordinants for node is outside matrix
				if (nextRow == NR_OF_ROWS || nextColumn == NR_OF_COLUMNS || nextRow < 0 || nextColumn < 0) continue;
			
				Node nextNode = heatMap[nextRow, nextColumn];

				currNode.costToNeighbor.Add(nextNode.NodeNr, nextNode.HeatReduction);
				adjacencyMatrix[currNode.NodeNr, nextNode.NodeNr] = nextNode.HeatReduction;
									
				// Adds new node to queue, if not yet visited
				if (!visited[nextRow, nextColumn])
				{
					visited[nextRow, nextColumn] = true;
					bfsQueue.Enqueue(nextNode);
				}
			}
		}
	}

	// Gets the proportion of the input matrix
	private void getInputSize()
	{
		StreamReader sr = new StreamReader("ExampleInput.txt");
		//StreamReader sr = new StreamReader("ClumsyCrucibleInput.txt");
		NR_OF_ROWS = (File.ReadLines("ExampleInput.txt").Count());
		//NR_OF_ROWS = (File.ReadLines("ClumsyCrucibleInput.txt").Count());
		NR_OF_COLUMNS = sr.ReadLine().Count();
	}

	// Creates the map that will be used to map the possible routes.
	private void readInput()
	{
		getInputSize();
		StreamReader sr = new StreamReader("ExampleInput.txt");
		//StreamReader sr = new StreamReader("ClumsyCrucibleInput.txt");

		heatMap = new Node[NR_OF_ROWS, NR_OF_COLUMNS];
		int nodeNr = 0;

		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			string line = sr.ReadLine();
			int index = 0; 
			foreach (char charInString in line)
			{
				heatMap[i, index] = new Node((int)char.GetNumericValue(charInString), nodeNr++, i, index++);
			}
		}
	}
}
public class Node
{
	public int HeatReduction { get; set; }
	public int NodeNr { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }
	public bool Visited { get; set; }
	public Dictionary<int, int> costToNeighbor { get; set; }

	public Node(int heatReduction, int nodeNr, int row, int column, bool visited = false)
	{
		HeatReduction = heatReduction;
		NodeNr = nodeNr;
		Row = row;
		Column = column;
		costToNeighbor = new Dictionary<int, int>();
		Visited = visited;
	}
}


