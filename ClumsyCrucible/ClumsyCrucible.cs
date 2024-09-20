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


