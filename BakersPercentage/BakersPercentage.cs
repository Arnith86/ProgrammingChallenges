using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingChallenges.BakersPercentage
{
	using System;
	using System.IO;
	using System.Xml.Linq;
	using System.Globalization;
	using System.Collections.Generic;
	using System.Transactions;

	/*
		A recipe is a list of ingredients and a set of instructions to prepare a dish. It is often written for
		a particular number of portions. If you have a recipe for 4 portions and you want to make 6 portions, it turns out that simply
		multiplying the amounts for each ingredient by 1.5 is often wrong! The reason is that the original recipe 
		may have been rounded to the nearest teaspoon, gram, etc., and the rounding errors magnify when a recipe is scaled.

		Some recipes are specifically written to ease the task of scaling. These recipes are developed using "Baker's
		percentages." Each ingredient is listed not only by weight (in grams), but also as a percentage relative to
		the "main ingredient." The main ingredient will always have a 100% Baker's percentage. Note that the
		sum of the Baker's percentages from all ingredients is greater than 100%, and that the Baker's percentages
		of some ingredients may exceed 100%.
	
		To scale a recipe:

		1. determine the scaling factor by dividing the number of desired portions by the number of
		   portions for which the recipe is written;

		2. multiply the weight of the main ingredient with a 100% Baker's percentage by the scaling factor.
		   This is the scaled weight of the main ingredient;
		
		3. calculate the scaled weight of every other ingredient by multiplying its Baker's percentage
		   by the scaled weight of the main ingredient.

		Input
		
		The first line of input specifies a positive integer T ≤ 1000, consisting of the cases to follow. Each case
		starts with a line with three integers R, P, and D: 1≤ R ≤ 20 is the number 1 ≤ P ≤ 12 is the number of portions for which the
		recipe is written, and 1 ≤ D ≤ 1000 is the number of desired portions. Each of the next R lines is of the form
		
		<name> <weight> <percentage>

		where <name> is the name of the ingredient (an alphabetic string of up to 20 characters with no embedded spaces), <weight> is the 
		weight in grams for that ingredient, and <percentage> is its Baker's percentage. Both <weight> and <percentage> are
		floating-point numbers with exactly one digit after the decimal point. Each recipe will only have one ingredient with a Baker's 
		percentage of 100%.

		Output

		For each case, print Recipe # followed by a space and the appropriate case number (see sample output below). 
		This is followed by the list of ingredients and their scaled weights in grams. The name of the ingredient and 
		its weight should be separated by a single space. Each ingredient is listed on its own line, in the same order as in the input. 
		After each case, print a line of 40 dashes ('-'). Answers within 0.1g of the correct result are acceptable.

		
	 */


	/* 
	 * First Input: T
	 * T : number of cases to follow (1 - 1000) 
	 * 
	 * Second input: R, P, D
	 * R : number of ingredients (1 - 20)
	 * P : number of portions (the recepie is written for) (1 - 12) 
	 * D : number of desired portions (1 - 1000)
	 * 
	 * Subsequent inputs: <name>   <weight>   <percentage>
	 * <name>		: string	(1 - 20)	: name of ingridient
	 * <weight>		: double				: weight in gram 
	 * <percentage>	: double	(n.n)		: only one ingredient will have 100 %
	 * 
	 * Output:
	 * Recipe # n
	 * each_ingredient its_weight (same order as input)
	 * ------------------------ ("-" x 40)
	 */

	/*
	 * 
	 */

	class BakersPercentage
	{
		string input;
		bool[] prereqInputs = new bool[2];

		int nrOfCases = 0;
		int nrOfingredients = 0;
		int originalNrPortions = 0;
		int desiredPortions = 0;

		float scalingFactor = 0.0f;

		int currentCase = 0;
		int currentIngredient = 0;

		string line = "";

		Dictionary<int, List<Ingrident>> ingridientInfo = new Dictionary<int, List<Ingrident>>();

		public BakersPercentage()
		{
			// build the 40 "_" long line
			for (int i = 0; i < 40; i++)
			{
				line = string.Concat(line, "-");
			}

			start();
		}

		public void start()
		{
			while ((input = Console.ReadLine()) != null)
			{

				string[] inputToArray = input.Split(' ');

				// Registers the number of cases
				if (!prereqInputs[0])
				{
					nrOfCases = int.Parse(inputToArray[0]);
					prereqInputs[0] = true;
				}
				// Registers case information, and calculates scaling factor 
				else if (!prereqInputs[1])
				{

					nrOfingredients = int.Parse(inputToArray[0]);
					originalNrPortions = int.Parse(inputToArray[1]);
					desiredPortions = int.Parse(inputToArray[2]);

					// Handles divisions by 0
					if (originalNrPortions != 0)
					{
						scalingFactor = (float)desiredPortions / (float)originalNrPortions;
					}
					else
					{
						scalingFactor = 1.0f;
					}

					prereqInputs[1] = true;
					currentCase++;
				}
				else
				{

					// InvariantCulture used to enable parsing of float regardles if it uses "." or ","
					string name = inputToArray[0];
					double weight = double.Parse(inputToArray[1], CultureInfo.InvariantCulture);
					double percentage = (double.Parse(inputToArray[2], CultureInfo.InvariantCulture)) / 100;

					// Check to see if this case already exists otherwise create new case in dic
					if (!ingridientInfo.ContainsKey(currentCase))
					{
						ingridientInfo.Add(currentCase, new List<Ingrident>());
					}

					ingridientInfo[currentCase].Add(new Ingrident(name, weight, percentage));
					currentIngredient++;

					// When all ingredients for case has been registered
					if (currentIngredient == nrOfingredients && prereqInputs[0] && prereqInputs[1])
					{
						Ingrident mainIngredient = null;

						// Find the main ingridient (100 percentage)
						foreach (Ingrident ingrident in ingridientInfo[currentCase])
						{
							if (ingrident.Percentage == 1)
							{
								mainIngredient = ingrident;
								break;
							}
						}

						// set the scaleweigh 
						double scaledWeight = (double)mainIngredient.Weight * scalingFactor;

						// Set the ajusted weights   
						foreach (Ingrident ingrident in ingridientInfo[currentCase])
						{
							if (!ingrident.Equals(mainIngredient))
							{
								ingrident.NewWeight = (double)ingrident.Percentage * scaledWeight;
							}
							else
							{
								ingrident.NewWeight = scaledWeight;
							}
						}

						// Sets values to enable new case to be registered
						prereqInputs[1] = false;
						currentIngredient = 0;

						// The expected number of cases has been registered 
						if (currentCase == nrOfCases)
						{
							break;
						}
					}
				}
			}

			// Prints the result
			foreach (KeyValuePair<int, List<Ingrident>> recipeCase in ingridientInfo)
			{
				Console.WriteLine("Recipe # " + recipeCase.Key);

				foreach (Ingrident ingrident in recipeCase.Value)
				{
					Console.WriteLine($"{ingrident.Name} {ingrident.NewWeight}");
				}
				Console.WriteLine(line);
			}
		}
	}

	// Houses all information regarding the individual ingridents 
	class Ingrident
	{
		public string Name { get; set; }
		public double Weight { get; set; }
		public double NewWeight { get; set; }
		public double Percentage { get; set; }

		public Ingrident(string name, double weight, double percentage)
		{
			Name = name;
			Weight = weight;
			Percentage = percentage;
		}
	}
}
