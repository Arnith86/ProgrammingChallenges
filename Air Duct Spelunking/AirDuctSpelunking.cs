using System;
using System.IO;

namespace AirDuctSpelunking;

public class AirDuctSpelunking
{
	public AirDuctSpelunking()
	{
		start();
	}

	private char[][] readInput()
	{
		int row = 0;
		int column = 0;

		char[][] map = new char[42][];
		char[] mapRow = null;

		int inputSize = -1;
		string line;

		try
		{
			//Pass the file path and file name to the StreamReader constructor
			StreamReader sr = new StreamReader("AirDuctSpelunking_Input.txt");
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
					inputSize = line.Count();
				}

				mapRow = new char[inputSize];

				// Fills the map row array with char values
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
		Dictionary<int, string> mustVisit = new Dictionary<int, string>();

		// int i = 0;
		//Console.WriteLine(map.Count());
		//foreach (char[] test in map)
		//{
		//	Console.WriteLine(i);
		//	i++;
		//}

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
					mustVisit.Add(key, row+","+column);
				}
			}
		}

		//foreach (KeyValuePair<int, string> item in mustVisit)
		//{
		//	string[] temp = item.Value.Split(","); 
		//	int rowIndex = int.Parse(temp[0]);
		//	int columnIndex = int.Parse(temp[1]);

		//	Console.WriteLine("key: "+item.Key+" row index: "+rowIndex+" column index: "+columnIndex);
		//}
	}
}
