using System;
using System.IO;
using System.Collections.Generic;
/* 
 * Dish = N: ingredients 
 * ingridient amount: total needed
 * ingredient amount: home
 * ingredient cost : per unit
 * 
 * problem: how much will buying the needed ingreients cost?
 * 
 * output: single integer with complete cost 
 */

/*
 * Structure:
 *     N	     H      B        C      (0-500)
 * ingredient: home - needed - cost
 */

class TheRecipe
{
	public TheRecipe()
	{
		start();
	}

	private void start()
	{
		string input;

		long numberOfIngredients = 0;
		long currentIngridient = 0;
		long completePurchase = 0;

		while ((input = Console.ReadLine()) != null)
		{

			string[] inputToArray = input.Split(' ');

			if (inputToArray.Length > 1)
			{

				long ingredientHome = Int64.Parse(inputToArray[0]);
				long ingredientNeeded = Int64.Parse(inputToArray[1]);
				long ingredientCost = Int64.Parse(inputToArray[2]);

				long missing = ingredientNeeded - ingredientHome;

				if (missing > 0)
				{
					long totalCostOfIngredient = missing * ingredientCost;
					completePurchase += totalCostOfIngredient;
				}

				currentIngridient++;
			}
			else if (inputToArray.Length == 1)
			{
				numberOfIngredients = Int32.Parse(inputToArray[0]);
			}

			if (currentIngridient == numberOfIngredients)
			{
				break;
			}
		}
		Console.WriteLine(completePurchase);
	}
}
	