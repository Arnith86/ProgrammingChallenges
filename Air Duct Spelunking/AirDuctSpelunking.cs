using System;
using System.IO;

namespace AirDuctSpelunking;

public class AirDuctSpelunking
{
	int matrixRows = 0;
	int matrixColumns = 0;

	Queue<string> bfsQueue = new Queue<string>();

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

		char[][] map = readInput();
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

		foreach (KeyValuePair<int, Node> item in mustVisit)
		{
			//string[] temp = item.Value.Split(",");
			//int rowIndex = int.Parse(temp[0]);
			//int columnIndex = int.Parse(temp[1]);

			Console.WriteLine("key: " + item.Key + " row index: " + item.Value.Row + " column index: " + item.Value.Column);
		}
	}
}

public class Node
{
	public int NodeNr { get; set; }
	public int Row { get; set; }
	public int Column { get; set; }

	public Dictionary<int, int> LengthToNextNode { get; set; }

	public Node(int nodeNr, int row, int column) 
	{ 
		NodeNr = nodeNr;
		Row = row;
		Column = column;
	}
}
