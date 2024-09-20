using System;
using System.Collections.Generic;
using System.Linq;
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
		adjacencyMatrixCreator();
	}

	// Make sure this returns an array 
	private int adjacencyMatrixCreator()
	{

		Queue<Node> bfsQueue = new Queue<Node>();
		bool[,] visited = new bool[NR_OF_ROWS, NR_OF_COLUMNS];

		int[,] direction = new int[,]
		{
			{  1,  0 },  // down
			{ -1,  0 },  // upp
			{  0, -1 },	 // left
			{  0,  1 }   // right
		};

		// Start position of the bfs inserted 
		bfsQueue.Enqueue(new Node(0,0,0));

		while (true)
		{
			if (!(bfsQueue.Count > 0)) break;

			Node currNode = bfsQueue.Dequeue();

			for (int i = 0; i < 4; i++)
			{
				int nextRow = currNode.Row + direction[i, 0];
				int nextColumn = currNode.Column + direction[i, 1];

				if (!visited[nextRow, nextColumn]) 
				{
					visited[nextRow, nextColumn] = true;
					int heatReduction = heatMap[nextRow, nextColumn];

					bfsQueue.Enqueue(new Node (heatReduction, nextRow, nextColumn));
				} 
				
			}
		}

		return 0;  // // Make sure this returns an array 
	}

	private void bfsTraversal()
	{

	}
	// Gets the proportion of the input matrix
	private void getInputSize()
	{
		StreamReader sr = new StreamReader("ExampleInput.txt");
		NR_OF_ROWS = (File.ReadLines("ExampleInput.txt").Count());
		NR_OF_COLUMNS = sr.ReadLine().Count();
	}

	// Creates the map that will be used to map the possible routes.
	private void readInput()
	{
		getInputSize();
		StreamReader sr = new StreamReader("ExampleInput.txt");
		heatMap = new int[NR_OF_ROWS, NR_OF_COLUMNS];

		for (int i = 0; i < NR_OF_ROWS; i++)
		{
			string line = sr.ReadLine();
			int index = 0; 
			foreach (char charInString in line)
			{
				heatMap[i, index++] = (int)char.GetNumericValue(charInString);
			}
		}
	}
}
public class Node
{
	public int HeadReduction { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }
	public int[] StepsInDirection { get; set; }
	public Dictionary<Node, int> costToNeighbor { get; set; }

	public Node(int heatReduction, int row, int column, int[] stepsInDirection = null)
	{
		HeadReduction = heatReduction;
		Row = row;
		Column = column;
		costToNeighbor = new Dictionary<Node, int>();
		// Each index is a direction, 0:down, 1:upp, 2:left, 3:right 
		// Defaults to {0,0,0,0} if no steps are provided
		StepsInDirection = stepsInDirection ?? new int[] {0,0,0,0};
	}
}


