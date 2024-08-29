using System;
using System.IO;

/*
 * Äpplen = 7 kr st
 * Päron = 13 kr st
 * 
 */

class ApplesAndPears
{
	string input;

	public ApplesAndPears()
	{
		start();
	}
	private void start()
	{
		while ((input = Console.ReadLine()) != null)
		{
			string[] inputToArray = input.Split(' ');
			long axelSold = Int64.Parse(inputToArray[0]);
			long petraSold = Int64.Parse(inputToArray[1]);

			long appleRevenue = (axelSold * 7);
			long pearRevenue = (petraSold * 13);

			if (appleRevenue == pearRevenue)
			{
				Console.WriteLine("lika");
			}
			else if (appleRevenue > pearRevenue)
			{
				Console.WriteLine("Axel");
			}
			else
			{
				Console.WriteLine("Petra");
			}
		}
	}
}