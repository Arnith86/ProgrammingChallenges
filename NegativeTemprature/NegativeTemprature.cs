using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.NegativeTemprature
{
	using System;
	using System.IO;

	/*
		We're not going to sugar-coat it: Chicago's winters can be rough. The temperatures sometimes dip to
		uncomfortable levels and, after last year's "polar vortex", the University of Chicago Weather Service
		wants to find out exactly how bad the winter was. More specifically, they are interested in knowing the
		total number of days in which the temperature was below zero degrees Celsius.

		Input

		The input is composed of two lines. The first line contains a single positive integer n (1 ≤ n ≤ 100) that
		specifies the number of temperatures collected by the University of Chicago Weather Service. The second
		line contains n temperatures, each separated by a single space. Each temperature is represented by an 
		integer t(-1000000 < t ≤ 1 000 000) 

		Output

		You must print a single integer: the number of temperatures strictly less than zero.
		
		Sample input:1		Expected output:
		3					1
		5 -10 15

		Sample input:2		Expected output:
		5					5
		-14 -5 -39 -5 -7
	 */

	/* 
	 * Input first line:
	 * n = number of tempratures collected (1 - 100)
	 * 
	 * Input Second line:
	 * instances of tempratures separated by a single space
	 * 
	 * Tempratures: (- 1 000 000 - 1 000 000) 
	 */

	/*
	 * Problem print the number of instances the temprature is bellow zero 
	 */

	class NegativeTemprature
	{
		public NegativeTemprature()
		{
			//start();
		}

		public void start()
		{
			string input;
			bool firstInputReceived = false;

			long nrOfTempratures = 0;
			long nrOfRegisteredTemp = 0;
			long tempBellowZero = 0;

			while ((input = Console.ReadLine()) != null)
			{

				string[] inputToArray = input.Split(' ');

				if (!firstInputReceived)
				{
					nrOfTempratures = Int64.Parse(inputToArray[0]);
					firstInputReceived = true;
				}
				else
				{
					foreach (string temprature in inputToArray)
					{
						long currentTemp = long.Parse(temprature);

						if (currentTemp < 0)
						{
							tempBellowZero++;
						}
						nrOfRegisteredTemp++;
					}
				}

				if (nrOfRegisteredTemp == nrOfTempratures)
				{
					Console.WriteLine(tempBellowZero);
				}
			}


		}

	}
}
