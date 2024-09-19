using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.ClumsyCrucible;

public class ClumsyCrucible
{
	int NR_OF_ROWS = 0; 

	public ClumsyCrucible()
	{
		readInput();
	}

	private void readInput()
	{
		StreamReader sr = new StreamReader("ExampleInput.txt");
		NR_OF_ROWS = sr.ReadLine().Length;
	}
}

