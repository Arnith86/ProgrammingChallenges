/* 
 *  A person wants to check how many unique cities on postcards he/she owns. The uniqueness is by year basis. 
 *
 *	T : number of years to investigate 
 * 
 * For each year:
 *  n: number of postcards that year
 *  what follows is the name of each city the postcard was from 
 *  
 *  Problem: find all unique city names
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class uniqueCities
{
	public uniqueCities()
	{
		start();
	}
	private void start()
	{
		string input;
		bool yearsSet = false;
		bool numberOfPostcardsSet = false;

		long numberOfYears = 0;
		int numberOfPostcards = 0;

		long currentYear = 0;
		int currentPostcard = 0;

		Dictionary<string, HashSet<string>> postCardsByYear = new Dictionary<string, HashSet<string>>();
		HashSet<string> cityNames = new HashSet<string>();

		while ((input = Console.ReadLine()) != null)
		{
			string[] inputToArray = input.Split(' ');

			if (!yearsSet)
			{
				numberOfYears = Int64.Parse(inputToArray[0]);
				yearsSet = true;
			}
			//else if (!Regex.IsMatch(input, @"^[a-z]+$"))
			else if (!numberOfPostcardsSet)
			{
				numberOfPostcards = int.Parse(inputToArray[0]);
				numberOfPostcardsSet = true;
			}
			else
			{
				cityNames.Add(inputToArray[0]);
				currentPostcard++;
			}

			if ((numberOfPostcards == currentPostcard) && yearsSet && numberOfPostcardsSet)
			{
				postCardsByYear.Add("year" + currentYear, cityNames);

				cityNames = new HashSet<string>();
				currentPostcard = numberOfPostcards = 0;
				numberOfPostcardsSet = false;

				currentYear++;
			}

			if (numberOfYears == currentYear)
			{
				foreach (KeyValuePair<string, HashSet<string>> keyValuePair in postCardsByYear)
				{
					int uniqueCitys = 0;
					foreach (string city in keyValuePair.Value)
					{
						uniqueCitys++;
					}

					Console.WriteLine(uniqueCitys);
				}
			}
		}
	}
}