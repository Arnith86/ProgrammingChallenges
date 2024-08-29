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
		int i = 0;
		//Console.WriteLine(map.Count());
		//foreach (char[] test in map)
		//{
		//	Console.WriteLine(i);
		//	i++;
		//}

		//foreach (char[] item in map)
		//{
		//	string tempString = "";
		//	foreach (char item2 in item)
		//	{
		//		tempString = string.Concat(tempString, item2);
		//	}
		//	Console.WriteLine(tempString);
		//}
	}
}
