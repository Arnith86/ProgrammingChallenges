using System;
using System.IO;

namespace AirDuctSpelunking;

public class AirDuctSpelunking
{
	int matrixRows = 0;
	int matrixColumns = 0;
	char[][] map = null;

	//Queue<string> bfsQueue = new Queue<string>();
	Queue<int[]> bfsQueue = new Queue<int[]>();
	//List<int[]> bfsVisited = new List<int[]>();
	Dictionary<int, List<int>> bfsVisited = new Dictionary<int, List<int>>();
	List<int[]> nodeToNodeDistance = new List<int[]>();

	public AirDuctSpelunking()
	{
		start();
	}

	private char[][] readInput()
	{
		char[][] map = null;

		// First loop, registers the size of the matrix
		// Second loop, regesters the values of the matrix 
		
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
			
			// Minus one to account for counting starting at 1
			this.matrixRows = (File.ReadLines("AirDuctSpelunking_Input.txt").Count() - 1);

			Console.WriteLine(this.matrixRows);
			// creates the map md-array with the expected number of columns
			map = new char[this.matrixRows][];

			//Read the first line of text
			line = sr.ReadLine();

			//Continue to read until you reach end of file
			while (line != null)
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

		/*char[][]*/this.map = readInput();
		Dictionary<int, Node> mustVisit = new Dictionary<int, Node>();

		int width = 0;
		int hight = 0;

		// Finds and sets the hight and width of the map matrix
		foreach (char[] row in map)
		{
			if (hight == 0)
			{
				foreach (char column in row){	width++;	}
			}
			hight++;
		}

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
		Node[] mustVisitNodes = new Node[nrMustVisit];

		// Converts the Dictionery to an array for ease of access of specific nodes
		foreach (KeyValuePair<int, Node> item in mustVisit)
		{
			mustVisitNodes[item.Key] = item.Value;
		}

		for (int i = 0; i < nrMustVisit; i++)
		{
			Console.WriteLine("key: " + mustVisitNodes[i].NodeNr + " row index: " + mustVisitNodes[i].Row + " column index: " + mustVisitNodes[i].Column);
		}

		bfsVisited.Add(mustVisitNodes[0].Row,  new List<int> {mustVisitNodes[0].Column});

		spelunkingBFS(mustVisitNodes[0].Row, mustVisitNodes[0].Column);
	}

	// Find the distance between nodes
	// Recursion moves in all four directions 
	private void spelunkingBFS(int row, int column, int step = 0, string movedInDirection = "")
	{
		string currChar = "";

		if (step != 0) {

			bool keyFound = false;
			
			// Check if we have visited this coordinant, exits if we have.
			foreach (KeyValuePair<int, List<int>> kvp in bfsVisited)
			{
				if (kvp.Key == row)
				{
					keyFound= true;

					foreach (int co in kvp.Value)
					{
						if (co == column)
						{
							//Console.WriteLine("Already visited: r: " + row + " c:" + column);
							return;
						}
					}
					break;
				}
			}

			if (!keyFound)
			{
				bfsVisited.Add(row, new List<int>(column));
			}
			else
			{ 
				bfsVisited[row].Add(column); 
			}
		}

		
		//if (bfsVisited.ContainsKey(row))
		//{
		//	foreach (var item in collection)
		//	{

		//	}
		//	if ()
		//}

		// End current branch of recursion, outside of map border.
		if ((row == -1 || column == -1) || (row == this.matrixRows || column == this.matrixColumns))
		{
			return;
		} 
		else // Get the value of current position.
		{
			currChar = map[row][column].ToString();
		}		

		// End recursion branch, wall was hit.
		if (currChar.Equals("#"))
		{
			//Console.WriteLine("#");
			return; 
		}

		if (!currChar.Equals(".") && step != 0)
		{
			Console.WriteLine("We have reached a node" + currChar);
			return; 
		}


		step++;
		//Console.WriteLine(movedInDirection);
		// Moves in specified direction, with the exception of the direction it came from
		if (!movedInDirection.Equals("upp"))
		{
			// Moves down 
			spelunkingBFS(row + 1, column, step, "down");
		}
		
		if (!movedInDirection.Equals("down"))
		{	// Moves upp
			spelunkingBFS(row - 1, column, step, "upp");
		}
		
		if (!movedInDirection.Equals("right"))
		{
			// Moves left
			spelunkingBFS(row, column - 1, step, "left");
		}
		
		if (!movedInDirection.Equals("left"))
		{	
			// Moves right
			spelunkingBFS(row, column + 1, step, "right");
		}

		//Console.WriteLine("END!");
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
	}
}
