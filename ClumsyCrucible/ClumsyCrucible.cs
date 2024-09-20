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

