using System;
using System.IO;


/*
 * Biosalong kapacitet	= N besökare
 * Upptagna platser		= UP
 * Bio sälskap			= M 
 * 
 * Sällskap blir insläpta i ordningen de anmält sig.
 * M > (N - UP), sälskap blir inte insläppt  
 * 
 */
public class Biosällskap
{
	public Biosällskap()
	{
		string input;
		bool seatsAndVisitorsSet = false; 
		long capacity = 0;
		long visitingGroups = 0;
		long groupsRejected = 0;
	
		while ((input = Console.ReadLine()) != null) 
		{
			string[] inputToArray = input.Split(' ');
			
			if (!seatsAndVisitorsSet)
			{
				capacity = Int64.Parse(inputToArray[0]);
				visitingGroups = Int64.Parse(inputToArray[1]);
				seatsAndVisitorsSet = true;
			} 
			else
			{
				for (int i = 0; i < visitingGroups; i++)
				{
					long canTheyFit = (capacity - Int64.Parse(inputToArray[i]));
					
					if (canTheyFit < 0)
					{
						groupsRejected++;
					}
					else
					{
						capacity = canTheyFit; 
					}
				}
				Console.WriteLine(groupsRejected);
			}
		}
	}
}